using Material.Components.Maui.Core.CheckBox;
using Material.Components.Maui.Core;
using Microsoft.Maui.Animations;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Topten.RichTextKit;

namespace Material.Components.Maui;
public partial class CheckBox : SKCanvasView, IView, ITextElement, IForegroundElement, IRippleElement
{
    #region interface

    #region IView

    private ControlState controlState = ControlState.Normal;
    public ControlState ControlState
    {
        get => this.controlState;
        private set
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
        var width = 52d + this.Margin.HorizontalThickness;
        var height = 48d + this.Margin.VerticalThickness;
        if (!string.IsNullOrEmpty(this.Text))
            width = this.TextBlock.MeasuredWidth + 66d;
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
    public float RippleSize { get; private set; } = 0f;
    public float RipplePercent { get; private set; } = 0f;
    public SKPoint TouchPoint { get; private set; } = new SKPoint(-1, -1);

    #endregion

    #endregion

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly bool isChecked;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly Color onColor;

    [AutoBindable(DefaultValue = "1f", OnChanged = nameof(OnPropertyChanged))]
    private readonly float onOpacity;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly Color markColor;

    [AutoBindable(DefaultValue = "1f", OnChanged = nameof(OnPropertyChanged))]
    private readonly float markOpacity;

    [AutoBindable]
    private readonly ICommand command;

    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<CheckedChangedEventArgs> CheckedChanged;

    public float ChangingPercent { get; private set; } = 1f;

    private readonly CheckBoxDrawable drawable;
    private IAnimationManager animationManager;

    public CheckBox()
    {
        this.MinimumWidthRequest = 52;
        this.MinimumHeightRequest = 48;
        this.drawable = new CheckBoxDrawable(this);
    }

    protected override void OnTouch(SKTouchEventArgs e)
    {
        if (this.ControlState == ControlState.Disabled) return;

        if (e.ActionType == SKTouchAction.Pressed)
        {
            this.ControlState = ControlState.Pressed;
            this.StartRippleEffect();
            e.Handled = true;
        }
        else if (e.ActionType == SKTouchAction.Released)
        {
#if WINDOWS || MACCATALYST
            this.ControlState = ControlState.Hovered;
#else
            this.ControlState = ControlState.Normal;
#endif

            this.IsChecked = !this.IsChecked;
            this.StartChangingEffect();
            if (this.RipplePercent == 1f)
            {
                this.RipplePercent = 0f;
            }

            this.InvalidateSurface();

            this.Command?.Execute(this.CommandParameter);
            this.CheckedChanged?.Invoke(this, new CheckedChangedEventArgs(this.IsChecked));
            e.Handled = true;
        }
        else if (e.ActionType == SKTouchAction.Entered)
        {
            this.ControlState = ControlState.Hovered;
            this.InvalidateSurface();
        }
        else if (e.ActionType == SKTouchAction.Cancelled || e.ActionType == SKTouchAction.Exited)
        {
            this.ControlState = ControlState.Normal;
            this.InvalidateSurface();
        }
    }

    private void StartChangingEffect()
    {
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

    private void StartRippleEffect()
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
                this.RipplePercent = 0;
                this.InvalidateSurface();
            }
        }));
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        var bounds = new SKRect(e.Info.Rect.Left + 16, e.Info.Rect.Top + 15, e.Info.Rect.Right - 16, e.Info.Rect.Bottom - 15);
        this.drawable.Draw(e.Surface.Canvas, bounds);
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == "IsEnabled")
        {
            this.ControlState = this.IsEnabled ? ControlState.Normal : ControlState.Disabled;
            this.InvalidateSurface();
        }
    }
}
