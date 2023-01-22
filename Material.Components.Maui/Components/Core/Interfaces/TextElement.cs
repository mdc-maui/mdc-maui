namespace Material.Components.Maui.Core.Interfaces;

internal static class TextElement
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(ITextElement.Text),
        typeof(string),
        typeof(ITextElement),
        "default",
        propertyChanged: OnTextChanged
    );

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        nameof(ITextElement.FontFamily),
        typeof(string),
        typeof(ITextElement),
        null,
        propertyChanged: OnFontFamilyChanged
    );

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        nameof(ITextElement.FontSize),
        typeof(float),
        typeof(ITextElement),
        14f,
        propertyChanged: OnFontSizeChanged
    );

    public static readonly BindableProperty FontWeightProperty = BindableProperty.Create(
        nameof(ITextElement.FontWeight),
        typeof(int),
        typeof(ITextElement),
        400,
        propertyChanged: OnFontWeightChanged
    );

    public static readonly BindableProperty FontItalicProperty = BindableProperty.Create(
        nameof(ITextElement.FontItalic),
        typeof(bool),
        typeof(ITextElement),
        false,
        propertyChanged: OnFontItalicChanged
    );

    private static void OnTextChanged(BindableObject bo, object oldValue, object newValue)
    {
        var element = (ITextElement)bo;
        element.InternalText.Clear();
        element.InternalText.AddText(element.Text, element.TextStyle);
        element.OnChanged();
    }

    private static void OnFontFamilyChanged(BindableObject bo, object oldValue, object newValue)
    {
        var element = (ITextElement)bo;
        element.TextStyle.FontFamily = (string)newValue;
        element.InternalText.Clear();
        element.InternalText.AddText(element.Text, element.TextStyle);
        element.OnChanged();
    }

    private static void OnFontSizeChanged(BindableObject bo, object oldValue, object newValue)
    {
        var element = (ITextElement)bo;
        element.TextStyle.FontSize = (float)newValue;
        element.InternalText.Clear();
        element.InternalText.AddText(element.Text, element.TextStyle);
        element.OnChanged();
    }

    private static void OnFontWeightChanged(BindableObject bo, object oldValue, object newValue)
    {
        var element = (ITextElement)bo;
        element.TextStyle.FontWeight = (int)newValue;
        element.InternalText.Clear();
        element.InternalText.AddText(element.Text, element.TextStyle);
        element.OnChanged();
    }

    private static void OnFontItalicChanged(BindableObject bo, object oldValue, object newValue)
    {
        var element = (ITextElement)bo;
        element.TextStyle.FontItalic = (bool)newValue;
        element.InternalText.Clear();
        element.InternalText.AddText(element.Text, element.TextStyle);
        element.OnChanged();
    }
}
