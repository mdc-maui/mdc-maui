namespace Material.Components.Maui.Core.Card;
internal class CardDrawable
{
    private readonly MCard view;
    public CardDrawable(MCard card)
    {
        this.view = card;
    }

    internal void Draw(SKCanvas canvas, SKRect bounds)
    {
        canvas.Clear();
        this.DrawBackground(canvas, bounds);
        this.DrawOutline(canvas, bounds);
        this.DrawOverlayLayer(canvas, bounds);
        this.DrawRippleEffect(canvas, bounds);
    }

    internal void DrawBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.BackgroundColour.MultiplyAlpha(this.view.BackgroundOpacity);
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawBackground(bounds, color, radii);
    }

    internal void DrawOverlayLayer(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.ControlState == ControlState.Disabled) return;
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawOverlayLayer(bounds, this.view.Elevation, radii);
    }

    internal void DrawOutline(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.OutlineWidth != 0)
        {
            var color = this.view.OutlineColor.MultiplyAlpha(this.view.OutlineOpacity);
            var width = this.view.OutlineWidth;
            var radii = this.view.GetRadii(bounds.Width, bounds.Height);
            canvas.DrawOutline(bounds, color, width, radii);
        }
    }

    internal void DrawRippleEffect(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.RippleColor;
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawRippleEffect(bounds, radii, this.view.RippleSize, this.view.TouchPoint, color, this.view.RipplePercent);
    }
}
