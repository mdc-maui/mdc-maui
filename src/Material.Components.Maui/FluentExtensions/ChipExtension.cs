namespace Material.Components.Maui.FluentExtensions;

public static class ChipExtension
{
    public static TBindable IsSelected<TBindable>(this TBindable view, bool value)
        where TBindable : Chip
    {
        view.SetValue(Chip.IsSelectedProperty, value);
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
    ) where TBindable : Chip
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(Chip.IsSelectedProperty, binding);
        return view;
    }

    public static TBindable HasCloseButton<TBindable>(this TBindable view, bool value)
        where TBindable : Chip
    {
        view.SetValue(Chip.HasCloseButtonProperty, value);
        return view;
    }

    public static TBindable BindHasCloseButton<TBindable>(
        this TBindable view,
        object source,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null
    ) where TBindable : Chip
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(Chip.HasCloseButtonProperty, binding);
        return view;
    }
}
