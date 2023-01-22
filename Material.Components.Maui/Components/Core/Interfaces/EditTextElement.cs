namespace Material.Components.Maui.Core.Interfaces;

internal static class EditTextElement
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(IEditTextElement.Text),
        typeof(string),
        typeof(IEditTextElement),
        string.Empty,
        propertyChanged: OnTextChanged
    );

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        nameof(IEditTextElement.FontFamily),
        typeof(string),
        typeof(IEditTextElement),
        null,
        propertyChanged: OnFontFamilyChanged
    );

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        nameof(IEditTextElement.FontSize),
        typeof(float),
        typeof(IEditTextElement),
        14f,
        propertyChanged: OnFontSizeChanged
    );

    public static readonly BindableProperty FontWeightProperty = BindableProperty.Create(
        nameof(IEditTextElement.FontWeight),
        typeof(int),
        typeof(IEditTextElement),
        400,
        propertyChanged: OnFontWeightChanged
    );

    public static readonly BindableProperty FontItalicProperty = BindableProperty.Create(
        nameof(IEditTextElement.FontItalic),
        typeof(bool),
        typeof(IEditTextElement),
        false,
        propertyChanged: OnFontItalicChanged
    );

    private static void OnTextChanged(BindableObject bo, object oldValue, object newValue)
    {
        var element = (IEditTextElement)bo;
        element.TextDocument.Text = element.Text;
        element.OnChanged();
        element.OnTextChanged((string)oldValue, (string)newValue);
    }

    private static void OnFontFamilyChanged(BindableObject bo, object oldValue, object newValue)
    {
        var element = (IEditTextElement)bo;
        element.TextStyle.FontFamily = (string)newValue;
    }

    private static void OnFontSizeChanged(BindableObject bo, object oldValue, object newValue)
    {
        var element = (IEditTextElement)bo;
        element.TextStyle.FontSize = (float)newValue;
        element.OnChanged();
    }

    private static void OnFontWeightChanged(BindableObject bo, object oldValue, object newValue)
    {
        var element = (IEditTextElement)bo;
        element.TextStyle.FontWeight = (int)newValue;
        element.OnChanged();
    }

    private static void OnFontItalicChanged(BindableObject bo, object oldValue, object newValue)
    {
        var element = (IEditTextElement)bo;
        element.TextStyle.FontItalic = (bool)newValue;
        element.OnChanged();
    }
}
