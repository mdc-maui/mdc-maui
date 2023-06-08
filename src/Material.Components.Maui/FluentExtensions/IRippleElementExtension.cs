namespace Material.Components.Maui.FluentExtensions;

public static class IRippleElementExtension
{
    public static TBindable RippleDuration<TBindable>(this TBindable view, float value)
        where TBindable : BindableObject, IRippleElement
    {
        view.SetValue(IRippleElement.RippleDurationProperty, value);
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
    ) where TBindable : BindableObject, IRippleElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IRippleElement.RippleDurationProperty, binding);
        return view;
    }

    public static TBindable RippleEasing<TBindable>(this TBindable view, Easing value)
        where TBindable : BindableObject, IRippleElement
    {
        view.SetValue(IRippleElement.RippleEasingProperty, value);
        return view;
    }

    public static TBindable RippleEasing<TBindable>(
        this TBindable view,
        object source,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null
    ) where TBindable : BindableObject, IRippleElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IRippleElement.RippleEasingProperty, binding);
        return view;
    }
}
