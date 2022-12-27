using System.Globalization;

namespace Material.Components.Maui.Converters;

public class DisplayModeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (DrawerDisplayMode)value != DrawerDisplayMode.Split;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? 1 : 0;
    }
}
