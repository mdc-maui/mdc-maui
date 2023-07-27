namespace Material.Components.Maui;

internal class ProgressIndicatorDrawable : IDrawable
{
    readonly ProgressIndicator view;

    public ProgressIndicatorDrawable(ProgressIndicator view)
    {
        this.view = view;
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(this.view.GetClipPath(rect));
        canvas.DrawBackground(this.view, rect);

        var scale = this.view.IndicatorType is IndicatorType.Circular ? rect.Height / 48f : 1f;
        canvas.StrokeColor = this.view.ActiveIndicatorColor;
        canvas.StrokeSize = this.view.ActiveIndicatorHeight * scale;

        if (this.view.IndicatorType is IndicatorType.Circular)
        {
            if (this.view.Percent == -1f)
                this.DrawCircularIndeterminateAnimation(canvas, rect, scale);
            else
                this.DrawCircularProgress(canvas, rect, scale);
        }
        else
        {
            if (this.view.Percent == -1f)
                this.DrawLinearIndeterminateAnimation(canvas, rect);
            else
                this.DrawLinearProgress(canvas, rect);
        }

        canvas.RestoreState();
    }

    void DrawCircularIndeterminateAnimation(ICanvas canvas, RectF rect, float scale)
    {
        var size = 36f * scale;
        var x = rect.Left + 6f * scale;
        var y = rect.Top + 6f * scale;
        var percent = this.view.AnimationPercent;

        if (this.view.AnimationIsPositive)
            canvas.DrawArc(
                x,
                y,
                size,
                size,
                360f - 360f * percent,
                360f - 360f * percent - 315f * percent,
                true,
                false
            );
        else
            canvas.DrawArc(
                x,
                y,
                size,
                size,
                360f - 360f * (1 - percent) * 2,
                360f - 360f * (1 - percent) * 2 - 315f * percent,
                true,
                false
            );
    }

    void DrawCircularProgress(ICanvas canvas, RectF rect, float scale)
    {
        var size = 36f * scale;
        var x = rect.Left + 6f * scale;
        var y = rect.Top + 6f * scale;
        var percent = this.view.Percent;

        if (percent != 100)
            canvas.DrawArc(x, y, size, size, 90f, 90f - 3.6f * percent, true, false);
        else
            canvas.DrawCircle(rect.Center, size / 2);
    }

    private void DrawLinearIndeterminateAnimation(ICanvas canvas, RectF rect)
    {
        var percent = this.view.AnimationPercent;

        var startX = (rect.Width * percent) - (rect.Width * (1f - percent) * 0.5f) + rect.Left;
        var endX = startX + (rect.Width * 0.5f);

        canvas.DrawLine(startX, rect.Center.Y, endX, rect.Center.Y);
    }

    private void DrawLinearProgress(ICanvas canvas, RectF rect)
    {
        var startX = rect.Left;
        var endX = rect.Width * this.view.Percent * 0.01f;

        canvas.DrawLine(startX, rect.Center.Y, endX, rect.Center.Y);
    }
}
