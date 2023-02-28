using Microsoft.Maui.Animations;
namespace Material.Components.Maui.Extensions;

internal static class DrawableExtensions
{
    internal static Shape GetShape(this IShapeElement view, float width, float height)
    {
        if (
            view.Shape.TopLeft is -1
            && view.Shape.TopRight is -1
            && view.Shape.BottomLeft is -1
            && view.Shape.BottomRight is -1
        )
        {
            return Math.Min(width, height) / 2;
        }
        return view.Shape;
    }

    internal static SKPoint[] GetRadii(this IShapeElement view, float width, float height)
    {
        var radius = view.GetShape(width, height);
        return new SKPoint[]
        {
            new SKPoint((float)radius.TopLeft, (float)radius.TopLeft),
            new SKPoint((float)radius.TopRight, (float)radius.TopRight),
            new SKPoint((float)radius.BottomRight, (float)radius.BottomRight),
            new SKPoint((float)radius.BottomLeft, (float)radius.BottomLeft),
        };
    }

    internal static SKPoint[] GetRadii(this CornerRadius radius)
    {
        return new SKPoint[]
        {
            new SKPoint((float)radius.TopLeft, (float)radius.TopLeft),
            new SKPoint((float)radius.TopRight, (float)radius.TopRight),
            new SKPoint((float)radius.BottomRight, (float)radius.BottomRight),
            new SKPoint((float)radius.BottomLeft, (float)radius.BottomLeft),
        };
    }

    internal static float GetOverlayOpacity(this IElevationElement view)
    {
        return view.Elevation.Value * 0.05f;
    }

    internal static void DrawBackground(
        this SKCanvas canvas,
        SKRect bounds,
        Color color,
        CornerRadius radius
    )
    {
        var radii = radius.GetRadii();
        canvas.DrawBackground(bounds, color, radii);
    }

    internal static void DrawBackground(
        this SKCanvas canvas,
        SKRect bounds,
        Color color,
        SKPoint[] radii
    )
    {
        canvas.Save();
        var paint = new SKPaint { Color = color.ToSKColor(), };
        var path = new SKPath();
        foreach (var point in radii)
        {
            if (point.X != 0 || point.Y != 0)
            {
                paint.IsAntialias = true;
                var rect = new SKRoundRect();
                rect.SetRectRadii(bounds, radii);
                path.AddRoundRect(rect);
                canvas.DrawPath(path, paint);
                canvas.Restore();
                return;
            }
        }
        path.AddRect(bounds);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    internal static void DrawStateLayer(
        this SKCanvas canvas,
        SKRect bounds,
        Color color,
        SKPoint[] radii
    )
    {
        canvas.Save();
        var paint = new SKPaint { Color = color.ToSKColor(), };
        var path = new SKPath();
        foreach (var point in radii)
        {
            if (point.X != 0 || point.Y != 0)
            {
                paint.IsAntialias = true;
                var rect = new SKRoundRect();
                rect.SetRectRadii(bounds, radii);
                path.AddRoundRect(rect);
                canvas.DrawPath(path, paint);
                canvas.Restore();
                return;
            }
        }
        path.AddRect(bounds);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    internal static void DrawShadow(
        this SKCanvas canvas,
        SKRect bounds,
        Elevation elevation,
        Color color,
        CornerRadius radius
    )
    {
        var radii = radius.GetRadii();
        canvas.DrawShadow(bounds, elevation, color, radii);
    }

    internal static void DrawShadow(
        this SKCanvas canvas,
        SKRect bounds,
        Elevation elevation,
        Color color,
        SKPoint[] radii
    )
    {
        if (elevation.Value is 0)
            return;
        canvas.Save();
        var left = bounds.Left;
        var top = bounds.Top;
        var right = bounds.Right;
        var bottom = bounds.Bottom;
        var sigma = 0.85f * elevation.Value;
        var xDrop = 0.5f * elevation.Value;
        var yDrop = elevation.Value;
        var paint = new SKPaint
        {
            Color = color.ToSKColor(),
            IsAntialias = true,
            MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, sigma, false)
        };
        var path = new SKPath();
        var rect = new SKRoundRect();
        rect.SetRectRadii(new SKRect(left + xDrop, top + yDrop, right, bottom), radii);
        path.AddRoundRect(rect);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    internal static void DrawOverlayLayer(
        this SKCanvas canvas,
        SKRect bounds,
        Elevation elevation,
        CornerRadius radius,
        Color color = null
    )
    {
        var radii = radius.GetRadii();
        canvas.DrawOverlayLayer(bounds, elevation, radii, color);
    }

    internal static void DrawOverlayLayer(
        this SKCanvas canvas,
        SKRect bounds,
        Elevation elevation,
        SKPoint[] radii,
        Color color = null
    )
    {
        canvas.Save();
        color ??= MaterialColors.SurfaceTint;
        var opacity = elevation.Value * 0.05f;
        var paint = new SKPaint { Color = color.MultiplyAlpha(opacity).ToSKColor(), };
        var path = new SKPath();
        foreach (var point in radii)
        {
            if (point.X != 0 || point.Y != 0)
            {
                paint.IsAntialias = true;
                var rect = new SKRoundRect();
                rect.SetRectRadii(bounds, radii);
                path.AddRoundRect(rect);
                canvas.DrawPath(path, paint);
                canvas.Restore();
                return;
            }
        }
        path.AddRect(bounds);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    internal static void DrawOutline(
        this SKCanvas canvas,
        SKRect bounds,
        Color color,
        float width,
        CornerRadius radius
    )
    {
        var radii = radius.GetRadii();
        canvas.DrawOutline(bounds, color, width, radii);
    }

    internal static void DrawOutline(
        this SKCanvas canvas,
        SKRect bounds,
        Color color,
        float width,
        SKPoint[] radii
    )
    {
        canvas.Save();
        var paint = new SKPaint
        {
            Color = color.ToSKColor(),
            StrokeWidth = width,
            IsStroke = true,
            IsAntialias = true,
        };
        var left = bounds.Left + (width / 2);
        var top = bounds.Top + (width / 2);
        var right = bounds.Right - (width / 2);
        var bottom = bounds.Bottom - (width / 2);
        var path = new SKPath();
        var rect = new SKRoundRect();
        rect.SetRectRadii(new SKRect(left, top, right, bottom), radii);
        path.AddRoundRect(rect);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    internal static float GetRippleSize(this SKRectI bounds, SKPoint touchPoint)
    {
        var points = new SKPoint[4];
        points[0].X = points[2].X = touchPoint.X - bounds.Left;
        points[0].Y = points[1].Y = touchPoint.Y - bounds.Top;
        points[1].X = points[3].X = touchPoint.X - bounds.Right;
        points[2].Y = points[3].Y = touchPoint.Y - bounds.Bottom;
        var maxSize = 0f;
        foreach (var point in points)
        {
            var size = MathF.Pow(
                MathF.Pow(point.X - touchPoint.X, 2) + MathF.Pow(point.Y - touchPoint.Y, 2),
                0.5f
            );
            if (size > maxSize)
            {
                maxSize = size;
            }
        }
        return maxSize;
    }

    internal static float GetRippleSize(this SKRect bounds, SKPoint touchPoint)
    {
        var points = new SKPoint[4];
        points[0].X = points[2].X = touchPoint.X - bounds.Left;
        points[0].Y = points[1].Y = touchPoint.Y - bounds.Top;
        points[1].X = points[3].X = touchPoint.X - bounds.Right;
        points[2].Y = points[3].Y = touchPoint.Y - bounds.Bottom;
        var maxSize = 0f;
        foreach (var point in points)
        {
            var size = MathF.Pow(
                MathF.Pow(point.X - touchPoint.X, 2f) + MathF.Pow(point.Y - touchPoint.Y, 2f),
                0.5f
            );
            if (size > maxSize)
            {
                maxSize = size;
            }
        }
        return maxSize;
    }

    internal static void DrawRippleEffect(
        this SKCanvas canvas,
        SKRect bounds,
        CornerRadius radius,
        float rippleSize,
        SKPoint touchPoint,
        Color color,
        float percent,
        bool isClip = true
    )
    {
        var radii = radius.GetRadii();
        canvas.DrawRippleEffect(bounds, radii, rippleSize, touchPoint, color, percent, isClip);
    }

    internal static void DrawRippleEffect(
        this SKCanvas canvas,
        SKRect bounds,
        SKPoint[] radii,
        float rippleSize,
        SKPoint touchPoint,
        Color color,
        float percent,
        bool isClip = true
    )
    {
        if (!bounds.Contains((int)touchPoint.X, touchPoint.Y))
            return;
        canvas.Save();
        if (isClip)
        {
            var path = new SKPath();
            var rect = new SKRoundRect();
            rect.SetRectRadii(bounds, radii);
            path.AddRoundRect(rect);
            canvas.ClipPath(path);
        }
        var paint = new SKPaint
        {
            Color = color.MultiplyAlpha(StateLayerOpacity.Pressed.ToFloat()).ToSKColor(),
            IsAntialias = true,
        };
        var minimumRippleEffectSize = 0.0f;
        var rippleEffectSize = minimumRippleEffectSize.Lerp(rippleSize, percent);
        canvas.DrawCircle(touchPoint, rippleEffectSize, paint);
        canvas.Restore();
    }

    internal static SKPoint ToSKPoint(this System.Drawing.PointF point)
    {
        return new SKPoint(point.X, point.Y);
    }
}
