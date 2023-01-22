namespace Material.Components.Maui.Core;

internal class RadioButtonItemDrawable
{
    private readonly RadioButtonItem view;

    public RadioButtonItemDrawable(RadioButtonItem view)
    {
        this.view = view;
    }

    public void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        this.DrawRing(canvas, bounds);
        this.DrawCircle(canvas, bounds);
        this.DrawStateLayer(canvas, bounds);
        this.DrawText(canvas, bounds);
        this.DrawRippleEffect(canvas, bounds);
    }

    private void DrawRing(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var color = this.view.IsSelected ? this.view.OnColor : this.view.ForegroundColor;
        var paint = new SKPaint
        {
            Color = color.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor(),
            IsStroke = true,
            StrokeWidth = 2 + (8 * (1 - this.view.ChangingPercent)),
            IsAntialias = true,
        };
        var path = new SKPath();
        path.AddCircle(bounds.Left + 20, bounds.MidY, 10 * this.view.ChangingPercent);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private void DrawCircle(SKCanvas canvas, SKRect bounds)
    {
        if (!this.view.IsSelected)
            return;
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.OnColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor(),
            IsAntialias = true,
        };
        var path = new SKPath();
        path.AddCircle(bounds.Left + 20, bounds.MidY, 6 + (4 * (1 - this.view.ChangingPercent)));
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private void DrawStateLayer(SKCanvas canvas, SKRect bounds)
    {
#if !__MOBILE__
        if (this.view.StateLayerOpacity != 0f)
        {
            var _bounds = new SKRect(bounds.Left, bounds.Top, bounds.Left + 40, bounds.Bottom);
            var color = this.view.StateLayerColor.MultiplyAlpha(this.view.StateLayerOpacity);
            var radii = new CornerRadius(20).GetRadii();
            canvas.DrawStateLayer(_bounds, color, radii);
        }
#endif
    }

    private void DrawText(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        this.view.TextStyle.TextColor = this.view.ForegroundColor
            .MultiplyAlpha(this.view.ForegroundOpacity)
            .ToSKColor();
        var x = bounds.Left + 40;
        var y = bounds.MidY - (this.view.InternalText.MeasuredHeight / 2);
        this.view.InternalText.Paint(canvas, new SKPoint(x, y));
        canvas.Restore();
    }

    private void DrawRippleEffect(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.RipplePercent == 0)
            return;

        var _bounds = new SKRect(bounds.Left, bounds.Top, bounds.Left + 40, bounds.Bottom);
        var color = this.view.RippleColor;
        var point = new SKPoint(Math.Min(39, this.view.TouchPoint.X), this.view.TouchPoint.Y);
        var size = _bounds.GetRippleSize(point);
        canvas.DrawRippleEffect(_bounds, 20, size, point, color, this.view.RipplePercent, true);
    }
}
