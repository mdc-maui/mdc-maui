namespace Material.Components.Maui.FluentExtensions;

public static class ITrailingIconElementExtension
{
    public static TBindable TrailingIconData<TBindable>(this TBindable view, string value)
        where TBindable : BindableObject, ITrailingIconElement
    {
        view.SetValue(ITrailingIconElement.TrailingIconDataProperty, value);
        return view;
    }

    public static TBindable BindTrailingIconData<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    )
        where TBindable : BindableObject, ITrailingIconElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(ITrailingIconElement.TrailingIconDataProperty, binding);
        return view;
    }

    public static TBindable TrailingIconColor<TBindable>(this TBindable view, Color value)
        where TBindable : BindableObject, ITrailingIconElement
    {
        view.SetValue(ITrailingIconElement.TrailingIconColorProperty, value);
        return view;
    }

    public static TBindable BindTrailingIconColor<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    )
        where TBindable : BindableObject, ITrailingIconElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(ITrailingIconElement.TrailingIconColorProperty, binding);
        return view;
    }
}
