namespace Material.Components.Maui;

class IconButtonDrawable : IDrawable
{
    readonly IconButton view;

    public IconButtonDrawable(IconButton view) => this.view = view;

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(this.view.GetClipPath(rect));

        canvas.DrawBackground(this.view, rect);
        canvas.DrawOutline(this.view, rect);

        var scale = rect.Height / 40f;
        canvas.DrawIcon(this.view, rect, 24, scale);
        canvas.DrawOverlayLayer(this.view, rect);

        if (this.view.RipplePercent is 0f or 1f)
            canvas.DrawStateLayer(this.view, rect, this.view.ViewState);
        else
            canvas.DrawRipple(
                this.view,
                this.view.LastTouchPoint,
                this.view.RippleSize,
                this.view.RipplePercent
            );

        canvas.ResetState();
    }
}
