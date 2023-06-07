namespace Material.Components.Maui.Interfaces;

public interface ITextElement : IElement
{
    string Text { get; set; }
    Color TextColor { get; set; }
    float FontSize { get; set; }
    string FontFamily { get; set; }
    FontSlant FontSlant { get; set; }
    FontWeight FontWeight { get; set; }

    [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(ITextElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).InvalidateMeasure()
    );

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor),
        typeof(Color),
        typeof(ITextElement),
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        nameof(FontSize),
        typeof(float),
        typeof(ITextElement),
        14f,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).InvalidateMeasure()
    );

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        nameof(FontFamily),
        typeof(string),
        typeof(ITextElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).InvalidateMeasure()
    );

    public static readonly BindableProperty FontSlantProperty = BindableProperty.Create(
        nameof(FontSlant),
        typeof(FontSlant),
        typeof(ITextElement),
        FontSlant.Default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).InvalidateMeasure()
    );

    public static readonly BindableProperty FontWeightProperty = BindableProperty.Create(
        nameof(FontWeight),
        typeof(FontWeight),
        typeof(ITextElement),
        FontWeight.Regular,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).InvalidateMeasure()
    );
}
