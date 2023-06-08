namespace Material.Components.Maui.FluentExtensions;

public static class ITextElementExtension
{
    public static TBindable Text<TBindable>(this TBindable view, string value)
        where TBindable : BindableObject, ITextElement
    {
        view.SetValue(ITextElement.TextProperty, value);
        return view;
    }

    public static TBindable Text<TBindable>(
        this TBindable view,
        object source,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null
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

    public static TBindable TextColor<TBindable>(
        this TBindable view,
        object source,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null
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

    public static TBindable FontSize<TBindable>(
        this TBindable view,
        object source,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null
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

    public static TBindable FontFamily<TBindable>(
        this TBindable view,
        object source,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null
    ) where TBindable : BindableObject, ITextElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(ITextElement.FontFamilyProperty, binding);
        return view;
    }

    public static TBindable FontSlant<TBindable>(this TBindable view, FontSlant value)
        where TBindable : BindableObject, ITextElement
    {
        view.SetValue(ITextElement.FontSlantProperty, value);
        return view;
    }

    public static TBindable FontSlant<TBindable>(
        this TBindable view,
        object source,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null
    ) where TBindable : BindableObject, ITextElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(ITextElement.FontSlantProperty, binding);
        return view;
    }

    public static TBindable FontWeight<TBindable>(this TBindable view, FontWeight value)
        where TBindable : BindableObject, ITextElement
    {
        view.SetValue(ITextElement.FontWeightProperty, value);
        return view;
    }

    public static TBindable FontWeight<TBindable>(
        this TBindable view,
        object source,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null
    ) where TBindable : BindableObject, ITextElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(ITextElement.FontWeightProperty, binding);
        return view;
    }
}
