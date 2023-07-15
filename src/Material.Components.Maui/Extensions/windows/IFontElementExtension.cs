using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;

namespace Material.Components.Maui.Extensions.Platform;
internal static class IFontElementExtension
{
    internal static SizeF GetStringSize<TElement>(this TElement element)
        where TElement : ITextElement, IFontElement
    {
        return element.GetStringSize(element.Text);
    }

    internal static SizeF GetStringSize<TElement>(this TElement element, string text)
        where TElement : IFontElement
    {
        return element.GetStringSize(text, element.FontSize);
    }

    internal static SizeF GetStringSize<TElement>(this TElement element, string text, float fontSize)
        where TElement : IFontElement
    {
        if (string.IsNullOrEmpty(text))
            return Size.Zero;

        var weight = (int)element.FontWeight;
        var style = element.FontIsItalic ? FontStyleType.Italic : FontStyleType.Normal;

        var font = new Microsoft.Maui.Graphics.Font(element.FontFamily, weight, style);

        var format = new CanvasTextFormat
        {
            FontFamily = font.Name,
            FontSize = fontSize,
            FontWeight = new Windows.UI.Text.FontWeight { Weight = (ushort)font.Weight },
            FontStyle =
                font.StyleType is FontStyleType.Italic
                    ? Windows.UI.Text.FontStyle.Italic
                    : Windows.UI.Text.FontStyle.Normal,
            WordWrapping = CanvasWordWrapping.NoWrap
        };

        var device = CanvasDevice.GetSharedDevice();
        var textLayout = new CanvasTextLayout(device, text, format, 0.0f, 0.0f)
        {
            HorizontalAlignment = CanvasHorizontalAlignment.Left,
            VerticalAlignment = CanvasVerticalAlignment.Top
        };
        var size = new SizeF(
            (float)textLayout.LayoutBounds.Width,
            (float)textLayout.LayoutBounds.Height
        );

        return new SizeF(MathF.Ceiling(size.Width), MathF.Ceiling(size.Height));
    }
}
