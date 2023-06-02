namespace Material.Components.Maui.Interfaces;

public interface ITextElement : IElement
{
    string Text { get; set; }
    string FontFamily { get; set; }
    FontAttributes FontAttributes { get; set; }

    [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
    double FontSize { get; set; }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(ITextElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        nameof(FontFamily),
        typeof(string),
        typeof(ITextElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
        nameof(FontAttributes),
        typeof(FontAttributes),
        typeof(ITextElement),
        FontAttributes.None,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        nameof(FontSize),
        typeof(double),
        typeof(ITextElement),
        14f,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}
