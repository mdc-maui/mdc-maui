namespace Material.Components.Maui.FluentExtensions;

public static class ITextElementExtension
{
    public static TBindable Text<TBindable>(this TBindable view, string value)
        where TBindable : BindableObject, ITextElement
    {
        view.SetValue(ITextElement.TextProperty, value);
        return view;
    }

    public static TBindable BindText<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, ITextElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(ITextElement.TextProperty, binding);
        return view;
    }

    public static TBindable TextColor<TBindable>(this TBindable view, Color value)
        where TBindable : BindableObject, ITextElement
    {
        view.SetValue(ITextElement.TextColorProperty, value);
        return view;
    }

    public static TBindable BindTextColor<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, ITextElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(ITextElement.TextColorProperty, binding);
        return view;
    }

    public static TBindable FontSize<TBindable>(this TBindable view, float value)
        where TBindable : BindableObject, ITextElement
    {
        view.SetValue(ITextElement.FontSizeProperty, value);
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
    ) where TBindable : BindableObject, ITextElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(ITextElement.FontSizeProperty, binding);
        return view;
    }

    public static TBindable FontFamily<TBindable>(this TBindable view, string value)
        where TBindable : BindableObject, ITextElement
    {
        view.SetValue(ITextElement.FontFamilyProperty, value);
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
    ) where TBindable : BindableObject, ITextElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(ITextElement.FontFamilyProperty, binding);
        return view;
    }

    public static TBindable FontAttributes<TBindable>(this TBindable view, FontSlant value)
        where TBindable : BindableObject, ITextElement
    {
        view.SetValue(ITextElement.FontAttributesProperty, value);
        return view;
    }

    public static TBindable BindFontAttributes<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, ITextElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(ITextElement.FontAttributesProperty, binding);
        return view;
    }
}
