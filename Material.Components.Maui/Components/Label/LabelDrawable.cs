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
        var x = this.view.HorizontalTextAlignment switch
        {
            TextAlignment.Center => bounds.MidX - (this.view.InternalText.MeasuredWidth / 2),
            TextAlignment.End => bounds.Right - this.view.InternalText.MeasuredWidth - 1,
            _ => bounds.Left,
        };
        var y = this.view.VerticalTextAlignment switch
        {
            TextAlignment.Center => bounds.MidY - (this.view.InternalText.MeasuredHeight / 2),
            TextAlignment.End => bounds.Bottom - this.view.InternalText.MeasuredHeight - 1,
            _ => bounds.Top,
        };
        this.view.InternalText.Paint(canvas, new SKPoint(x, y));
        canvas.Restore();
    }
}
