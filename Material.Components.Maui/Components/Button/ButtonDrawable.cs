namespace Material.Components.Maui.Core;

internal abstract class ButtonDrawable
{
    private readonly IButton view;

    internal ButtonDrawable(IButton view)
    {
        this.view = view;
    }

    internal abstract void Draw(SKCanvas canvas, SKRect bounds);

    internal void DrawBackground(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.BackgroundColour.MultiplyAlpha(this.view.BackgroundOpacity);
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawBackground(bounds, color, radii);
    }

    internal void DrawStateLayer(SKCanvas canvas, SKRect bounds)
    {
#if !__MOBILE__
        if (this.view.StateLayerOpacity != 0f)
        {
            var color = this.view.StateLayerColor.MultiplyAlpha(this.view.StateLayerOpacity);
            var radii = this.view.GetRadii(bounds.Width, bounds.Height);
            canvas.DrawStateLayer(bounds, color, radii);
        }
#endif
    }

    internal void DrawOverlayLayer(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.ControlState == ControlState.Disabled)
            return;
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawOverlayLayer(bounds, this.view.Elevation, radii);
    }

    internal void DrawOutline(SKCanvas canvas, SKRect bounds)
    {
        if (this.view.OutlineWidth > 0)
        {
            var color = this.view.OutlineColor;
            var width = this.view.OutlineWidth;
            var radii = this.view.GetRadii(bounds.Width, bounds.Height);
            canvas.DrawOutline(bounds, color, width, radii);
        }
    }

    internal void DrawRippleEffect(SKCanvas canvas, SKRect bounds)
    {
        var color = this.view.RippleColor;
        var radii = this.view.GetRadii(bounds.Width, bounds.Height);
        canvas.DrawRippleEffect(
            bounds,
            radii,
            this.view.RippleSize,
            this.view.TouchPoint,
            color,
            this.view.RipplePercent
        );
    }
}
