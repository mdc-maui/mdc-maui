namespace Material.Components.Maui;

internal class SegmentedButtonDrawable(SegmentedButton view) : IDrawable, IDisposable
{
    readonly PathF markPath = PathBuilder.Build(
        "M9.55,18 L3.85,12.3 5.275,10.875 9.55,15.15 18.725,5.975 20.15,7.4Z"
    );

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();

        canvas.Antialias = true;
        canvas.ClipPath(view.GetClipPath(rect));

        var itemWidth = rect.Width / view.Items.Count;

        canvas.StrokeSize = view.OutlineWidth;
        canvas.StrokeColor = view.OutlineColor;

        canvas.ResetState();

        for (var i = 0; i < view.Items.Count; i++)
        {
            this.DrawItem(
                canvas,
                new RectF(itemWidth * i, rect.Top, itemWidth, rect.Height),
                view.Items[i]
            );
        }

        canvas.SaveState();

#if ANDROID
        canvas.Scale(canvas.DisplayScale, canvas.DisplayScale);
#endif

        canvas.DrawOutline(view, rect);
        for (var i = 0; i < view.Items.Count - 1; i++)
        {
            var x = itemWidth * (i + 1);
            canvas.DrawLine(x - view.OutlineWidth / 2f, rect.Top, x - view.OutlineWidth / 2f, rect.Bottom);
        }
        canvas.ResetState();
    }

    void DrawItem(ICanvas canvas, RectF rect, SegmentedItem item)
    {
        canvas.SaveState();
#if ANDROID
        canvas.Scale(canvas.DisplayScale, canvas.DisplayScale);
#endif
        this.ClipItemRect(canvas, rect, item);
        canvas.DrawBackground(item, rect);

        if (
            rect.Contains(view.LastTouchPoint)
            && item.ViewState is ViewState.Hovered or ViewState.Pressed
            && view.RipplePercent is not 0f or 1f
        )
            canvas.DrawRipple(
                view,
                view.LastTouchPoint,
                view.RippleSize / 3,
                view.RipplePercent
            );
        else
            canvas.DrawStateLayer(item, rect, item.ViewState);

        var scale = rect.Height / 40f;
        var textSize = view.GetStringSize(item.Text);

        if (item.IsSelected)
        {
            var markIconRect = textSize.IsZero
                ? new RectF(
                    rect.Center.X
                        - (18f + 8f + (!string.IsNullOrEmpty(item.IconData) ? 18f : 0f))
                            / 2
                            * scale,
                    rect.Center.Y - 18f / 2 * scale,
                    18f * scale,
                    18f * scale
                )
                : new RectF(
                    rect.Center.X - (18f + 8f) / 2 * scale - (float)textSize.Width / 2,
                    rect.Center.Y - 18f / 2 * scale,
                    18f * scale,
                    18f * scale
                );
            canvas.FillColor = item.IconColor.WithAlpha(
                view.ViewState is ViewState.Disabled ? 0.38f : 1f
            );

            var path = this.markPath.AsScaledPath(18f / 24f * scale);
            var sx = markIconRect.Center.X - 18f / 2 * scale;
            var sy = markIconRect.Center.Y - 18f / 2 * scale;
            path.Move(sx, sy);
            canvas.FillPath(path);
        }

        if (!string.IsNullOrEmpty(item.IconData) && (textSize.IsZero || !item.IsSelected))
        {
            var iconRect = item.IsSelected
                ? new RectF(
                    rect.Center.X - (18f + 8f + 18f) / 2 * scale + (18f + 8f) * scale,
                    rect.Center.Y - 18f / 2 * scale,
                    18f * scale,
                    18f * scale
                )
                : !textSize.IsZero
                ? new RectF(
                    rect.Center.X - (18f + 8f) / 2 * scale - (float)textSize.Width / 2,
                    rect.Center.Y - 18f / 2 * scale,
                    18f * scale,
                    18f * scale
                )
                : new RectF(
                    rect.Center.X - 18f / 2 * scale,
                    rect.Center.Y - 18f / 2 * scale,
                    18f * scale,
                    18f * scale
                );
            canvas.DrawIcon(item, iconRect, 18, scale);
        }

        if (!textSize.IsZero)
        {
            if (item.IsSelected || !string.IsNullOrEmpty(item.IconData))
            {
                canvas.DrawText(
                    view,
                    item.Text,
                    new RectF(
                        rect.X + (18f + 8f) * scale,
                        rect.Y,
                        rect.Width - (18f + 8f) * scale,
                        rect.Height
                    )
                );
            }
            else
            {
                canvas.DrawText(view, item.Text, rect);
            }
        }

        canvas.ResetState();
    }

    void ClipItemRect(ICanvas canvas, RectF rect, SegmentedItem item)
    {
        using var path = new PathF();
        rect = new RectF(
            rect.X + view.OutlineWidth / 2,
            rect.Y + view.OutlineWidth / 2,
            rect.Width - view.OutlineWidth,
            rect.Height - view.OutlineWidth
        );
        var index = view.Items.IndexOf(item);
        var shape = view.GetShape(rect.Width, rect.Height);
        if (index == 0)
            path.AppendRoundedRectangle(rect, shape.TopLeft, 0, shape.BottomLeft, 0, true);
        else if (index == view.Items.Count - 1)
            path.AppendRoundedRectangle(rect, 0, shape.TopRight, 0, shape.BottomRight, true);
        else
            path.AppendRoundedRectangle(rect, 0, 0, 0, 0, true);

        canvas.ClipPath(path);
    }

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.markPath.Dispose();
            }
            this.disposedValue = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
