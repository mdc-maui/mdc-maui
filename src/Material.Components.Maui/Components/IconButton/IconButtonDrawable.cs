using Material.Components.Maui.Extensions;

namespace Material.Components.Maui;

class IconButtonDrawable : IDrawable
{
    readonly IconButton view;

    public IconButtonDrawable(IconButton view) => this.view = view;

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        var scale = rect.Height / 40;
        canvas.Antialias = true;
        canvas.ClipPath(this.view.GetClipPath(rect));

        canvas.DrawBackground(this.view, rect);
        canvas.DrawOutline(this.view, rect);
        canvas.DrawOverlayLayer(this.view, rect);
        canvas.DrawIcon(this.view, rect, scale);
        canvas.DrawStateLayer(this.view, rect, this.view.ViewState);

        if (this.view.RipplePercent != 0f)
            canvas.DrawRipple(
                this.view,
                this.view.LastTouchPoint,
                this.view.RippleSize,
                this.view.RipplePercent
            );

        canvas.ResetState();
    }
}
