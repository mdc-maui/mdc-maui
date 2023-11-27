namespace Material.Components.Maui;
internal class NavigationDrawerItemDrawable(NavigationDrawerItem view) : IDrawable
{
    private readonly NavigationDrawerItem view = view;

    public void Draw(ICanvas canvas, RectF rect)
    {
        if (rect == RectF.Zero) return;

        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(this.view.GetClipPath(rect));

        canvas.DrawBackground(this.view, rect);

        if (this.view.RipplePercent is 0f or 1f)
            canvas.DrawStateLayer(this.view, rect, this.view.ViewState);
        else
            canvas.DrawRipple(
                this.view,
                this.view.LastTouchPoint,
                this.view.RippleSize,
                this.view.RipplePercent
            );

        canvas.DrawIcon(
           this.view,
           new RectF(24f, 16f, 24f, 24f),
           24,
           1f
       );

        canvas.DrawText(this.view, new RectF(60f, 0, rect.Width - 40f, rect.Height), HorizontalAlignment.Left);
        canvas.ResetState();
    }
}
