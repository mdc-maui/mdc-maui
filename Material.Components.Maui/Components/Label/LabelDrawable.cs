namespace Material.Components.Maui.Core;

internal class LabelDrawable
{
    private readonly Label view;

    public LabelDrawable(Label view)
    {
        this.view = view;
    }

    public void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        this.DrawBackground(canvas, bounds);
        var textBounds = new SKRect(
            (float)(bounds.Left + this.view.Padding.Left),
            (float)(bounds.Top + this.view.Padding.Top),
            (float)(bounds.Right - this.view.Padding.Right),
            (float)(bounds.Bottom - this.view.Padding.Bottom)
        );
        this.DrawText(canvas, textBounds);
    }

    internal void DrawBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.BackgroundColour.MultiplyAlpha(this.view.BackgroundOpacity);
        canvas.DrawBackground(bounds, color, this.view.GetRadii(bounds.Width, bounds.Height));
    }

    private void DrawText(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        this.view.TextStyle.TextColor = this.view.ForegroundColor
            .MultiplyAlpha(this.view.ForegroundOpacity)
            .ToSKColor();
        var x = this.view.HorizontalTextAlignment switch
        {
            TextAlignment.Center => bounds.MidX - (this.view.TextBlock.MeasuredWidth / 2),
            TextAlignment.End => bounds.Right - this.view.TextBlock.MeasuredWidth - 1,
            _ => bounds.Left,
        };
        var y = this.view.VerticalTextAlignment switch
        {
            TextAlignment.Center => bounds.MidY - (this.view.TextBlock.MeasuredHeight / 2),
            TextAlignment.End => bounds.Bottom - this.view.TextBlock.MeasuredHeight - 1,
            _ => bounds.Top,
        };
        this.view.TextBlock.Paint(canvas, new SKPoint(x, y));
        canvas.Restore();
    }
}
