using Svg.Skia;

namespace Material.Components.Maui.Core;

internal class NavigationBarItemDrawable
{
    private readonly NavigationBarItem view;

    internal NavigationBarItemDrawable(NavigationBarItem view)
    {
        this.view = view;
    }

    internal void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear(this.view.BackgroundColour.ToSKColor());
        this.DrawOverlayLayer(canvas, bounds);
        this.DrawStateLayer(canvas, bounds);
        this.DrawActiveIndicator(canvas, bounds);
        this.DrawPathIcon(canvas, bounds);
        this.DrawImageIcon(canvas, bounds);
        this.DrawText(canvas, bounds);
        this.DrawRippleEffect(canvas, bounds);
    }

    private void DrawBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.BackgroundColour.MultiplyAlpha(this.view.BackgroundOpacity);
        var radii = new CornerRadius(0).GetRadii();
        canvas.DrawBackground(bounds, color, radii);
    }

    private void DrawActiveIndicator(SKCanvas canvas, SKRect bounds)
    {
        if (!this.view.IsActived)
            return;
        canvas.Save();
        var percent = this.view.RipplePercent is 0f ? 1f : this.view.RipplePercent;
        var _bounds = new SKRect
        {
            Left = (bounds.Width / 2) - 16 - (16 * percent),
            Top = this.view.HasLabel ? 12 : bounds.MidY - 16,
            Right = (bounds.Width / 2) + 16 + (16 * percent),
            Bottom = this.view.HasLabel ? 44 : bounds.MidY + 16,
        };
        canvas.DrawBackground(_bounds, this.view.ActiveIndicatorColor, 16);
        canvas.Restore();
    }

    private void DrawOverlayLayer(SKCanvas canvas, SKRect bounds)
    {
        var radii = new CornerRadius(0).GetRadii();
        var _bounds = new SKRect(
            bounds.Left,
            bounds.Top,
            bounds.Right,
            bounds.Bottom
        );
        canvas.DrawOverlayLayer(_bounds, Elevation.Level2, radii);
    }

    private void DrawStateLayer(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var _bounds = new SKRect
        {
            Left = (bounds.Width / 2) - 32,
            Top = this.view.HasLabel ? 12 : bounds.MidY - 16,
            Right = (bounds.Width / 2) + 32,
            Bottom = this.view.HasLabel ? 44 : bounds.MidY + 16,
        };
        var color = this.view.StateLayerColor.MultiplyAlpha(this.view.StateLayerOpacity);
        canvas.DrawBackground(_bounds, color, 16);
        canvas.Restore();
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
        if (this.view.IsActived && !string.IsNullOrEmpty(this.view.ActivedIconData))
        {
            path = SKPath.ParseSvgPathData(this.view.ActivedIconData);
        }
        var x = (bounds.Width / 2) - 12;
        var y = this.view.HasLabel ? 16 : bounds.MidY - 12;
        path.Offset(x, y);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private void DrawImageIcon(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.IconSource is null)
            return;
        canvas.Save();
        var paint = new SKPaint { IsAntialias = true, };
        if (this.view.ForegroundOpacity != 1)
        {
            paint.ColorFilter = SKColorFilter.CreateBlendMode(
                this.view.ForegroundColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor(),
                SKBlendMode.SrcIn
            );
        }

        var scale = 24 / this.view.IconSource.CullRect.Width;
        var x = (bounds.Width / 2) - 12;
        var y = this.view.HasLabel ? 16 : bounds.MidY - 12;
        var matrix = new SKMatrix
        {
            ScaleX = scale,
            ScaleY = scale,
            TransX = x,
            TransY = y,
            Persp2 = 1f
        };
        if (this.view.IsActived && this.view.ActivedIconSource != null)
        {
            canvas.DrawPicture(this.view.ActivedIconSource, ref matrix, paint);
        }
        else
        {
            canvas.DrawPicture(this.view.IconSource, ref matrix, paint);
        }
        canvas.Restore();
    }

    private void DrawText(SKCanvas canvas, SKRect bounds)
    {
        if (!this.view.HasLabel)
            return;
        canvas.Save();
        var x = bounds.MidX - (this.view.InternalText.MeasuredWidth / 2);
        var y = 48 + ((16 - this.view.InternalText.MeasuredHeight) / 2);
        this.view.InternalText.Paint(canvas, new SKPoint(x, y));
        canvas.Restore();
    }

    private void DrawRippleEffect(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.RipplePercent < 0)
            return;
        var color = this.view.RippleColor;
        var _bounds = new SKRect
        {
            Left = (bounds.Width / 2) - 32,
            Top = this.view.HasLabel ? 12 : bounds.MidY - 16,
            Right = (bounds.Width / 2) + 32,
            Bottom = this.view.HasLabel ? 44 : bounds.MidY + 16,
        };
        var point = new SKPoint(_bounds.MidX, _bounds.MidY);
        canvas.DrawRippleEffect(_bounds, 16, 32, point, color, this.view.RipplePercent);
    }
}
