namespace Material.Components.Maui.Core.Interfaces;
internal class SupportingTextElement
{
    public static readonly BindableProperty SupportingTextProperty = BindableProperty.Create(
        nameof(ISupportingTextElement.SupportingText),
        typeof(string),
        typeof(ISupportingTextElement),
        string.Empty,
        propertyChanged: OnSupportingTextChanged
    );

    public static readonly BindableProperty SupportingTextColorProperty = BindableProperty.Create(
        nameof(ISupportingTextElement.SupportingTextColor),
        typeof(Color),
        typeof(ISupportingTextElement),
        propertyChanged: OnChanged
    );

    public static readonly BindableProperty SupportingTextOpacityProperty = BindableProperty.Create(
    nameof(ISupportingTextElement.SupportingTextOpacity),
    typeof(float),
    typeof(ISupportingTextElement),
    1f,
    propertyChanged: OnChanged
);

    private static void OnSupportingTextChanged(BindableObject bo, object oldValue, object newValue)
    {
        var element = (ISupportingTextElement)bo;
        element.InternalSupportingText.Clear();
        element.InternalSupportingText.AddText(element.SupportingText, element.SupportingTextStyle);
        OnChanged(bo, null, null);
    }

    private static void OnChanged(BindableObject bo, object oldValue, object newValue)
    {
        ((IView)bo).OnPropertyChanged();
    }
}
