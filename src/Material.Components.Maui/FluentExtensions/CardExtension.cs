namespace Material.Components.Maui.FluentExtensions;

public static class CardExtension
{
    public static TBindable Content<TBindable>(this TBindable view, View value)
        where TBindable : Card
    {
        view.SetValue(Card.ContentProperty, value);
        return view;
    }

    public static TBindable BindContent<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : Card
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(Card.ContentProperty, binding);
        return view;
    }
}
