using System.Windows.Input;

namespace Material.Components.Maui.Core;

internal static class TouchElement
{
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(ITouchElement.Command),
        typeof(ICommand),
        typeof(ITouchElement),
        null);

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(ITouchElement.CommandParameter),
        typeof(object),
        typeof(ITouchElement),
        null);
}
