namespace Material.Components.Maui.FluentExtensions;

public static class IShapeElementExtension
{
    public static TBindable Shape<TBindable>(this TBindable view, Shape value)
        where TBindable : BindableObject, IShapeElement
    {
        view.SetValue(IShapeElement.ShapeProperty, value);
        return view;
    }

    public static TBindable BindShape<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, IShapeElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IShapeElement.ShapeProperty, binding);
        return view;
    }
}
