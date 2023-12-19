namespace Material.Components.Maui.FluentExtensions;

public static class ILabelTextElementExtension
{
    public static TBindable LabelText<TBindable>(this TBindable view, int value)
        where TBindable : BindableObject, ILabelTextElement
    {
        view.SetValue(ILabelTextElement.LabelTextProperty, value);
        return view;
    }

    public static TBindable BindLabelText<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    )
        where TBindable : BindableObject, ILabelTextElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(ILabelTextElement.LabelTextProperty, binding);
        return view;
    }

    public static TBindable LabelFontColor<TBindable>(this TBindable view, int value)
        where TBindable : BindableObject, ILabelTextElement
    {
        view.SetValue(ILabelTextElement.LabelFontColorProperty, value);
        return view;
    }

    public static TBindable BindLabelFontColor<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    )
        where TBindable : BindableObject, ILabelTextElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(ILabelTextElement.LabelFontColorProperty, binding);
        return view;
    }
}
