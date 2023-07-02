namespace Material.Components.Maui.FluentExtensions;

public static class IFontElementExtension
{
    public static TBindable FontColor<TBindable>(this TBindable view, Color value)
        where TBindable : BindableObject, IFontElement
    {
        view.SetValue(IFontElement.FontColorProperty, value);
        return view;
    }

    public static TBindable BindFontColor<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, IFontElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IFontElement.FontColorProperty, binding);
        return view;
    }

    public static TBindable FontSize<TBindable>(this TBindable view, float value)
        where TBindable : BindableObject, IFontElement
    {
        view.SetValue(IFontElement.FontSizeProperty, value);
        return view;
    }

    public static TBindable BindFontSize<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, IFontElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IFontElement.FontSizeProperty, binding);
        return view;
    }

    public static TBindable FontFamily<TBindable>(this TBindable view, string value)
        where TBindable : BindableObject, IFontElement
    {
        view.SetValue(IFontElement.FontFamilyProperty, value);
        return view;
    }

    public static TBindable BindFontFamily<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, IFontElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IFontElement.FontFamilyProperty, binding);
        return view;
    }

    public static TBindable FontWeight<TBindable>(this TBindable view, FontSlant value)
        where TBindable : BindableObject, IFontElement
    {
        view.SetValue(IFontElement.FontWeightProperty, value);
        return view;
    }

    public static TBindable BindFontWeight<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, IFontElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IFontElement.FontWeightProperty, binding);
        return view;
    }
}
