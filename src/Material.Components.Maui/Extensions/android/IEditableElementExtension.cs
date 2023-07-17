using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Views.InputMethods;
using AndroidX.Core.View;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Platform;
using PointF = Microsoft.Maui.Graphics.PointF;
using RectF = Microsoft.Maui.Graphics.RectF;

namespace Material.Components.Maui.Extensions.Platform;

internal static class IEditableElementExtension
{
    public static SizeF GetLayoutSize<TElement>(this TElement element, float maxWidth)
        where TElement : IEditableElement, IFontElement
    {
        maxWidth -= 3;
        var text = element.Text;
        var weight = (int)element.FontWeight;
        var style = element.FontIsItalic ? FontStyleType.Italic : FontStyleType.Normal;
        var font = new Microsoft.Maui.Graphics.Font(element.FontFamily, weight, style);

        var density = Android.App.Application.Context.Resources.DisplayMetrics.Density;

        using var textPaint = new TextPaint { TextSize = element.FontSize * density };
        textPaint.SetTypeface(font.ToTypeface() ?? Typeface.Default);

        if (string.IsNullOrEmpty(text))
            return SizeF.Zero;

        using var bounds = new Android.Graphics.Rect();
        textPaint.TextAlign =
            element.TextAlignment is TextAlignment.End
                ? Android.Graphics.Paint.Align.Right
                : element.TextAlignment is TextAlignment.Center
                    ? Android.Graphics.Paint.Align.Center
                    : Android.Graphics.Paint.Align.Left;

        var alignment =
            element.TextAlignment is TextAlignment.End
                ? Android.Text.Layout.Alignment.AlignOpposite
                : element.TextAlignment is TextAlignment.Center
                    ? Android.Text.Layout.Alignment.AlignCenter
                    : Android.Text.Layout.Alignment.AlignNormal;

        var layout = CreateStaticLayout(text, textPaint, maxWidth, alignment);

        return new SizeF(layout.Width / density, layout.Height / density);
    }

    public static CaretInfo GetCaretInfo<TElement>(
        this TElement element,
        float maxWidth,
        PointF point
    ) where TElement : IEditableElement, IFontElement
    {
        var result = new CaretInfo();

        maxWidth -= 3;
        var text = element.Text;
        var weight = (int)element.FontWeight;
        var style = element.FontIsItalic ? FontStyleType.Italic : FontStyleType.Normal;
        var font = new Microsoft.Maui.Graphics.Font(element.FontFamily, weight, style);

        var density = Android.App.Application.Context.Resources.DisplayMetrics.Density;

        using var textPaint = new TextPaint { TextSize = element.FontSize * density };
        textPaint.SetTypeface(font.ToTypeface() ?? Typeface.Default);

        if (string.IsNullOrEmpty(text))
        {
            var fontMetrics = textPaint.GetFontMetrics();
            result.Position = 0;
            result.X = 0;
            result.Y = 0;
            result.Height = (fontMetrics.Bottom - fontMetrics.Top) / density;
            return result;
        }

        textPaint.TextAlign =
            element.TextAlignment is TextAlignment.End
                ? Android.Graphics.Paint.Align.Right
                : element.TextAlignment is TextAlignment.Center
                    ? Android.Graphics.Paint.Align.Center
                    : Android.Graphics.Paint.Align.Left;

        var alignment =
            element.TextAlignment is TextAlignment.End
                ? Android.Text.Layout.Alignment.AlignOpposite
                : element.TextAlignment is TextAlignment.Center
                    ? Android.Text.Layout.Alignment.AlignCenter
                    : Android.Text.Layout.Alignment.AlignNormal;

        var layout = CreateStaticLayout(text, textPaint, maxWidth, alignment);

        for (var line = 0; line < layout.LineCount; line++)
        {
            var lineBounds = new Android.Graphics.Rect();
            layout.GetLineBounds(line, lineBounds);
            if (lineBounds.Top <= point.Y * density && lineBounds.Bottom >= point.Y * density)
            {
                if (lineBounds.Left <= point.X * density && lineBounds.Right >= point.X * density)
                {
                    result.Position = layout.GetOffsetForHorizontal(line, (int)(point.X * density));
                    result.X = layout.GetPrimaryHorizontal(result.Position) / density;
                    result.Y = lineBounds.Top / density;
                    result.Height = lineBounds.Height() / density;
                    break;
                }
                else if (lineBounds.Left > point.X * density)
                {
                    result.Position = layout.GetLineStart(line);
                    result.X = layout.GetPrimaryHorizontal(result.Position) / density;
                    result.Y = lineBounds.Top / density;
                    result.Height = lineBounds.Height() / density;
                    break;
                }
                else if (lineBounds.Right < point.X * density)
                {
                    result.Position = layout.GetLineEnd(line);
                    result.X = layout.GetSecondaryHorizontal(result.Position) / density;
                    result.Y = lineBounds.Top / density;
                    result.Height = lineBounds.Height() / density;
                    break;
                }
            }
        }

        if (result.IsZero)
        {
            var lineBounds = new Android.Graphics.Rect();
            layout.GetLineBounds(layout.LineCount - 1, lineBounds);

            if (!lineBounds.IsEmpty)
            {
                result.Position = layout.GetLineEnd(layout.LineCount - 1);
                result.X = layout.GetSecondaryHorizontal(result.Position) / density;
                result.Y = lineBounds.Top / density;
                result.Height = lineBounds.Height() / density;
            }
            else
            {
                var fontMetrics = textPaint.GetFontMetrics();
                result.Position = 0;
                result.X = 0;
                result.Y = 0;
                result.Height = (fontMetrics.Bottom - fontMetrics.Top) / density;
            }
        }

        return result;
    }

    public static CaretInfo GetCaretInfo<TElement>(
        this TElement element,
        float maxWidth,
        int position
    ) where TElement : IEditableElement, IFontElement
    {
        var result = new CaretInfo();

        maxWidth -= 3;
        var text = element.Text;
        var weight = (int)element.FontWeight;
        var style = element.FontIsItalic ? FontStyleType.Italic : FontStyleType.Normal;
        var font = new Microsoft.Maui.Graphics.Font(element.FontFamily, weight, style);

        var density = Android.App.Application.Context.Resources.DisplayMetrics.Density;

        using var textPaint = new TextPaint { TextSize = element.FontSize * density };
        textPaint.SetTypeface(font.ToTypeface() ?? Typeface.Default);

        if (string.IsNullOrEmpty(text))
        {
            var fontMetrics = textPaint.GetFontMetrics();
            result.Position = 0;
            result.X =
                element.TextAlignment == TextAlignment.End
                    ? maxWidth
                    : element.TextAlignment == TextAlignment.Center
                        ? maxWidth / 2 + 2
                        : 0;
            result.Y = 0;
            result.Height = (fontMetrics.Bottom - fontMetrics.Top) / density;
            return result;
        }

        textPaint.TextAlign =
            element.TextAlignment is TextAlignment.End
                ? Android.Graphics.Paint.Align.Right
                : element.TextAlignment is TextAlignment.Center
                    ? Android.Graphics.Paint.Align.Center
                    : Android.Graphics.Paint.Align.Left;

        var alignment =
            element.TextAlignment is TextAlignment.End
                ? Android.Text.Layout.Alignment.AlignOpposite
                : element.TextAlignment is TextAlignment.Center
                    ? Android.Text.Layout.Alignment.AlignCenter
                    : Android.Text.Layout.Alignment.AlignNormal;

        var layout = CreateStaticLayout(text, textPaint, maxWidth, alignment);

        var line = layout.GetLineForOffset(position);
        var lineBounds = new Android.Graphics.Rect();
        layout.GetLineBounds(line, lineBounds);

        if (!lineBounds.IsEmpty)
        {
            result.Position = position;
            result.X = layout.GetSecondaryHorizontal(result.Position) / density;
            result.Y = lineBounds.Top / density;
            result.Height = lineBounds.Height() / density;
        }
        else
        {
            var fontMetrics = textPaint.GetFontMetrics();
            result.Position = 0;
            result.X = 0;
            result.Y = 0;
            result.Height = (fontMetrics.Bottom - fontMetrics.Top) / density;
        }

        return result;
    }

    public static (RectF, RectF) GetSelectionRect<TElement>(this TElement element, float maxWidth)
        where TElement : IEditableElement, IFontElement
    {
        maxWidth -= 3;
        var range = element.SelectionRange.Normalized();
        var text = element.Text;
        var weight = (int)element.FontWeight;
        var style = element.FontIsItalic ? FontStyleType.Italic : FontStyleType.Normal;
        var font = new Microsoft.Maui.Graphics.Font(element.FontFamily, weight, style);

        var density = Android.App.Application.Context.Resources.DisplayMetrics.Density;

        using var textPaint = new TextPaint { TextSize = element.FontSize * density };
        textPaint.SetTypeface(font.ToTypeface() ?? Typeface.Default);

        using var bounds = new Android.Graphics.Rect();
        textPaint.TextAlign =
            element.TextAlignment is TextAlignment.End
                ? Android.Graphics.Paint.Align.Right
                : element.TextAlignment is TextAlignment.Center
                    ? Android.Graphics.Paint.Align.Center
                    : Android.Graphics.Paint.Align.Left;

        var alignment =
            element.TextAlignment is TextAlignment.End
                ? Android.Text.Layout.Alignment.AlignOpposite
                : element.TextAlignment is TextAlignment.Center
                    ? Android.Text.Layout.Alignment.AlignCenter
                    : Android.Text.Layout.Alignment.AlignNormal;

        var layout = CreateStaticLayout(text, textPaint, maxWidth, alignment);

        var startLine = layout.GetLineForOffset(range.Start);
        var startLineBounds = new Android.Graphics.Rect();
        layout.GetLineBounds(startLine, startLineBounds);

        var startRect = new RectF
        {
            X = layout.GetPrimaryHorizontal(range.Start) / density,
            Y = startLineBounds.Top / density,
            Height = startLineBounds.Height() / density
        };

        var endLine = layout.GetLineForOffset(range.End);
        var endLineBounds = new Android.Graphics.Rect();
        layout.GetLineBounds(endLine, endLineBounds);

        var endRect = new RectF
        {
            X = layout.GetSecondaryHorizontal(range.End) / density,
            Y = endLineBounds.Top / density,
            Height = endLineBounds.Height() / density
        };

        return (startRect, endRect);
    }

    public static StaticLayout CreateStaticLayout(
        string text,
        TextPaint paint,
        float width,
        Android.Text.Layout.Alignment alignment
    )
    {
        var density = Android.App.Application.Context.Resources.DisplayMetrics.Density;

        StaticLayout layout;

#pragma warning disable CA1416
#pragma warning disable CA1422
        if ((int)Build.VERSION.SdkInt >= 23)
        {
            var layoutBuilder = StaticLayout.Builder.Obtain(
                text,
                0,
                text.Length,
                paint,
                (int)(width * density)
            );
            layoutBuilder.SetAlignment(alignment);
            layout = layoutBuilder.Build();
        }
        else
        {
            layout = new StaticLayout(text, paint, (int)width, alignment, 1.0f, 0.0f, false);
        }
#pragma warning restore CA1422
#pragma warning restore CA1416

        return layout;
    }

    public static bool ShowKeyboard(this BaseTextEditor editor)
    {
        if (editor.Handler?.PlatformView is PlatformTextEditor pv)
        {
            var inputMethodManager = (InputMethodManager)
                pv.Context?.GetSystemService(Context.InputMethodService);
            if (
                inputMethodManager is not null
                && (!inputMethodManager.InvokeIsActive(pv) || !CheckKeyboard(editor))
            )
            {
                if (!pv.HasFocus)
                    pv.RequestFocus();

                return inputMethodManager.ShowSoftInput(pv, 0);
            }
        }

        return false;
    }

    public static bool CheckKeyboard(this BaseTextEditor editor)
    {
        if (editor.Handler?.PlatformView is PlatformTextEditor pv)
        {
            var insets = ViewCompat.GetRootWindowInsets(pv);
            if (insets is null)
            {
                return false;
            }

            var result = insets.IsVisible(WindowInsetsCompat.Type.Ime());
            return result;
        }
        return false;
    }

    public static bool HideKeyboard(this BaseTextEditor editor)
    {
        if (editor.Handler?.PlatformView is PlatformTextEditor pv)
        {
            var focusedView = pv.Context?.GetActivity()?.Window?.CurrentFocus;
            var tokenView = focusedView ?? pv;
            var inputMethodManager = (InputMethodManager)
                tokenView.Context?.GetSystemService(Context.InputMethodService);
            var windowToken = tokenView.WindowToken;
            if (
                inputMethodManager.InvokeIsActive(tokenView)
                && windowToken is not null
                && inputMethodManager is not null
            )
            {
                return inputMethodManager.HideSoftInputFromWindow(
                    windowToken,
                    HideSoftInputFlags.None
                );
            }
        }

        return false;
    }
}
