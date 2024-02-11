namespace Material.Components.Maui;

internal class TabItemDrawable(TabItem view) : IDrawable
{
    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;

        canvas.DrawBackground(view, rect);

        if (view.RipplePercent is 0f or 1f)
            canvas.DrawStateLayer(view, rect, view.ViewState);
        else
            canvas.DrawRipple(view, view.LastTouchPoint, view.RippleSize, view.RipplePercent);

#if ANDROID
        canvas.Scale(canvas.DisplayScale, canvas.DisplayScale);
#endif

        var tabs = view.GetParentElement<Tabs>();
        switch (tabs.ItemStyle)
        {
            case ItemStyle.Secondary:
            case ItemStyle.ScrollingSecondary:
                this.DrawSecondaryStyle(canvas, rect);
                break;
            default:
                this.DrawPrimaryStyle(canvas, rect);
                break;
        }

        canvas.ResetState();
    }

    public void DrawPrimaryStyle(ICanvas canvas, RectF rect)
    {
        var iconBounds = new RectF(rect.Center.X - 12, rect.Top + 8, 24, 24);
        canvas.DrawIcon(view, iconBounds, 24, 1f);

        var textBounds = rect;
        if (((IIconElement)view).IconPath is not null)
            textBounds = new RectF(rect.Left, rect.Top + 32, rect.Width, rect.Height - 40);

        canvas.DrawText(view, textBounds, HorizontalAlignment.Center, VerticalAlignment.Center);

        if (view.IsActived)
        {
            var ActiveIndicatorWidth = string.IsNullOrEmpty(view.Text)
                ? 24f
                : view.GetStringSize().Width;

            var path = new PathF();
            path.AppendRoundedRectangle(
                new RectF(
                    rect.Center.X - ActiveIndicatorWidth / 2f,
                    rect.Bottom - view.ActiveIndicatorHeight,
                    ActiveIndicatorWidth,
                    view.ActiveIndicatorHeight
                ),
                view.ActiveIndicatorShape.TopLeft,
                view.ActiveIndicatorShape.TopRight,
                view.ActiveIndicatorShape.BottomLeft,
                view.ActiveIndicatorShape.BottomRight,
                true
            );

            canvas.SaveState();
            canvas.ClipPath(path);
            canvas.FillColor = view.ActiveIndicatorColor;
            canvas.FillRectangle(rect);
        }
    }

    public void DrawSecondaryStyle(ICanvas canvas, RectF rect)
    {
        var textWidth = view.GetStringSize().Width;
        var sx = (rect.Width - (24 + 8 + textWidth)) / 2;

        var iconBounds = new RectF(sx, rect.Center.Y - 12, 24, 24);
        canvas.DrawIcon(view, iconBounds, 24, 1f);

        if (((IIconElement)view).IconPath is not null)
        {
            var textBounds = new RectF(sx + 32, rect.Top, rect.Width - sx - 32, rect.Height);
            canvas.DrawText(view, textBounds, HorizontalAlignment.Left, VerticalAlignment.Center);
        }
        else
            canvas.DrawText(view, rect, HorizontalAlignment.Center, VerticalAlignment.Center);

        if (view.IsActived)
        {
            var path = new PathF();
            path.AppendRectangle(
                new RectF(
                    rect.Left,
                    rect.Bottom - view.ActiveIndicatorHeight,
                    rect.Width,
                    view.ActiveIndicatorHeight
                ),
                true
            );

            canvas.SaveState();
            canvas.ClipPath(path);
            canvas.FillColor = view.ActiveIndicatorColor;
            canvas.FillRectangle(rect);
        }
    }
}
