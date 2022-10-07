using System.ComponentModel;
using System.Globalization;

namespace Material.Components.Maui.Converters;

public class ElevationConverter : TypeConverter
{
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string text)
        {
            if (text == "Level0") return Elevation.Level0;
            else if (text == "Level1") return Elevation.Level1;
            else if (text == "Level2") return Elevation.Level2;
            else if (text == "Level3") return Elevation.Level3;
            else if (text == "Level4") return Elevation.Level4;
            else if (text == "Level5") return Elevation.Level5;
            else if (int.TryParse(text, out var i32))
            {
                return new Elevation(i32);
            }
        }
        return null;
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        return new(new[]
        {
            "Level0",
            "Level1",
            "Level2",
            "Level3",
            "Level4",
            "Level5",
        });
    }
}
