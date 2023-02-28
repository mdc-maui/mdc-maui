namespace Material.Components.Maui.Core.Interfaces;

public static class IconElement
{
    public static readonly BindableProperty IconKindProperty = BindableProperty.Create(
        nameof(IIconElement.IconKind),
        typeof(IconKind),
        typeof(IIconElement),
        IconKind.None,
        propertyChanged: OnIconKindChanged
    );

    public static readonly BindableProperty IconDataProperty = BindableProperty.Create(
        nameof(IIconElement.IconData),
        typeof(string),
        typeof(IIconElement),
        default,
        propertyChanged: OnIconDataChanged
    );

    public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(
        nameof(IIconElement.IconSource),
        typeof(SKPicture),
        typeof(IIconElement),
        null,
        propertyChanged: OnIconSourceChanged
    );

    public static void OnIconKindChanged(BindableObject bo, object oldValue, object newValue)
    {
        var iconElement = bo as IIconElement;
        iconElement.IconData = ((IconKind)newValue).GetData();
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
