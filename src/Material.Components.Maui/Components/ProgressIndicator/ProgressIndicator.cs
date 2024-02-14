using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Animations;

namespace Material.Components.Maui;

public class ProgressIndicator
    : GraphicsView,
        IActiveIndicatorElement,
        IBackgroundElement,
        IICommandElement,
        IElement,
        IStyleElement
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ViewState ViewState => ViewState.Normal;

    void IElement.OnPropertyChanged()
    {
        if (this.Handler != null)
            this.Invalidate();
    }

    public static readonly BindableProperty PercentProperty = BindableProperty.Create(
        nameof(Percent),
        typeof(float),
        typeof(IElement),
        -1f,
        propertyChanged: (bo, ov, nv) =>
        {
            var view = bo as ProgressIndicator;
            if (view.Percent == -1f)
            {
                view.AnimationIsPositive = true;
                view.StartIndeterminateAnimation();
            }
            else
                ((IElement)bo).OnPropertyChanged();

            view.PercentChanged?.Invoke(view, new ValueChangedEventArgs((double)ov, view.Percent));

            if (view.Command?.CanExecute(view.CommandParameter ?? view.Percent) is true)
                view.Command?.Execute(view.CommandParameter ?? view.Percent);
        }
    );

    public static readonly BindableProperty IndicatorTypeProperty = BindableProperty.Create(
        nameof(IndicatorType),
        typeof(IndicatorType),
        typeof(ProgressIndicator),
        IndicatorType.Circular,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty AnimationDurationProperty = BindableProperty.Create(
        nameof(AnimationDuration),
        typeof(float),
        typeof(ProgressIndicator),
        1.5f,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static new readonly BindableProperty IsEnabledProperty = IElement.IsEnabledProperty;

    public static readonly BindableProperty ActiveIndicatorHeightProperty =
        IActiveIndicatorElement.ActiveIndicatorHeightProperty;
    public static readonly BindableProperty ActiveIndicatorColorProperty =
        IActiveIndicatorElement.ActiveIndicatorColorProperty;

    public static new readonly BindableProperty BackgroundColorProperty =
        IBackgroundElement.BackgroundColorProperty;

    public static readonly BindableProperty ShapeProperty = IShapeElement.ShapeProperty;

    public static readonly BindableProperty CommandProperty = IICommandElement.CommandProperty;
    public static readonly BindableProperty CommandParameterProperty =
        IICommandElement.CommandParameterProperty;

    public static readonly BindableProperty DynamicStyleProperty =
        IStyleElement.DynamicStyleProperty;

    public string DynamicStyle
    {
        get => (string)this.GetValue(DynamicStyleProperty);
        set => this.SetValue(DynamicStyleProperty, value);
    }

    public float Percent
    {
        get => (float)this.GetValue(PercentProperty);
        set => this.SetValue(PercentProperty, value);
    }

    public IndicatorType IndicatorType
    {
        get => (IndicatorType)this.GetValue(IndicatorTypeProperty);
        set => this.SetValue(IndicatorTypeProperty, value);
    }

    public float AnimationDuration
    {
        get => (float)this.GetValue(AnimationDurationProperty);
        set => this.SetValue(AnimationDurationProperty, value);
    }

    public new bool IsEnabled
    {
        get => (bool)this.GetValue(ProgressIndicator.IsEnabledProperty);
        set => this.SetValue(ProgressIndicator.IsEnabledProperty, value);
    }

    public int ActiveIndicatorHeight
    {
        get => (int)this.GetValue(ActiveIndicatorHeightProperty);
        set => this.SetValue(ActiveIndicatorHeightProperty, value);
    }

    public Color ActiveIndicatorColor
    {
        get => (Color)this.GetValue(ActiveIndicatorColorProperty);
        set => this.SetValue(ActiveIndicatorColorProperty, value);
    }

    public new Color BackgroundColor
    {
        get => (Color)this.GetValue(BackgroundColorProperty);
        set => this.SetValue(BackgroundColorProperty, value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Shape Shape
    {
        get => (Shape)this.GetValue(ShapeProperty);
        set => this.SetValue(ShapeProperty, value);
    }

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

    public event EventHandler<ValueChangedEventArgs> PercentChanged;

    private IAnimationManager animationManager;
    internal float AnimationPercent { get; private set; } = 0f;
    internal bool AnimationIsPositive = true;

    public ProgressIndicator()
    {
        this.SetDynamicResource(StyleProperty, "CircularProgressIndicatorStyle");
        this.Drawable = new ProgressIndicatorDrawable(this);
    }

    private void StartIndeterminateAnimation()
    {
        if (this.Percent != -1f)
        {
            this.Invalidate();
            return;
        }

        this.animationManager ??= this.Handler
            .MauiContext
            ?.Services
            .GetRequiredService<IAnimationManager>();

        var animation = new Microsoft.Maui.Animations.Animation(
            callback: (progress) =>
            {
                this.AnimationPercent = this.AnimationIsPositive
                    ? 0f.Lerp(1f, progress)
                    : 1f - 0f.Lerp(1f, progress);

                this.Invalidate();
            }
        )
        {
            Duration = this.AnimationDuration,
            Easing = Easing.Linear,
            Repeats = true
        };
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

    protected override void OnHandlerChanged()
    {
        if (this.Percent == -1f)
        {
            this.AnimationIsPositive = true;
            this.StartIndeterminateAnimation();
        }
    }
}
