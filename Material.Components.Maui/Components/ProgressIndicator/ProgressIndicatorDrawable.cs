namespace Material.Components.Maui.Core;
internal class ProgressIndicatorDrawable
{
    private readonly ProgressIndicator view;
    internal ProgressIndicatorDrawable(ProgressIndicator view)
    {
        this.view = view;
    }

    internal void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        if (this.view.IndicatorType is IndicatorType.Circular)
        {
            this.DrawCircularBackground(canvas, bounds);
            if (this.view.IsIndeterminate)
            {
                this.DrawCircularIndeterminateAnimation(canvas, bounds);
            }
            else
            {
                this.DrawCircularProgressIndicator(canvas, bounds);
            }
        }
        else
        {
            this.DrawLinearBackground(canvas, bounds);
            if (this.view.IsIndeterminate)
            {
                this.DrawLinearIndeterminateAnimation(canvas, bounds);
            }
            else
            {
                this.DrawLinearProgressIndicator(canvas, bounds);
            }
        }
    }

    private void DrawCircularBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.BackgroundColour.MultiplyAlpha(this.view.BackgroundOpacity);
        var radii = new CornerRadius(bounds.Width * 0.5f).GetRadii();
        canvas.DrawBackground(bounds, color, radii);
    }

    private void DrawCircularIndeterminateAnimation(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        bounds.Inflate(-10f, -10f);
        var paint = new SKPaint
        {
            Color = this.view.ActiveIndicatorColor.ToSKColor(),
            IsAntialias = true,
            IsStroke = true,
            StrokeWidth = 4,
        };
        var percent = this.view.AnimationPercent;
        if (this.view.AnimationIsPositive)
        {
            canvas.DrawArc(bounds, 360 * percent, 320 * percent, false, paint);
        }
        else
        {
            canvas.DrawArc(bounds, -360 * percent * 2, 320 * percent, false, paint);
        }
        canvas.Restore();
    }


    private void DrawCircularProgressIndicator(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        bounds.Inflate(-10f, -10f);
        var paint = new SKPaint
        {
            Color = this.view.ActiveIndicatorColor.ToSKColor(),
            IsAntialias = true,
            IsStroke = true,
            StrokeWidth = 4,
        };
        canvas.DrawArc(bounds, 270, this.view.Percent * 3.6f, false, paint);
        canvas.Restore();
    }

    private void DrawLinearBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.BackgroundColour.MultiplyAlpha(this.view.BackgroundOpacity);
        var radii = new CornerRadius(0f).GetRadii();
        canvas.DrawBackground(bounds, color, radii);
    }

    private void DrawLinearIndeterminateAnimation(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.ActiveIndicatorColor.ToSKColor(),
            IsAntialias = true,
            IsStroke = true,
            StrokeWidth = 4,
        };
        var percent = this.view.AnimationPercent;
        var startX = (bounds.Width * percent) - (bounds.Width * (1f - percent) * 0.5f);
        var endX = startX + (bounds.Width * 0.5f);
        canvas.DrawLine(startX, bounds.MidY, endX, bounds.MidY, paint);
        canvas.Restore();
    }

    private void DrawLinearProgressIndicator(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();
        var paint = new SKPaint
        {
            Color = this.view.ActiveIndicatorColor.ToSKColor(),
            IsAntialias = true,
            IsStroke = true,
            StrokeWidth = 4,
        };
        var startX = 0f;
        var endX = bounds.Width * this.view.Percent * 0.01f;
        canvas.DrawLine(startX, bounds.MidY, endX, bounds.MidY, paint);
        canvas.Restore();
    }
}
