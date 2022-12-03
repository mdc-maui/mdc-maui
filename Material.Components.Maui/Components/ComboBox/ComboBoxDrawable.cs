using Topten.RichTextKit;

namespace Material.Components.Maui.Core;

internal class ComboBoxDrawable
{
    private readonly ComboBox view;

    public ComboBoxDrawable(ComboBox view)
    {
        this.view = view;
    }

    public void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        this.DrawBackground(canvas, bounds);
        this.DrawOutline(canvas, bounds);
        this.DrawLabelText(canvas, bounds);
        this.DrawIcon(canvas, bounds);
        this.DrawActiveIndicator(canvas, bounds);
        this.DrawSelectedItem(canvas, bounds);
    }

    private void DrawBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.BackgroundColour.MultiplyAlpha(this.view.BackgroundOpacity);
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawBackground(bounds, color, radii);
    }

    private void DrawOutline(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.OutlineWidth is 0)
            return;
        canvas.Save();
        var color = this.view.OutlineColor.MultiplyAlpha(this.view.OutlineOpacity);
        var width = this.view.OutlineWidth;
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawOutline(bounds, color, width, radii);
        canvas.Restore();

        if (this.view.TextFieldStyle is TextFieldStyle.Outlined)
        {
            if (this.view.SelectedIndex != -1 || this.view.IsDropDown)
            {
                canvas.Save();
                canvas.ClipRect(
                    new SKRect(
                        bounds.Left + 12,
                        bounds.Top,
                        bounds.Left + this.view.LabelTextBlock.MeasuredWidth + 20,
                        bounds.Top + width
                    )
                );
                canvas.Clear();
                canvas.Restore();
            }
        }
    }

    private void DrawIcon(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var paint = new SKPaint
        {
            IsAntialias = true,
            Color = this.view.ForegroundColor
                .MultiplyAlpha(this.view.ForegroundOpacity)
                .ToSKColor(),
        };
        var path = SKPath.ParseSvgPathData(IconKind.ArrowDropDown.GetData());
        var degrees = this.view.IsDropDown ? 180 : 0;
        var radians = degrees * (MathF.PI / 180f);
        var sin = (float)Math.Sin(radians);
        var cos = (float)Math.Cos(radians);
        var x = bounds.Right - 36 + (this.view.IsDropDown ? 24 : 0);
        var y = bounds.MidY - 12 + (this.view.IsDropDown ? 24 : 0);
        var matrix = new SKMatrix
        {
            ScaleX = cos,
            SkewX = 0f - sin,
            TransX = x,
            SkewY = sin,
            ScaleY = cos,
            TransY = y,
            Persp2 = 1f,
        };
        path.Transform(matrix);
        canvas.DrawPath(path, paint);
        canvas.Restore();
    }

    private void DrawLabelText(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var percent = this.view.PlaceholderAnimationPercent;
        if (this.view.SelectedIndex != -1 || this.view.IsDropDown)
        {
            percent = 1 - this.view.PlaceholderAnimationPercent;
        }
        var style = this.view.TextStyle.Modify(
            fontSize: this.view.TextStyle.FontSize - ((1 - percent) * 4),
            textColor: this.view.LabelTextColor
                .MultiplyAlpha(this.view.LabelTextOpacity)
                .ToSKColor()
        );
        var tb = new TextBlock();
        tb.AddText(this.view.LabelText, style);
        var y =
            this.view.TextFieldStyle is TextFieldStyle.Outlined
                ? ((16 - this.view.LabelTextBlock.MeasuredHeight) / 2)
                : bounds.Top + 8;
        var offsetY = (bounds.MidY - (tb.MeasuredHeight / 2) - y) * percent;
        tb.Paint(canvas, new SKPoint(bounds.Left + 16, y + offsetY));
        canvas.Restore();
    }

    private void DrawActiveIndicator(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.ActiveIndicatorHeight is 0)
            return;
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.ActiveIndicatorColor
                .MultiplyAlpha(this.view.ActiveIndicatorOpacity)
                .ToSKColor(),
            IsAntialias = true,
        };
        var x = bounds.Left;
        var y = bounds.Bottom - this.view.ActiveIndicatorHeight;
        canvas.DrawRect(x, y, bounds.Width, this.view.ActiveIndicatorHeight, paint);
        canvas.Restore();
    }

    private void DrawSelectedItem(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.SelectedIndex is -1)
            return;
        canvas.Save();
        this.view.TextStyle.TextColor = this.view.ForegroundColor
            .MultiplyAlpha(this.view.ForegroundOpacity)
            .ToSKColor();
        var y =
            this.view.TextFieldStyle is TextFieldStyle.Outlined
                ? bounds.MidY - (this.view.TextBlock.MeasuredHeight / 2)
                : bounds.Bottom - 8 - this.view.TextBlock.MeasuredHeight;
        this.view.TextBlock.Paint(canvas, new SKPoint(bounds.Left + 16, y));
        canvas.Restore();
    }
}
