namespace Material.Components.Maui.Interfaces;
public interface IActiveIndicatorElement : IElement
{
    int ActiveIndicatorHeight { get; set; }

    Color ActiveIndicatorColor { get; set; }

    public static readonly BindableProperty ActiveIndicatorHeightProperty = BindableProperty.Create(
        nameof(ActiveIndicatorHeight),
        typeof(int),
        typeof(IActiveIndicatorElement),
        0,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty ActiveIndicatorColorProperty = BindableProperty.Create(
        nameof(ActiveIndicatorColor),
        typeof(Color),
        typeof(IActiveIndicatorElement),
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}
