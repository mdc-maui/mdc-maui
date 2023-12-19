namespace Material.Components.Maui.FluentExtensions;

public static class IEditableElementExtension
{
    public static TBindable Text<TBindable>(this TBindable view, string value)
        where TBindable : BindableObject, IEditableElement
    {
        view.SetValue(IEditableElement.TextProperty, value);
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
    )
        where TBindable : BindableObject, IEditableElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IEditableElement.TextProperty, binding);
        return view;
    }

    public static TBindable SelectionRange<TBindable>(this TBindable view, TextRange value)
        where TBindable : BindableObject, IEditableElement
    {
        view.SetValue(IEditableElement.SelectionRangeProperty, value);
        return view;
    }

    public static TBindable BindSelectionRange<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    )
        where TBindable : BindableObject, IEditableElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IEditableElement.SelectionRangeProperty, binding);
        return view;
    }

    public static TBindable CaretColor<TBindable>(this TBindable view, Color value)
        where TBindable : BindableObject, IEditableElement
    {
        view.SetValue(IEditableElement.CaretColorProperty, value);
        return view;
    }

    public static TBindable BindCaretColor<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    )
        where TBindable : BindableObject, IEditableElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IEditableElement.CaretColorProperty, binding);
        return view;
    }

    public static TBindable TextAlignment<TBindable>(this TBindable view, TextAlignment value)
        where TBindable : BindableObject, IEditableElement
    {
        view.SetValue(IEditableElement.TextAlignmentProperty, value);
        return view;
    }

    public static TBindable BindTextAlignment<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    )
        where TBindable : BindableObject, IEditableElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IEditableElement.TextAlignmentProperty, binding);
        return view;
    }

    public static TBindable IsReadOnly<TBindable>(this TBindable view, bool value)
        where TBindable : BindableObject, IEditableElement
    {
        view.SetValue(IEditableElement.IsReadOnlyProperty, value);
        return view;
    }

    public static TBindable BindIsReadOnly<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    )
        where TBindable : BindableObject, IEditableElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IEditableElement.IsReadOnlyProperty, binding);
        return view;
    }

    public static TBindable InputType<TBindable>(this TBindable view, InputType value)
        where TBindable : BindableObject, IEditableElement
    {
        view.SetValue(IEditableElement.InputTypeProperty, value);
        return view;
    }

    public static TBindable BindInputType<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    )
        where TBindable : BindableObject, IEditableElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IEditableElement.InputTypeProperty, binding);
        return view;
    }
}
