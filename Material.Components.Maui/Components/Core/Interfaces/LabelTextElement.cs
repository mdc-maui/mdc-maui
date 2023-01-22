namespace Material.Components.Maui.Core.Interfaces;
internal class LabelTextElement
{
    public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
        nameof(ILabelTextElement.LabelText),
        typeof(string),
        typeof(ILabelTextElement),
        string.Empty,
        propertyChanged: OnLabelTextChanged
    );

    public static readonly BindableProperty LabelTextColorProperty = BindableProperty.Create(
        nameof(ILabelTextElement.LabelTextColor),
        typeof(Color),
        typeof(ILabelTextElement),
        propertyChanged: OnChanged
    );

    public static readonly BindableProperty LabelTextOpacityProperty = BindableProperty.Create(
    nameof(ILabelTextElement.LabelTextOpacity),
    typeof(float),
    typeof(ILabelTextElement),
    1f,
    propertyChanged: OnChanged
);
    private static void OnLabelTextChanged(BindableObject bo, object oldValue, object newValue)
    {
        var element = (ILabelTextElement)bo;
        element.InternalLabelText.Clear();
        element.InternalLabelText.AddText(element.LabelText, element.LabelTextStyle);
        OnChanged(bo, null, null);
    }

    private static void OnChanged(BindableObject bo, object oldValue, object newValue)
    {
        ((IView)bo).OnPropertyChanged();
    }
}
