using Microsoft.Maui.Animations;
using Microsoft.Maui.Platform;
using System.ComponentModel;
using System.Windows.Input;

namespace Material.Components.Maui;

public class TouchGraphicsView
    : GraphicsView,
        IElement,
        IBackgroundElement,
        IShapeElement,
        IStateLayerElement,
        IRippleElement,
        IContextMenuElement,
        IICommandElement,
        IVisualTreeElement,
        IDisposable
{
    public event EventHandler<TouchEventArgs> Clicked;
    public event EventHandler<TouchEventArgs> Pressed;
    public event EventHandler<TouchEventArgs> Released;
    public event EventHandler<TouchEventArgs> LongPressed;

#if WINDOWS || MACCATALYST
    public event EventHandler<TouchEventArgs> RightClicked;
#endif

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
    public static readonly BindableProperty ContextMenuProperty =
        IContextMenuElement.ContextMenuProperty;

    public static readonly BindableProperty CommandProperty = IICommandElement.CommandProperty;
    public static readonly BindableProperty CommandParameterProperty =
        IICommandElement.CommandParameterProperty;

    public new bool IsEnabled
    {
        get => (bool)this.GetValue(TouchGraphicsView.IsEnabledProperty);
        set => this.SetValue(TouchGraphicsView.IsEnabledProperty, value);
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

    public ContextMenu ContextMenu
    {
        get => (ContextMenu)this.GetValue(ContextMenuProperty);
        set => this.SetValue(ContextMenuProperty, value);
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

    internal float RippleSize { get; set; }
    internal float RipplePercent { get; set; }
    internal PointF LastTouchPoint { get; set; }

    protected IAnimationManager animationManager;
    readonly IDispatcherTimer touchTimer;
    protected bool isTouching;

    public TouchGraphicsView()
    {
        this.StartInteraction += this.OnStartInteraction;
        this.EndInteraction += this.OnEndInteraction;
        this.CancelInteraction += this.OnCancelInteraction;
        this.StartHoverInteraction += this.OnStartHoverInteraction;
        this.EndHoverInteraction += this.OnEndHoverInteraction;
        this.MoveHoverInteraction += this.OnMoveHoverInteraction;

        //#if WINDOWS || MACCATALYST
#if MACCATALYST
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += (s, e) =>
        {
            if (e.Buttons == ButtonsMask.Secondary)
            {
                var location = e.GetPosition(this)!;
                var points = new PointF[1];
                if (location != null)
                    points[0] = (PointF)location;

                this.RightClicked?.Invoke(this, new TouchEventArgs(points, true));

                if (this.ContextMenu != null)
                {
                    this.isTouching = false;
                    this.ContextMenu.Show(this, this.LastTouchPoint);
                }
            }
        };
        this.GestureRecognizers.Add(tapGestureRecognizer);
#endif

        this.touchTimer = this.Dispatcher.CreateTimer();
        this.touchTimer.Interval = TimeSpan.FromMilliseconds(500);
        this.touchTimer.IsRepeating = false;
        this.touchTimer.Tick += (s, e) =>
        {
            if (this.LongPressed != null)
            {
                this.isTouching = false;
                this.LongPressed.Invoke(
                    this,
                    new TouchEventArgs(new PointF[] { this.LastTouchPoint }, true)
                );
            }

            if (this.ContextMenu != null)
            {
                this.isTouching = false;
                this.ContextMenu.Show(this, this.LastTouchPoint);
            }
        };
    }

    protected virtual void OnStartInteraction(object sender, TouchEventArgs e)
    {
        if (!this.IsEnabled)
            return;

        this.ViewState = ViewState.Pressed;
        this.LastTouchPoint = e.Touches[0];
        this.RippleSize = this.GetRippleSize();
        this.StartRippleEffect();

        this.Pressed?.Invoke(this, e);

        this.isTouching = true;
        this.touchTimer.Start();
    }

    protected virtual void OnEndInteraction(object sender, TouchEventArgs e)
    {
        if (!this.IsEnabled)
            return;

#if __MOBILE__
        this.ViewState = ViewState.Normal;
#else
        this.ViewState = e.IsInsideBounds ? ViewState.Hovered : ViewState.Normal;
#endif

        this.Released?.Invoke(this, e);
        if (this.isTouching)
            this.Clicked?.Invoke(this, e);

        this.isTouching = false;
        this.touchTimer.Stop();
    }

    protected virtual void OnCancelInteraction(object sender, EventArgs e)
    {
        if (!this.IsEnabled)
            return;

        this.ViewState = ViewState.Normal;

        this.isTouching = false;
        this.touchTimer.Stop();
    }

    protected virtual void OnStartHoverInteraction(object sender, TouchEventArgs e)
    {
        if (!this.IsEnabled)
            return;
        this.LastTouchPoint = e.Touches[0];
        this.ViewState = ViewState.Hovered;
    }

    protected virtual void OnEndHoverInteraction(object sender, EventArgs e)
    {
        if (!this.IsEnabled)
            return;
        this.ViewState = ViewState.Normal;
    }

    protected virtual void OnMoveHoverInteraction(object sender, TouchEventArgs e)
    {
        if (!this.IsEnabled)
            return;
        if (!e.IsInsideBounds) this.ViewState = ViewState.Normal;
    }

    protected virtual void StartRippleEffect()
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

    void IContextMenuElement.OnContextMenuChanged(object oldValue, object newValue)
    {
        if (oldValue is not null and ContextMenu ov)
        {
            this.OnChildRemoved(ov, 0);
            VisualDiagnostics.OnChildRemoved(this, ov, 0);
            SetInheritedBindingContext(ov, null);
        }

        if (newValue is not null and ContextMenu nv)
        {
            this.OnChildAdded(nv);
            VisualDiagnostics.OnChildAdded(this, nv);
            if (this.BindingContext != null)
            {
                SetInheritedBindingContext(nv, this.BindingContext);
            }
        }
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if (this.ContextMenu != null)
        {
            SetInheritedBindingContext(this.ContextMenu, this.BindingContext);
        }
    }

    protected virtual float GetRippleSize()
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

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        if (this.Handler != null)
        {
#if WINDOWS
            var pv = this.Handler.PlatformView as PlatformTouchGraphicsView;
            pv.RightTapped += this.OnRightTapped;
#endif
        }
    }

#if WINDOWS
    private void OnRightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
    {
        var position = e.GetPosition(sender as PlatformTouchGraphicsView);
        this.ContextMenu?.Show(this, new Point(position.X, position.Y));
    }
#endif

    protected bool disposedValue;

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
                if (this.Handler != null)
                {
#if WINDOWS
                    var pv = this.Handler.PlatformView as PlatformTouchGraphicsView;
                    pv.RightTapped -= this.OnRightTapped;
#endif
                }
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
