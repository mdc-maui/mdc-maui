using System.ComponentModel;
using System.Globalization;

namespace Material.Components.Maui.Converters;

public class StateLayerOpacityConverter : TypeConverter
{
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string text)
        {
            if (text is "Normal") return StateLayerOpacity.Normal;
            else if (text is "Hovered") return StateLayerOpacity.Hovered;
            else if (text is "Focused") return StateLayerOpacity.Focused;
            else if (text is "Pressed") return StateLayerOpacity.Pressed;
            else if (float.TryParse(text, out var f))
            {
                return new StateLayerOpacity(f);
            }
        }
        return null;
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        var opacity = (StateLayerOpacity)value;
        if (opacity == StateLayerOpacity.Normal) return "Normal";
        else if (opacity == StateLayerOpacity.Hovered) return "Hovered";
        else if (opacity == StateLayerOpacity.Focused) return "Focused";
        else if (opacity == StateLayerOpacity.Pressed) return "Pressed";
        else
        {
            return opacity.ToString();
        }
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        return new(new[]
        {
            "Normal",
            "Hovered",
            "Pressed",
            "Pressed",
        });
    }
}
