namespace Material.Components.Maui;

internal class NavigationBarItemDrawable : IDrawable
{
    private readonly NavigationBarItem view;

    public NavigationBarItemDrawable(NavigationBarItem view)
    {
        this.view = view;
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;

        canvas.DrawBackground(this.view, rect);
        this.DrawStateLayer(canvas, rect);

        canvas.DrawOverlayLayer(this.view, rect);

        var iconBounds = new RectF(rect.Center.X - 12, rect.X + 16, 24, 24);
        canvas.DrawIcon(this.view, iconBounds, 24, 1f);

        var textBounds = new RectF(rect.Left, rect.Top, rect.Width, rect.Height - 16);
        canvas.DrawText(
            this.view,
            textBounds,
            HorizontalAlignment.Center,
            VerticalAlignment.Bottom
        );
        canvas.ResetState();
    }

    private void DrawStateLayer(ICanvas canvas, RectF rect)
    {
        var bounds = new RectF(rect.Center.X - 32, rect.Top + 12, 64, 32);

        var path = new PathF();
        path.AppendRoundedRectangle(bounds, 16f, 16f, 16f, 16f, true);

        canvas.SaveState();
        canvas.ClipPath(path);

        if (this.view.IsActived)
        {
            canvas.FillColor = this.view.ActiveIndicatorColor.MultiplyAlpha(
                this.view.ViewState is ViewState.Disabled ? 0.12f : 1f
            );
            canvas.FillRectangle(rect);
        }

        if (this.view.RipplePercent is 0f or 1f)
            canvas.DrawStateLayer(this.view, bounds, this.view.ViewState);
        else
            canvas.DrawRipple(
                this.view,
                this.view.LastTouchPoint,
                this.view.RippleSize,
                this.view.RipplePercent
            );

        canvas.ResetState();
    }

    private void DrawIcon(ICanvas canvas, RectF rect)
    {
        var iconBounds = new RectF(rect.Center.X - 12, rect.Center.Y - 12, 24, 24);
    }
}
