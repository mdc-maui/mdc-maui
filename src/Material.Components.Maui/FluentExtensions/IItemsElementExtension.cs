using System.Collections.ObjectModel;

namespace Material.Components.Maui.FluentExtensions;

public static class IItemsElementExtension
{
    public static TBindable Items<TBindable, T>(this TBindable view, ObservableCollection<T> value)
        where TBindable : BindableObject, IItemsElement<T>
    {
        view.SetValue(IItemsElement<T>.ItemsProperty, value);
        return view;
    }
}
