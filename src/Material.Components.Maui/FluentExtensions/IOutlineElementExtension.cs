namespace Material.Components.Maui.FluentExtensions;

public static class IOutlineElementExtension
{
    public static TBindable OutlineWidth<TBindable>(this TBindable view, bool value)
        where TBindable : BindableObject, IOutlineElement
    {
        view.SetValue(IOutlineElement.OutlineWidthProperty, value);
        return view;
    }

    public static TBindable OutlineWidth<TBindable>(
        this TBindable view,
        object source,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null
    ) where TBindable : BindableObject, IOutlineElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IOutlineElement.OutlineWidthProperty, binding);
        return view;
    }

    public static TBindable OutlineColor<TBindable>(this TBindable view, Color value)
        where TBindable : BindableObject, IOutlineElement
    {
        view.SetValue(IOutlineElement.OutlineColorProperty, value);
        return view;
    }

    public static TBindable OutlineColor<TBindable>(
        this TBindable view,
        object source,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null
    ) where TBindable : BindableObject, IOutlineElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IOutlineElement.OutlineColorProperty, binding);
        return view;
    }
}
