namespace Material.Components.Maui;

internal class FABDrawable : IDrawable
{
    readonly FAB view;

    public FABDrawable(FAB view)
    {
        this.view = view;
    }

    void IDrawable.Draw(ICanvas canvas, RectF rect)
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
        canvas.ResetState();
    }
}
