namespace Material.Components.Maui.Core.Interfaces;

internal static class StateLayerElement
{
    public static readonly BindableProperty StateLayerColorProperty = BindableProperty.Create(
        nameof(IStateLayerElement.StateLayerColor),
        typeof(Color),
        typeof(IStateLayerElement),
        propertyChanged: OnChanged
    );

    public static readonly BindableProperty StateLayerOpacityProperty = BindableProperty.Create(
        nameof(IStateLayerElement.StateLayerOpacity),
        typeof(float),
        typeof(IStateLayerElement),
        0f,
        propertyChanged: OnChanged
    );

    public static void OnChanged(BindableObject bo, object oldValue, object newValue)
    {
        ((IView)bo).OnPropertyChanged();
    }
}
