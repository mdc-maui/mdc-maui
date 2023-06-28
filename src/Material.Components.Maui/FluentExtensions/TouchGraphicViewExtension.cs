using System.Windows.Input;

namespace Material.Components.Maui.FluentExtensions;

public static class TouchGraphicViewExtension
{
    public static TBindable Command<TBindable>(this TBindable view, ICommand value)
        where TBindable : TouchGraphicsView
    {
        view.SetValue(TouchGraphicsView.CommandProperty, value);
        return view;
    }

    public static TBindable BindCommand<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : TouchGraphicsView
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(TouchGraphicsView.CommandProperty, binding);
        return view;
    }

    public static TBindable CommandParameter<TBindable>(this TBindable view, object value)
        where TBindable : TouchGraphicsView
    {
        view.SetValue(TouchGraphicsView.CommandParameterProperty, value);
        return view;
    }

    public static TBindable BindCommandParameter<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : TouchGraphicsView
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(TouchGraphicsView.CommandParameterProperty, binding);
        return view;
    }
}
