namespace Material.Components.Maui;

internal class ExtendedFABDrawable(ExtendedFAB view) : IDrawable
{
    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(view.GetClipPath(rect));

        canvas.DrawBackground(view, rect);
        canvas.DrawOverlayLayer(view, rect);

        if (view.RipplePercent is 0f or 1f)
            canvas.DrawStateLayer(view, rect, view.ViewState);
        else
            canvas.DrawRipple(
                view,
                view.LastTouchPoint,
                view.RippleSize,
                view.RipplePercent
            );

        var scale = rect.Height / 56f;
        canvas.DrawIcon(
            view,
            new RectF(16f * scale, 16f * scale, 24f * scale, 24f * scale),
            24,
            scale
        );

        var iconSize = (!string.IsNullOrEmpty(view.IconData) ? 24f : 0f) * scale;
        canvas.DrawText(view, new RectF(iconSize, 0, rect.Width - iconSize, rect.Height));
        canvas.ResetState();
    }
}
