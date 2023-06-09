namespace Material.Components.Maui.FluentExtensions;

public static class IBackgroundElementExtension
{
    public static TBindable BackgroundColor<TBindable>(this TBindable view, Color value)
        where TBindable : BindableObject, IBackgroundElement
    {
        view.SetValue(IBackgroundElement.BackgroundColorProperty, value);
        return view;
    }

    public static TBindable BindBackgroundColor<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, IBackgroundElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IBackgroundElement.BackgroundColorProperty, binding);
        return view;
    }
}
