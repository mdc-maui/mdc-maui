using Material.Components.Maui.Core;
using Microsoft.Maui.Animations;

namespace Material.Components.Maui;
public partial class ProgressIndicator : SKCanvasView, IBackgroundElement, IView
{
    #region IView
    private ControlState controlState = ControlState.Normal;
    public ControlState ControlState
    {
        get => this.controlState;
        set
        {
            VisualStateManager.GoToState(this, value switch
            {
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

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly IndicatorType indicatorType;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly Color activeIndicatorColor;

    [AutoBindable(DefaultValue = "0.5f", OnChanged = nameof(OnPropertyChanged))]
    private readonly float percent;

    [AutoBindable(OnChanged = nameof(OnIsIndeterminate))]
    private readonly bool isIndeterminate;

    [AutoBindable(DefaultValue = "1.5f")]
    private readonly float animationDuration;

    private void OnIsIndeterminate()
    {
        if (this.IsIndeterminate)
        {
            this.AnimationIsPositive = true;
        }
        this.StartIndeterminateAnimation();
    }

    private readonly ProgressIndicatorDrawable drawable;
    private IAnimationManager animationManager;
    internal float AnimationPercent { get; private set; } = 0f;
    internal bool AnimationIsPositive = true;
    public ProgressIndicator()
    {
        this.drawable = new ProgressIndicatorDrawable(this);
    }

    private void StartIndeterminateAnimation()
    {
        if (!this.IsIndeterminate || this.Handler is null) return;
        this.animationManager ??= this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        var start = this.AnimationIsPositive ? 0f : 1f;
        var end = this.AnimationIsPositive ? 1f : 0f;
        this.animationManager.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
        {
            this.AnimationPercent = start.Lerp(end, progress);
            this.InvalidateSurface();
        },
        duration: this.AnimationDuration,
        easing: Easing.Linear,
        finished: () =>
        {
            if (this.IndicatorType is IndicatorType.Circular)
            {
                if (start is 0f)
                    this.AnimationIsPositive = false;
                else
                    this.AnimationIsPositive = true;
            }
            this.StartIndeterminateAnimation();
        }));
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        this.drawable.Draw(e.Surface.Canvas, e.Info.Rect);
    }

    protected override void OnHandlerChanged()
    {
        if (this.IsIndeterminate)
        {
            this.StartIndeterminateAnimation();
        }
    }
}
