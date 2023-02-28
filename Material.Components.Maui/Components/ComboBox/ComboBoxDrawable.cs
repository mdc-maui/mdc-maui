namespace Material.Components.Maui.Core;

internal class ComboBoxDrawable
{
    private readonly ComboBox view;

    public ComboBoxDrawable(ComboBox view)
    {
        this.view = view;
    }

    public void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        this.DrawBackground(canvas, bounds);
        this.DrawOutline(canvas, bounds);
        this.DrawLabelText(canvas, bounds);
        this.DrawDrapIcon(canvas, bounds);
        this.DrawActiveIndicator(canvas, bounds);
        this.DrawText(canvas, bounds);
    }

    private void DrawBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.BackgroundColour.MultiplyAlpha(this.view.BackgroundOpacity);
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawBackground(bounds, color, radii);
    }

    private void DrawOutline(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.OutlineWidth == 0)
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
                (this.view.IsDropDown && this.view.LabelTextAnimationPercent > 0.5f)
                || (!this.view.IsDropDown && this.view.LabelTextAnimationPercent < 0.5f)
                || (this.view.SelectedIndex != -1)
            )
            {
                canvas.Save();
                canvas.ClipRect(
                    new SKRect(
                        bounds.Left + 12,
                        bounds.Top,
                        bounds.Left + this.view.InternalLabelText.MeasuredWidth + 20,
                        bounds.Top + width
                    )
                );
                canvas.Clear();
                canvas.Restore();
            }
        }
    }

    private void DrawDrapIcon(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var paint = new SKPaint
        {
            IsAntialias = true,
            Color = this.view.ForegroundColor
                .MultiplyAlpha(this.view.ForegroundOpacity)
                .ToSKColor(),
        };
        var path = SKPath.ParseSvgPathData(IconKind.ArrowDropDown.GetData());
        var degrees = this.view.IsDropDown ? 180f : 0f;
        var radians = degrees * (MathF.PI / 180f);
        var sin = (float)Math.Sin(radians);
        var cos = (float)Math.Cos(radians);
        var x = bounds.Right - 36 + (this.view.IsDropDown ? 24 : 0);
        var y = bounds.MidY - 12 + (this.view.IsDropDown ? 24 : 0);
        var matrix = new SKMatrix
        {
            ScaleX = cos,
            SkewX = 0f - sin,
            TransX = x,
            SkewY = sin,
            ScaleY = cos,
            TransY = y,
            Persp2 = 1f,
        };
        path.Transform(matrix);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private void DrawLabelText(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var percent =
            this.view.SelectedIndex != -1 || this.view.IsDropDown
                ? 1 - this.view.LabelTextAnimationPercent
                : this.view.LabelTextAnimationPercent;

        this.view.LabelTextStyle.FontSize =
            this.view.TextStyle.FontSize - ((1f - percent) * this.view.TextStyle.FontSize * 0.25f);
        this.view.LabelTextStyle.TextColor = this.view.LabelTextColor
            .MultiplyAlpha(this.view.LabelTextOpacity)
            .ToSKColor();
        this.view.InternalLabelText.Clear();
        this.view.InternalLabelText.AddText(this.view.LabelText, this.view.LabelTextStyle);
        var y = this.view.IsOutline
            ? ((16 - this.view.InternalLabelText.MeasuredHeight) / 2 + 1)
            : bounds.Top + 8;
        var offsetY =
            (bounds.MidY - (this.view.InternalLabelText.MeasuredHeight / 2) - y) * percent;
        this.view.InternalLabelText.Paint(canvas, new SKPoint(bounds.Left + 16, y + offsetY));
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

    private void DrawText(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.SelectedIndex is -1)
            return;
        canvas.Save();
        var y = this.view.IsOutline
            ? bounds.MidY - (this.view.InternalText.MeasuredHeight / 2)
            : bounds.Bottom - 8 - this.view.InternalText.MeasuredHeight;
        this.view.InternalText.Paint(canvas, new SKPoint(bounds.Left + 16, y));
        canvas.Restore();
    }
}
