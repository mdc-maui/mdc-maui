using System.ComponentModel;
using System.Globalization;

namespace Material.Components.Maui.Converters;
public class ShapeConverter : TypeConverter
{
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string text)
        {
            if (text == "None") return Shape.None;
            else if (text == "ExtraSmall") return Shape.ExtraSmall;
            else if (text == "ExtraSmallTop") return Shape.ExtraSmallTop;
            else if (text == "Small") return Shape.Small;
            else if (text == "Medium") return Shape.Medium;
            else if (text == "Large") return Shape.Large;
            else if (text == "LargeTop") return Shape.LargeTop;
            else if (text == "LargeEnd") return Shape.LargeEnd;
            else if (text == "ExtraLarge") return Shape.ExtraLarge;
            else if (text == "ExtraLargeTop") return Shape.ExtraLargeTop;
            else if (text == "Full") return Shape.Full;
            else
            {
                var arr = text.Split(',');
                if (arr.Length == 1)
                {
                    if (int.TryParse(arr[0], out var i32))
                    {
                        return new Shape(i32);
                    }
                }
                if (arr.Length == 4)
                {
                    if (int.TryParse(arr[0], out var topLeft) &&
                        int.TryParse(arr[1], out var topRight) &&
                        int.TryParse(arr[2], out var bottomLeft) &&
                        int.TryParse(arr[3], out var bottomRight))
                    {
                        return new Shape(topLeft, topRight, bottomLeft, bottomRight);
                    }
                }
            }
        }
        return null;
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        return new(new[]
        {
            "None",
            "ExtraSmall",
            "ExtraSmallTop",
            "Small",
            "Medium",
            "Large",
            "LargeTop",
            "LargeEnd",
            "ExtraLarge",
            "ExtraLargeTop",
            "Full",
        });
    }
}
