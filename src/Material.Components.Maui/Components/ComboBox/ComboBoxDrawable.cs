namespace Material.Components.Maui;

internal class ComboBoxDrawable : IDrawable, IDisposable
{
    readonly ComboBox view;

    readonly PathF arrowDropDownIcon = PathBuilder.Build("M12,15 L7,10H17Z");
    readonly PathF arrowDropUpIcon = PathBuilder.Build("M7,14 L12,9 17,14Z");

    public ComboBoxDrawable(ComboBox view)
    {
        this.view = view;
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        var scale = rect.Height / 64f;

        var containerRect = new RectF
        {
            Left = rect.Left,
            Top = rect.Top + 8f * scale,
            Width = rect.Width,
            Height = rect.Height - 8f * scale
        };

        canvas.DrawBackground(this.view, containerRect);
        this.DrawItem(canvas, containerRect, scale);
        this.DrawDrapIcon(canvas, containerRect, scale);
        this.DrawActiveIndicator(canvas, containerRect);
        this.DrawLabelText(canvas, containerRect, scale);
        canvas.DrawOutline(this.view, containerRect);

        canvas.RestoreState();
    }

    void DrawItem(ICanvas canvas, RectF rect, float scale)
    {
        if (this.view.SelectedItem != null)
        {
            var textRect = new RectF
            {
                Left = rect.Left + 16f * scale,
                Width = rect.Width - (16f - 24f - 16f) * scale,
                Top = rect.Top,
                Height = this.view.OutlineWidth == 0 ? rect.Height - 8f * scale : rect.Height
            };

            canvas.DrawText(
                this.view,
                this.view.SelectedItem.Text,
                textRect,
                HorizontalAlignment.Left,
                this.view.OutlineWidth == 0 ? VerticalAlignment.Bottom : VerticalAlignment.Center
            );
        }
    }

    void DrawDrapIcon(ICanvas canvas, RectF rect, float scale)
    {
        using var path = (
            this.view.IsDropDown ? this.arrowDropUpIcon : this.arrowDropDownIcon
        ).AsScaledPath(scale);
        var sx = rect.Right - 40f * scale;
        var sy = rect.Center.Y - 24f / 2 * scale;
        path.Move(sx, sy);
        canvas.FillColor = MaterialColors.OnSurface;
        canvas.FillPath(path);
    }

    private void DrawActiveIndicator(ICanvas canvas, RectF rect)
    {
        if (this.view.ActiveIndicatorHeight == 0)
            return;

        canvas.FillColor = this.view.ActiveIndicatorColor.MultiplyAlpha(
            this.view.ViewState is ViewState.Disabled ? 0.38f : 1f
        );

        canvas.FillRectangle(
            rect.Left,
            rect.Bottom - this.view.ActiveIndicatorHeight,
            rect.Width,
            this.view.ActiveIndicatorHeight
        );
    }

    void DrawLabelText(ICanvas canvas, RectF rect, float scale)
    {
        var percent =
            this.view.SelectedIndex != -1 || this.view.IsDropDown
                ? 1f - this.view.LabelAnimationPercent
                : this.view.LabelAnimationPercent;

        var fontSize = 12f + (this.view.FontSize - 12f) * percent;

        canvas.FontColor = this.view.LabelFontColor.MultiplyAlpha(
            this.view.ViewState is ViewState.Disabled ? 0.38f : 1f
        );

        var labelSize = this.view.GetStringSize(this.view.LabelText, fontSize);
        var minLabelSize = this.view.GetStringSize(this.view.LabelText, 12f);
        var maxLabelSize = this.view.GetStringSize(this.view.LabelText);

        if (this.view.OutlineWidth == 0)
        {
            var minY = rect.Top + 8f * scale;
            var maxY = rect.Center.Y - maxLabelSize.Height / 2;

            var labelRect = new RectF
            {
                Left = rect.Left + 16f * scale,
                Top = minY + (maxY - minY) * percent,
                Width = rect.Width - (16f - 24f - 16f) * scale,
                Height = labelSize.Height
            };

            canvas.DrawText(
                this.view,
                this.view.LabelText,
                this.view.LabelFontColor,
                fontSize,
                labelRect,
                HorizontalAlignment.Left,
                VerticalAlignment.Center
            );
        }
        else
        {
            var minY = rect.Top - minLabelSize.Height / 2;
            var maxY = rect.Center.Y - maxLabelSize.Height / 2;

            var labelRect = new RectF
            {
                Left = rect.Left + 16f * scale,
                Top = minY + (maxY - minY) * percent,
                Width = rect.Width - (16f - 24f - 16f) * scale,
                Height = labelSize.Height
            };

            canvas.DrawText(
                this.view,
                this.view.LabelText,
                this.view.LabelFontColor,
                fontSize,
                labelRect,
                HorizontalAlignment.Left,
                VerticalAlignment.Center
            );

            var clipRect = new RectF
            {
                Left =
                    rect.Left
                    + 16f * scale
                    + minLabelSize.Width / 2
                    - (minLabelSize.Width / 2 + 4f * scale) * (1 - percent),
                Top = rect.Top - minLabelSize.Height / 2,
                Width = (minLabelSize.Width + 8f * scale) * (1 - percent),
                Height = minLabelSize.Height
            };

            //clip rect in outline
            canvas.SubtractFromClip(clipRect);
        }
    }

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.arrowDropDownIcon.Dispose();
                this.arrowDropUpIcon.Dispose();
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
