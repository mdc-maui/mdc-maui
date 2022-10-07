namespace Material.Components.Maui.Core;

internal static class BackgroundElement
{
    public static readonly BindableProperty BackgroundColourProperty = BindableProperty.Create(
        nameof(IBackgroundElement.BackgroundColour),
        typeof(Color),
        typeof(IBackgroundElement),
        null,
        propertyChanged: OnChanged);

    public static readonly BindableProperty BackgroundOpacityProperty = BindableProperty.Create(
        nameof(IBackgroundElement.BackgroundOpacity),
        typeof(float),
        typeof(IBackgroundElement),
        1f,
        propertyChanged: OnChanged);


    public static void OnChanged(BindableObject bo, object oldValue, object newValue)
    {
        ((IView)bo).OnPropertyChanged();
    }
}
