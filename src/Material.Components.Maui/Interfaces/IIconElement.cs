namespace Material.Components.Maui.Interfaces;

public interface IIconElement : IElement
{
    string IconData { get; set; }
    PathF IconPath { get; set; }
    Color IconColor { get; set; }
    float IconOpacity { get; set; }

    public static readonly BindableProperty IconDataProperty = BindableProperty.Create(
        nameof(IconData),
        typeof(string),
        typeof(IIconElement),
        default,
        propertyChanged: (bo, ov, nv) =>
        {
            ((IIconElement)bo).IconPath = PathBuilder.Build((string)nv);
            ((IElement)bo).OnPropertyChanged();
        }
    );

    public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
        nameof(IconColor),
        typeof(Color),
        typeof(IIconElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty IconOpacityProperty = BindableProperty.Create(
        nameof(IconOpacity),
        typeof(float),
        typeof(IIconElement),
        1f,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}
