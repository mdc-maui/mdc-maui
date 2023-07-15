using Android.Graphics;
using Android.Text;
using Microsoft.Maui.Graphics.Platform;

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

        var density = Android.App.Application.Context.Resources.DisplayMetrics.Density;
        using var textPaint = new TextPaint { TextSize = fontSize * density };
        textPaint.SetTypeface(font.ToTypeface() ?? Typeface.Default);
        using var bounds = new Android.Graphics.Rect();

        textPaint.GetTextBounds(text, 0, text.Length, bounds);
        var size = new SizeF(bounds.Width(), bounds.Height());
        return new SizeF(MathF.Ceiling(size.Width / density), MathF.Ceiling(size.Height / density));
    }
}
