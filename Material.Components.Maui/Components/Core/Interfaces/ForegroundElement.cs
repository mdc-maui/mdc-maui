namespace Material.Components.Maui.Core.Interfaces;

internal static class ForegroundElement
{
    public static readonly BindableProperty ForegroundColorProperty = BindableProperty.Create(
        nameof(IForegroundElement.ForegroundColor),
        typeof(Color),
        typeof(IForegroundElement),
        null,
        propertyChanged: OnChanged
    );

    public static readonly BindableProperty ForegroundOpacityProperty = BindableProperty.Create(
        nameof(IForegroundElement.ForegroundOpacity),
        typeof(float),
        typeof(IForegroundElement),
        1f,
        propertyChanged: OnChanged
    );

    public static void OnChanged(BindableObject bo, object oldValue, object newValue)
    {
        ((IView)bo).OnPropertyChanged();
    }
}
