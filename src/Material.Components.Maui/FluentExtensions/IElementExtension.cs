namespace Material.Components.Maui.FluentExtensions;

public static class IElementExtension
{
    public static TBindable IsEnabled<TBindable>(this TBindable view, bool value)
        where TBindable : BindableObject, IElement
    {
        view.SetValue(IElement.IsEnabledProperty, value);
        return view;
    }

    public static TBindable IsEnabled<TBindable>(
        this TBindable view,
        object source,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null
    ) where TBindable : BindableObject, IElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IBackgroundElement.BackgroundColorProperty, binding);
        return view;
    }
}
