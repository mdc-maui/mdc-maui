namespace Material.Components.Maui;

internal class ExtendedFABDrawable : IDrawable
{
    readonly ExtendedFAB view;

    public ExtendedFABDrawable(ExtendedFAB view)
    {
        this.view = view;
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(this.view.GetClipPath(rect));

        canvas.DrawBackground(this.view, rect);
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

        var scale = rect.Height / 56f;
        canvas.DrawIcon(
            this.view,
            new RectF(16f * scale, 16f * scale, 24f * scale, 24f * scale),
            24,
            scale
        );

        var iconSize = (!string.IsNullOrEmpty(this.view.IconData) ? 24f : 0f) * scale;
        canvas.DrawText(this.view, new RectF(iconSize, 0, rect.Width - iconSize, rect.Height));
        canvas.ResetState();
    }
}
