namespace Material.Components.Maui.Core.ComboBox;
internal class ComboBoxItemDrawable
{
    private readonly ComboBoxItem view;

    public ComboBoxItemDrawable(ComboBoxItem view)
    {
        this.view = view;
    }

    internal void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        this.DrawStateLayer(canvas, bounds);
        this.DrawText(canvas, bounds);
        this.DrawRippleEffect(canvas, bounds);
    }

    internal void DrawStateLayer(SKCanvas canvas, SKRect bounds)
    {
#if WINDOWS || MACCATALYST
        if (this.view.StateLayerOpacity != 0f)
        {
            var color = this.view.StateLayerColor.MultiplyAlpha(this.view.StateLayerOpacity);
            var radii = new CornerRadius(0).GetRadii();
            canvas.DrawStateLayer(bounds, color, radii);
        }
#endif
    }

    private void DrawText(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        this.view.TextStyle.TextColor = this.view.ForegroundColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor();
        var y = bounds.MidY - (this.view.TextBlock.MeasuredHeight / 2);
        this.view.TextBlock.Paint(canvas, new SKPoint(16, y));
        canvas.Restore();

        //var paint = new SKPaint
        //{
        //    Color = MaterialColors.OnSurface.ToSKColor(),
        //    Typeface = this.view.Typography.Typeface,
        //    TextSize = this.view.Typography.Size,
        //    IsAntialias = true,
        //};
        //var height = paint.FontMetrics.Bottom - paint.FontMetrics.Top;
        //var x = 16;
        //var y = bounds.Top + ((bounds.Height - height) / 2) - paint.FontMetrics.Top;
        //canvas.DrawText(this.view.Text, x, y, paint);

    }

    private void DrawRippleEffect(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.RipplePercent > 0 && bounds.Contains(this.view.TouchPoint))
        {
            canvas.DrawRippleEffect(bounds, 0, this.view.RippleSize, this.view.TouchPoint, this.view.RippleColor, this.view.RipplePercent);
        }
    }
}
