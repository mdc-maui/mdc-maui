using Material.Components.Maui.Components.Core;
using Material.Components.Maui.Core;
using Microsoft.Maui.Animations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Topten.RichTextKit;

namespace Material.Components.Maui;
public partial class RadioButtonItem : SKTouchCanvasView, IView, ITextElement, IForegroundElement, IStateLayerElement, IRippleElement
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
        var width = this.TextBlock.MeasuredWidth + 70d + this.Margin.HorizontalThickness;
        var height = 48d + this.Margin.VerticalThickness;
        this.WidthRequest = Math.Max(this.WidthRequest, width);
        this.HeightRequest = Math.Max(this.HeightRequest, height);
        this.DesiredSize = new Size(this.WidthRequest, this.HeightRequest);
        this.InvalidateSurface();
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


    [AutoBindable]
    private readonly Color onColor;

    [AutoBindable(OnChanged = nameof(OnIsSelectedChanged))]
    public bool isSelected;


    public event EventHandler<SelectedChangedEventArgs> SelectedChanged;

    private void OnIsSelectedChanged()
    {
        this.StartChangingAnimation();
        SelectedChanged?.Invoke(this, new SelectedChangedEventArgs(this.IsSelected));
        this.Command?.Execute(this.CommandParameter ?? this.IsSelected);
    }

    public float ChangingPercent { get; private set; } = 1f;
    private readonly RadioButtonItemDrawable drawable;
    private IAnimationManager animationManager;

    public RadioButtonItem()
    {
        this.drawable = new RadioButtonItemDrawable(this);
    }

    private void StartChangingAnimation()
    {
        this.animationManager ??= this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();

        var start = 0f;
        var end = 1f;

        this.animationManager?.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
        {
            this.ChangingPercent = start.Lerp(end, progress);
            this.InvalidateSurface();
        },
        duration: 0.35f,
        easing: Easing.SinInOut));
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void StartRippleEffect()
    {
        this.animationManager ??= this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();

        var start = 0f;
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
            }
        }));
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        var bounds = new SKRect(e.Info.Rect.Left + 4, e.Info.Rect.Top + 4, e.Info.Rect.Right - 4, e.Info.Rect.Bottom - 4);
        this.drawable.Draw(e.Surface.Canvas, bounds);
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
