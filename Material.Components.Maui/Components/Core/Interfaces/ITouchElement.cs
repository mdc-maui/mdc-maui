namespace Material.Components.Maui.Core.Interfaces;

public interface ITouchElement : ICommandElement
{
    event EventHandler<SKTouchEventArgs> Pressed;
    event EventHandler<SKTouchEventArgs> Moved;
    event EventHandler<SKTouchEventArgs> Released;
    event EventHandler<SKTouchEventArgs> LongPressed;
    event EventHandler<SKTouchEventArgs> Clicked;
    event EventHandler<SKTouchEventArgs> Entered;
    event EventHandler<SKTouchEventArgs> Exited;
}
