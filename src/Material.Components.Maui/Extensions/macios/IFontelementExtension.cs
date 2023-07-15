using Microsoft.Maui.Graphics.Platform;

namespace Material.Components.Maui.Extensions.Platform;
internal static class IFontelementExtension
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

        var service = new PlatformStringSizeService();
        var size = service.GetStringSize(text, font, element.FontSize);

        return new SizeF(MathF.Ceiling(size.Width), MathF.Ceiling(size.Height));
    }
}
