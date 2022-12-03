using System.ComponentModel;
using System.Globalization;

namespace Material.Components.Maui.Converters;

public class StateLayerOpacityConverter : TypeConverter
{
    public override object ConvertFrom(
        ITypeDescriptorContext context,
        CultureInfo culture,
        object value
    )
    {
        if (value is string text)
        {
            if (text is "normal")
                return StateLayerOpacity.Normal;
            else if (text is "hovered")
                return StateLayerOpacity.Hovered;
            else if (text is "Focused")
                return StateLayerOpacity.Focused;
            else if (text is "pressed")
                return StateLayerOpacity.Pressed;
            else if (float.TryParse(text, out var f))
            {
                return new StateLayerOpacity(f);
            }
        }
        return null;
    }

    public override object ConvertTo(
        ITypeDescriptorContext context,
        CultureInfo culture,
        object value,
        Type destinationType
    )
    {
        var opacity = (StateLayerOpacity)value;
        if (opacity == StateLayerOpacity.Normal)
            return "normal";
        else if (opacity == StateLayerOpacity.Hovered)
            return "hovered";
        else if (opacity == StateLayerOpacity.Focused)
            return "Focused";
        else if (opacity == StateLayerOpacity.Pressed)
            return "pressed";
        else
        {
            return opacity.ToString();
        }
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        return new(new[] { "normal", "hovered", "pressed", "pressed", });
    }
}
