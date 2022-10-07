using Material.Components.Maui.Core.ComboBox;
using Material.Components.Maui.Core;
using Microsoft.Maui.Animations;
using System.Runtime.CompilerServices;
using Topten.RichTextKit;

namespace Material.Components.Maui;
public partial class ComboBoxItem : SKCanvasView, IView, ITextElement, IRippleElement
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

    public event EventHandler<SKTouchEventArgs> Clicked;

    private readonly ComboBoxItemDrawable drawable;
    private IAnimationManager animationManager;

    public ComboBoxItem()
    {
        this.HeightRequest = 48d;
        this.drawable = new ComboBoxItemDrawable(this);
    }

    protected override void OnTouch(SKTouchEventArgs e)
    {
        if (e.ActionType == SKTouchAction.Pressed)
        {
            this.ControlState = ControlState.Pressed;
            this.TouchPoint = e.Location;
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
            if (this.RipplePercent == 1f)
            {
                this.RipplePercent = 0f;
            }
            this.InvalidateSurface();
            Clicked?.Invoke(this, e);
            e.Handled = true;
        }
        else if (e.ActionType == SKTouchAction.Entered)
        {
            this.ControlState = ControlState.Hovered;
            this.InvalidateSurface();
            e.Handled = true;
        }
        else if (e.ActionType == SKTouchAction.Cancelled || e.ActionType == SKTouchAction.Exited)
        {
            this.ControlState = ControlState.Normal;
            this.InvalidateSurface();
            e.Handled = true;
        }
    }

    private void StartRippleEffect()
    {
        this.animationManager ??= this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        var start = 0f;
        var end = 1f;

        this.animationManager.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
        {
            if (this.ControlState == ControlState.Pressed)
            {
                this.RipplePercent = start.Lerp(end, progress);
                this.InvalidateSurface();
            }
        },
        duration: 0.15f,
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
        this.RippleSize = e.Info.Rect.GetRippleSize(this.TouchPoint);
        this.drawable.Draw(e.Surface.Canvas, e.Info.Rect);
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
