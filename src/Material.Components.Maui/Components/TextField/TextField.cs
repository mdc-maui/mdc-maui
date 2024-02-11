using System.ComponentModel;
using Microsoft.Maui.Animations;

namespace Material.Components.Maui;

public class TextField
    : TouchGraphicsView,
        IEditableElement,
        ILabelTextElement,
        ISupportingTextElement,
        IFontElement,
        IIconElement,
        ITrailingIconElement,
        IActiveIndicatorElement,
        IOutlineElement,
        IShapeElement,
        IBackgroundElement,
        IDisposable
{
    protected override void ChangeVisualState()
    {
        this.IsVisualStateChanging = true;
        var state = this.ViewState switch
        {
            ViewState.Disabled => "disabled",
            _
                => this.IsFocused
                    ? this.IsError
                        ? "error_focused"
                        : "focused"
                    : this.IsError
                        ? "error_normal"
                        : "normal",
        };

        VisualStateManager.GoToState(this, state);
        this.IsVisualStateChanging = false;

        this.Invalidate();
    }

    public event EventHandler<TextChangedEventArgs> TextChanged;
    public event EventHandler<EventArgs> TrailingIconClicked;

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(IEditableElement),
        default,
        propertyChanged: (bo, ov, nv) =>
        {
            var view = bo as TextField;

            if (view.Bounds.Width != -1)
            {
                var size = view.GetLayoutSize((float)view.Bounds.Width);

                if (size.Height != view.Bounds.Height - view.EditablePadding.VerticalThickness)
                    (view as IElement).InvalidateMeasure();
                else
                    (view as IElement).OnPropertyChanged();
            }

            view.TextChanged?.Invoke(view, new(ov as string, nv as string));

            if (
                view.Command?.CanExecute(
                    view.CommandParameter ?? new TextChangedEventArgs(ov as string, nv as string)
                )
                is true
            )
                view.Command?.Execute(
                    view.CommandParameter ?? new TextChangedEventArgs(ov as string, nv as string)
                );
        }
    );

    public static readonly BindableProperty IsErrorProperty = BindableProperty.Create(
        nameof(IsError),
        typeof(bool),
        typeof(IEditableElement),
        default,
        propertyChanged: (bo, ov, nv) =>
        {
            var view = bo as TextField;
            view.ChangeVisualState();
        }
    );

    public static readonly BindableProperty SelectionRangeProperty =
        IEditableElement.SelectionRangeProperty;
    public static readonly BindableProperty CaretColorProperty =
        IEditableElement.CaretColorProperty;
    public static readonly BindableProperty TextAlignmentProperty =
        IEditableElement.TextAlignmentProperty;
    public static readonly BindableProperty IsReadOnlyProperty =
        IEditableElement.IsReadOnlyProperty;
    public static readonly BindableProperty EditablePaddingProperty =
        IEditableElement.EditablePaddingProperty;
    public static readonly BindableProperty InputTypeProperty = IEditableElement.InputTypeProperty;

    public static readonly BindableProperty LabelTextProperty = ILabelTextElement.LabelTextProperty;
    public static readonly BindableProperty LabelFontColorProperty =
        ILabelTextElement.LabelFontColorProperty;

    public static readonly BindableProperty SupportingTextProperty =
        ISupportingTextElement.SupportingTextProperty;
    public static readonly BindableProperty SupportingFontColorProperty =
        ISupportingTextElement.SupportingFontColorProperty;

    public static readonly BindableProperty FontColorProperty = IFontElement.FontColorProperty;
    public static readonly BindableProperty FontSizeProperty = IFontElement.FontSizeProperty;
    public static readonly BindableProperty FontFamilyProperty = IFontElement.FontFamilyProperty;
    public static readonly BindableProperty FontWeightProperty = IFontElement.FontWeightProperty;
    public static readonly BindableProperty FontIsItalicProperty =
        IFontElement.FontIsItalicProperty;

    public static readonly BindableProperty IconDataProperty = IIconElement.IconDataProperty;
    public static readonly BindableProperty IconColorProperty = IIconElement.IconColorProperty;

    public static readonly BindableProperty TrailingIconDataProperty =
        ITrailingIconElement.TrailingIconDataProperty;
    public static readonly BindableProperty TrailingIconColorProperty =
        ITrailingIconElement.TrailingIconColorProperty;

    public static readonly BindableProperty ActiveIndicatorHeightProperty =
        IActiveIndicatorElement.ActiveIndicatorHeightProperty;
    public static readonly BindableProperty ActiveIndicatorColorProperty =
        IActiveIndicatorElement.ActiveIndicatorColorProperty;

    public static readonly BindableProperty OutlineWidthProperty =
        IOutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineColorProperty =
        IOutlineElement.OutlineColorProperty;

    public string Text
    {
        get => (string)this.GetValue(TextProperty);
        set => this.SetValue(TextProperty, value);
    }

    public bool IsError
    {
        get => (bool)this.GetValue(IsErrorProperty);
        set => this.SetValue(IsErrorProperty, value);
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

    public bool IsReadOnly
    {
        get => (bool)this.GetValue(IsReadOnlyProperty);
        set => this.SetValue(IsReadOnlyProperty, value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Thickness EditablePadding
    {
        get => (Thickness)this.GetValue(EditablePaddingProperty);
        set => this.SetValue(EditablePaddingProperty, value);
    }
    public InputType InputType
    {
        get => (InputType)this.GetValue(InputTypeProperty);
        set => this.SetValue(InputTypeProperty, value);
    }

    public string LabelText
    {
        get => (string)this.GetValue(LabelTextProperty);
        set => this.SetValue(LabelTextProperty, value);
    }

    public Color LabelFontColor
    {
        get => (Color)this.GetValue(LabelFontColorProperty);
        set => this.SetValue(LabelFontColorProperty, value);
    }

    public string SupportingText
    {
        get => (string)this.GetValue(SupportingTextProperty);
        set => this.SetValue(SupportingTextProperty, value);
    }

    public Color SupportingFontColor
    {
        get => (Color)this.GetValue(SupportingFontColorProperty);
        set => this.SetValue(SupportingFontColorProperty, value);
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

    public string IconData
    {
        get => (string)this.GetValue(IconDataProperty);
        set => this.SetValue(IconDataProperty, value);
    }

    PathF IIconElement.IconPath { get; set; }

    public Color IconColor
    {
        get => (Color)this.GetValue(IconColorProperty);
        set => this.SetValue(IconColorProperty, value);
    }

    public string TrailingIconData
    {
        get => (string)this.GetValue(TrailingIconDataProperty);
        set => this.SetValue(TrailingIconDataProperty, value);
    }

    PathF ITrailingIconElement.TrailingIconPath { get; set; }

    public Color TrailingIconColor
    {
        get => (Color)this.GetValue(TrailingIconColorProperty);
        set => this.SetValue(TrailingIconColorProperty, value);
    }

    public int ActiveIndicatorHeight
    {
        get => (int)this.GetValue(ActiveIndicatorHeightProperty);
        set => this.SetValue(ActiveIndicatorHeightProperty, value);
    }

    public Color ActiveIndicatorColor
    {
        get => (Color)this.GetValue(ActiveIndicatorColorProperty);
        set => this.SetValue(ActiveIndicatorColorProperty, value);
    }

    public int OutlineWidth
    {
        get => (int)this.GetValue(OutlineWidthProperty);
        set => this.SetValue(OutlineWidthProperty, value);
    }

    public Color OutlineColor
    {
        get => (Color)this.GetValue(OutlineColorProperty);
        set => this.SetValue(OutlineColorProperty, value);
    }

    internal CaretInfo CaretInfo { get; private set; }
    internal bool IsDrawCaret { get; private set; }

    internal float LabelAnimationPercent { get; private set; } = 1f;

    readonly IDispatcherTimer caretAnimationTimer;

    public TextField()
    {
        this.Drawable = new TextFieldDrawable(this);

#if WINDOWS
        this.StartInteraction += this.OnEditorStartInteraction;
        this.DragInteraction += this.OnEditorDragInteraction;
#endif

        this.Focused += this.OnFocusChanged;
        this.Unfocused += this.OnFocusChanged;

        this.Clicked += this.OnClicked;

        this.caretAnimationTimer = this.Dispatcher.CreateTimer();
        this.caretAnimationTimer.Interval = TimeSpan.FromMilliseconds(500);
        this.caretAnimationTimer.IsRepeating = true;
        this.caretAnimationTimer.Tick += this.OnCaretAnimationTimerTick;
    }

    private void OnClicked(object sender, TouchEventArgs e)
    {
        var x = this.LastTouchPoint.X;
        var y = this.LastTouchPoint.Y;

        if (
            x >= this.Bounds.Width - 48f
            && x <= this.Bounds.Width - 8f
            && y >= this.Bounds.Height / 2 - 20f
            && y <= this.Bounds.Height / 2 + 20f
        )
            TrailingIconClicked?.Invoke(this, e);
    }

    private void OnEditorStartInteraction(object sender, TouchEventArgs e)
    {
        this.CaretInfo = this.GetCaretInfo((float)this.Bounds.Width, e.Touches[0]);
        this.SelectionRange = new(this.CaretInfo.Position);
    }

    private void OnEditorDragInteraction(object sender, TouchEventArgs e)
    {
        if (!this.isTouching)
            return;
        var caretInfo = this.GetCaretInfo((float)this.Bounds.Width, e.Touches[0]);
        this.SelectionRange = new(this.SelectionRange.Start, caretInfo.Position);
    }

    private void OnCaretAnimationTimerTick(object sender, EventArgs e)
    {
        this.IsDrawCaret = !this.IsDrawCaret;
        this.Invalidate();
    }

    private void OnFocusChanged(object sender, FocusEventArgs e)
    {
        if (e.IsFocused)
        {
            if (!this.IsReadOnly)
            {
                if (this.CaretInfo.IsZero)
                    this.CaretInfo = this.GetCaretInfo((float)this.Bounds.Width, 0);

                this.IsDrawCaret = true;
                this.caretAnimationTimer.Start();
            }
        }
        else
        {
            this.IsDrawCaret = false;
            this.caretAnimationTimer.Stop();
        }

#if WINDOWS
        if (e.IsFocused && !this.IsReadOnly)
            this.ShowKeyboard();
        else
            this.HideKeyboard();

#endif
        if (e.IsFocused && !this.IsReadOnly)
            this.StartLabelTextAnimation();
    }

    public void UpdateSelectionRange(TextRange range)
    {
        if (!range.IsRange && !this.SelectionRange.Equals(range))
            this.CaretInfo = this.GetCaretInfo((float)this.Bounds.Width, range.Start);

        this.SelectionRange = TextRange.CopyOf(range);
    }

    public void StartLabelTextAnimation()
    {
        if (!string.IsNullOrEmpty(this.Text))
        {
            this.Invalidate();
            return;
        }

        this.animationManager ??= this.Handler
            .MauiContext
            ?.Services
            .GetRequiredService<IAnimationManager>();
        var start = 0f;
        var end = 1f;

        this.animationManager?.Add(
            new Microsoft.Maui.Animations.Animation(
                callback: (progress) =>
                {
                    this.LabelAnimationPercent = start.Lerp(end, progress);
                    this.Invalidate();
                },
                duration: 0.5f,
                easing: Easing.Default
            )
        );
    }

    protected override float GetRippleSize()
    {
        var points = new PointF[4];
        points[0].X = points[2].X = this.LastTouchPoint.X;
        points[0].Y = points[1].Y = this.LastTouchPoint.Y;
        points[1].X = points[3].X = this.LastTouchPoint.X - 40f;
        points[2].Y = points[3].Y = this.LastTouchPoint.Y - 40f;
        var maxSize = 0f;
        foreach (var point in points)
        {
            var size = MathF.Pow(
                MathF.Pow(point.X - this.LastTouchPoint.X, 2f)
                    + MathF.Pow(point.Y - this.LastTouchPoint.Y, 2f),
                0.5f
            );
            if (size > maxSize)
            {
                maxSize = size;
            }
        }
        return maxSize;
    }

    protected override void StartRippleEffect()
    {
        var x = this.LastTouchPoint.X + this.Bounds.Left;
        var y = this.LastTouchPoint.Y + this.Bounds.Top;

        if (
            x >= this.Bounds.Right - 48f
            && x <= this.Bounds.Right - 8f
            && y >= this.Bounds.Center.Y - 20f
            && y <= this.Bounds.Center.Y + 20f
        )
        {
            this.animationManager ??= this.Handler
                .MauiContext
                ?.Services
                .GetRequiredService<IAnimationManager>();

            this.animationManager?.Add(
                new Microsoft.Maui.Animations.Animation(
                    callback: (progress) =>
                    {
                        this.RipplePercent = 0f.Lerp(1f, progress);
                        this.Invalidate();
                    },
                    duration: this.RippleDuration,
                    easing: this.RippleEasing
                )
            );
        }
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        var maxWidth =
            Math.Min(
                Math.Min(widthConstraint, this.MaximumWidthRequest),
                this.WidthRequest != -1 ? this.WidthRequest : double.PositiveInfinity
            ) - this.Margin.HorizontalThickness;

        var maxHeight =
            Math.Min(
                Math.Min(heightConstraint, this.MaximumHeightRequest),
                this.HeightRequest != -1 ? this.HeightRequest : double.PositiveInfinity
            ) - this.Margin.VerticalThickness;

        var iconWidth = !string.IsNullOrEmpty(this.IconData) ? 24f : 0f;
        var TrailingIconWdith = !string.IsNullOrEmpty(this.TrailingIconData) ? 24f : 0f;

        if (this.OutlineWidth == 0)
            this.EditablePadding = new Thickness
            {
                Left = iconWidth != 0f ? 12f + 24f + 16f : 16f,
                Right = TrailingIconWdith != 0f ? 16f + 24f + 12f : 16f,
                Top = 8f + 8f + 16f,
                Bottom = 8f + 4f + 16f
            };
        else
            this.EditablePadding = new Thickness
            {
                Left = iconWidth != 0f ? 12f + 24f + 16f : 16f,
                Right = TrailingIconWdith != 0f ? 16f + 24f + 12f : 16f,
                Top = 8f + 16f,
                Bottom = 16f + 4f + 16f
            };

        var textSize = this.GetLayoutSize((float)maxWidth);

        var desiredHeight = textSize.Height + 8f + 32f + 4f + 16f;

        var width =
            this.HorizontalOptions.Alignment == LayoutAlignment.Fill
                ? maxWidth
                : this.Margin.HorizontalThickness
                    + Math.Max(
                        this.MinimumWidthRequest,
                        this.WidthRequest == -1
                            ? Math.Min(
                                maxWidth,
                                textSize.Width + this.EditablePadding.HorizontalThickness
                            )
                            : this.WidthRequest
                    );
        var height =
            this.VerticalOptions.Alignment == LayoutAlignment.Fill
                ? maxHeight
                : this.Margin.VerticalThickness
                    + Math.Max(
                        this.MinimumHeightRequest,
                        this.HeightRequest == -1
                            ? Math.Min(maxHeight, desiredHeight)
                            : this.HeightRequest
                    );

        this.DesiredSize = new Size(Math.Ceiling(width), Math.Ceiling(height));
        return this.DesiredSize;
    }

    protected override void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            this.Focused -= this.OnFocusChanged;
            this.Unfocused -= this.OnFocusChanged;

            this.caretAnimationTimer.Stop();
            this.caretAnimationTimer.Tick -= this.OnCaretAnimationTimerTick;

            ((IIconElement)this).IconPath?.Dispose();
            ((ITrailingIconElement)this).TrailingIconPath?.Dispose();
        }
        base.Dispose(disposing);
    }
}
