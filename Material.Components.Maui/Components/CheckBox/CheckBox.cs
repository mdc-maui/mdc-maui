using Microsoft.Maui.Animations;
using System.ComponentModel;
using System.Windows.Input;

namespace Material.Components.Maui;

public partial class CheckBox
    : SKTouchCanvasView,
        IView,
        IOutlineElement,
        IForegroundElement,
        IRippleElement
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
            ControlState.Normal => "normal",
            ControlState.Hovered => "hovered",
            ControlState.Pressed => "pressed",
            ControlState.Disabled => "disabled",
            _ => "normal",
        };
        VisualStateManager.GoToState(this, state);
        this.isVisualStateChanging = false;

        if (!this.IsFocused)
            this.InvalidateSurface();
    }

    public void OnPropertyChanged()
    {
        if (this.Handler != null && !this.isVisualStateChanging)
        {
            this.InvalidateSurface();
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

    internal float ChangingPercent { get; private set; } = 0f;

    private readonly CheckBoxDrawable drawable;
    private IAnimationManager animationManager;

    public CheckBox()
    {
        this.Clicked += (sender, e) => this.IsChecked = !this.IsChecked;
        this.drawable = new CheckBoxDrawable(this);
    }

    private void StartChangingEffect()
    {
        if (this.Handler is null)
        {
            this.ChangingPercent = this.IsChecked ? 1f : 0f;
            return;
        }
        this.animationManager ??=
            this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        var start = 0f;
        var end = this.IsChecked ? 1f : 0.9f;

        this.animationManager?.Add(
            new Microsoft.Maui.Animations.Animation(
                callback: (progress) =>
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
                }
            )
        );
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void StartRippleEffect()
    {
        this.animationManager ??=
            this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        var start = 0f;
        var end = 1f;

        this.animationManager?.Add(
            new Microsoft.Maui.Animations.Animation(
                callback: (progress) =>
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
                }
            )
        );
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        this.drawable.Draw(e.Surface.Canvas, e.Info.Rect);
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
        var width =
            this.HorizontalOptions.Alignment is LayoutAlignment.Fill
                ? maxWidth
                : this.Margin.HorizontalThickness
                    + Math.Max(
                        this.MinimumWidthRequest,
                        this.WidthRequest is -1 ? Math.Min(maxWidth, 40d) : this.WidthRequest
                    );
        var height =
            this.VerticalOptions.Alignment is LayoutAlignment.Fill
                ? maxHeight
                : this.Margin.VerticalThickness
                    + Math.Max(
                        this.MinimumHeightRequest,
                        this.HeightRequest is -1 ? Math.Min(maxHeight, 40d) : this.HeightRequest
                    );
        var result = new Size(width, height);
        this.DesiredSize = result;
        return result;
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
