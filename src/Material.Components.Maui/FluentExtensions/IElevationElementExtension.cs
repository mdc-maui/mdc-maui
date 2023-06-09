namespace Material.Components.Maui.FluentExtensions;

public static class IElevationElementExtension
{
    public static TBindable Elevation<TBindable>(this TBindable view, Elevation value)
        where TBindable : BindableObject, IElevationElement
    {
        view.SetValue(IElevationElement.ElevationProperty, value);
        return view;
    }

    public static TBindable BindBackgroundColor<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, IElevationElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IElevationElement.ElevationProperty, binding);
        return view;
    }
}
