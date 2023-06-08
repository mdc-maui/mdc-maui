namespace Material.Components.Maui.FluentExtensions;

public static class CardExtension
{
    public static TBindable Content<TBindable>(this TBindable view, View value)
        where TBindable : Card
    {
        view.SetValue(Card.ContentProperty, value);
        return view;
    }

    public static TBindable Content<TBindable>(
        this TBindable view,
        object source,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null
    ) where TBindable : Card
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(Card.ContentProperty, binding);
        return view;
    }
}
