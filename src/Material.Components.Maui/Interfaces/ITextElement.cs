namespace Material.Components.Maui.Interfaces;

public interface ITextElement : IElement
{
    string Text { get; set; }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(ITextElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).InvalidateMeasure()
    );
}
