using Microsoft.Maui.Graphics.Platform;

#if ANDROID
using Android.Graphics;
using Android.Text;

#elif WINDOWS
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas;

#elif MACCATALYST || IOS
using CoreGraphics;
using CoreText;
using Foundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Text;
using UIKit;
#endif

namespace Material.Components.Maui.Extensions;

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

#if WINDOWS
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

#elif ANDROID
        using var textPaint = new TextPaint { TextSize = fontSize };
        textPaint.SetTypeface(font.ToTypeface() ?? Typeface.Default);
        using var bounds = new Android.Graphics.Rect();

        textPaint.GetTextBounds(text, 0, text.Length, bounds);
        var size = new SizeF(bounds.Width(), bounds.Height());

#elif MACCATALYST || IOS
        var service = new PlatformStringSizeService();
        var size = service.GetStringSize(text, font, element.FontSize);

#endif
        return new SizeF(MathF.Ceiling(size.Width), MathF.Ceiling(size.Height));
    }

    //#if MACCATALYST || IOS
    //    internal static SizeF GetStringSize(CTFrame frame)
    //    {
    //        var minY = float.MaxValue;
    //        var maxY = float.MinValue;
    //        float width = 0;

    //        var lines = frame.GetLines();
    //        var origins = new CGPoint[lines.Length];
    //        frame.GetLineOrigins(new NSRange(0, 0), origins);

    //        for (var i = 0; i < lines.Length; i++)
    //        {
    //            var line = lines[i];
    //            var lineWidth = (float)
    //                line.GetTypographicBounds(out var ascent, out var descent, out var _);

    //            if (lineWidth > width)
    //            {
    //                width = lineWidth;
    //            }

    //            var origin = origins[i];
    //            minY = (float)Math.Min(minY, origin.Y - ascent);
    //            maxY = (float)Math.Max(maxY, origin.Y + descent);
    //        }

    //        return new SizeF(width, Math.Max(0, maxY - minY));
    //    }
    //#endif
}
