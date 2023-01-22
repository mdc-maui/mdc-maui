namespace Material.Components.Maui.Core;

internal class ChipDrawable
{
    private readonly Chip view;

    public ChipDrawable(Chip view)
    {
        this.view = view;
    }

    public void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        this.DrawBackground(canvas, bounds);
        this.DrawOutline(canvas, bounds);
        this.DrawOverlayLayer(canvas, bounds);
        this.DrawStateLayer(canvas, bounds);
        this.DrawPathIcon(canvas, bounds);
        this.DrawImageIcon(canvas, bounds);
        this.DrawCloseIcon(canvas, bounds);
        this.DrawText(canvas, bounds);
        this.DrawRippleEffect(canvas, bounds);
    }

    private void DrawBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.BackgroundColour.MultiplyAlpha(this.view.BackgroundOpacity);
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawBackground(bounds, color, radii);
    }

    internal void DrawStateLayer(SKCanvas canvas, SKRect bounds)
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

    private void DrawOutline(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.OutlineWidth > 0)
        {
            var color = this.view.OutlineColor;
            var width = this.view.OutlineWidth;
            var radii = this.view.GetRadii(bounds.Width, bounds.Height);
            canvas.DrawOutline(bounds, color, width, radii);
        }
    }

    private void DrawPathIcon(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.IconSource != null || this.view.Icon == IconKind.None)
            return;
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.IconColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor(),
            IsAntialias = true,
        };
        var path = SKPath.ParseSvgPathData(this.view.Icon.GetData());
        var scale = 18 / 24f;
        var x = bounds.Left + 8;
        var y = bounds.MidY - 9;
        var matrix = new SKMatrix
        {
            ScaleX = scale,
            ScaleY = scale,
            TransX = x,
            TransY = y,
            Persp2 = 1f
        };
        path.Transform(matrix);
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
                this.view.IconColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor(),
                SKBlendMode.SrcIn
            )
        };
        var svgBounds = this.view.IconSource.CullRect;
        var scale = 18 / svgBounds.Width;
        var x = bounds.Left + 8;
        var y = bounds.MidY - 9;
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

    private void DrawCloseIcon(SKCanvas canvas, SKRect bounds)
    {
        if (!this.view.HasCloseIcon)
            return;
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.IconColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor(),
            IsAntialias = true,
        };
        var path = SKPath.ParseSvgPathData(IconKind.Close.GetData());
        var scale = 18 / 24f;
        var x = bounds.Right - 26;
        var y = bounds.MidY - 9;
        var matrix = new SKMatrix
        {
            ScaleX = scale,
            ScaleY = scale,
            TransX = x,
            TransY = y,
            Persp2 = 1f
        };
        path.Transform(matrix);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private void DrawText(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        this.view.TextStyle.TextColor = this.view.ForegroundColor
            .MultiplyAlpha(this.view.ForegroundOpacity)
            .ToSKColor();
        var leftPadding =
            this.view.Icon != IconKind.None || this.view.IconSource != null ? 34f : 16f;
        var rightPadding = this.view.HasCloseIcon ? 34f : 16f;
        var x =
            leftPadding
            + (bounds.Width - leftPadding - rightPadding - this.view.InternalText.MeasuredWidth) / 2;
        //var x = leftPadding + ((bounds.Right - leftPadding - rightPadding) / 2) - (this.view.TextBlock.MeasuredWidth / 2);
        var y = bounds.MidY - (this.view.InternalText.MeasuredHeight / 2);
        this.view.InternalText.Paint(canvas, new SKPoint(x, y));
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
