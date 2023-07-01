#if ANDROID
using Android.Graphics;
using Android.Text;

#elif MACCATALYST || IOS
using CoreGraphics;
using CoreText;
using Foundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Text;
using UIKit;
#endif

using Microsoft.Maui.Graphics.Platform;

namespace Material.Components.Maui.Extensions;

internal static class TextElementExtension
{
    internal static Size GetStringSize<TElement>(this TElement element) where TElement : ITextElement, IFontElement
    {
        return element.GetStringSize(element.Text);
    }

    internal static Size GetStringSize<TElement>(this TElement element, string text) where TElement : IFontElement
    {
        if (string.IsNullOrEmpty(text))
            return Size.Zero;

        var weight = element.FontAttributes is FontAttributes.Bold or FontAttributes.BoldItalic
            ? FontWeight.Bold
            : FontWeight.Regular;
        var style = element.FontAttributes is FontAttributes.Italic or FontAttributes.BoldItalic
            ? FontStyleType.Italic
            : FontStyleType.Normal;

        var font = new Microsoft.Maui.Graphics.Font(element.FontFamily, (int)weight, style);

#if WINDOWS
        var service = new PlatformStringSizeService();
        var size = service.GetStringSize(text, font, element.FontSize);
#elif ANDROID
        using var textPaint = new TextPaint { TextSize = element.FontSize };
        textPaint.SetTypeface(font.ToTypeface() ?? Typeface.Default);
        using var bounds = new Android.Graphics.Rect();

        textPaint.GetTextBounds(text, 0, text.Length, bounds);
        var size = new Size(bounds.Width(), bounds.Height());
#elif MACCATALYST || IOS
        using var path = new CGPath();
        path.AddRect(new CGRect(0, 0, 10000, 10000));
        var attrText = new AttributedText(text, null);
        using var attrString = attrText.AsNSAttributedString(
            font,
            element.FontSize,
            element.FontColor.ToHex(),
            true
        );

        using var framesetter = new CTFramesetter(attrString);
        using var frame = framesetter.GetFrame(new NSRange(0, 0), path, null);
        var size = GetTextSize(frame);

#endif
        return new Size(Math.Ceiling(size.Width), Math.Ceiling(size.Height));
    }

#if MACCATALYST || IOS
    internal static SizeF GetTextSize(CTFrame frame)
    {
        var minY = float.MaxValue;
        var maxY = float.MinValue;
        float width = 0;

        var lines = frame.GetLines();
        var origins = new CGPoint[lines.Length];
        frame.GetLineOrigins(new NSRange(0, 0), origins);

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var lineWidth = (float)
                line.GetTypographicBounds(out var ascent, out var descent, out var _);

            if (lineWidth > width)
            {
                width = lineWidth;
            }

            var origin = origins[i];
            minY = (float)Math.Min(minY, origin.Y - ascent);
            maxY = (float)Math.Max(maxY, origin.Y + descent);
        }

        return new SizeF(width, Math.Max(0, maxY - minY));
    }
#endif
}
