using System.Windows.Input;

namespace Material.Components.Maui.Core.Interfaces;

public interface ITouchElement
{
    Timer PressedTimer { get; set; }
    ICommand Command { get; set; }
    object CommandParameter { get; set; }

    event EventHandler<SKTouchEventArgs> Pressed;
    event EventHandler<SKTouchEventArgs> LongPressed;
    event EventHandler<SKTouchEventArgs> Clicked;

    public void OnPressed(SKTouchEventArgs e);
    public void OnLongPressed(SKTouchEventArgs e);
    public void OnClicked(SKTouchEventArgs e);
}
