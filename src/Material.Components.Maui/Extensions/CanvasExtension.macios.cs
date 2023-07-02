//using CoreGraphics;
//using CoreText;
//using Foundation;
//using Microsoft.Maui.Graphics.Platform;
//using Microsoft.Maui.Platform;
//using FontExtensions = Microsoft.Maui.Graphics.Platform.FontExtensions;

//namespace Material.Components.Maui.Extensions;

//internal static class PlatformCanvasExtension
//{
//    internal static void PlatformDrawText<TElement>(
//        this ICanvas canvas,
//        TElement element,
//        string text,
//        Color fontColor,
//        float fontSize,
//        RectF rect,
//        HorizontalAlignment horizontal = HorizontalAlignment.Center,
//        VerticalAlignment vertical = VerticalAlignment.Center,
//        float ix = 0f,
//        float iy = 0f
//    ) where TElement : IFontElement
//    {
//        var rx = rect.X;
//        var ry = -rect.Y;
//        var rw = rect.Width;
//        var rh = rect.Height;
//        var cGRect = new CGRect(rx, ry, rw, rh);

//        using var path = new CGPath();
//        path.AddRect(cGRect);

//        var platformCanvas = canvas as PlatformCanvas;
//        var context = platformCanvas.Context;

//        context.SaveState();
//        context.TranslateCTM(0, cGRect.Height);
//        context.ScaleCTM(1, -1f);

//        context.TextMatrix = CGAffineTransform.MakeIdentity();
//        context.TextMatrix.Translate(0, 0);

//        using var attributedString = new NSMutableAttributedString(text);

//        var attributes = new CTStringAttributes();

//        var font = new Microsoft.Maui.Graphics.Font(element.FontFamily);

//        var platformFont =
//            font.ToCTFont(fontSize) ?? FontExtensions.GetDefaultCTFont(fontSize);

//        var traits = CTFontSymbolicTraits.None;
//        if (element.FontWeight is FontAttributes.Italic)
//            traits |= CTFontSymbolicTraits.Italic;
//        else if (element.FontWeight is FontAttributes.Bold)
//            traits |= CTFontSymbolicTraits.Bold;
//        else if (element.FontWeight is FontAttributes.BoldItalic)
//            traits |= CTFontSymbolicTraits.Italic | CTFontSymbolicTraits.Bold;

//        platformFont = platformFont.WithSymbolicTraits(fontSize, traits, traits);

//        attributes.Font = platformFont;
//        attributes.ForegroundColor = fontColor.ToCGColor();

//        if (vertical == VerticalAlignment.Center)
//        {
//            iy += -(float)(platformFont.DescentMetric / 2);
//        }
//        else if (vertical == VerticalAlignment.Bottom)
//        {
//            iy += -(float)platformFont.DescentMetric;
//        }

//        var paragraphSettings = new CTParagraphStyleSettings();
//        switch (horizontal)
//        {
//            case HorizontalAlignment.Left:
//                paragraphSettings.Alignment = CTTextAlignment.Left;
//                break;
//            case HorizontalAlignment.Center:
//                paragraphSettings.Alignment = CTTextAlignment.Center;
//                break;
//            case HorizontalAlignment.Right:
//                paragraphSettings.Alignment = CTTextAlignment.Right;
//                break;
//            case HorizontalAlignment.Justified:
//                paragraphSettings.Alignment = CTTextAlignment.Justified;
//                break;
//        }

//        using var paragraphStyle = new CTParagraphStyle(paragraphSettings);
//        attributes.ParagraphStyle = paragraphStyle;
//        attributedString.SetAttributes(attributes, new NSRange(0, text.Length));

//        using var framesetter = new CTFramesetter(attributedString);

//        using var frame = framesetter.GetFrame(new NSRange(0, 0), path, null);

//        if (frame != null)
//        {
//            if (vertical != VerticalAlignment.Top)
//            {
//                var textSize = IFontElementExtension.GetStringSize(frame);
//                if (textSize.Height > 0)
//                {
//                    if (vertical == VerticalAlignment.Bottom)
//                    {
//                        var dy = cGRect.Height - textSize.Height + iy;
//                        context.TranslateCTM(-ix, -dy);
//                    }
//                    else
//                    {
//                        var dy = (cGRect.Height - textSize.Height) / 2 + iy;
//                        context.TranslateCTM(-ix, -dy);
//                    }
//                }
//            }
//            else
//            {
//                context.TranslateCTM(-ix, -iy);
//            }

//            frame.Draw(context);
//        }
//        platformFont.Dispose();
//        context.RestoreState();
//    }
//}
