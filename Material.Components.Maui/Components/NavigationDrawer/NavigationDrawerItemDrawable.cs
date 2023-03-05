namespace Material.Components.Maui.Core;

internal class NavigationDrawerItemDrawable
{
    private readonly NavigationDrawerItem view;

    internal NavigationDrawerItemDrawable(NavigationDrawerItem view)
    {
        this.view = view;
    }

    internal void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        this.DrawBackground(canvas, bounds);
        this.DrawActiveIndicator(canvas, bounds);
        this.DrawStateLayer(canvas, bounds);
        this.DrawPathIcon(canvas, bounds);
        this.DrawImageIcon(canvas, bounds);
        this.DrawText(canvas, bounds);
        this.DrawRippleEffect(canvas, bounds);
    }

    private void DrawBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.BackgroundColour.MultiplyAlpha(this.view.BackgroundOpacity);
        canvas.DrawBackground(bounds, color, 16);
    }

    private void DrawActiveIndicator(SKCanvas canvas, SKRect bounds)
    {
        if (!this.view.IsActived)
            return;
        canvas.DrawBackground(bounds, this.view.ActiveIndicatorColor, 16);
    }

    private void DrawStateLayer(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var color = this.view.StateLayerColor.MultiplyAlpha(this.view.StateLayerOpacity);
        canvas.DrawBackground(bounds, color, 16);
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
        var x = 16f;
        var y = 12f;
        path.Offset(x, y);
        canvas.DrawPath(path, paint);

        canvas.Restore();
    }

    private void DrawImageIcon(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.IconSource is null || this.view.IconSource is null)
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

        var scale = 24f / this.view.IconSource.CullRect.Width;
        var x = 16f;
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
        if (!this.view.IsExtended)
            return;
        canvas.Save();
        var x = 52f;
        var y = bounds.MidY - this.view.InternalText.MeasuredHeight / 2;
        this.view.InternalText.Paint(canvas, new SKPoint(x, y));
        canvas.Restore();
    }

    private void DrawRippleEffect(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.RipplePercent < 0f)
            return;
        var color = this.view.RippleColor;
        canvas.DrawRippleEffect(
            bounds,
            16,
            this.view.RippleSize,
            this.view.TouchPoint,
            color,
            this.view.RipplePercent
        );
    }
}
