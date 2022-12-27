using Material.Components.Maui;
using Label = Material.Components.Maui.Label;

namespace SampleApp.Panels;

internal static class Extensions
{
    internal static void AddPropertyDescribe(
        this VerticalStackLayout stack,
        string name,
        string type,
        string describe
    )
    {
        var fontSize = 16f;
        var labelName = new Label
        {
            Text = name,
            FontSize = fontSize,
            Shape = 5,
            Padding = new Thickness(2, 0),
        };
        labelName.SetDynamicResource(Label.BackgroundColourProperty, "SurfaceVariantColor");

        var labelType = new Label
        {
            Text = type,
            FontSize = fontSize,
            Shape = 5,
            Padding = new Thickness(2, 0),
        };
        labelType.SetDynamicResource(Label.BackgroundColourProperty, "SurfaceVariantColor");

        var labelOf = new Label { Text = ",  of type", FontSize = fontSize, };
        labelOf.SetDynamicResource(Label.ForegroundColorProperty, "OnBackgroundColor");

        var labelDescribe = new Label { Text = $",  {describe}", FontSize = fontSize, };
        // labelDescribe.SetDynamicResource(Label.ForegroundColorProperty, "OnBackgroundColor");

        var wrap = new WrapLayout { HorizontalSpacing = 2 };
        wrap.Add(labelName);
        wrap.Add(labelOf);
        wrap.Add(labelType);
        wrap.Add(labelDescribe);

        stack.Add(wrap);
    }
}
