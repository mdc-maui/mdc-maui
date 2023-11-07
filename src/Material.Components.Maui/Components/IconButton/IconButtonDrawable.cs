namespace Material.Components.Maui;

class IconButtonDrawable(IconButton view) : IDrawable
{
    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(view.GetClipPath(rect));

        canvas.DrawBackground(view, rect);
        canvas.DrawOutline(view, rect);

        var scale = rect.Height / 40f;
        canvas.DrawIcon(view, rect, 24, scale);
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

        canvas.ResetState();
    }
}
