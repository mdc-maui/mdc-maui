using CoreGraphics;
using CoreText;
using Foundation;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Platform;
using FontExtensions = Microsoft.Maui.Graphics.Platform.FontExtensions;

namespace Material.Components.Maui.Extensions;

internal static class PlatformCanvasExtension
{
    internal static void PlatformDrawText(
        this ICanvas canvas,
        ITextElement element,
        RectF rect,
        HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment verticalAlignment = VerticalAlignment.Center,
        float ix = 0f,
        float iy = 0f
    )
    {
        float rx = rect.X;
        float ry = -rect.Y;
        float rw = rect.Width;
        float rh = rect.Height;
        var cGRect = new CGRect(rx, ry, rw, rh);

        using var path = new CGPath();
        path.AddRect(cGRect);

        var platformCanvas = canvas as PlatformCanvas;
        var context = platformCanvas.Context;

        context.SaveState();
        context.TranslateCTM(0, cGRect.Height);
        context.ScaleCTM(1, -1f);

        context.TextMatrix = CGAffineTransform.MakeIdentity();
        context.TextMatrix.Translate(0, 0);

        using var attributedString = new NSMutableAttributedString(element.Text);

        var attributes = new CTStringAttributes();

        var font = new Microsoft.Maui.Graphics.Font(element.FontFamily);

        var platformFont =
            font.ToCTFont(element.FontSize) ?? FontExtensions.GetDefaultCTFont(element.FontSize);

        var traits = CTFontSymbolicTraits.None;
        if (element.FontAttributes is FontAttributes.Italic)
            traits |= CTFontSymbolicTraits.Italic;
        else if (element.FontAttributes is FontAttributes.Bold)
            traits |= CTFontSymbolicTraits.Bold;
        else if (element.FontAttributes is FontAttributes.BoldItalic)
            traits |= CTFontSymbolicTraits.Italic | CTFontSymbolicTraits.Bold;

        platformFont = platformFont.WithSymbolicTraits(element.FontSize, traits, traits);

        attributes.Font = platformFont;
        attributes.ForegroundColor = element.TextColor.ToCGColor();

        if (verticalAlignment == VerticalAlignment.Center)
        {
            iy += -(float)(platformFont.DescentMetric / 2);
        }
        else if (verticalAlignment == VerticalAlignment.Bottom)
        {
            iy += -(float)(platformFont.DescentMetric);
        }

        var paragraphSettings = new CTParagraphStyleSettings();
        switch (horizontalAlignment)
        {
            case HorizontalAlignment.Left:
                paragraphSettings.Alignment = CTTextAlignment.Left;
                break;
            case HorizontalAlignment.Center:
                paragraphSettings.Alignment = CTTextAlignment.Center;
                break;
            case HorizontalAlignment.Right:
                paragraphSettings.Alignment = CTTextAlignment.Right;
                break;
            case HorizontalAlignment.Justified:
                paragraphSettings.Alignment = CTTextAlignment.Justified;
                break;
        }

        using var paragraphStyle = new CTParagraphStyle(paragraphSettings);
        attributes.ParagraphStyle = paragraphStyle;
        attributedString.SetAttributes(attributes, new NSRange(0, element.Text.Length));

        using var framesetter = new CTFramesetter(attributedString);

        using var frame = framesetter.GetFrame(new NSRange(0, 0), path, null);

        if (frame != null)
        {
            if (verticalAlignment != VerticalAlignment.Top)
            {
                var textSize = TextElementExtension.GetTextSize(frame);
                if (textSize.Height > 0)
                {
                    if (verticalAlignment == VerticalAlignment.Bottom)
                    {
                        var dy = cGRect.Height - textSize.Height + iy;
                        context.TranslateCTM(-ix, -dy);
                    }
                    else
                    {
                        var dy = (cGRect.Height - textSize.Height) / 2 + iy;
                        context.TranslateCTM(-ix, -dy);
                    }
                }
            }
            else
            {
                context.TranslateCTM(-ix, -iy);
            }

            frame.Draw(context);
        }
        platformFont.Dispose();
        context.RestoreState();
    }
}
