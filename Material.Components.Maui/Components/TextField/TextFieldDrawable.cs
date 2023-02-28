using Topten.RichTextKit;

namespace Material.Components.Maui.Core;

internal class TextFieldDrawable
{
    private readonly TextField view;
    private float scale;

    public TextFieldDrawable(TextField view)
    {
        this.view = view;
    }

    public void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        this.scale = this.view.TextStyle.FontSize / 16f;
        this.DrawBackground(canvas, bounds);
        this.DrawOutline(canvas, bounds);
        this.DrawPathIcon(canvas, bounds);
        this.DrawImageIcon(canvas, bounds);
        this.DrawPathTrailingIcon(canvas, bounds);
        this.DrawImageTrailingIcon(canvas, bounds);
        this.DrawLabelText(canvas, bounds);
        this.DrawSupportingText(canvas, bounds);
        this.DrawActiveIndicator(canvas, bounds);
        this.DrawText(canvas);
        this.DrawCursor(canvas);
    }

    private void DrawBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.BackgroundColour.MultiplyAlpha(this.view.BackgroundOpacity);
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawBackground(bounds, color, radii);
    }

    private void DrawOutline(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.OutlineWidth is 0)
            return;
        canvas.Save();
        var color = this.view.OutlineColor.MultiplyAlpha(this.view.OutlineOpacity);
        var width = this.view.OutlineWidth;
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawOutline(bounds, color, width, radii);
        canvas.Restore();

        if (this.view.IsOutline)
        {
            if (
                (this.view.InternalFocus && this.view.LabelTextAnimationPercent > 0.5f)
                || (!this.view.InternalFocus && this.view.LabelTextAnimationPercent < 0.5f)
                || (!this.view.InternalFocus && this.view.TextDocument.Length > 1)
            )
            {
                canvas.Save();
                var x = this.view.TextDocument.MarginLeft - 4f * this.scale;
                canvas.ClipRect(
                    new SKRect(
                        x,
                        bounds.Top,
                        x + this.view.InternalLabelText.MeasuredWidth + 8f * this.scale,
                        bounds.Top + width
                    )
                );
                canvas.Clear();
                canvas.Restore();
            }
        }
    }

    private void DrawPathIcon(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.IconSource != null || string.IsNullOrEmpty(this.view.IconData))
            return;
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.ForegroundColor
                .MultiplyAlpha(this.view.ForegroundOpacity)
                .ToSKColor(),
            IsAntialias = true,
        };
        var path = SKPath.ParseSvgPathData(this.view.IconData);
        var x = bounds.Left + 16f * this.scale;
        var y = bounds.MidY - 12f * this.scale;
        var matrix = new SKMatrix
        {
            ScaleX = scale,
            ScaleY = scale,
            TransX = x,
            TransY = y,
            Persp2 = 1f
        };
        path.Transform(matrix);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private void DrawImageIcon(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.IconSource == null)
            return;
        canvas.Save();
        var paint = new SKPaint
        {
            IsAntialias = true,
            ColorFilter = SKColorFilter.CreateBlendMode(
                this.view.ForegroundColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor(),
                SKBlendMode.SrcIn
            )
        };
        var iconScale = 24 / this.view.IconSource.CullRect.Width * this.scale;
        var x = bounds.Left + 16f * this.scale;
        var y = bounds.MidY - 12f * this.scale;
        var matrix = new SKMatrix
        {
            ScaleX = iconScale,
            ScaleY = iconScale,
            TransX = x,
            TransY = y,
            Persp2 = 1f
        };
        canvas.DrawPicture(this.view.IconSource, ref matrix, paint);
        canvas.Restore();
    }

    private void DrawPathTrailingIcon(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.TrailingIconSource != null || string.IsNullOrEmpty(this.view.TrailingIconData))
            return;
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.TrailingIconColor
                .MultiplyAlpha(this.view.ForegroundOpacity)
                .ToSKColor(),
            IsAntialias = true,
        };
        var path = SKPath.ParseSvgPathData(this.view.TrailingIconData);
        var x = bounds.Right - (12f + 24f) * this.scale;
        var y = bounds.MidY - 12f * this.scale;
        var matrix = new SKMatrix
        {
            ScaleX = scale,
            ScaleY = scale,
            TransX = x,
            TransY = y,
            Persp2 = 1f
        };
        path.Transform(matrix);
        canvas.DrawPath(path, paint);
        canvas.Restore();

        this.view.TrailingIconBounds = new SKRect(x, y, x + 24f * this.scale, y + 24f * this.scale);
    }

    private void DrawImageTrailingIcon(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.TrailingIconSource == null)
            return;
        canvas.Save();
        var paint = new SKPaint
        {
            IsAntialias = true,
            ColorFilter = SKColorFilter.CreateBlendMode(
                this.view.TrailingIconColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor(),
                SKBlendMode.SrcIn
            )
        };
        var iconScale = 24f / this.view.TrailingIconSource.CullRect.Width * this.scale;
        var x = bounds.Right - (12f + 24f) * this.scale;
        var y = bounds.MidY - 12f * this.scale;
        var matrix = new SKMatrix
        {
            ScaleX = iconScale,
            ScaleY = iconScale,
            TransX = x,
            TransY = y,
            Persp2 = 1f
        };
        canvas.DrawPicture(this.view.TrailingIconSource, ref matrix, paint);
        canvas.Restore();

        this.view.TrailingIconBounds = new SKRect(x, y, x + 24f * this.scale, y + 24f * this.scale);
    }

    private void DrawLabelText(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var percent =
            this.view.InternalFocus || this.view.TextDocument.Length > 1
                ? 1 - this.view.LabelTextAnimationPercent
                : this.view.LabelTextAnimationPercent;

        this.view.LabelTextStyle.FontSize =
            this.view.TextStyle.FontSize - ((1f - percent) * this.view.TextStyle.FontSize * 0.25f);
        this.view.LabelTextStyle.TextColor = this.view.LabelTextColor
            .MultiplyAlpha(this.view.LabelTextOpacity)
            .ToSKColor();
        this.view.InternalLabelText.Clear();
        this.view.InternalLabelText.AddText(this.view.LabelText, this.view.LabelTextStyle);
        var x = this.view.TextDocument.MarginLeft;
        var y = this.view.IsOutline
            ? ((16 * this.scale - this.view.InternalLabelText.MeasuredHeight) / 2 + 1)
            : bounds.Top + 8 * this.scale;
        var offsetY =
            (bounds.MidY - (this.view.InternalLabelText.MeasuredHeight / 2) - y) * percent;
        this.view.InternalLabelText.Paint(canvas, new SKPoint(x, y + offsetY));
        canvas.Restore();
    }

    private void DrawSupportingText(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        this.view.SupportingTextStyle.TextColor = this.view.SupportingTextColor
            .MultiplyAlpha(this.view.SupportingTextOpacity)
            .ToSKColor();
        var x = bounds.Left + 16f * this.scale;
        var y = bounds.Bottom + 4f * this.scale;
        this.view.InternalSupportingText.Paint(canvas, new SKPoint(x, y));
        canvas.Restore();
    }

    private void DrawActiveIndicator(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.ActiveIndicatorHeight is 0)
            return;
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.ActiveIndicatorColor
                .MultiplyAlpha(this.view.ActiveIndicatorOpacity)
                .ToSKColor(),
            IsAntialias = true,
        };
        var x = bounds.Left;
        var y = bounds.Bottom - this.view.ActiveIndicatorHeight;
        canvas.DrawRect(x, y, bounds.Width, this.view.ActiveIndicatorHeight, paint);
        canvas.Restore();
    }

    private void DrawText(SKCanvas canvas)
    {
        canvas.Save();
        var caretColor = this.view.CaretColor.ToSKColor();
        var options = new TextPaintOptions();
        var range = this.view.SelectionTextRange;
        if (range.IsRange)
        {
            options.Selection = range;
            options.SelectionColor = caretColor.WithAlpha(200);
        }
        this.view.TextDocument.Paint(canvas, 0f, this.view.TextDocument.MeasuredHeight, options);
        canvas.Restore();
    }

    private void DrawCursor(SKCanvas canvas)
    {
#if ANDROID || IOS
        var range = this.view.SelectionTextRange;
        if (!range.IsRange) return;

        canvas.Save();

        var leftCaretInfo = this.view.TextDocument.GetCaretInfo(
                new CaretPosition(range.Start, false)
            );
        var rightCaretInfo = this.view.TextDocument.GetCaretInfo(
                new CaretPosition(range.End, false)
            );

        {
            var paint = new SKPaint
            {
                Color = this.view.CaretColor.ToSKColor(),
                IsAntialias = true,
            };
            var x = leftCaretInfo.CaretRectangle.Left - 24;
            var y = leftCaretInfo.CaretRectangle.Bottom;
            canvas.DrawRect(x + 12, y, 12, 12, paint);
            canvas.DrawCircle(x + 12, y + 12, 12, paint);
        }

        {
            var paint = new SKPaint
            {
                Color = this.view.CaretColor.ToSKColor(),
                IsAntialias = true,
            };
            var x = rightCaretInfo.CaretRectangle.Right;
            var y = rightCaretInfo.CaretRectangle.Bottom;
            canvas.DrawRect(x, y, 12, 12, paint);
            canvas.DrawCircle(x + 12, y + 12, 12, paint);
        }

        canvas.Restore();
#endif
    }

    internal void DrawCaret(SKCanvas canvas)
    {
        canvas.Save();
        var caretColor = this.view.CaretColor.ToSKColor();
        var range = this.view.SelectionTextRange;
        if (this.view.InternalFocus && !range.IsRange)
        {
            var caretInfo = this.view.TextDocument.GetCaretInfo(
                new CaretPosition(range.End, false)
            );
            var paint = new SKPaint
            {
                Color = caretColor,
                IsAntialias = true,
                IsStroke = false,
            };
            var caretBounds = caretInfo.CaretRectangle;
            canvas.DrawRect(caretBounds.Left, caretBounds.Top, 2, caretBounds.Height, paint);
        }
        canvas.Restore();
    }
}
