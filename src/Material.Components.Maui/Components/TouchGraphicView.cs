using Microsoft.Maui.Animations;
using System.ComponentModel;
using System.Windows.Input;

namespace Material.Components.Maui;

public class TouchGraphicView
    : GraphicsView,
        IElement,
        IBackgroundElement,
        IShapeElement,
        IStateLayerElement,
        IRippleElement,
        IVisualTreeElement,
        IDisposable
{
    protected bool IsVisualStateChanging { get; set; }
    ViewState viewState = ViewState.Normal;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ViewState ViewState
    {
        get => this.viewState;
        set
        {
            this.viewState = value;
            this.ChangeVisualState();
        }
    }

    void IElement.OnPropertyChanged()
    {
        if (this.Handler != null && !this.IsVisualStateChanging)
        {
            this.Invalidate();
        }
    }

    public static readonly new BindableProperty IsEnabledProperty = IElement.IsEnabledProperty;
    public static new readonly BindableProperty BackgroundColorProperty =
        IBackgroundElement.BackgroundColorProperty;
    public static readonly BindableProperty ShapeProperty = IShapeElement.ShapeProperty;
    public static readonly BindableProperty StateLayerColorProperty =
        IStateLayerElement.StateLayerColorProperty;
    public static readonly BindableProperty RippleDurationProperty =
        IRippleElement.RippleDurationProperty;
    public static readonly BindableProperty RippleEasingProperty =
        IRippleElement.RippleEasingProperty;

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(TouchGraphicView),
        default
    );

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(TouchGraphicView),
        default
    );

    public new bool IsEnabled
    {
        get => (bool)this.GetValue(TouchGraphicView.IsEnabledProperty);
        set => this.SetValue(TouchGraphicView.IsEnabledProperty, value);
    }

    public new Color BackgroundColor
    {
        get => (Color)this.GetValue(BackgroundColorProperty);
        set => this.SetValue(BackgroundColorProperty, value);
    }

    [TypeConverter(typeof(ShapeConverter))]
    public Shape Shape
    {
        get => (Shape)this.GetValue(ShapeProperty);
        set => this.SetValue(ShapeProperty, value);
    }

    public Color StateLayerColor
    {
        get => (Color)this.GetValue(StateLayerColorProperty);
        set => this.SetValue(StateLayerColorProperty, value);
    }

    public float RippleDuration
    {
        get => (float)this.GetValue(RippleDurationProperty);
        set => this.SetValue(RippleDurationProperty, value);
    }

    public Easing RippleEasing
    {
        get => (Easing)this.GetValue(RippleEasingProperty);
        set => this.SetValue(RippleEasingProperty, value);
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

    internal float RippleSize { get; private set; }
    internal float RipplePercent { get; private set; }
    internal PointF LastTouchPoint { get; set; }

    protected IAnimationManager animationManager;
    protected bool disposedValue;

    public TouchGraphicView()
    {
        this.StartInteraction += this.OnStartInteraction;
        this.EndInteraction += this.OnEndInteraction;
        this.CancelInteraction += this.OnCancelInteraction;
        this.StartHoverInteraction += this.OnStartHoverInteraction;
        this.EndHoverInteraction += this.OnEndHoverInteraction;
    }

    void OnStartInteraction(object sender, TouchEventArgs e)
    {
        if (!this.IsEnabled)
            return;
        this.ViewState = ViewState.Pressed;
        this.LastTouchPoint = e.Touches[0];
        this.RippleSize = this.GetRippleSize();
        this.StartRippleEffect();
    }

    void OnEndInteraction(object sender, TouchEventArgs e)
    {
        if (!this.IsEnabled)
            return;

#if __MOBILE__
        this.ViewState = ViewState.Normal;
#else
        if (e.IsInsideBounds)
        {
            this.ViewState = e.IsInsideBounds ? ViewState.Hovered : ViewState.Normal;
        }
#endif
    }

    void OnCancelInteraction(object sender, EventArgs e)
    {
        if (!this.IsEnabled)
            return;

        this.ViewState = ViewState.Normal;
    }

    void OnStartHoverInteraction(object sender, TouchEventArgs e)
    {
        if (!this.IsEnabled)
            return;
        this.LastTouchPoint = e.Touches[0];
        this.ViewState = ViewState.Hovered;
    }

    void OnEndHoverInteraction(object sender, EventArgs e)
    {
        if (!this.IsEnabled)
            return;
        this.ViewState = ViewState.Normal;
    }

    void StartRippleEffect()
    {
        this.animationManager ??=
            this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();

        this.animationManager?.Add(
            new Microsoft.Maui.Animations.Animation(
                callback: (progress) =>
                {
                    this.RipplePercent = 0f.Lerp(1f, progress);
                    this.Invalidate();
                },
                duration: this.RippleDuration,
                easing: this.RippleEasing
            )
        );
    }

    float GetRippleSize()
    {
        var points = new PointF[4];
        points[0].X = points[2].X = this.LastTouchPoint.X;
        points[0].Y = points[1].Y = this.LastTouchPoint.Y;
        points[1].X = points[3].X = this.LastTouchPoint.X - (float)this.Bounds.Width;
        points[2].Y = points[3].Y = this.LastTouchPoint.Y - (float)this.Bounds.Height;
        var maxSize = 0f;
        foreach (var point in points)
        {
            var size = MathF.Pow(
                MathF.Pow(point.X - this.LastTouchPoint.X, 2f)
                    + MathF.Pow(point.Y - this.LastTouchPoint.Y, 2f),
                0.5f
            );
            if (size > maxSize)
            {
                maxSize = size;
            }
        }
        return maxSize;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.StartInteraction -= this.OnStartInteraction;
                this.EndInteraction -= this.OnEndInteraction;
                this.CancelInteraction -= this.OnCancelInteraction;
                this.StartHoverInteraction -= this.OnStartHoverInteraction;
                this.EndHoverInteraction -= this.OnEndHoverInteraction;
            }
            this.disposedValue = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
