namespace Material.Components.Maui.Core;

internal static class ElevationElement
{
    public static readonly BindableProperty ElevationProperty = BindableProperty.Create(
        nameof(IElevationElement.Elevation),
        typeof(Elevation),
        typeof(IElevationElement),
        Elevation.Level0,
        propertyChanged: OnChanged
    );

    public static void OnChanged(BindableObject bo, object oldValue, object newValue)
    {
        ((IView)bo).OnPropertyChanged();
    }
}
