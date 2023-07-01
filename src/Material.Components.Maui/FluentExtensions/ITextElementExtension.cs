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
}
