using Material.Components.Maui.Core;
using System.Windows.Input;

namespace Material.Components.Maui;

public partial class SKTouchCanvasView : SKCanvasView, ITouchElement, IContextMenu
{
    [AutoBindable]
    private readonly ContextMenu contextMenu;

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

    private readonly IDispatcherTimer touchTimer;
    private SKTouchEventArgs touchEventArgs;
    private bool isPressed;

    public SKTouchCanvasView()
    {
        this.touchTimer = this.Dispatcher.CreateTimer();
        this.touchTimer.Interval = TimeSpan.FromMilliseconds(500);
        this.touchTimer.IsRepeating = false;
        this.touchTimer.Tick += (s, e) =>
        {
            if (this.LongPressed != null)
            {
                this.isPressed = false;
                this.LongPressed.Invoke(this, this.touchEventArgs);
            }
#if __MOBILE__
            if (this.ContextMenu != null)
            {
                this.isPressed = false;
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
            this.isPressed = true;
            e.Handled = true;
            return;
        }
        else if (e.ActionType == SKTouchAction.Released)
        {
            if (e.MouseButton != SKMouseButton.Right)
            {
                this.Released?.Invoke(this, e);
                if (this.isPressed)
                {
                    this.Clicked?.Invoke(this, e);
                }
                this.touchTimer?.Stop();
            }
            else
            {
                if (this.isPressed)
                {
                    this.ContextMenu?.Show(this);
                }
            }
            this.isPressed = false;
            e.Handled = true;
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
            this.Exited?.Invoke(this, e);
            e.Handled = true;
        }
        this.InvalidateSurface();
    }

    private void ChangeControlState(SKTouchEventArgs e)
    {
        if (this is IView view)
        {
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
            if (e.ActionType == SKTouchAction.Pressed)
            {
                element.TouchPoint = e.Location;
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
}
