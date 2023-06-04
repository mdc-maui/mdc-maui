using Material.Components.Maui.Extensions;
using RectF = Microsoft.Maui.Graphics.RectF;

namespace Material.Components.Maui;

internal class ButtonDrawable : IDrawable
{
    readonly Button view;

    public ButtonDrawable(Button view)
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
        canvas.DrawStateLayer(this.view, rect, this.view.ViewState);

        if (this.view.RipplePercent != 0f)
            canvas.DrawRipple(
                this.view,
                this.view.LastTouchPoint,
                this.view.RippleSize,
                this.view.RipplePercent
            );

        var scale = rect.Height / 40;
        canvas.DrawIcon(
            this.view,
            new RectF(16 * scale, 11 * scale, 18 * scale, 18 * scale),
            18,
            scale
        );

        var iconSize = (!string.IsNullOrEmpty(this.view.IconData) ? 18 : 0) * scale;
        canvas.DrawText(this.view, new RectF(iconSize, 0, rect.Width - iconSize, rect.Height));
        canvas.ResetState();
    }
}
