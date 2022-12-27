using System.Diagnostics;

namespace Material.Components.Maui.Core;

internal class CheckBoxDrawable
{
    private readonly CheckBox view;

    public CheckBoxDrawable(CheckBox checkBox)
    {
        this.view = checkBox;
    }

    internal void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        if (!this.view.IsChecked)
        {
            this.DrawBox(canvas, bounds);
            this.DrawUnCkecked(canvas, bounds);
        }
        else
        {
            this.DrawBackgound(canvas, bounds);
            this.DrawMarkIcon(canvas, bounds);
        }
        this.DrawStateLayer(canvas, bounds);
        this.DrawText(canvas, bounds);
        this.DrawRippleEffect(canvas, bounds);
    }

    private void DrawBox(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.ForegroundColor
                .MultiplyAlpha(this.view.ForegroundOpacity)
                .ToSKColor(),
            IsStroke = true,
            StrokeWidth = 1,
            IsAntialias = true,
        };
        var radii = new CornerRadius(2).GetRadii();
        var path = new SKPath();
        var rect = new SKRoundRect();
        rect.SetRectRadii(
            new SKRect(bounds.Left + 11, bounds.Top + 11, bounds.Left + 29, bounds.Bottom - 11),
            radii
        );
        path.AddRoundRect(rect);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private void DrawUnCkecked(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.ChangingPercent is not 1f and not 0f)
        {
            canvas.Save();
            var paint = new SKPaint
            {
                Color = this.view.ForegroundColor
                    .MultiplyAlpha(this.view.ForegroundOpacity)
                    .ToSKColor(),
                IsAntialias = true,
            };

            var radii = new CornerRadius(2).GetRadii();
            var path = new SKPath();
            var rect = new SKRoundRect();
            rect.SetRectRadii(
                new SKRect(bounds.Left + 11, bounds.Top + 11, bounds.Left + 29, bounds.Bottom - 11),
                radii
            );
            path.AddRoundRect(rect);
            canvas.DrawPath(path, paint);
            canvas.Restore();
        }

        {
            canvas.Save();

            var paint = new SKPaint
            {
                Color = MaterialColors.SecondaryContainer.ToSKColor(),
                IsAntialias = true,
            };

            var radii = new CornerRadius(2).GetRadii();
            var path = new SKPath();
            var rect = new SKRoundRect();
            rect.SetRectRadii(new SKRect(0, 0, 18, 18), radii);
            path.AddRoundRect(rect);

            var matrix = new SKMatrix
            {
                ScaleX = this.view.ChangingPercent,
                ScaleY = this.view.ChangingPercent,
                TransX = bounds.Left + 11 + ((1 - this.view.ChangingPercent) * 9),
                TransY = bounds.Top + 11 + ((1 - this.view.ChangingPercent) * 9),
                Persp2 = 1f
            };
            path.Transform(matrix);
            canvas.DrawPath(path, paint);
            canvas.Restore();
        }
    }

    private void DrawBackgound(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.OnColor.MultiplyAlpha(this.view.OnOpacity).ToSKColor(),
            IsAntialias = true,
        };
        var radii = new CornerRadius(2).GetRadii();
        var path = new SKPath();
        var rect = new SKRoundRect();
        rect.SetRectRadii(
            new SKRect(bounds.Left + 11, bounds.Top + 11, bounds.Left + 29, bounds.Bottom - 11),
            radii
        );
        path.AddRoundRect(rect);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private void DrawMarkIcon(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        canvas.ClipRect(
            new SKRect(
                bounds.Left + 11,
                bounds.Top + 11,
                bounds.Left + 11 + (18 * this.view.ChangingPercent),
                bounds.Bottom - 11
            )
        );

        var path = SKPath.ParseSvgPathData("m 7,10.9 -2.6,-2.6 -1.4,1.4 4,4 8,-8 -1.4,-1.4 z");
        path.Offset(bounds.Left + 11, bounds.Top + 11);
        var paint = new SKPaint { Color = this.view.MarkColor.ToSKColor(), IsAntialias = true, };
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    internal void DrawStateLayer(SKCanvas canvas, SKRect bounds)
    {
#if !__MOBILE__
        if (this.view.StateLayerOpacity != 0f)
        {
            var color = this.view.StateLayerColor.MultiplyAlpha(this.view.StateLayerOpacity);
            var radii = new CornerRadius(20).GetRadii();
            canvas.DrawStateLayer(
                new SKRect(bounds.Left, bounds.Top, bounds.Left + 40, bounds.Bottom),
                color,
                radii
            );
        }
#endif
    }

    private void DrawText(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();

        this.view.TextStyle.TextColor = this.view.ForegroundColor
            .MultiplyAlpha(this.view.ForegroundOpacity)
            .ToSKColor();
        var x = 40;
        var y = bounds.MidY - (this.view.TextBlock.MeasuredHeight / 2);
        this.view.TextBlock.Paint(canvas, new SKPoint(x, y));

        canvas.Restore();
    }

    private void DrawRippleEffect(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.RipplePercent == 0f)
            return;

        var _bounds = new SKRect(bounds.Left, bounds.Top, bounds.Left + 40, bounds.Bottom);
        var color = this.view.RippleColor;
        var point = new SKPoint(Math.Min(39, this.view.TouchPoint.X), this.view.TouchPoint.Y);
        var size = _bounds.GetRippleSize(point);
        canvas.DrawRippleEffect(_bounds, 20, size, point, color, this.view.RipplePercent, true);
    }
}
