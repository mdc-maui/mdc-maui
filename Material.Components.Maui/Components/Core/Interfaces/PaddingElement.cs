namespace Material.Components.Maui.Core.Interfaces;

internal class PaddingElement
{
    public static readonly BindableProperty PaddingProperty = BindableProperty.Create(
        nameof(IPaddingElement.Padding),
        typeof(Thickness),
        typeof(IPaddingElement),
        Thickness.Zero,
        propertyChanged: OnChanged
    );

    public static void OnChanged(BindableObject bo, object oldValue, object newValue)
    {
        ((IView)bo).OnPropertyChanged();
    }
}
