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
        var cx = this.view.IsChecked ? lx + (this.view.ChangingPercent * offX) : rx - (this.view.ChangingPercent * offX);
        var cy = 24;
        canvas.Clear();
        this.DrawTrack(canvas, bounds);
        this.DrawOutline(canvas, bounds);
        this.DrawStateLayer(canvas, cx, cy);
        this.DrawThumb(canvas, cx, cy);
        this.DrawIcon(canvas, cx, cy);
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
        var radius = this.view.IsChecked ? 8 + (4 * this.view.ChangingPercent) : 12 - (4 * this.view.ChangingPercent);
        canvas.DrawCircle(cx, cy, radius, paint);
        canvas.Restore();
    }

    private void DrawIcon(SKCanvas canvas, float cx, float cy)
    {
        if (!this.view.IsChecked) return;
        canvas.Save();
        var path = SKPath.ParseSvgPathData("M 5.8181543,10.027623 3.4153733,7.2675632 2.0000004,8.7537509 6.0066836,12.999999 14,4.5075 12.714286,3.0000004 Z");
        var matrix = new SKMatrix
        {
            ScaleX = this.view.ChangingPercent,
            ScaleY = this.view.ChangingPercent,
            TransX = cx - (8 * this.view.ChangingPercent),
            TransY = cy - (8 * this.view.ChangingPercent),
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
        if (this.view.RipplePercent < 0) return;
        var color = this.view.RippleColor;
        var point = new SKPoint(cx, cy);
        canvas.DrawRippleEffect(bounds, 0, this.view.RippleSize, point, color, this.view.RipplePercent, false);
    }
}