namespace Material.Components.Maui.Interfaces;

public interface IBackgroundElement : IShapeElement
{
    Color BackgroundColor { get; set; }

    public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(
        nameof(BackgroundColor),
        typeof(Color),
        typeof(IBackgroundElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}
