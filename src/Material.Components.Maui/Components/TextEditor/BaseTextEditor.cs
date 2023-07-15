namespace Material.Components.Maui;

public class BaseTextEditor
    : TouchGraphicsView,
        IEditableElement,
        IFontElement,
        IElement,
        IICommandElement
{

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(IEditableElement),
        default,
        propertyChanged: (bo, ov, nv) =>
        {
            var view = bo as BaseTextEditor;

            if (view.Bounds.Width != -1)
            {
                var size = view.GetLayoutSize((float)view.Bounds.Width);

                if (size.Height != view.Bounds.Height)
                {
                    (view as IElement).InvalidateMeasure();
                }
                else
                    (view as IElement).OnPropertyChanged();
            }

        }
    );

    public static readonly BindableProperty SelectionRangeProperty =
        IEditableElement.SelectionRangeProperty;
    public static readonly BindableProperty CaretColorProperty =
        IEditableElement.CaretColorProperty;
    public static readonly BindableProperty TextAlignmentProperty =
        IEditableElement.TextAlignmentProperty;

    public static readonly BindableProperty FontColorProperty = IFontElement.FontColorProperty;
    public static readonly BindableProperty FontSizeProperty = IFontElement.FontSizeProperty;
    public static readonly BindableProperty FontFamilyProperty = IFontElement.FontFamilyProperty;
    public static readonly BindableProperty FontWeightProperty = IFontElement.FontWeightProperty;
    public static readonly BindableProperty FontIsItalicProperty =
        IFontElement.FontIsItalicProperty;

    public string Text
    {
        get => (string)this.GetValue(TextProperty);
        set => this.SetValue(TextProperty, value);
    }

    public TextRange SelectionRange
    {
        get => (TextRange)this.GetValue(SelectionRangeProperty);
        set => this.SetValue(SelectionRangeProperty, value);
    }

    public Color CaretColor
    {
        get => (Color)this.GetValue(CaretColorProperty);
        set => this.SetValue(CaretColorProperty, value);
    }

    public TextAlignment TextAlignment
    {
        get => (TextAlignment)this.GetValue(TextAlignmentProperty);
        set => this.SetValue(TextAlignmentProperty, value);
    }

    public Color FontColor
    {
        get => (Color)this.GetValue(FontColorProperty);
        set => this.SetValue(FontColorProperty, value);
    }
    public float FontSize
    {
        get => (float)this.GetValue(FontSizeProperty);
        set => this.SetValue(FontSizeProperty, value);
    }
    public string FontFamily
    {
        get => (string)this.GetValue(FontFamilyProperty);
        set => this.SetValue(FontFamilyProperty, value);
    }
    public FontWeight FontWeight
    {
        get => (FontWeight)this.GetValue(FontWeightProperty);
        set => this.SetValue(FontWeightProperty, value);
    }
    public bool FontIsItalic
    {
        get => (bool)this.GetValue(FontIsItalicProperty);
        set => this.SetValue(FontIsItalicProperty, value);
    }

    internal CaretInfo CaretInfo { get; private set; }
    internal bool IsDrawCaret { get; private set; }

    readonly IDispatcherTimer caretAnimationTimer;

    public BaseTextEditor()
    {
        this.Drawable = new TextEditorDrawable(this);
        this.Clicked += this.OnClicked;
        this.Focused += this.OnFocused;
        this.Unfocused += this.OnUnFocused;

        this.caretAnimationTimer = this.Dispatcher.CreateTimer();
        this.caretAnimationTimer.Interval = TimeSpan.FromMilliseconds(500);
        this.caretAnimationTimer.IsRepeating = true;
        this.caretAnimationTimer.Tick += this.OnCaretAnimationTimerTick;
    }

    private void OnClicked(object sender, TouchEventArgs e)
    {
#if WINDOWS
        if (!this.SelectionRange.IsRange)
        {

            this.CaretInfo = this.GetCaretInfo((float)this.Bounds.Width, e.Touches[0]);
            this.SelectionRange = new(this.CaretInfo.Position);
        }
#endif
    }

    private void OnCaretAnimationTimerTick(object sender, EventArgs e)
    {
        this.IsDrawCaret = !this.IsDrawCaret;
        this.Invalidate();
    }

    private void OnFocused(object sender, FocusEventArgs e)
    {
        if (e.IsFocused)
        {
            if (this.CaretInfo.IsZero)
                this.CaretInfo = this.GetCaretInfo((float)this.Bounds.Width, 0);

            this.IsDrawCaret = true;
            this.Invalidate();
            this.caretAnimationTimer.Start();
        }
    }

    private void OnUnFocused(object sender, FocusEventArgs e)
    {
        if (!e.IsFocused)
        {
            this.IsDrawCaret = false;
            this.Invalidate();
            this.caretAnimationTimer.Stop();
        }
    }

    public void UpdateSelectionRange(TextRange range)
    {
        if (!range.IsRange && !this.SelectionRange.Equals(range))
            this.CaretInfo = this.GetCaretInfo((float)this.Bounds.Width, range.Start);

        this.SelectionRange = TextRange.CopyOf(range);
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        var maxWidth = Math.Min(
            Math.Min(widthConstraint, this.MaximumWidthRequest),
            this.WidthRequest != -1 ? this.WidthRequest : double.PositiveInfinity
        );
        var maxHeight = Math.Min(
            Math.Min(heightConstraint, this.MaximumHeightRequest),
            this.HeightRequest != -1 ? this.HeightRequest : double.PositiveInfinity
        );

        var textSize = this.GetLayoutSize((float)maxWidth);

        var width =
            this.HorizontalOptions.Alignment == LayoutAlignment.Fill
                ? maxWidth
                : this.Margin.HorizontalThickness
                    + Math.Max(
                        this.MinimumWidthRequest,
                        this.WidthRequest == -1
                            ? Math.Min(maxWidth, textSize.Width)
                            : this.WidthRequest
                    );
        var height =
            this.VerticalOptions.Alignment == LayoutAlignment.Fill
                ? maxHeight
                : this.Margin.VerticalThickness
                    + Math.Max(
                        this.MinimumHeightRequest,
                        this.HeightRequest == -1
                            ? Math.Min(maxHeight, textSize.Height)
                            : this.HeightRequest
                    );

        this.DesiredSize = new Size(Math.Ceiling(width), Math.Ceiling(height));
        return this.DesiredSize;
    }

    protected override void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.Focused -= this.OnFocused;
                this.Unfocused -= this.OnUnFocused;

                this.caretAnimationTimer.Stop();
                this.caretAnimationTimer.Tick -= this.OnCaretAnimationTimerTick;
            }
        }
        base.Dispose(disposing);
    }
}
