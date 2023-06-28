namespace Material.Components.Maui;

internal class MenuItemDrawable : IDrawable
{
    readonly MenuItem view;

    public MenuItemDrawable(MenuItem view)
    {
        this.view = view;
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipRectangle(rect);

        if (this.view.RipplePercent is 0f or 1f)
            canvas.DrawStateLayer(this.view, rect, this.view.ViewState);
        else
            canvas.DrawRipple(
                this.view,
                this.view.LastTouchPoint,
                this.view.RippleSize,
                this.view.RipplePercent
            );

        canvas.DrawIcon(this.view, new RectF(12f, 12f, 24f, 24f), 24, 1f);

        var iconSize = !string.IsNullOrEmpty(this.view.IconData) ? 24f : 0f;
        canvas.DrawText(
            this.view,
            new RectF(12f + iconSize + 12f, 0, rect.Width - iconSize, rect.Height),
            HorizontalAlignment.Left,
            VerticalAlignment.Center
        );

        if (!string.IsNullOrEmpty(view.TrailingIconData))
        {
            canvas.FillColor = view.TrailingIconColor.WithAlpha(
                view.ViewState is ViewState.Disabled ? 0.38f : 1f
            );

            using var path = new PathF((view as ITrailingIconElement).TrailingIconPath);
            var sx = rect.Right - 12f - 24f;
            var sy = rect.Center.Y - 12f;
            path.Move(sx, sy);
            canvas.FillPath(path);
        }

        canvas.ResetState();
    }
}
