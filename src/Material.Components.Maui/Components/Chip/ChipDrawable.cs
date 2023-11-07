namespace Material.Components.Maui;

internal class ChipDrawable(Chip view) : IDrawable, IDisposable
{
    readonly PathF closePath = PathBuilder.Build(
        "M6.4,19 L5,17.6 10.6,12 5,6.4 6.4,5 12,10.6 17.6,5 19,6.4 13.4,12 19,17.6 17.6,19 12,13.4Z"
    );

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(view.GetClipPath(rect));

        canvas.DrawBackground(view, rect);
        canvas.DrawOutline(view, rect);
        canvas.DrawOverlayLayer(view, rect);

        var scale = rect.Height / 32f;
        var closeRect = new RectF(
            rect.Width - (8f + 18f) * scale,
            8f * scale,
            18f * scale,
            18f * scale
        );

        if (view.RipplePercent is 0f or 1f)
            canvas.DrawStateLayer(view, rect, view.ViewState);
        else
            canvas.DrawRipple(
                view,
                view.LastTouchPoint,
                view.RippleSize,
                view.RipplePercent
            );

        canvas.DrawIcon(
            view,
            new RectF(8f * scale, 8f * scale, 18f * scale, 18f * scale),
            18,
            scale
        );
        var iconSize = (!string.IsNullOrEmpty(view.IconData) ? 18f : 0f) * scale;
        canvas.DrawText(
            view,
            new RectF(
                iconSize,
                0f,
                rect.Width - iconSize - (view.HasCloseButton ? 18f * scale : 0f),
                rect.Height
            )
        );

        if (view.HasCloseButton)
            this.DrawCloseIcon(canvas, view, closeRect, scale);

        canvas.ResetState();
    }

    void DrawCloseIcon(ICanvas canvas, IIconElement element, RectF rect, float scale)
    {
        canvas.FillColor = element.IconColor.WithAlpha(
            element.ViewState is ViewState.Disabled ? 0.38f : 1f
        );

        var path = this.closePath.AsScaledPath(18f / 24f * scale);
        var sx = rect.Center.X - 18f / 2 * scale;
        var sy = rect.Center.Y - 18f / 2 * scale;
        path.Move(sx, sy);
        canvas.FillPath(path);
    }

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.closePath?.Dispose();
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
