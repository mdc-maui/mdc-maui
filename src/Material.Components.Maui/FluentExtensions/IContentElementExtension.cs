namespace Material.Components.Maui.FluentExtensions;

public static class IContentElementExtension
{
    public static TBindable Content<TBindable>(this TBindable view, View value)
        where TBindable : BindableObject, IContentElement
    {
        view.SetValue(IContentElement.ContentProperty, value);
        return view;
    }

    public static TBindable BindContent<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    )
        where TBindable : BindableObject, IContentElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IContentElement.ContentProperty, binding);
        return view;
    }
}
