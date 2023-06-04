using Material.Components.Maui.Extensions;

namespace Material.Components.Maui;

internal class CardContainerDrawable : IDrawable
{
    readonly CardContainer view;

    public CardContainerDrawable(CardContainer view)
    {
        this.view = view;
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;

        canvas.ClipPath(this.view.GetClipPath(rect));
        canvas.DrawBackground(this.view, rect);
        canvas.DrawOutline(this.view, rect);
        canvas.DrawOverlayLayer(this.view, rect);

        canvas.ResetState();
    }
}
