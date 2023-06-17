#if ANDROID
using Android.Content.Res;
using Android.Views;
#endif

namespace Material.Components.Maui;

internal class SwitchDrawable : IDrawable
{
    readonly Switch view;

    public SwitchDrawable(Switch view)
    {
        this.view = view;
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        var scale = rect.Height / 40f;
        var drawRect = new RectF(4f * scale, 4f * scale, rect.Width - 8f * scale, 32f * scale);
        canvas.ClipPath(this.view.GetClipPath(drawRect));
        canvas.DrawBackground(this.view, drawRect);
        canvas.DrawOutline(this.view, drawRect);

        canvas.ResetState();

#if ANDROID
        canvas.Scale(canvas.DisplayScale, canvas.DisplayScale);
#endif
        this.DrawStateLayer(canvas, rect, scale);

        this.DrawThumb(canvas, drawRect, scale);
    }

    void DrawStateLayer(ICanvas canvas, RectF rect, float scale)
    {
        canvas.SaveState();

        var drawRect = new PathF();
        var x = (this.view.IsSelected ? rect.Width - 20f * scale : 20f * scale);
        drawRect.AppendCircle(x, rect.Center.Y, rect.Height / 2f);
        canvas.ClipPath(drawRect);
        canvas.DrawStateLayer(this.view, rect, this.view.ViewState);

        canvas.RestoreState();
    }

    void DrawThumb(ICanvas canvas, RectF rect, float scale)
    {
        var thumbSize =
            (
                this.view.ViewState is ViewState.Pressed
                    ? 28f
                    : this.view.IsSelected || !string.IsNullOrEmpty(this.view.IconData)
                        ? 24f
                        : 16f
            ) * scale;

        var x =
            (this.view.IsSelected ? rect.Width - 16f * scale : 16f * scale)
            - thumbSize / 2f
            + rect.X;
        var thumbRect = new RectF(x, 16f * scale - thumbSize / 2f + rect.Y, thumbSize, thumbSize);

        canvas.FillColor = this.view.ThumbColor;
        canvas.FillCircle(thumbRect.Center, thumbSize / 2);

        if (!string.IsNullOrEmpty(this.view.IconData))
            canvas.DrawIcon(this.view, thumbRect, 16, scale);
    }
}
