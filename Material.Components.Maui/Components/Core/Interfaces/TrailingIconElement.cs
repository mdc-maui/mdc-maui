namespace Material.Components.Maui.Core.Interfaces;

internal static class TrailingIconElement
{
    public static readonly BindableProperty TrailingIconProperty = BindableProperty.Create(
        nameof(ITrailingIconElement.TrailingIcon),
        typeof(IconKind),
        typeof(ITrailingIconElement),
        IconKind.None,
        propertyChanged: OnChanged
    );

    public static readonly BindableProperty TrailingIconSourceProperty = BindableProperty.Create(
        nameof(ITrailingIconElement.TrailingIconSource),
        typeof(SKPicture),
        typeof(ITrailingIconElement),
        null,
        propertyChanged: OnChanged
    );

    public static readonly BindableProperty TrailingIconColorProperty = BindableProperty.Create(
       nameof(ITrailingIconElement.TrailingIconColor),
       typeof(Color),
       typeof(ITrailingIconElement),
       null,
       propertyChanged: OnChanged
   );

    public static void OnChanged(BindableObject bo, object oldValue, object newValue)
    {
        ((IView)bo).OnPropertyChanged();
    }
}
