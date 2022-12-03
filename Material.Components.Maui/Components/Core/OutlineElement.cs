namespace Material.Components.Maui.Core;

internal static class OutlineElement
{
    public static readonly BindableProperty OutlineWidthProperty = BindableProperty.Create(
        nameof(IOutlineElement.OutlineWidth),
        typeof(int),
        typeof(IOutlineElement),
        0,
        propertyChanged: OnChanged
    );

    public static readonly BindableProperty OutlineColorProperty = BindableProperty.Create(
        nameof(IOutlineElement.OutlineColor),
        typeof(Color),
        typeof(IOutlineElement),
        null,
        propertyChanged: OnChanged
    );

    public static readonly BindableProperty OutlineOpacityProperty = BindableProperty.Create(
        nameof(IOutlineElement.OutlineOpacity),
        typeof(float),
        typeof(IOutlineElement),
        1f,
        propertyChanged: OnChanged
    );

    public static void OnChanged(BindableObject bo, object oldValue, object newValue)
    {
        ((IView)bo).OnPropertyChanged();
    }
}
