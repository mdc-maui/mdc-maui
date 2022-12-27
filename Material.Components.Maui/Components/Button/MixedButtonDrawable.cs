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

        var paddingBounds = new SKRect(
            (int)(bounds.Left + this.view.Padding.Left),
            (int)(bounds.Top + this.view.Padding.Top),
            (int)(bounds.Right - this.view.Padding.Right),
            (int)(bounds.Bottom - this.view.Padding.Bottom)
        );
        var textScale = this.view.FontSize / 14f;
        this.DrawPathIcon(canvas, paddingBounds, textScale);
        this.DrawImageIcon(canvas, paddingBounds, textScale);
        this.DrawText(canvas, paddingBounds, textScale);
        this.DrawRippleEffect(canvas, bounds);
    }

    private void DrawPathIcon(SKCanvas canvas, SKRect bounds, float textScale)
    {
        if (this.view.IconSource != null || this.view.Icon is IconKind.None)
            return;
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.ForegroundColor
                .MultiplyAlpha(this.view.ForegroundOpacity)
                .ToSKColor(),
            IsAntialias = true,
        };
        var path = SKPath.ParseSvgPathData(this.view.Icon.GetData());
        var iconScale = 18f / 24f * textScale;
        var x =
            bounds.MidX
            - (this.view.TextBlock.MeasuredWidth + (18f + 48f) * textScale) / 2f
            + 16f * textScale;
        var y = bounds.MidY - 9f * textScale;
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

    private void DrawImageIcon(SKCanvas canvas, SKRect bounds, float textScale)
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
        var iconScale = 18f / svgBounds.Width * textScale;
        var x =
            bounds.MidX
            - (this.view.TextBlock.MeasuredWidth + (18f + 48f) * textScale) / 2f
            + 16f * textScale;
        var y = bounds.MidY - 9f * textScale;
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

    internal void DrawText(SKCanvas canvas, SKRect bounds, float textScale)
    {
        canvas.Save();
        this.view.TextStyle.TextColor = this.view.ForegroundColor
            .MultiplyAlpha(this.view.ForegroundOpacity)
            .ToSKColor();
        var iconSize =
            this.view.Icon != IconKind.None || this.view.IconSource != null ? 18f * textScale : 0f;
        var x =
            bounds.MidX
            - (this.view.TextBlock.MeasuredWidth + iconSize + 48f * textScale) / 2f
            + iconSize
            + 24f * textScale;
        var y = bounds.MidY - (this.view.TextBlock.MeasuredHeight / 2f);
        this.view.TextBlock.Paint(canvas, new SKPoint(x, y));
        canvas.Restore();
    }
}
