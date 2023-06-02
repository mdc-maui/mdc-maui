namespace Material.Components.Maui.Interfaces;

public interface IShapeElement : IElement
{
    Shape Shape { get; set; }

    public static readonly BindableProperty ShapeProperty = BindableProperty.Create(
        nameof(Shape),
        typeof(Shape),
        typeof(IShapeElement),
        Shape.None,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}
