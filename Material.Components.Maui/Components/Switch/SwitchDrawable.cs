using Microsoft.Maui.Animations;

namespace Material.Components.Maui.Core;

internal class SwitchDrawable
{
    private readonly Switch view;

    public SwitchDrawable(Switch view)
    {
        this.view = view;
    }

    internal void Draw(SKCanvas canvas, SKRect bounds)
    {
        var lx = 24;
        var rx = bounds.Right - 16;
        var offX = rx - lx;
        var cx = this.GetThumbX(lx, rx, offX);
        var cy = 24;
        canvas.Clear();
        this.DrawTrack(canvas, bounds);
        this.DrawOutline(canvas, bounds);
        this.DrawStateLayer(canvas, cx, cy);
        this.DrawThumb(canvas, cx, cy);
        if (this.view.HasIcon)
        {
            this.DrawIcon(canvas, cx, cy);
        }

        this.DrawRippleEffect(canvas, bounds, cx, cy);
    }

    private void DrawTrack(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var shape = new CornerRadius(bounds.Height / 2);
        var radii = shape.GetRadii();
        var paint = new SKPaint
        {
            Color = this.view.TrackColor.MultiplyAlpha(this.view.TrackOpacity).ToSKColor(),
            IsAntialias = true,
        };
        var path = new SKPath();
        var rect = new SKRoundRect();
        rect.SetRectRadii(bounds, radii);
        path.AddRoundRect(rect);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private void DrawOutline(SKCanvas canvas, SKRect bounds)
    {
        if (true)
        {
            var color = this.view.OutlineColor.MultiplyAlpha(this.view.TrackOpacity);
            var radii = this.view.GetRadii(bounds.Width, bounds.Height);
            canvas.DrawOutline(bounds, color, this.view.OutlineWidth, radii);
        }
    }

    private void DrawThumb(SKCanvas canvas, float cx, float cy)
    {
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.ThumbColor.MultiplyAlpha(this.view.ThumbOpacity).ToSKColor(),
            IsAntialias = true,
        };

        var radius = GetThumbRadius(8, 12, 14);
        canvas.DrawCircle(cx, cy, radius, paint);
        canvas.Restore();
    }

    private void DrawIcon(SKCanvas canvas, float cx, float cy)
    {
        if (!this.view.IsChecked)
            return;
        canvas.Save();
        var sx = cx - 8f;
        var sy = cy - 8f;
        canvas.ClipRect(
            new SKRect(
                sx,
                sy,
                sx + 16f * this.view.ChangingPercent,
                sy + 16f * this.view.ChangingPercent
            )
        );
        var path = SKPath.ParseSvgPathData(
            "M 5.8181543,10.027623 3.4153733,7.2675632 2.0000004,8.7537509 6.0066836,12.999999 14,4.5075 12.714286,3.0000004 Z"
        );
        var matrix = new SKMatrix
        {
            ScaleX = 1f,
            ScaleY = 1f,
            TransX = sx,
            TransY = sy,
            Persp2 = 1f
        };
        path.Transform(matrix);
        var paint = new SKPaint
        {
            Color = this.view.IconColor.MultiplyAlpha(this.view.IconOpacity).ToSKColor(),
            IsAntialias = true,
        };
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    internal void DrawStateLayer(SKCanvas canvas, float cx, float cy)
    {
#if !__MOBILE__
        if (this.view.StateLayerOpacity != 0f)
        {
            var color = this.view.StateLayerColor.MultiplyAlpha(this.view.StateLayerOpacity);
            var radii = new CornerRadius(20).GetRadii();
            canvas.DrawStateLayer(new SKRect(cx - 20, cy - 20, cx + 20, cy + 20), color, radii);
        }
#endif
    }

    private void DrawRippleEffect(SKCanvas canvas, SKRect bounds, float cx, float cy)
    {
        if (this.view.RipplePercent < 0)
            return;
        var color = this.view.RippleColor;
        var point = new SKPoint(cx, cy);
        canvas.DrawRippleEffect(
            bounds,
            0,
            this.view.RippleSize,
            point,
            color,
            this.view.RipplePercent,
            false
        );
    }

    /**
     * Calculate the thumb's X position. Position is calculated based on the current state of the switch.
     * And the current progress of the animation.
     * @param lx The left position of the switch.
     * @param rx The right position of the switch.
     * @param offX The offset between the left and right position.
     */
    private float GetThumbX(float lx, float rx, float offX)
    {
        var isAnimating = this.view.ChangingPercent != 1.0;
        float cx;

        // If we are animating, we need to adjust the cx
        if (this.view.IsChecked)
            cx = isAnimating ? lx : rx;
        else
            cx = isAnimating ? rx : lx;

        // Check if we are in the dead zone
        var dz = this.view.ChangingPercent < 0.25 || this.view.ChangingPercent > 0.75;
        if (!dz)
        {
            // Extract percentage when between 25% and 75% of animation
            var percent = (this.view.ChangingPercent - 0.25f) / 0.5f;
            cx = this.view.IsChecked ? lx + (percent * offX) : rx - (percent * offX);
        }
        else if (isAnimating)
        {
            // If we are in the dead zone and animating, we need to adjust the cx
            // to the correct position
            if (this.view.IsChecked)
                cx = this.view.ChangingPercent < 0.25f ? lx : rx;
            else
                cx = this.view.ChangingPercent < 0.25f ? rx : lx;
        }

        return cx;
    }

    /**
     * Calculate the thumb's radius. Radius is calculated based on the current state of the switch.
     * And the current progress of the animation.
     * @param leftSize This is the default size of the thumb when the switch is off.
     * @param rightSize This is the default size of the thumb when the switch is on.
     * @param moveSize This is the size of the thumb when the switch is moving.
     */
    private float GetThumbRadius(float leftSize, float rightSize, float moveSize)
    {
        var isAnimating = this.view.ChangingPercent != 1.0;
        float radius;

        // We need default size when we are not animating
        if (this.view.IsChecked)
            radius = isAnimating ? leftSize : rightSize;
        else
            radius = isAnimating ? rightSize : leftSize;

        // Check if we are in the dead zone
        var dz = this.view.ChangingPercent < 0.25 || this.view.ChangingPercent > 0.75;
        if (!dz)
        {
            radius = moveSize;
        }
        else if (isAnimating)
        {
            // Current percent when we are in the dead zone
            var percent =
                this.view.ChangingPercent < 0.25f
                    ? this.view.ChangingPercent / 0.25f
                    : (this.view.ChangingPercent - 0.75f) / 0.25f;

            // If we are in the dead zone and animating, we need to adjust the radius
            if (this.view.IsChecked)
                // When we are animating to the right
                radius =
                    this.view.ChangingPercent < 0.25f
                        ? leftSize.Lerp(moveSize, percent)
                        : moveSize.Lerp(rightSize, percent);
            else
                // When we are animating to the left
                radius =
                    this.view.ChangingPercent < 0.25f
                        ? rightSize.Lerp(moveSize, percent)
                        : moveSize.Lerp(leftSize, percent);
        }

        return radius;
    }
}
