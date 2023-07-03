using System.Windows.Input;

namespace Material.Components.Maui.Interfaces;
public interface IICommandElement
{
    ICommand Command { get; set; }
    object CommandParameter { get; set; }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(IICommandElement),
        default
    );

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(IICommandElement),
        default
    );
}
