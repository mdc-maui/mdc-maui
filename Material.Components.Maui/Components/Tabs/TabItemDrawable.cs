namespace Material.Components.Maui.Core;

internal class TabItemDrawable
{
    private readonly TabItem view;

    public TabItemDrawable(TabItem view)
    {
        this.view = view;
    }

    internal void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear(this.view.BackgroundColour.ToSKColor());
        this.DrawOverlayLayer(canvas, bounds);
        this.DrawStateLayer(canvas, bounds);
        this.DrawPathIcon(canvas, bounds);
        this.DrawImageIcon(canvas, bounds);
        this.DrawText(canvas, bounds);
        this.DrawActiveIndicator(canvas, bounds);
        this.DrawRippleEffect(canvas, bounds);
    }

    private void DrawBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.BackgroundColour.MultiplyAlpha(this.view.BackgroundOpacity);
        var _bounds = new SKRect(
            bounds.Left - 1,
            bounds.Top - 1,
            bounds.Right + 1,
            bounds.Bottom + 1
        );
        canvas.DrawBackground(_bounds, color, 0);
    }

    private void DrawOverlayLayer(SKCanvas canvas, SKRect bounds)
    {
        var radii = new CornerRadius(0).GetRadii();
        var _bounds = new SKRect(
            bounds.Left - 1,
            bounds.Top - 1,
            bounds.Right + 1,
            bounds.Bottom + 1
        );
        canvas.DrawOverlayLayer(_bounds, Elevation.Level2, radii);
    }

    private void DrawStateLayer(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var color = this.view.StateLayerColor.MultiplyAlpha(this.view.StateLayerOpacity);
        canvas.DrawBackground(bounds, color, 0);
        canvas.Restore();
    }

    private void DrawPathIcon(SKCanvas canvas, SKRect bounds)
    {
        if (!this.view.HasIcon || this.view.IconSource != null || this.view.Icon is IconKind.None)
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
        var x = bounds.MidX - 12;
        var y = this.view.HasLabel ? 12 : bounds.MidY - 12;
        path.Offset(x, y);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private void DrawImageIcon(SKCanvas canvas, SKRect bounds)
    {
        if (!this.view.HasIcon || this.view.IconSource is null)
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
        var scale = 24 / this.view.IconSource.CullRect.Width;
        var x = bounds.MidX - 12;
        var y = this.view.HasLabel ? 12 : bounds.MidY - 12;
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
        if (!this.view.HasLabel)
            return;
        canvas.Save();
        this.view.TextStyle.TextColor = this.view.ForegroundColor
            .MultiplyAlpha(this.view.ForegroundOpacity)
            .ToSKColor();
        var x = bounds.MidX - (this.view.InternalText.MeasuredWidth / 2);
        var y = this.view.HasIcon
            ? 36 + ((25 - this.view.InternalText.MeasuredHeight) / 2)
            : bounds.MidY - (this.view.InternalText.MeasuredHeight / 2);
        this.view.InternalText.Paint(canvas, new SKPoint(x, y));
        canvas.Restore();
    }

    private void DrawActiveIndicator(SKCanvas canvas, SKRect bounds)
    {
        if (!this.view.IsActived)
            return;
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.ActiveIndicatorColor
                .MultiplyAlpha(this.view.ActiveIndicatorOpacity)
                .ToSKColor(),
            IsAntialias = true,
        };
        var x = bounds.MidX - 20;
        var y = bounds.Bottom - 3;
        var path = new SKPath();
        var rect = new SKRoundRect();
        rect.SetRectRadii(
            new SKRect(x, y, x + 40, y + 3),
            this.view.ActiveIndicatorShape.GetRadii()
        );
        path.AddRoundRect(rect);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private void DrawRippleEffect(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.RipplePercent < 0)
            return;
        var color = this.view.RippleColor;
        var point = this.view.TouchPoint;
        canvas.DrawRippleEffect(
            bounds,
            0,
            this.view.RippleSize,
            point,
            color,
            this.view.RipplePercent
        );
    }
}
