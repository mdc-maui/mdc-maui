namespace Material.Components.Maui.FluentExtensions;

public static class CheckBoxExtension
{
    public static TBindable IsChecked<TBindable>(this TBindable view, bool value)
        where TBindable : CheckBox
    {
        view.SetValue(CheckBox.IsCheckedProperty, value);
        return view;
    }

    public static TBindable BindIsChecked<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : CheckBox
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(CheckBox.IsCheckedProperty, binding);
        return view;
    }
}
