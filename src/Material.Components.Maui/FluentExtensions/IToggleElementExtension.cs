namespace Material.Components.Maui.FluentExtensions;

public static class IToggleElementExtension
{
    public static TBindable IsToggleEnabled<TBindable>(this TBindable view, bool value)
        where TBindable : BindableObject, IToggleElement
    {
        view.SetValue(IToggleElement.IsToggleEnabledProperty, value);
        return view;
    }

    public static TBindable IsToggleEnabled<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, IToggleElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IToggleElement.IsToggleEnabledProperty, binding);
        return view;
    }

    public static TBindable IsSelected<TBindable>(this TBindable view, bool value)
        where TBindable : BindableObject, IToggleElement
    {
        view.SetValue(IToggleElement.IsSelectedProperty, value);
        return view;
    }

    public static TBindable BindIsSelected<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, IToggleElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IToggleElement.IsSelectedProperty, binding);
        return view;
    }
}
