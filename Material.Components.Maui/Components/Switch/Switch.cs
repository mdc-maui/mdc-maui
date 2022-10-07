using Material.Components.Maui.Core;
using Material.Components.Maui.Core.Switch;
using Microsoft.Maui.Animations;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Material.Components.Maui;
public partial class Switch : SKCanvasView, IView, IOutlineElement, IShapeElement, IStateLayerElement, IRippleElement
{
    #region interface

    #region IView

    private ControlState controlState = ControlState.Normal;
    public ControlState ControlState
    {
        get => this.controlState;
        private set
        {
            var state = value switch
            {
                ControlState.Normal => this.IsSelected ? "normal:actived" : "normal",
                ControlState.Hovered => this.IsSelected ? "hovered:actived" : "hovered",
                ControlState.Pressed => this.IsSelected ? "pressed:actived" : "pressed",
                ControlState.Disabled => this.IsSelected ? "disabled:actived" : "disabled",
                _ => "normal",
            };
            VisualStateManager.GoToState(this, state);
            this.controlState = value;
        }
    }
    public void OnPropertyChanged()
    {
        this.InvalidateSurface();
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

    #endregion


    [AutoBindable(OnChanged = nameof(OnSelectChanged))]
    private readonly bool isSelected;

    [AutoBindable]
    private readonly Color trackColor;

    [AutoBindable(DefaultValue = "1f")]
    private readonly float trackOpacity;

    [AutoBindable]
    private readonly Color thumbColor;

    [AutoBindable(DefaultValue = "1f")]
    private readonly float thumbOpacity;

    [AutoBindable]
    private readonly Color iconColor;

    [AutoBindable(DefaultValue = "1f")]
    private readonly float iconOpacity;

    private void OnSelectChanged()
    {
        var state = this.ControlState switch
        {
            ControlState.Normal => this.IsSelected ? "normal:actived" : "normal",
            ControlState.Hovered => this.IsSelected ? "hovered:actived" : "hovered",
            ControlState.Pressed => this.IsSelected ? "pressed:actived" : "pressed",
            ControlState.Disabled => this.IsSelected ? "disabled:actived" : "disabled",
            _ => "normal",
        };
        VisualStateManager.GoToState(this, state);
    }

    [AutoBindable]
    private readonly ICommand command;

    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<bool> SelectedChanged;



    internal float ChangingPercent { get; private set; } = 1f;

    private IAnimationManager animationManager;
    private readonly SwitchDrawable drawable;

    public Switch()
    {
        this.WidthRequest = 68;
        this.HeightRequest = 48;
        this.drawable = new SwitchDrawable(this);
    }

    protected override void OnTouch(SKTouchEventArgs e)
    {
        if (this.ControlState != ControlState.Disabled)
        {
            if (e.ActionType == SKTouchAction.Pressed)
            {
                this.ControlState = ControlState.Pressed;
                this.StartRippleEffect();
            }
            else if (e.ActionType == SKTouchAction.Released)
            {
                this.IsSelected = !this.IsSelected;
#if WINDOWS || MACCATALYST
                this.ControlState = ControlState.Hovered;
#else
                this.ControlState = ControlState.Normal;
#endif
                this.RipplePercent = 0f;
                this.StartChangingEffect();
                SelectedChanged?.Invoke(this, this.IsSelected);
                this.Command?.Execute(this.CommandParameter);
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
            e.Handled = true;
        }
    }

    private void StartChangingEffect()
    {
        this.animationManager ??= this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();

        var start = 0f;
        var end = 1f;

        this.animationManager?.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
        {
            this.ChangingPercent = start.Lerp(end, progress);
            this.InvalidateSurface();
        },
        duration: 0.25f,
        easing: Easing.SinInOut));
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
        var bounds = new SKRect(e.Info.Rect.Left + 8, e.Info.Rect.Top + 8, e.Info.Rect.Right - 8, e.Info.Rect.Bottom - 8);
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
