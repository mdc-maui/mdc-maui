namespace Material.Components.Maui.Interfaces;
public interface IFontElement : IElement
{
    Color FontColor { get; set; }
    float FontSize { get; set; }
    string FontFamily { get; set; }
    FontAttributes FontAttributes { get; set; }

    public static readonly BindableProperty FontColorProperty = BindableProperty.Create(
        nameof(FontColor),
        typeof(Color),
        typeof(IFontElement),
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        nameof(FontSize),
        typeof(float),
        typeof(IFontElement),
        14f,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).InvalidateMeasure()
    );

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        nameof(FontFamily),
        typeof(string),
        typeof(IFontElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).InvalidateMeasure()
    );

    public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
        nameof(FontAttributes),
        typeof(FontAttributes),
        typeof(IFontElement),
        FontAttributes.None,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).InvalidateMeasure()
    );
}
