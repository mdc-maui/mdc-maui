namespace Material.Components.Maui;

internal class NavigationBarItemDrawable(NavigationBarItem view) : IDrawable
{
    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;

        canvas.DrawBackground(view, rect);
        this.DrawStateLayer(canvas, rect);


        var iconBounds = new RectF(rect.Center.X - 12, rect.X + 16, 24, 24);
        canvas.DrawIcon(view, iconBounds, 24, 1f);

        var textBounds = new RectF(rect.Left, rect.Top, rect.Width, rect.Height - 16);
        canvas.DrawText(
            view,
            textBounds,
            HorizontalAlignment.Center,
            VerticalAlignment.Bottom
        );
        canvas.ResetState();
    }

    private void DrawStateLayer(ICanvas canvas, RectF rect)
    {
        var bounds = new RectF(
            rect.Center.X - 32,
            rect.Top + 12,
            64,
            view.ActiveIndicatorHeight
        );

        var path = new PathF();
        path.AppendRoundedRectangle(bounds, 16f, 16f, 16f, 16f, true);

        canvas.SaveState();
        canvas.ClipPath(path);

        if (view.IsActived)
        {
            canvas.FillColor = view.ActiveIndicatorColor.MultiplyAlpha(
                view.ViewState is ViewState.Disabled ? 0.12f : 1f
            );
            canvas.FillRectangle(rect);
        }

        if (view.RipplePercent is 0f or 1f)
            canvas.DrawStateLayer(view, bounds, view.ViewState);
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
