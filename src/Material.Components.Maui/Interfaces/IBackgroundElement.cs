namespace Material.Components.Maui.Interfaces;

public interface IBackgroundElement : IShapeElement
{
    Color BackgroundColour { get; set; }
    float BackgroundOpacity { get; set; }

    public static readonly BindableProperty BackgroundColourProperty = BindableProperty.Create(
        nameof(BackgroundColour),
        typeof(Color),
        typeof(IBackgroundElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty BackgroundOpacityProperty = BindableProperty.Create(
        nameof(BackgroundOpacity),
        typeof(float),
        typeof(IBackgroundElement),
        1f,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}
