namespace Material.Components.Maui;

internal class ChipDrawable : IDrawable, IDisposable
{
    readonly Chip view;

    readonly PathF closePath = PathBuilder.Build(
        "M6.4,19 L5,17.6 10.6,12 5,6.4 6.4,5 12,10.6 17.6,5 19,6.4 13.4,12 19,17.6 17.6,19 12,13.4Z"
    );

    public ChipDrawable(Chip view)
    {
        this.view = view;
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(this.view.GetClipPath(rect));

        canvas.DrawBackground(this.view, rect);
        canvas.DrawOutline(this.view, rect);
        canvas.DrawOverlayLayer(this.view, rect);

        var scale = rect.Height / 32f;
        var closeRect = new RectF(
            rect.Width - (8f + 18f) * scale,
            8f * scale,
            18f * scale,
            18f * scale
        );

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
            new RectF(8f * scale, 8f * scale, 18f * scale, 18f * scale),
            18,
            scale
        );
        var iconSize = (!string.IsNullOrEmpty(this.view.IconData) ? 18f : 0f) * scale;
        canvas.DrawText(
            this.view,
            new RectF(
                iconSize,
                0f,
                rect.Width - iconSize - (this.view.HasCloseButton ? 18f * scale : 0f),
                rect.Height
            )
        );

        if (this.view.HasCloseButton)
            this.DrawCloseIcon(canvas, this.view, closeRect, scale);

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
