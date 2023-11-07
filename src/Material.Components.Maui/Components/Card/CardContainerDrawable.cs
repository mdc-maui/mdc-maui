namespace Material.Components.Maui;

internal class CardContainerDrawable(CardContainer view) : IDrawable
{
    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;

        canvas.ClipPath(view.GetClipPath(rect));
        canvas.DrawBackground(view, rect);
        canvas.DrawOutline(view, rect);
        canvas.DrawOverlayLayer(view, rect);

        canvas.ResetState();
    }
}
