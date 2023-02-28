using Svg.Skia;

namespace Material.Components.Maui.Core;

internal class MixedButtonDrawable : ButtonDrawable
{
    private readonly Button view;
    private float scale;

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

        var paddingBounds = new SKRect(
            (int)(bounds.Left + this.view.Padding.Left),
            (int)(bounds.Top + this.view.Padding.Top),
            (int)(bounds.Right - this.view.Padding.Right),
            (int)(bounds.Bottom - this.view.Padding.Bottom)
        );
        this.scale = this.view.FontSize / 14f;
        this.DrawPathIcon(canvas, paddingBounds);
        this.DrawImageIcon(canvas, paddingBounds);
        this.DrawText(canvas, paddingBounds);
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
        var iconScale = 18f / size * this.scale;
        var x =
            bounds.MidX
            - (this.view.InternalText.MeasuredWidth + (18f + 48f) * this.scale) / 2f
            + 16f * this.scale;
        var y = bounds.MidY - 9f * this.scale;
        var matrix = new SKMatrix
        {
            ScaleX = iconScale,
            ScaleY = iconScale,
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
        if (this.view.IconSource is null)
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
        var svgBounds = this.view.IconSource.CullRect;
        var size = Math.Max(svgBounds.Width, svgBounds.Height);
        var iconScale = 18f / size * this.scale;
        var x =
            bounds.MidX
            - (this.view.InternalText.MeasuredWidth + (18f + 48f) * this.scale) / 2f
            + 16f * this.scale;
        var y = bounds.MidY - 9f * this.scale;
        var matrix = new SKMatrix
        {
            ScaleX = iconScale,
            ScaleY = iconScale,
            TransX = x,
            TransY = y,
            Persp2 = 1f
        };
        canvas.DrawPicture(this.view.IconSource, ref matrix, paint);
        canvas.Restore();
    }

    internal void DrawText(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var iconSize =
            !string.IsNullOrEmpty(this.view.IconData) || this.view.IconSource != null
                ? 18f * this.scale
                : 0f;
        var x =
            bounds.MidX
            - (this.view.InternalText.MeasuredWidth + iconSize + 48f * this.scale) / 2f
            + iconSize
            + 24f * this.scale;
        var y = bounds.MidY - (this.view.InternalText.MeasuredHeight / 2f);
        this.view.InternalText.Paint(canvas, new SKPoint(x, y));
        canvas.Restore();
    }
}
