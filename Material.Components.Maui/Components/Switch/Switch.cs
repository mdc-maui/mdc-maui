using Material.Components.Maui.Core;
using Microsoft.Maui.Animations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Material.Components.Maui;

public partial class Switch : SKTouchCanvasView, IView, IOutlineElement, IShapeElement, IStateLayerElement,
    IRippleElement
{
    #region interface

    #region IView

    private ControlState controlState = ControlState.Normal;

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
        var state = this.ControlState switch
        {
            ControlState.Normal => this.IsChecked ? "normal:actived" : "normal",
            ControlState.Hovered => this.IsChecked ? "hovered:actived" : "hovered",
            ControlState.Pressed => this.IsChecked ? "pressed:actived" : "pressed",
            ControlState.Disabled => this.IsChecked ? "disabled:actived" : "disabled",
            _ => "normal",
        };
        VisualStateManager.GoToState(this, state);
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

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float RippleSize { get; private set; } = 0f;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float RipplePercent { get; set; } = 0f;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public SKPoint TouchPoint { get; set; } = new SKPoint(-1, -1);

    #endregion

    #endregion

    [AutoBindable(OnChanged = nameof(OnCheckedChanged))]
    private readonly bool isChecked;

    [AutoBindable] 
    private readonly Color trackColor;

    [AutoBindable(DefaultValue = "1f")]
    private readonly float trackOpacity;

    [AutoBindable]
    private readonly Color thumbColor;

    [AutoBindable(DefaultValue = "1f")]
    private readonly float thumbOpacity;

    [AutoBindable(DefaultValue = "true")]
    private readonly bool isIconVisible;

    [AutoBindable]
    private readonly Color iconColor;

    [AutoBindable(DefaultValue = "1f")]
    private readonly float iconOpacity;

    private void OnCheckedChanged()
    {
        this.ChangeVisualState();
        this.StartChangingEffect();
        CheckedChanged?.Invoke(this, new CheckedChangedEventArgs(this.IsChecked));
        this.Command?.Execute(this.CommandParameter ?? this.IsChecked);
    }

    [AutoBindable] private readonly ICommand command;

    [AutoBindable] private readonly object commandParameter;

    public event EventHandler<CheckedChangedEventArgs> CheckedChanged;

    internal float ChangingPercent { get; private set; } = 1f;

    private IAnimationManager animationManager;
    private readonly SwitchDrawable drawable;

    public Switch()
    {
        this.Clicked += (sender, e) => this.IsChecked = !this.IsChecked;
        this.drawable = new SwitchDrawable(this);
    }

    private void StartChangingEffect()
    {
        if (this.Handler is null) return;
        this.animationManager ??= this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        this.ChangingPercent = 0f;
        var start = 0f;
        var end = 1f;
        this.animationManager?.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
            {
                this.ChangingPercent = start.Lerp(end, progress);
                this.InvalidateSurface();
            },
            duration: 0.75f,
            easing: Easing.SinInOut));
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
                    this.RipplePercent = 0;
                    this.InvalidateSurface();
                }
            }));
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        var bounds = new SKRect(e.Info.Rect.Left + 8, e.Info.Rect.Top + 8, e.Info.Rect.Right - 8,
            e.Info.Rect.Bottom - 8);
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