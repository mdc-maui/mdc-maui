namespace Material.Components.Maui.FluentExtensions;

public static class IActiveIndicatorElementExtension
{
    public static TBindable ActiveIndicatorHeight<TBindable>(this TBindable view, int value)
        where TBindable : BindableObject, IActiveIndicatorElement
    {
        view.SetValue(IActiveIndicatorElement.ActiveIndicatorHeightProperty, value);
        return view;
    }

    public static TBindable BindActiveIndicatorHeight<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    )
        where TBindable : BindableObject, IActiveIndicatorElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IActiveIndicatorElement.ActiveIndicatorHeightProperty, binding);
        return view;
    }

    public static TBindable ActiveIndicatorColor<TBindable>(this TBindable view, Color value)
        where TBindable : BindableObject, IActiveIndicatorElement
    {
        view.SetValue(IActiveIndicatorElement.ActiveIndicatorColorProperty, value);
        return view;
    }

    public static TBindable BindActiveIndicatorColor<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    )
        where TBindable : BindableObject, IActiveIndicatorElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IActiveIndicatorElement.ActiveIndicatorColorProperty, binding);
        return view;
    }
}
