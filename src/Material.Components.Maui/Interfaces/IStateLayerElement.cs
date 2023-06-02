namespace Material.Components.Maui.Interfaces;

public interface IStateLayerElement : IShapeElement
{
    Color StateLayerColor { get; set; }

    public static readonly BindableProperty StateLayerColorProperty = BindableProperty.Create(
        nameof(StateLayerColor),
        typeof(Color),
        typeof(IStateLayerElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}
