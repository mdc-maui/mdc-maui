namespace Material.Components.Maui.Interfaces;

public interface ILabelTextElement : ITextElement
{
    string LabelText { get; set; }

    float LabelFontSize { get; set; }

    public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
        nameof(Label),
        typeof(string),
        typeof(ILabelTextElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty LabelFontSizeProperty = BindableProperty.Create(
        nameof(LabelFontSize),
        typeof(float),
        typeof(ILabelTextElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}