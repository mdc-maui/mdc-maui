namespace Material.Components.Maui;

internal class SegmentedButtonDrawable : IDrawable, IDisposable
{
    readonly SegmentedButton view;

    readonly PathF markPath = PathBuilder.Build(
        "M9.55,18 L3.85,12.3 5.275,10.875 9.55,15.15 18.725,5.975 20.15,7.4Z"
    );

    public SegmentedButtonDrawable(SegmentedButton view)
    {
        this.view = view;
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();

        canvas.Antialias = true;
        canvas.ClipPath(this.view.GetClipPath(rect));

        var itemWidth = rect.Width / this.view.Items.Count;

        canvas.StrokeSize = this.view.OutlineWidth;
        canvas.StrokeColor = this.view.OutlineColor;
        for (var i = 0; i < this.view.Items.Count - 1; i++)
        {
            var x = itemWidth * (i + 1);
            using var path = new PathF();
            path.MoveTo(x, rect.Top);
            path.LineTo(x, rect.Bottom);
            canvas.DrawPath(path);
        }

        canvas.ResetState();
        for (var i = 0; i < this.view.Items.Count; i++)
        {
            this.DrawItem(
                canvas,
                new RectF(itemWidth * i, rect.Top, itemWidth, rect.Height),
                this.view.Items[i]
            );
        }
        canvas.SaveState();

#if ANDROID
        canvas.Scale(canvas.DisplayScale, canvas.DisplayScale);
#endif
        canvas.DrawOutline(this.view, rect);
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
            rect.Contains(this.view.LastTouchPoint)
            && item.ViewState is ViewState.Hovered or ViewState.Pressed
            && this.view.RipplePercent is not 0f or 1f
        )
            canvas.DrawRipple(
                this.view,
                this.view.LastTouchPoint,
                this.view.RippleSize / 3,
                this.view.RipplePercent
            );
        else
            canvas.DrawStateLayer(item, rect, item.ViewState);

        var scale = rect.Height / 40f;
        var textSize = item.GetStringSize();

        if (item.IsSelected)
        {
            RectF markIconRect;
            if (textSize.IsZero)
                markIconRect = new RectF(
                    rect.Center.X
                        - (18f + 8f + (!string.IsNullOrEmpty(item.IconData) ? 18f : 0f))
                            / 2
                            * scale,
                    rect.Center.Y - 18f / 2 * scale,
                    18f * scale,
                    18f * scale
                );
            else
                markIconRect = new RectF(
                    rect.Center.X - (18f + 8f) / 2 * scale - (float)textSize.Width / 2,
                    rect.Center.Y - 18f / 2 * scale,
                    18f * scale,
                    18f * scale
                );

            canvas.FillColor = item.IconColor.WithAlpha(
                this.view.ViewState is ViewState.Disabled ? 0.38f : 1f
            );

            var path = this.markPath.AsScaledPath(18f / 24f * scale);
            var sx = markIconRect.Center.X - 18f / 2 * scale;
            var sy = markIconRect.Center.Y - 18f / 2 * scale;
            path.Move(sx, sy);
            canvas.FillPath(path);
        }

        if (!string.IsNullOrEmpty(item.IconData) && (textSize.IsZero || !item.IsSelected))
        {
            RectF iconRect;
            if (item.IsSelected)
                iconRect = new RectF(
                    rect.Center.X - (18f + 8f + 18f) / 2 * scale + (18f + 8f) * scale,
                    rect.Center.Y - 18f / 2 * scale,
                    18f * scale,
                    18f * scale
                );
            else if (!textSize.IsZero)
                iconRect = new RectF(
                    rect.Center.X - (18f + 8f) / 2 * scale - (float)textSize.Width / 2,
                    rect.Center.Y - 18f / 2 * scale,
                    18f * scale,
                    18f * scale
                );
            else
                iconRect = new RectF(
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
                    item,
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
                canvas.DrawText(item, rect);
            }
        }

        canvas.ResetState();
    }

    void ClipItemRect(ICanvas canvas, RectF rect, SegmentedItem item)
    {
        using var path = new PathF();
        rect = new RectF(
            rect.X + this.view.OutlineWidth / 2,
            rect.Y + this.view.OutlineWidth / 2,
            rect.Width - this.view.OutlineWidth,
            rect.Height - this.view.OutlineWidth
        );
        var index = this.view.Items.IndexOf(item);
        var shape = this.view.GetShape(rect.Width, rect.Height);
        if (index == 0)
            path.AppendRoundedRectangle(rect, shape.TopLeft, 0, shape.BottomLeft, 0, true);
        else if (index == this.view.Items.Count - 1)
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
