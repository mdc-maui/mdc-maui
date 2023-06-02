namespace Material.Components.Maui.Interfaces;

public interface IElevationElement : IShapeElement
{
    Elevation Elevation { get; set; }

    public static readonly BindableProperty ElevationProperty = BindableProperty.Create(
        nameof(Elevation),
        typeof(Elevation),
        typeof(IElevationElement),
        Elevation.Level0,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}
