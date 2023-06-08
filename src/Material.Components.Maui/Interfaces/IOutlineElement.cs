namespace Material.Components.Maui.Interfaces;

public interface IOutlineElement : IShapeElement
{
    int OutlineWidth { get; set; }
    Color OutlineColor { get; set; }

    public static readonly BindableProperty OutlineWidthProperty = BindableProperty.Create(
        nameof(OutlineWidth),
        typeof(int),
        typeof(IOutlineElement),
        0,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty OutlineColorProperty = BindableProperty.Create(
        nameof(OutlineColor),
        typeof(Color),
        typeof(IOutlineElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}
