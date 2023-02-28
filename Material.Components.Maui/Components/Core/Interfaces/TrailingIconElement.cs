namespace Material.Components.Maui.Core.Interfaces;

internal static class TrailingIconElement
{
    public static readonly BindableProperty TrailingIconKindProperty = BindableProperty.Create(
        nameof(ITrailingIconElement.TrailingIconKind),
        typeof(IconKind),
        typeof(ITrailingIconElement),
        IconKind.None,
        propertyChanged: OnIconKindChanged
    );

    public static readonly BindableProperty TrailingIconDataProperty = BindableProperty.Create(
        nameof(ITrailingIconElement.TrailingIconData),
        typeof(string),
        typeof(ITrailingIconElement),
        default,
        propertyChanged: OnIconDataChanged
    );

    public static readonly BindableProperty TrailingIconSourceProperty = BindableProperty.Create(
        nameof(ITrailingIconElement.TrailingIconSource),
        typeof(SKPicture),
        typeof(ITrailingIconElement),
        null,
        propertyChanged: OnIconSourceChanged
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

    public static void OnIconKindChanged(BindableObject bo, object oldValue, object newValue)
    {
        var iconElement = bo as ITrailingIconElement;
        iconElement.TrailingIconData = ((IconKind)newValue).GetData();
    }

    public static void OnIconDataChanged(BindableObject bo, object oldValue, object newValue)
    {
        if (string.IsNullOrEmpty(oldValue as string) || string.IsNullOrEmpty(newValue as string))
        {
            (bo as View)?.SendInvalidateMeasure();
        }
        else
        {
            ((IView)bo).OnPropertyChanged();
        }
    }

    public static void OnIconSourceChanged(BindableObject bo, object oldValue, object newValue)
    {
        if (oldValue is null || newValue is null)
        {
            (bo as View)?.SendInvalidateMeasure();
        }
        else
        {
            ((IView)bo).OnPropertyChanged();
        }
    }
}
