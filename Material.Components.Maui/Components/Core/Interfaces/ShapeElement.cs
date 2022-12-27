namespace Material.Components.Maui.Core.Interfaces;

internal static class ShapeElement
{
    public static readonly BindableProperty ShapeProperty = BindableProperty.Create(
        nameof(IShapeElement.Shape),
        typeof(Shape),
        typeof(IShapeElement),
        Shape.None,
        propertyChanged: OnChanged
    );

    public static void OnChanged(BindableObject bo, object oldValue, object newValue)
    {
        ((IView)bo).OnPropertyChanged();
    }
}
