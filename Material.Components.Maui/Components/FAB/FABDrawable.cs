namespace Material.Components.Maui.Core;

internal class FABDrawable
{
    private readonly FAB view;

    public FABDrawable(FAB view)
    {
        this.view = view;
    }

    public void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        this.DrawBackground(canvas, bounds);
        this.DrawOverlayLayer(canvas, bounds);
        this.DrawStateLayer(canvas, bounds);
        this.DrawPathIcon(canvas, bounds);
        this.DrawImageIcon(canvas, bounds);
        this.DrawText(canvas, bounds);
        this.DrawRippleEffect(canvas, bounds);
    }

    private void DrawBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.BackgroundColour.MultiplyAlpha(this.view.BackgroundOpacity);
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawBackground(bounds, color, radii);
    }

    private void DrawStateLayer(SKCanvas canvas, SKRect bounds)
    {
#if !__MOBILE__
        if (this.view.StateLayerOpacity != 0f)
        {
            var color = this.view.StateLayerColor.MultiplyAlpha(this.view.StateLayerOpacity);
            var radii = this.view.GetRadii(bounds.Width, bounds.Height);
            canvas.DrawStateLayer(bounds, color, radii);
        }
#endif
    }

    private void DrawOverlayLayer(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.ControlState == ControlState.Disabled)
            return;
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawOverlayLayer(bounds, this.view.Elevation, radii);
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
        var offset =
            this.view.FABType == FABType.Default
                ? 16f
                : this.view.FABType == FABType.Small
                    ? 8f
                    : 30f;
        var scale = this.view.FABType == FABType.Large ? 1.5f : 1f;
        var path = SKPath.ParseSvgPathData(this.view.Icon.GetData());
        var matrix = new SKMatrix
        {
            ScaleX = scale,
            ScaleY = scale,
            TransX = offset,
            TransY = offset,
            Persp2 = 1f
        };
        path.Transform(matrix);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private void DrawImageIcon(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.IconSource == null || this.view.IconSource == null)
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
        var offset =
            this.view.FABType == FABType.Default
                ? 16f
                : this.view.FABType == FABType.Small
                    ? 8f
                    : 30f;
        var scale = this.view.FABType == FABType.Large ? 1.5f : 1f;
        var matrix = new SKMatrix
        {
            ScaleX = scale,
            ScaleY = scale,
            TransX = offset,
            TransY = offset,
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
        this.view.TextStyle.TextColor = this.view.ForegroundColor
            .MultiplyAlpha(this.view.ForegroundOpacity)
            .ToSKColor();
        var x =
            this.view.FABType == FABType.Default
                ? 52f
                : this.view.FABType == FABType.Small
                    ? 38f
                    : 88f;
        var y = bounds.MidY - (this.view.TextBlock.MeasuredHeight / 2);
        this.view.TextBlock.Paint(canvas, new SKPoint(x, y));
        canvas.Restore();
    }

    private void DrawRippleEffect(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.RippleColor;
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawRippleEffect(
            bounds,
            radii,
            this.view.RippleSize,
            this.view.TouchPoint,
            color,
            this.view.RipplePercent
        );
    }
}
