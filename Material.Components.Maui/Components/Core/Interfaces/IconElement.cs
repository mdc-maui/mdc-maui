namespace Material.Components.Maui.Core.Interfaces;

internal static class IconElement
{
    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(IIconElement.Icon),
        typeof(IconKind),
        typeof(IIconElement),
        IconKind.None,
        propertyChanged: OnChanged
    );

    public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(
        nameof(IIconElement.IconSource),
        typeof(SKPicture),
        typeof(IIconElement),
        null,
        propertyChanged: OnChanged
    );

    public static void OnChanged(BindableObject bo, object oldValue, object newValue)
    {
        ((IView)bo).OnPropertyChanged();
    }
}
