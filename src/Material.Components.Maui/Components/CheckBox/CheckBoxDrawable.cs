namespace Material.Components.Maui;

internal class CheckBoxDrawable : IDrawable
{
    readonly CheckBox view;

    public CheckBoxDrawable(CheckBox view)
    {
        this.view = view;
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.Antialias = true;
        var scale = rect.Height / 40f;
        var drawRect = new RectF(11f * scale, 11f * scale, 18f * scale, 18f * scale);
        if (this.view.IsChecked)
        {
            canvas.SaveState();
            canvas.ClipPath(this.view.GetClipPath(drawRect));
            canvas.DrawBackground(this.view, drawRect);
            canvas.RestoreState();
        }

        this.DrawStateLayer(canvas, rect);

        if (this.view.IsChecked)
            canvas.DrawIcon(this.view, drawRect, 18, scale);
        else
            canvas.DrawOutline(this.view, drawRect);
    }

    void DrawStateLayer(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        var drawRect = new PathF();
        drawRect.AppendCircle(rect.Center.X, rect.Center.Y, Math.Max(rect.Width, rect.Height) / 2f);
        canvas.ClipPath(drawRect);

        if (this.view.RipplePercent is 0f or 1f)
            canvas.DrawStateLayer(this.view, rect, this.view.ViewState);
        else
            canvas.DrawRipple(
                this.view,
                this.view.LastTouchPoint,
                this.view.RippleSize,
                this.view.RipplePercent
            );

        canvas.RestoreState();
    }
}
