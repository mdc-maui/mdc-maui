using Material.Components.Maui.Converters;
using Microsoft.Maui.Animations;
using System.ComponentModel;
using Topten.RichTextKit;
using Topten.RichTextKit.Editor;

namespace Material.Components.Maui;

public partial class TextField
    : SKTouchCanvasView,
        IView,
        IEditTextElement,
        IBackgroundElement,
        IForegroundElement,
        IShapeElement,
        IOutlineElement,
        ICommandElement,
        ILabelTextElement,
        ISupportingTextElement,
        IIconElement,
        ITrailingIconElement
{
    #region interface
    #region IView
    private bool isVisualStateChanging;
    private ControlState controlState = ControlState.Normal;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ControlState ControlState
    {
        get => this.controlState;
        set
        {
            this.controlState = value;
            this.ChangeVisualState();
        }
    }

    protected override void ChangeVisualState()
    {
        this.isVisualStateChanging = true;
        var state = this.ControlState switch
        {
            ControlState.Disabled => "disabled",
            _
                => this.IsError && this.InternalFocus
                    ? "focused:error"
                    : !this.IsError && this.InternalFocus
                        ? "focused"
                        : this.IsError && !this.InternalFocus
                            ? "normal:error"
                            : "normal",
        };
        VisualStateManager.GoToState(this, state);
        this.isVisualStateChanging = false;
    }

    public void OnPropertyChanged()
    {
        if (this.Handler != null && !this.isVisualStateChanging)
        {
            this?.InvalidateSurface();
        }
    }
    #endregion

    #region ITextElement
    public static readonly BindableProperty TextProperty = EditTextElement.TextProperty;
    public static readonly BindableProperty FontFamilyProperty = EditTextElement.FontFamilyProperty;
    public static readonly BindableProperty FontSizeProperty = EditTextElement.FontSizeProperty;
    public static readonly BindableProperty FontWeightProperty = EditTextElement.FontWeightProperty;
    public static readonly BindableProperty FontItalicProperty = EditTextElement.FontItalicProperty;

    public event EventHandler<TextChangedEventArgs> TextChanged;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public TextDocument TextDocument { get; set; } = new();

    [EditorBrowsable(EditorBrowsableState.Never)]
    public TextStyle TextStyle { get; set; } = FontMapper.DefaultStyle.Modify();
    public string Text
    {
        get => (string)this.GetValue(TextProperty);
        set => this.SetValue(TextProperty, value);
    }
    public string FontFamily
    {
        get => (string)this.GetValue(FontFamilyProperty);
        set => this.SetValue(FontFamilyProperty, value);
    }
    public float FontSize
    {
        get => (float)this.GetValue(FontSizeProperty);
        set => this.SetValue(FontSizeProperty, value);
    }
    public int FontWeight
    {
        get => (int)this.GetValue(FontWeightProperty);
        set => this.SetValue(FontWeightProperty, value);
    }
    public bool FontItalic
    {
        get => (bool)this.GetValue(FontItalicProperty);
        set => this.SetValue(FontItalicProperty, value);
    }

    void IEditTextElement.OnTextChanged(string oldValue, string newValue)
    {
        this.TextChanged?.Invoke(this, new TextChangedEventArgs(oldValue, newValue));
        this.Command?.Execute(this.CommandParameter ?? newValue);
    }

    void IEditTextElement.OnChanged()
    {
        this.TextDocument.DefaultStyle = this.TextStyle;
        if (this.LabelTextStyle.FontFamily != this.TextStyle.FontFamily)
            this.LabelTextStyle.FontFamily = this.TextStyle.FontFamily;
        if (this.LabelTextStyle.FontWeight != this.TextStyle.FontWeight)
            this.LabelTextStyle.FontWeight = this.TextStyle.FontWeight;
        if (this.LabelTextStyle.FontItalic != this.TextStyle.FontItalic)
            this.LabelTextStyle.FontItalic = this.TextStyle.FontItalic;
        if (this.LabelTextStyle.FontSize != this.TextStyle.FontSize * 0.75f)
            this.LabelTextStyle.FontSize = this.TextStyle.FontSize * 0.75f;

        if (this.SupportingTextStyle.FontFamily != this.TextStyle.FontFamily)
            this.SupportingTextStyle.FontFamily = this.TextStyle.FontFamily;
        if (this.SupportingTextStyle.FontWeight != this.TextStyle.FontWeight)
            this.SupportingTextStyle.FontWeight = this.TextStyle.FontWeight;
        if (this.SupportingTextStyle.FontItalic != this.TextStyle.FontItalic)
            this.SupportingTextStyle.FontItalic = this.TextStyle.FontItalic;
        if (this.SupportingTextStyle.FontSize != this.TextStyle.FontSize * 0.75f)
            this.SupportingTextStyle.FontSize = this.TextStyle.FontSize * 0.75f;

        this.isDrawCaret = true;

        var oldSize = this.DesiredSize;
        this.SendInvalidateMeasure();
        if (oldSize == this.DesiredSize)
        {
            this.OnPropertyChanged();
        }
    }
    #endregion

    #region IForegroundElement
    public static readonly BindableProperty ForegroundColorProperty =
        ForegroundElement.ForegroundColorProperty;
    public static readonly BindableProperty ForegroundOpacityProperty =
        ForegroundElement.ForegroundOpacityProperty;
    public Color ForegroundColor
    {
        get => (Color)this.GetValue(ForegroundColorProperty);
        set => this.SetValue(ForegroundColorProperty, value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float ForegroundOpacity
    {
        get => (float)this.GetValue(ForegroundOpacityProperty);
        set => this.SetValue(ForegroundOpacityProperty, value);
    }
    #endregion

    #region IBackgroundElement
    public static readonly BindableProperty BackgroundColourProperty =
        BackgroundElement.BackgroundColourProperty;
    public static readonly BindableProperty BackgroundOpacityProperty =
        BackgroundElement.BackgroundOpacityProperty;
    public Color BackgroundColour
    {
        get => (Color)this.GetValue(BackgroundColourProperty);
        set => this.SetValue(BackgroundColourProperty, value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float BackgroundOpacity
    {
        get => (float)this.GetValue(BackgroundOpacityProperty);
        set => this.SetValue(BackgroundOpacityProperty, value);
    }
    #endregion

    #region IIconElement
    public static readonly BindableProperty IconKindProperty = IconElement.IconKindProperty;
    public static readonly BindableProperty IconDataProperty = IconElement.IconDataProperty;
    public static readonly BindableProperty IconSourceProperty = IconElement.IconSourceProperty;

    public IconKind IconKind
    {
        get => (IconKind)this.GetValue(IconKindProperty);
        set => this.SetValue(IconKindProperty, value);
    }

    public string IconData
    {
        get => (string)this.GetValue(IconDataProperty);
        set => this.SetValue(IconDataProperty, value);
    }

    [TypeConverter(typeof(IconSourceConverter))]
    public SKPicture IconSource
    {
        get => (SKPicture)this.GetValue(IconSourceProperty);
        set => this.SetValue(IconSourceProperty, value);
    }
    #endregion

    #region ITrailingIconElement
    public static readonly BindableProperty TrailingIconKindProperty =
        TrailingIconElement.TrailingIconKindProperty;
    public static readonly BindableProperty TrailingIconDataProperty =
        TrailingIconElement.TrailingIconDataProperty;
    public static readonly BindableProperty TrailingIconSourceProperty =
        TrailingIconElement.TrailingIconSourceProperty;
    public static readonly BindableProperty TrailingIconColorProperty =
        TrailingIconElement.TrailingIconColorProperty;

    public IconKind TrailingIconKind
    {
        get => (IconKind)this.GetValue(TrailingIconKindProperty);
        set => this.SetValue(TrailingIconKindProperty, value);
    }

    public string TrailingIconData
    {
        get => (string)this.GetValue(TrailingIconDataProperty);
        set => this.SetValue(TrailingIconDataProperty, value);
    }

    [TypeConverter(typeof(IconSourceConverter))]
    public SKPicture TrailingIconSource
    {
        get => (SKPicture)this.GetValue(TrailingIconSourceProperty);
        set => this.SetValue(TrailingIconSourceProperty, value);
    }
    public Color TrailingIconColor
    {
        get => (Color)this.GetValue(TrailingIconColorProperty);
        set => this.SetValue(TrailingIconColorProperty, value);
    }

    public event EventHandler<SKTouchEventArgs> TrailingIconClicked;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public SKRect TrailingIconBounds { get; set; } = new();
    #endregion

    #region ILabelTextElement
    public static readonly BindableProperty LabelTextProperty = LabelTextElement.LabelTextProperty;
    public static readonly BindableProperty LabelTextColorProperty =
        LabelTextElement.LabelTextColorProperty;
    public static readonly BindableProperty LabelTextOpacityProperty =
        LabelTextElement.LabelTextOpacityProperty;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public TextBlock InternalLabelText { get; set; } = new();

    [EditorBrowsable(EditorBrowsableState.Never)]
    public TextStyle LabelTextStyle { get; set; } = FontMapper.DefaultStyle.Modify(fontSize: 12f);

    public string LabelText
    {
        get => (string)this.GetValue(LabelTextProperty);
        set => this.SetValue(LabelTextProperty, value);
    }
    public Color LabelTextColor
    {
        get => (Color)this.GetValue(LabelTextColorProperty);
        set => this.SetValue(LabelTextColorProperty, value);
    }
    public float LabelTextOpacity
    {
        get => (float)this.GetValue(LabelTextOpacityProperty);
        set => this.SetValue(LabelTextOpacityProperty, value);
    }
    #endregion

    #region ISupportingTextElement
    public static readonly BindableProperty SupportingTextProperty =
        SupportingTextElement.SupportingTextProperty;
    public static readonly BindableProperty SupportingTextColorProperty =
        SupportingTextElement.SupportingTextColorProperty;
    public static readonly BindableProperty SupportingTextOpacityProperty =
        SupportingTextElement.SupportingTextOpacityProperty;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public TextBlock InternalSupportingText { get; set; } = new();

    [EditorBrowsable(EditorBrowsableState.Never)]
    public TextStyle SupportingTextStyle { get; set; } =
        FontMapper.DefaultStyle.Modify(fontSize: 12f);

    public string SupportingText
    {
        get => (string)this.GetValue(SupportingTextProperty);
        set => this.SetValue(SupportingTextProperty, value);
    }
    public Color SupportingTextColor
    {
        get => (Color)this.GetValue(SupportingTextColorProperty);
        set => this.SetValue(SupportingTextColorProperty, value);
    }
    public float SupportingTextOpacity
    {
        get => (float)this.GetValue(SupportingTextOpacityProperty);
        set => this.SetValue(SupportingTextOpacityProperty, value);
    }
    #endregion

    #region IOutlineElement
    public static readonly BindableProperty OutlineColorProperty =
        OutlineElement.OutlineColorProperty;
    public static readonly BindableProperty OutlineWidthProperty =
        OutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineOpacityProperty =
        OutlineElement.OutlineOpacityProperty;
    public Color OutlineColor
    {
        get => (Color)this.GetValue(OutlineColorProperty);
        set => this.SetValue(OutlineColorProperty, value);
    }
    public int OutlineWidth
    {
        get => (int)this.GetValue(OutlineWidthProperty);
        set => this.SetValue(OutlineWidthProperty, value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float OutlineOpacity
    {
        get => (float)this.GetValue(OutlineOpacityProperty);
        set => this.SetValue(OutlineOpacityProperty, value);
    }
    #endregion

    #region IShapeElement
    public static readonly BindableProperty ShapeProperty = ShapeElement.ShapeProperty;
    public Shape Shape
    {
        get => (Shape)this.GetValue(ShapeProperty);
        set => this.SetValue(ShapeProperty, value);
    }
    #endregion

    #region IStateLayerElement
    public static readonly BindableProperty StateLayerColorProperty =
        StateLayerElement.StateLayerColorProperty;
    public static readonly BindableProperty StateLayerOpacityProperty =
        StateLayerElement.StateLayerOpacityProperty;
    public Color StateLayerColor
    {
        get => (Color)this.GetValue(StateLayerColorProperty);
        set => this.SetValue(StateLayerColorProperty, value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float StateLayerOpacity
    {
        get => (float)this.GetValue(StateLayerOpacityProperty);
        set => this.SetValue(StateLayerOpacityProperty, value);
    }
    #endregion
    #endregion

    [AutoBindable(OnChanged = nameof(OnIsErrorChanged))]
    private readonly bool isError;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly bool isOutline;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly int activeIndicatorHeight;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly Color activeIndicatorColor;

    [AutoBindable(DefaultValue = "1f", OnChanged = nameof(OnPropertyChanged))]
    private readonly float activeIndicatorOpacity;

    [AutoBindable(OnChanged = nameof(OnInternalFocusChanged))]
    private readonly bool internalFocus;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly int caretPosition;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly Color caretColor;

    [AutoBindable(
        DefaultValue = "new Topten.RichTextKit.TextRange(0)",
        OnChanged = nameof(OnPropertyChanged)
    )]
    private readonly TextRange selectionTextRange;

    private void OnIsErrorChanged()
    {
        this.ChangeVisualState();
    }

    private void OnInternalFocusChanged()
    {
        if (this.TextDocument.Length <= 1)
        {
            this.LabelTextAnimationPercent = 0f;
            this.OnPropertyChanged();
            this.StartLabelTextAnimation();
        }
        else
            this.OnPropertyChanged();

        if (this.InternalFocus)
        {
            this.isDrawCaret = true;
            _ = Task.Run(() =>
            {
                while (this.InternalFocus)
                {
                    Thread.Sleep(500);
                    this.isDrawCaret = !this.isDrawCaret;
                    this.OnPropertyChanged();
                }
            });
        }
        else
        {
            this.CloseIME();
        }
    }

    internal float LabelTextAnimationPercent { get; private set; } = 1f;

    private readonly TextFieldDrawable drawable;
    private IAnimationManager animationManager;

    private bool isDrawCaret;
    private int movingCursor;
    private TextRange rangeCache;

    public TextField()
    {
        this.TextDocument.DefaultStyle = this.TextStyle;
        this.TextDocument.PlainTextMode = true;
        this.Pressed += this.OnPressed;
        this.Clicked += this.OnClicked;
        this.Moved += this.OnMoved;

#if ANDROID || IOS
        this.LongPressed += this.OnLongPressed;
        this.Released += this.OnReleased;
#endif

        this.drawable = new TextFieldDrawable(this);
    }

    private void OnPressed(object sender, SKTouchEventArgs e)
    {
#if ANDROID || IOS
        if (this.SelectionTextRange.IsRange)
        {
            var leftCaretInfo = this.TextDocument.GetCaretInfo(
                new CaretPosition(this.SelectionTextRange.Start, false)
            );
            var rightCaretInfo = this.TextDocument.GetCaretInfo(
                new CaretPosition(this.SelectionTextRange.End, false)
            );

            if (
                e.Location.X > leftCaretInfo.CaretRectangle.Left - 36
                && e.Location.X < leftCaretInfo.CaretRectangle.Left + 12
                && e.Location.Y > leftCaretInfo.CaretRectangle.Bottom - 12
                && e.Location.Y < leftCaretInfo.CaretRectangle.Bottom + 36
            )
            {
                this.rangeCache = this.SelectionTextRange;
                this.movingCursor = 1;
                return;
            }
            else if (
                e.Location.X > rightCaretInfo.CaretRectangle.Right - 12
                && e.Location.X < rightCaretInfo.CaretRectangle.Right + 36
                && e.Location.Y > rightCaretInfo.CaretRectangle.Bottom - 12
                && e.Location.Y < rightCaretInfo.CaretRectangle.Bottom + 36
            )
            {
                this.rangeCache = this.SelectionTextRange;
                this.movingCursor = 2;
                return;
            }
        }
#endif

        if (
            e.Location.X > this.TextDocument.MarginLeft
            && e.Location.X < this.CanvasSize.Width - this.TextDocument.MarginRight
            && e.Location.Y > this.TextDocument.MarginTop / 2
            && e.Location.Y < this.CanvasSize.Height - this.TextDocument.MarginBottom / 2
        )
        {
            this.OpenIME();
            this.InternalFocus = true;
            this.isDrawCaret = true;
            var htr = this.TextDocument.HitTest(e.Location.X, e.Location.Y);
            this.CaretPosition = htr.CaretPosition.CodePointIndex;
        }
    }

    private void OnClicked(object sender, SKTouchEventArgs e)
    {
        if (this.TrailingIconBounds.Contains(e.Location))
        {
            this.TrailingIconClicked?.Invoke(sender, e);
        }
    }

    private void OnLongPressed(object sender, SKTouchEventArgs e)
    {
        if (
            e.Location.X > this.TextDocument.MarginLeft
            && e.Location.X < this.CanvasSize.Width - this.TextDocument.MarginRight
            && e.Location.Y > this.TextDocument.MarginTop / 2
            && e.Location.Y < this.CanvasSize.Height - this.TextDocument.MarginBottom / 2
        )
        {
            var htr = this.TextDocument.HitTest(e.Location.X, e.Location.Y);
            this.SelectionTextRange = new TextRange(
                Math.Max(0, htr.CaretPosition.CodePointIndex - 2),
                Math.Min(htr.CaretPosition.CodePointIndex + 2, this.TextDocument.Length - 1)
            );
        }
    }

    private void OnMoved(object sender, SKTouchEventArgs e)
    {
#if ANDROID || IOS
        if (this.movingCursor == 1)
        {
            var htr = this.TextDocument.HitTest(e.Location.X, e.Location.Y);
            this.SelectionTextRange = new TextRange(
                htr.CaretPosition.CodePointIndex,
                this.rangeCache.End
            ).Normalized;
        }
        else if (this.movingCursor == 2)
        {
            var htr = this.TextDocument.HitTest(e.Location.X, e.Location.Y);
            this.SelectionTextRange = new TextRange(
                this.rangeCache.Start,
                htr.CaretPosition.CodePointIndex
            ).Normalized;
        }
#endif

#if WINDOWS
        if (this.isPressing)
        {
            var position = Math.Min(
                this.TextDocument.HitTest(e.Location.X, e.Location.Y).ClosestCodePointIndex,
                this.TextDocument.Length - 1
            );
            if (position != this.CaretPosition && position > this.CaretPosition)
                this.SelectionTextRange = new TextRange(this.CaretPosition, position);
            else
                this.SelectionTextRange = new TextRange(position, this.CaretPosition);
        }

        if (
            e.Location.X > this.TextDocument.MarginLeft
            && e.Location.X < this.CanvasSize.Width - this.TextDocument.MarginRight
            && e.Location.Y > this.TextDocument.MarginTop / 2
            && e.Location.Y < this.CanvasSize.Height - this.TextDocument.MarginBottom / 2
        )
            this.SetCursor(isArrow: false);
        else
            this.SetCursor(isArrow: true);
#endif
    }

    private void OnReleased(object sender, SKTouchEventArgs e)
    {
        this.movingCursor = 0;
    }

    public void StartLabelTextAnimation()
    {
        this.animationManager ??=
            this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        var start = 0f;
        var end = 1f;

        this.animationManager?.Add(
            new Microsoft.Maui.Animations.Animation(
                callback: (progress) =>
                {
                    this.LabelTextAnimationPercent = start.Lerp(end, progress);
                    this.InvalidateSurface();
                },
                duration: 0.25f,
                easing: Easing.SinInOut
            )
        );
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        var scale = this.FontSize / 16f;
        var bounds = new SKRect(
            e.Info.Rect.Left,
            e.Info.Rect.Top + 8 * scale,
            e.Info.Rect.Right,
            e.Info.Rect.Bottom - this.InternalSupportingText.MeasuredHeight - 4f * scale
        );
        this.drawable.Draw(e.Surface.Canvas, bounds);
        if (this.InternalFocus && this.isDrawCaret)
        {
            this.drawable.DrawCaret(e.Surface.Canvas);
        }
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
        var scale = this.FontSize / 16f;

        var leftWidth =
            ((!string.IsNullOrEmpty(this.IconData) || this.IconSource != null ? 24f + 12f : 0f) + 16f)
            * scale;
        var rightWidth =
            (
                (
                    !string.IsNullOrEmpty(this.TrailingIconData) || this.TrailingIconSource != null
                        ? 24f + 12f
                        : 0f
                ) + 16f
            ) * scale;

        this.TextDocument.SetMargins(
            leftWidth,
            this.IsOutline
                ? (16f + 10f) * scale
                : this.InternalLabelText.MeasuredHeight + (8f + 8f + 2f) * scale,
            rightWidth,
            this.InternalSupportingText.MeasuredHeight
                + (this.IsOutline ? (12f + 8f) * scale : (8f + 4f) * scale)
        );
        ;

        this.TextDocument.PageWidth = (float)(maxWidth);
        var width =
            this.HorizontalOptions.Alignment == LayoutAlignment.Fill
                ? maxWidth
                : this.Margin.HorizontalThickness
                    + Math.Max(
                        this.MinimumWidthRequest,
                        this.WidthRequest == -1
                            ? Math.Min(
                                maxWidth,
                                Math.Max(
                                    Math.Max(
                                        this.InternalLabelText.MeasuredWidth,
                                        this.InternalSupportingText.MeasuredWidth
                                    )
                                        + leftWidth
                                        + rightWidth,
                                    this.TextDocument.MeasuredWidth
                                )
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
                            ? Math.Min(maxHeight, this.TextDocument.MeasuredHeight)
                            : this.HeightRequest
                    );
        this.DesiredSize = new Size(Math.Ceiling(width), Math.Ceiling(height));
        return this.DesiredSize;
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == "IsEnabled")
        {
            this.ControlState = this.IsEnabled ? ControlState.Normal : ControlState.Disabled;
        }
    }
}
