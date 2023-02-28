using Svg.Skia;

namespace Material.Components.Maui.Core;

internal class IconButtonDrawable : ButtonDrawable
{
    private readonly IconButton view;

    public IconButtonDrawable(IconButton button) : base(button)
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
        this.DrawRippleEffect(canvas, bounds);
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

        path.GetTightBounds(out var tb);
        var size = Math.Max(tb.MidX, tb.MidY) * 2;
        var scale = 24 / size;
        var x = bounds.MidX - 12;
        var y = bounds.MidY - 12;
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
                this.view.ForegroundColor.MultiplyAlpha(this.view.ForegroundOpacity).ToSKColor(),
                SKBlendMode.SrcIn
            )
        };
        var scale = 24 / this.view.IconSource.CullRect.Width;
        var x = bounds.MidX - 12;
        var y = bounds.MidY - 12;
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
}
