#if ANDROID
using Android.Graphics;
using Android.Text;
using Microsoft.Maui.Graphics.Platform;
#endif

namespace Material.Components.Maui.Extensions;

internal static class TextElementExtension
{
    internal static Size GetStringSize(this ITextElement view)
    {
        var font = string.IsNullOrEmpty(view.FontFamily)
            ? Microsoft.Maui.Graphics.Font.Default
            : new Microsoft.Maui.Graphics.Font(view.FontFamily);
#if WINDOWS
        var service = new Microsoft.Maui.Graphics.Win2D.W2DStringSizeService();
        var size = service.GetStringSize(view.Text, font, view.FontSize);
#elif ANDROID
        var textPaint = new TextPaint { TextSize = view.FontSize };
        textPaint.SetTypeface(font.ToTypeface() ?? Typeface.Default);
        var m = textPaint.GetFontMetrics();
        var bounds = new Android.Graphics.Rect();
        textPaint.GetTextBounds(view.Text, 0, view.Text.Length, bounds);
        var size = new Size(bounds.Width(), bounds.Height());
#elif MACCATALYST || IOS
        var service = new Microsoft.Maui.Graphics.Platform.PlatformStringSizeService();
        var size = service.GetStringSize(view.Text, font, view.FontSize);
#endif

        return new Size(Math.Ceiling(size.Width), Math.Ceiling(size.Height));
    }
}
