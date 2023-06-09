namespace Material.Components.Maui.FluentExtensions;

public static class IStateLayerElementExtension
{
    public static TBindable StateLayerColor<TBindable>(this TBindable view, Color value)
        where TBindable : BindableObject, IStateLayerElement
    {
        view.SetValue(IStateLayerElement.StateLayerColorProperty, value);
        return view;
    }

    public static TBindable BindStateLayerColor<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, IStateLayerElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IStateLayerElement.StateLayerColorProperty, binding);
        return view;
    }
}
