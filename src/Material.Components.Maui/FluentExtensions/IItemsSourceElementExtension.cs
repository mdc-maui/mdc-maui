namespace Material.Components.Maui.FluentExtensions;

public static class IItemsSourceElementExtension
{
    public static TBindable BindItemsSource<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    )
        where TBindable : BindableObject, IItemsSourceElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IItemsSourceElement.ItemsSourceProperty, binding);
        return view;
    }
}
