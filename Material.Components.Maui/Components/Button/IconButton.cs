using Material.Components.Maui.Core.Button;
using Material.Components.Maui.Core;
using Material.Components.Maui.Converters;
using Microsoft.Maui.Animations;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using IButton = Material.Components.Maui.Core.Button.IButton;

namespace Material.Components.Maui;
public partial class IconButton : SKCanvasView, IButton
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

    #region IImageElement

    public static readonly BindableProperty IconProperty = ImageElement.IconProperty;
    public static readonly BindableProperty ImageProperty = ImageElement.ImageProperty;
    public IconKind Icon
    {
        get => (IconKind)this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    [System.ComponentModel.TypeConverter(typeof(ImageConverter))]
    public SKPicture Image
    {
        get => (SKPicture)this.GetValue(ImageProperty);
        set => this.SetValue(ImageProperty, value);
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

    #region IBackgroundElement

    public static readonly BindableProperty BackgroundColourProperty = BackgroundElement.BackgroundColourProperty;
    public static readonly BindableProperty BackgroundOpacityProperty = BackgroundElement.BackgroundOpacityProperty;
    public Color BackgroundColour
    {
        get => (Color)this.GetValue(BackgroundColourProperty);
        set => this.SetValue(BackgroundColourProperty, value);
    }
    public float BackgroundOpacity
    {
        get => (float)this.GetValue(BackgroundOpacityProperty);
        set => this.SetValue(BackgroundOpacityProperty, value);
    }

    #endregion

    #region IOutlineElement

    public static readonly BindableProperty OutlineColorProperty = OutlineElement.OutlineColorProperty;
    public static readonly BindableProperty OutlineWidthProperty = OutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineOpacityProperty = OutlineElement.OutlineOpacityProperty;
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
    public float OutlineOpacity
    {
        get => (float)this.GetValue(OutlineOpacityProperty);
        set => this.SetValue(OutlineOpacityProperty, value);
    }

    #endregion

    #region IElevationElement

    public static readonly BindableProperty ElevationProperty = ElevationElement.ElevationProperty;
    public Elevation Elevation
    {
        get => (Elevation)this.GetValue(ElevationProperty);
        set => this.SetValue(ElevationProperty, value);
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

    #region ITouchElement

    public static readonly BindableProperty CommandProperty = TouchElement.CommandProperty;
    public static readonly BindableProperty CommandParameterProperty = TouchElement.CommandParameterProperty;
    public ICommand Command
    {
        get => (ICommand)this.GetValue(CommandProperty);
        set => this.SetValue(CommandProperty, value);
    }
    public object CommandParameter
    {
        get => this.GetValue(CommandParameterProperty);
        set => this.SetValue(CommandParameterProperty, value);
    }
    public Timer PressedTimer { get; set; }
    public event EventHandler<SKTouchEventArgs> Pressed;
    public event EventHandler<SKTouchEventArgs> LongPressed;
    public event EventHandler<SKTouchEventArgs> Clicked;
    public void OnPressed(SKTouchEventArgs e) => Pressed?.Invoke(this, e);
    public void OnLongPressed(SKTouchEventArgs e) => LongPressed?.Invoke(this, e);
    public void OnClicked(SKTouchEventArgs e) => Clicked?.Invoke(this, e);

    #endregion
    #endregion

    private readonly IconButtonDrawable drawable;
    private IAnimationManager animationManager;

    public IconButton()
    {
        this.WidthRequest = 40d;
        this.HeightRequest = 40d;
        this.drawable = new IconButtonDrawable(this);
    }

    protected override void OnTouch(SKTouchEventArgs e)
    {
        if (e.ActionType == SKTouchAction.Pressed)
        {
            this.ControlState = ControlState.Pressed;
            this.TouchPoint = e.Location;
            this.StartRippleEffect();

            this.Pressed?.Invoke(this, e);
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

            this.Clicked?.Invoke(this, e);
            this.Command?.Execute(this.CommandParameter);
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

    private void StartRippleEffect()
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
                this.RipplePercent = 0;
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
