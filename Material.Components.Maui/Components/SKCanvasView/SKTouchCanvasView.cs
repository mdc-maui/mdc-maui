using System.Windows.Input;

namespace Material.Components.Maui;

public partial class SKTouchCanvasView
    : SKCanvasView,
        ITouchElement,
        IContextMenu,
        IVisualTreeElement
{
    [AutoBindable(OnChanged = nameof(OnContextMenuChanged))]
    private readonly ContextMenu contextMenu;

    private void OnContextMenuChanged(ContextMenu oldValue, ContextMenu newValue)
    {
        if (oldValue != null)
        {
            this.OnChildRemoved(oldValue, 0);
            VisualDiagnostics.OnChildRemoved(this, oldValue, 0);
            SetInheritedBindingContext(oldValue, null);
        }
        if (newValue != null)
        {
            this.OnChildAdded(newValue);
            VisualDiagnostics.OnChildAdded(this, newValue);
            if (this.BindingContext != null)
            {
                SetInheritedBindingContext(newValue, this.BindingContext);
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

    public static readonly BindableProperty CommandProperty = TouchElement.CommandProperty;
    public static readonly BindableProperty CommandParameterProperty =
        TouchElement.CommandParameterProperty;
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

    public event EventHandler<SKTouchEventArgs> Pressed;
    public event EventHandler<SKTouchEventArgs> Moved;
    public event EventHandler<SKTouchEventArgs> Released;
    public event EventHandler<SKTouchEventArgs> LongPressed;
    public event EventHandler<SKTouchEventArgs> Clicked;
    public event EventHandler<SKTouchEventArgs> Entered;
    public event EventHandler<SKTouchEventArgs> Exited;

    protected bool isPressing;
    protected SKPoint PressLocation;

    private readonly IDispatcherTimer touchTimer;
    private SKTouchEventArgs touchEventArgs;

    public SKTouchCanvasView()
    {
        this.EnableTouchEvents = true;
        this.IgnorePixelScaling = true;

        this.touchTimer = this.Dispatcher.CreateTimer();
        this.touchTimer.Interval = TimeSpan.FromMilliseconds(500);
        this.touchTimer.IsRepeating = false;
        this.touchTimer.Tick += (s, e) =>
        {
            if (this.LongPressed != null)
            {
                this.isPressing = false;
                this.LongPressed.Invoke(this, this.touchEventArgs);
            }
#if __MOBILE__
            if (this.ContextMenu != null)
            {
                this.isPressing = false;
                this.ContextMenu.Show(this);
            }
#endif
        };
    }

    protected override void OnTouch(SKTouchEventArgs e)
    {
        if (this is IView view && view.ControlState == ControlState.Disabled)
        {
            return;
        }
        this.ChangeControlState(e);
        this.ChangeRippleElement(e);
        this.touchEventArgs = e;
        if (e.ActionType == SKTouchAction.Pressed)
        {
            if (e.MouseButton != SKMouseButton.Right)
            {
                this.touchTimer.Start();
                this.Pressed?.Invoke(this, e);
            }
            this.PressLocation = e.Location;
            this.isPressing = true;
            e.Handled = true;
            return;
        }
        else if (e.ActionType == SKTouchAction.Released)
        {
            if (
                e.Location.X >= 0
                && e.Location.Y >= 0
                && e.Location.X < this.CanvasSize.Width
                && e.Location.Y < this.CanvasSize.Height
            )
            {
                if (e.MouseButton != SKMouseButton.Right)
                {
                    this.Released?.Invoke(this, e);
                    if (this.isPressing)
                    {
                        this.Clicked?.Invoke(this, e);
                    }
                    this.touchTimer?.Stop();
                }
                else
                {
                    if (this.isPressing)
                    {
                        this.ContextMenu?.Show(this);
                    }
                }
                e.Handled = true;
            }
            this.isPressing = false;
        }
        else if (e.ActionType == SKTouchAction.Moved)
        {
            this.Moved?.Invoke(this, e);
            e.Handled = true;
        }
        else if (e.ActionType == SKTouchAction.Entered)
        {
            this.Entered?.Invoke(this, e);
            e.Handled = true;
        }
        else if (e.ActionType == SKTouchAction.Exited)
        {
            //this.isPressing = false;
            this.Exited?.Invoke(this, e);
            e.Handled = true;
        }
        this.InvalidateSurface();
    }

    private void ChangeControlState(SKTouchEventArgs e)
    {
        if (this is IView view)
        {
            if (
                e.Location.X < 0
                || e.Location.Y < 0
                || e.Location.X >= this.CanvasSize.Width
                || e.Location.Y >= this.CanvasSize.Height
            )
            {
                view.ControlState = ControlState.Normal;
                return;
            }

            view.ControlState = e.ActionType switch
            {
                SKTouchAction.Pressed => ControlState.Pressed,
                SKTouchAction.Released
                    =>
#if __MOBILE__
                    ControlState.Normal,
#else
                    ControlState.Hovered,
#endif
                SKTouchAction.Entered
                    => ControlState.Hovered,
                SKTouchAction.Moved => view.ControlState,
                _ => ControlState.Normal
            };
        }
    }

    private void ChangeRippleElement(SKTouchEventArgs e)
    {
        if (this is IRippleElement element)
        {
            element.TouchPoint = e.Location;
            if (e.ActionType == SKTouchAction.Pressed)
            {
                element.StartRippleEffect();
            }
            else if (
                e.ActionType
                is SKTouchAction.Released
                    or SKTouchAction.Exited
                    or SKTouchAction.Cancelled
            )
            {
                if (element.RipplePercent == 1f)
                {
                    element.RipplePercent = 0f;
                }
            }
        }
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.ContextMenu != null
            ? new List<IVisualTreeElement> { this.ContextMenu }
            : Array.Empty<IVisualTreeElement>().ToList();

    public IVisualTreeElement GetVisualParent() => null;
}
