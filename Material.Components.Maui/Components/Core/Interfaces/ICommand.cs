using System.Windows.Input;

namespace Material.Components.Maui.Core.Interfaces;

public interface ICommandElement
{
    ICommand Command { get; set; }
    object CommandParameter { get; set; }
}
