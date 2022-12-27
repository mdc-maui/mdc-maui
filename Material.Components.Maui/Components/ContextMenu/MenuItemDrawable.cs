namespace Material.Components.Maui.Core;

internal class MenuItemDrawable
{
    private readonly MenuItem view;

    public MenuItemDrawable(MenuItem view)
    {
        this.view = view;
    }

    internal void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        this.DrawStateLayer(canvas, bounds);
        this.DrawPathIcon(canvas, bounds);
        this.DrawImageIcon(canvas, bounds);
        this.DrawTrailPathIcon(canvas, bounds);
        this.DrawTrailImageIcon(canvas, bounds);
        this.DrawText(canvas, bounds);
        this.DrawRippleEffect(canvas, bounds);
    }

    internal void DrawStateLayer(SKCanvas canvas, SKRect bounds)
    {
#if !__MOBILE__
        if (this.view.StateLayerOpacity != 0f)
        {
            var color = this.view.StateLayerColor.MultiplyAlpha(this.view.StateLayerOpacity);
            var radii = new CornerRadius(0).GetRadii();
            canvas.DrawStateLayer(bounds, color, radii);
        }
#endif
    }

    private void DrawPathIcon(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.IconSource != null || this.view.Icon == IconKind.None)
            return;
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.ForegroundColor
                .MultiplyAlpha(this.view.ForegroundOpacity)
                .ToSKColor(),
            IsAntialias = true,
        };
        var path = SKPath.ParseSvgPathData(this.view.Icon.GetData());
        var x = 12f;
        var y = 12f;
        path.Offset(x, y);
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
        var scale = 24f / this.view.IconSource.CullRect.Width;
        var x = 12f;
        var y = 12f;
        var matrix = new SKMatrix
        {
            ScaleX = scale,
            ScaleY = scale,
            TransX = x,
            TransY = y,
            Persp2 = 1f
        };
        canvas.DrawPicture(this.view.IconSource, ref matrix, paint);
        canvas.Restore();
    }

    private void DrawTrailPathIcon(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.TrailIconSource != null || this.view.TrailIcon == IconKind.None)
            return;
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.ForegroundColor
                .MultiplyAlpha(this.view.ForegroundOpacity)
                .ToSKColor(),
            IsAntialias = true,
        };
        var path = SKPath.ParseSvgPathData(this.view.TrailIcon.GetData());
        var x = bounds.Right - 36f;
        var y = 12f;
        path.Offset(x, y);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private void DrawTrailImageIcon(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.TrailIconSource == null)
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
        var scale = 24f / this.view.TrailIconSource.CullRect.Width;
        var x = bounds.Right - 36f;
        var y = 12f;
        var matrix = new SKMatrix
        {
            ScaleX = scale,
            ScaleY = scale,
            TransX = x,
            TransY = y,
            Persp2 = 1f
        };
        canvas.DrawPicture(this.view.IconSource, ref matrix, paint);
        canvas.Restore();
    }

    private void DrawText(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        this.view.TextStyle.TextColor = this.view.ForegroundColor
            .MultiplyAlpha(this.view.ForegroundOpacity)
            .ToSKColor();
        var x = this.view.IconSource != null || this.view.Icon != IconKind.None ? 48f : 12f;
        var y = bounds.MidY - (this.view.TextBlock.MeasuredHeight / 2f);
        this.view.TextBlock.Paint(canvas, new SKPoint(x, y));
        canvas.Restore();
    }

    private void DrawRippleEffect(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.RipplePercent > 0f && bounds.Contains(this.view.TouchPoint))
            canvas.DrawRippleEffect(
                bounds,
                0,
                this.view.RippleSize,
                this.view.TouchPoint,
                this.view.RippleColor,
                this.view.RipplePercent
            );
    }
}
