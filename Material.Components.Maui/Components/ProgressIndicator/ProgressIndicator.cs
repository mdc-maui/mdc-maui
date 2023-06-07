using Microsoft.Maui.Animations;
using System.ComponentModel;
using System.Windows.Input;

namespace Material.Components.Maui;

public partial class ProgressIndicator : SKCanvasView, IBackgroundElement, IView
{
    #region IView
    private bool isVisualStateChanging;
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
        this.isVisualStateChanging = true;
        var state = this.ControlState switch
        {
            _ => "normal",
        };
        VisualStateManager.GoToState(this, state);
        this.isVisualStateChanging = false;
    }

    public void OnPropertyChanged()
    {
        if (this.Handler != null && !this.isVisualStateChanging)
        {
            this.InvalidateSurface();
        }
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

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly IndicatorType indicatorType;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly Color activeIndicatorColor;

    [AutoBindable(DefaultValue = "-1f", OnChanged = nameof(OnPercentChanged))]
    private readonly float percent;

    [AutoBindable(DefaultValue = "1.5f")]
    private readonly float animationDuration;

    [AutoBindable]
    private readonly ICommand command;

    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<ValueChangedEventArgs> PercentChanged;

    private void OnPercentChanged()
    {
        if (this.Percent == -1f)
        {
            this.AnimationIsPositive = true;
            this.StartIndeterminateAnimation();
        }
        OnPropertyChanged();
        PercentChanged?.Invoke(this, new ValueChangedEventArgs(this.Percent));
        this.Command?.Execute(this.CommandParameter ?? this.Percent);
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
        if (this.Percent != -1f || this.Handler is null)
            return;
        this.animationManager ??=
            this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();

        var animation = new Microsoft.Maui.Animations.Animation(
            callback: (progress) =>
            {
                if (!this.AnimationIsPositive)
                {
                    this.AnimationPercent = 1 - 0f.Lerp(1f, progress);
                }
                else
                {
                    this.AnimationPercent = 0f.Lerp(1f, progress);
                }
                this.InvalidateSurface();
            }
        );
        animation.Duration = this.AnimationDuration;
        animation.Easing = Easing.Linear;
        animation.Repeats = true;
        animation.Finished = () =>
        {
            if (this.Percent != -1f)
            {
                this.animationManager.Remove(animation);
            }
            else if (this.IndicatorType is IndicatorType.Circular)
            {
                this.AnimationIsPositive = !this.AnimationIsPositive;
            }
        };
        this.animationManager.Add(animation);
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        this.drawable.Draw(e.Surface.Canvas, e.Info.Rect);
    }

    protected override void OnHandlerChanged()
    {
        if (this.Percent == -1f)
        {
            this.AnimationIsPositive = true;
            this.StartIndeterminateAnimation();
        }
    }
}
