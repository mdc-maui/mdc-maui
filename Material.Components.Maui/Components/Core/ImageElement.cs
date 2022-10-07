namespace Material.Components.Maui.Core;

internal static class ImageElement
{
    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(IImageElement.Icon),
        typeof(IconKind),
        typeof(IImageElement),
        IconKind.None,
        propertyChanged: OnChanged);

    public static readonly BindableProperty ImageProperty = BindableProperty.Create(
        nameof(IImageElement.Image),
        typeof(SKPicture),
        typeof(IImageElement),
        null,
        propertyChanged: OnChanged);


    public static void OnChanged(BindableObject bo, object oldValue, object newValue)
    {
        ((IView)bo).OnPropertyChanged();
    }
}
