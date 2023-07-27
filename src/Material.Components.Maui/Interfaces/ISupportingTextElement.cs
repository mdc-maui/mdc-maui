namespace Material.Components.Maui.Interfaces;

public interface ISupportingTextElement : IElement
{
    string SupportingText { get; set; }
    Color SupportingFontColor { get; set; }

    public static readonly BindableProperty SupportingTextProperty = BindableProperty.Create(
        nameof(SupportingText),
        typeof(string),
        typeof(ISupportingTextElement),
        "Supporting text",
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty SupportingFontColorProperty = BindableProperty.Create(
        nameof(SupportingFontColor),
        typeof(Color),
        typeof(ISupportingTextElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}
