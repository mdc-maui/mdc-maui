using Material.Components.Maui.Core;
using Microsoft.Maui.Animations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Topten.RichTextKit;

namespace Material.Components.Maui;
public partial class CheckBox : SKTouchCanvasView, IView, ITextElement, IForegroundElement, IRippleElement
{
    #region interface
    #region IView
    private ControlState controlState = ControlState.Normal;
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ControlState ControlState
    {
        get => this.controlState;
        set
        {
            VisualStateManager.GoToState(this, value switch
            {
                ControlState.Normal => "normal",
                ControlState.Hovered => "hovered",
                ControlState.Pressed => "pressed",
                ControlState.Disabled => "disabled",
                _ => "normal",
            });
            this.controlState = value;
        }
    }
    public void OnPropertyChanged()
    {
        this.InvalidateSurface();
    }
    #endregion

    #region ITextElement
    public static readonly BindableProperty TextProperty = TextElement.TextProperty;
    public static readonly BindableProperty FontFamilyProperty = TextElement.FontFamilyProperty;
    public static readonly BindableProperty FontSizeProperty = TextElement.FontSizeProperty;
    public static readonly BindableProperty FontWeightProperty = TextElement.FontWeightProperty;
    public static readonly BindableProperty FontItalicProperty = TextElement.FontItalicProperty;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public TextBlock TextBlock { get; set; } = new();
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
    void ITextElement.OnTextBlockChanged()
    {
        this.AllocateSize(this.MeasureOverride(this.widthConstraint, this.heightConstraint));
    }
    #endregion

    #region IForegroundElement
    public static readonly BindableProperty ForegroundColorProperty = ForegroundElement.ForegroundColorProperty;
    public static readonly BindableProperty ForegroundOpacityProperty = ForegroundElement.ForegroundOpacityProperty;
    public Color ForegroundColor
    {
        get => (Color)this.GetValue(ForegroundColorProperty);
        set => this.SetValue(ForegroundColorProperty, value);
    }
    public float ForegroundOpacity
    {
        get => (float)this.GetValue(ForegroundOpacityProperty);
        set => this.SetValue(ForegroundOpacityProperty, value);
    }
    #endregion

    #region IStateLayerElement
    public static readonly BindableProperty StateLayerColorProperty = StateLayerElement.StateLayerColorProperty;
    public static readonly BindableProperty StateLayerOpacityProperty = StateLayerElement.StateLayerOpacityProperty;
    public Color StateLayerColor
    {
        get => (Color)this.GetValue(StateLayerColorProperty);
        set => this.SetValue(StateLayerColorProperty, value);
    }
    public float StateLayerOpacity
    {
        get => (float)this.GetValue(StateLayerOpacityProperty);
        set => this.SetValue(StateLayerOpacityProperty, value);
    }
    #endregion

    #region IRippleElement
    public static readonly BindableProperty RippleColorProperty = RippleElement.RippleColorProperty;
    public Color RippleColor
    {
        get => (Color)this.GetValue(RippleColorProperty);
        set => this.SetValue(RippleColorProperty, value);
    }
    [EditorBrowsable(EditorBrowsableState.Never)]
    public float RippleSize { get; private set; } = 0f;
    [EditorBrowsable(EditorBrowsableState.Never)]
    public float RipplePercent { get; set; } = 0f;
    [EditorBrowsable(EditorBrowsableState.Never)]
    public SKPoint TouchPoint { get; set; } = new SKPoint(-1, -1);
    #endregion
    #endregion

    [AutoBindable(OnChanged = nameof(OnIsCheckedChanged))]
    private readonly bool isChecked;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly Color onColor;

    [AutoBindable(DefaultValue = "1f", OnChanged = nameof(OnPropertyChanged))]
    private readonly float onOpacity;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly Color markColor;

    [AutoBindable(DefaultValue = "1f", OnChanged = nameof(OnPropertyChanged))]
    private readonly float markOpacity;

    private void OnIsCheckedChanged()
    {
        this.StartChangingEffect();
        this.CheckedChanged?.Invoke(this, new CheckedChangedEventArgs(this.IsChecked));
        this.Command?.Execute(this.CommandParameter ?? this.IsChecked);
    }

    public event EventHandler<CheckedChangedEventArgs> CheckedChanged;

    [AutoBindable]
    private readonly ICommand command;

    [AutoBindable]
    private readonly object commandParameter;

    internal float ChangingPercent { get; private set; } = 1f;

    private readonly CheckBoxDrawable drawable;
    private IAnimationManager animationManager;

    private double widthConstraint = double.PositiveInfinity;
    private double heightConstraint = double.PositiveInfinity;

    public CheckBox()
    {
        this.Clicked += (sender, e) => this.IsChecked = !this.IsChecked;
        this.drawable = new CheckBoxDrawable(this);
    }

    private void StartChangingEffect()
    {
        if (this.Handler is null) return;
        this.animationManager ??= this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        var start = 0f;
        var end = this.IsChecked ? 1f : 0.9f;

        this.animationManager?.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
        {
            this.ChangingPercent = start.Lerp(end, progress);
            this.InvalidateSurface();
        },
        duration: 0.25f,
        easing: Easing.SinInOut,
        finished: () =>
        {
            if (!this.IsChecked)
            {
                this.ChangingPercent = 0f;
            }
        }));
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void StartRippleEffect()
    {
        this.animationManager ??= this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        var start = -1f;
        var end = 1f;

        this.animationManager?.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
        {
            this.RipplePercent = start.Lerp(end, progress);
            this.InvalidateSurface();
        },
        duration: 0.35f,
        easing: Easing.SinInOut,
        finished: () =>
        {
            if (this.ControlState != ControlState.Pressed)
            {
                this.RipplePercent = 0f;
                this.InvalidateSurface();
            }
        }));
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        var bounds = new SKRect(e.Info.Rect.Left + 16, e.Info.Rect.Top + 15, e.Info.Rect.Right - 16, e.Info.Rect.Bottom - 15);
        this.drawable.Draw(e.Surface.Canvas, bounds);
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        var maxWidth = Math.Min(Math.Min(widthConstraint, this.MaximumWidthRequest), this.WidthRequest != -1 ? this.WidthRequest : double.PositiveInfinity);
        var maxHeight = Math.Min(Math.Min(heightConstraint, this.MaximumHeightRequest), this.HeightRequest != -1 ? this.HeightRequest : double.PositiveInfinity);
        this.TextBlock.MaxWidth = (float)(maxWidth - 66d);
        this.TextBlock.MaxHeight = (float)(maxHeight - this.Margin.VerticalThickness);
        var width = this.HorizontalOptions.Alignment is LayoutAlignment.Fill
            ? maxWidth
            : this.Margin.HorizontalThickness + Math.Max(this.MinimumWidthRequest, this.WidthRequest is -1
                ? Math.Min(maxWidth, string.IsNullOrEmpty(this.Text) ? 52d : this.TextBlock.MeasuredWidth + 66d)
                : this.WidthRequest);
        var height = this.VerticalOptions.Alignment is LayoutAlignment.Fill
            ? maxHeight
            : this.Margin.VerticalThickness + Math.Max(this.MinimumHeightRequest, this.HeightRequest is -1
                ? Math.Min(maxHeight, 48d)
                : this.HeightRequest);
        var result = new Size(width, height);
        this.DesiredSize = result;
        return result;
    }

    protected override Size ArrangeOverride(Rect bounds)
    {
        this.widthConstraint = bounds.Width;
        this.heightConstraint = bounds.Height;
        return base.ArrangeOverride(bounds);
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName is "IsEnabled")
        {
            this.ControlState = this.IsEnabled ? ControlState.Normal : ControlState.Disabled;
        }
    }
}
