using Material.Components.Maui.Extensions;
using System.Diagnostics;

namespace Material.Components.Maui.Core.Label;
internal class LabelDrawable
{
    private readonly MLabel view;
    public LabelDrawable(MLabel view)
    {
        this.view = view;
    }

    public void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        this.DrawText(canvas, bounds);
    }

    private void DrawText(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();

        this.view.TextStyle.TextColor = this.view.ForegroundColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor();
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
