using Svg.Skia;

namespace Material.Components.Maui.Core;
internal class MixedButtonDrawable : ButtonDrawable
{
    private readonly Button view;

    public MixedButtonDrawable(Button button) : base(button)
    {
        this.view = button;
    }

    internal override void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        this.DrawBackground(canvas, bounds);
        this.DrawOutline(canvas, bounds);
        this.DrawOverlayLayer(canvas, bounds);
        this.DrawStateLayer(canvas, bounds);
        this.DrawPathIcon(canvas, bounds);
        this.DrawImageIcon(canvas, bounds);
        this.DrawText(canvas, bounds);
        this.DrawRippleEffect(canvas, bounds);
    }

    private void DrawPathIcon(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.Image != null || this.view.Icon is IconKind.None) return;
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.ForegroundColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor(),
            IsAntialias = true,
        };
        var path = SKPath.ParseSvgPathData(this.view.Icon.GetData());
        var scale = 18 / 24f;
        var x = bounds.Left + ((bounds.Width - 26 - (int)this.view.TextBlock.MeasuredWidth - 8) / 2);
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
        if (this.view.Image is null) return;
        canvas.Save();
        var paint = new SKPaint
        {
            IsAntialias = true,
            ColorFilter = SKColorFilter.CreateBlendMode(
               this.view.ForegroundColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor(),
                SKBlendMode.SrcIn)
        };
        var svgBounds = this.view.Image.CullRect;
        var scale = 18 / svgBounds.Width;
        var x = bounds.Left + ((bounds.Width - 26 - (int)this.view.TextBlock.MeasuredWidth - 8) / 2);
        var y = bounds.MidY - 9;
        var matrix = new SKMatrix
        {
            ScaleX = scale,
            ScaleY = scale,
            TransX = x,
            TransY = y,
            Persp2 = 1f
        };
        canvas.DrawPicture(this.view.Image, ref matrix, paint);
        canvas.Restore();
    }

    internal void DrawText(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        this.view.TextStyle.TextColor = this.view.ForegroundColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor();
        var x = bounds.MidX - (this.view.TextBlock.MeasuredWidth / 2);
        if (this.view.Icon != IconKind.None ||
            this.view.Image != null)
        {
            x += 9;
        }
        var y = bounds.MidY - (this.view.TextBlock.MeasuredHeight / 2);
        this.view.TextBlock.Paint(canvas, new SKPoint(x, y));
        canvas.Restore();
    }
}
