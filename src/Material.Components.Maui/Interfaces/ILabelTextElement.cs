namespace Material.Components.Maui.Interfaces;

public interface ILabelTextElement : IElement
{
    string LabelText { get; set; }
    Color LabelFontColor { get; set; }

    public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
        nameof(LabelText),
        typeof(string),
        typeof(ILabelTextElement),
        "Label text",
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty LabelFontColorProperty = BindableProperty.Create(
        nameof(LabelFontColor),
        typeof(Color),
        typeof(ILabelTextElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}