#if ANDROID
using Android.Graphics;
using Android.Text;
using Material.Components.Maui.Extensions.Platform;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Platform;
#endif

using RectF = Microsoft.Maui.Graphics.RectF;

namespace Material.Components.Maui;

internal class TextEditorDrawable : IDrawable
{
    readonly BaseTextEditor view;

    public TextEditorDrawable(BaseTextEditor view)
    {
        this.view = view;
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(this.view.GetClipPath(rect));
        canvas.DrawBackground(this.view, rect);
        this.DrawSelection(canvas, rect);
        this.DrawText(canvas, rect);
        this.DrawCaret(canvas, rect);
        canvas.RestoreState();
    }

    public void DrawText(ICanvas canvas, RectF rect)
    {
        if (string.IsNullOrEmpty(this.view.Text))
            return;

        var weight = (int)this.view.FontWeight;
        var style = this.view.FontIsItalic ? FontStyleType.Italic : FontStyleType.Normal;

        var font = new Microsoft.Maui.Graphics.Font(this.view.FontFamily, weight, style);

        canvas.Font = font;
        canvas.FontColor = this.view.FontColor.WithAlpha(
            this.view.ViewState is ViewState.Disabled ? 0.38f : 1f
        );
        canvas.FontSize = this.view.FontSize;
        var horizontal =
            this.view.TextAlignment == TextAlignment.End
                ? HorizontalAlignment.Right
                : this.view.TextAlignment == TextAlignment.Center
                    ? HorizontalAlignment.Center
                    : HorizontalAlignment.Left;

        canvas.DrawString(this.view.Text, rect, horizontal, VerticalAlignment.Top);
    }

    public void DrawSelection(ICanvas canvas, RectF rect)
    {
        if (!this.view.SelectionRange.IsRange)
            return;

        canvas.FillColor = this.view.CaretColor;
        var (startRect, endRect) = this.view.GetSelectionRect(rect.Width);
        if (startRect.Y != endRect.Y)
        {
            canvas.FillRectangle(
                rect.Left + startRect.X,
                rect.Top + startRect.Y,
                rect.Width - rect.Left - startRect.X,
                startRect.Height
            );

            var offset = endRect.Y - (startRect.Y + startRect.Height);
            if (offset > 0)
            {
                canvas.FillRectangle(
                    rect.Left,
                    rect.Top + startRect.Y + startRect.Height,
                    rect.Width,
                    offset
                );
            }

            canvas.FillRectangle(0, rect.Top + endRect.Y, rect.Left + endRect.X, startRect.Height);
        }
        else
        {
            canvas.FillRectangle(
                rect.Left + startRect.X,
                rect.Top + startRect.Y,
                endRect.X - startRect.X,
                startRect.Height
            );
        }
    }

    public void DrawCaret(ICanvas canvas, RectF rect)
    {
        if (this.view.SelectionRange.IsRange || !this.view.IsDrawCaret)
            return;

        canvas.FillColor = this.view.CaretColor;
        canvas.FillRectangle(
            rect.Left + this.view.CaretInfo.X,
            rect.Top + this.view.CaretInfo.Y,
            2,
            this.view.CaretInfo.Height
        );
    }
}
