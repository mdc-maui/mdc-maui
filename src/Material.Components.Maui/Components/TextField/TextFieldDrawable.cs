namespace Material.Components.Maui;

internal class TextFieldDrawable : IDrawable
{
    readonly TextField view;

    public TextFieldDrawable(TextField view)
    {
        this.view = view;
    }

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.Antialias = true;

        var containerRect = new RectF
        {
            Left = rect.Left,
            Top = rect.Top + 8f,
            Right = rect.Right,
            Bottom = rect.Bottom - 4f - 16f
        };

        var editableRect = new RectF
        {
            Left = rect.Left + (float)this.view.EditablePadding.Left,
            Top = rect.Top + (float)this.view.EditablePadding.Top,
            Width = rect.Width - (float)this.view.EditablePadding.HorizontalThickness,
            Height = rect.Height - (float)this.view.EditablePadding.VerticalThickness
        };

        canvas.DrawBackground(this.view, containerRect);

        this.DrawIcon(canvas, containerRect);
        this.DrawTrailingIcon(canvas, containerRect);
        this.DrawSelection(canvas, editableRect);
        this.DrawText(canvas, editableRect);
        this.DrawCaret(canvas, editableRect);
        this.DrawActiveIndicator(canvas, containerRect);
        this.DrawSupportingText(canvas, rect);
        this.DrawLabelText(canvas, containerRect);
        this.DrawRipple(canvas, containerRect);
        canvas.DrawOutline(this.view, containerRect);
    }

    private void DrawIcon(ICanvas canvas, RectF rect)
    {
        if (string.IsNullOrEmpty(this.view.IconData))
            return;

        canvas.FillColor = this.view.IconColor.WithAlpha(
            this.view.ViewState is ViewState.Disabled ? 0.38f : 1f
        );

        using var path = ((IIconElement)this.view).IconPath.AsScaledPath(1f);
        var sx = rect.Left + 12f;
        var sy = rect.Center.Y - 12f;
        path.Move(sx, sy);
        canvas.FillPath(path);
    }

    private void DrawTrailingIcon(ICanvas canvas, RectF rect)
    {
        if (string.IsNullOrEmpty(this.view.TrailingIconData))
            return;

        canvas.FillColor = this.view.TrailingIconColor.WithAlpha(
            this.view.ViewState is ViewState.Disabled ? 0.38f : 1f
        );

        using var path = ((ITrailingIconElement)this.view).TrailingIconPath.AsScaledPath(1f);
        var sx = rect.Right - 12f - 24f;
        var sy = rect.Center.Y - 12f;
        path.Move(sx, sy);
        canvas.FillPath(path);
    }

    private void DrawText(ICanvas canvas, RectF rect)
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

        var text =
            this.view.InputType is InputType.Password
                ? new string('•', this.view.Text.Length)
                : this.view.Text;
        canvas.DrawString(text, rect, horizontal, VerticalAlignment.Center);
    }

    public void DrawSelection(ICanvas canvas, RectF rect)
    {
        if (!this.view.SelectionRange.IsRange)
            return;

        canvas.FillColor = this.view.CaretColor;
        var (startRect, endRect) = this.view.GetSelectionRect(
            rect.Width + (float)this.view.EditablePadding.HorizontalThickness
        );
        if (startRect.Y != endRect.Y)
        {
            canvas.FillRectangle(
                rect.Left + startRect.X,
                rect.Top + startRect.Y,
                rect.Width - startRect.X,
                startRect.Height
            );

            var offset = endRect.Y - MathF.Floor(startRect.Y + startRect.Height);
            if (offset > 0)
            {
                canvas.FillRectangle(
                    rect.Left,
                    MathF.Floor(rect.Top + startRect.Y + startRect.Height),
                    rect.Width,
                    MathF.Ceiling(offset)
                );
            }

            canvas.FillRectangle(rect.Left, rect.Top + endRect.Y, endRect.X, startRect.Height);
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

    private void DrawSupportingText(ICanvas canvas, RectF rect)
    {
        if (string.IsNullOrEmpty(this.view.SupportingText))
            return;

        var stRect = new Rect
        {
            Left = rect.Left + 16f,
            Top = rect.Bottom - 16f,
            Width = rect.Width - 16f,
            Height = 16f
        };

        var weight = (int)this.view.FontWeight;
        var style = this.view.FontIsItalic ? FontStyleType.Italic : FontStyleType.Normal;

        var font = new Microsoft.Maui.Graphics.Font(this.view.FontFamily, weight, style);

        canvas.Font = font;
        canvas.FontColor = this.view.SupportingFontColor.WithAlpha(
            this.view.ViewState is ViewState.Disabled ? 0.38f : 1f
        );
        canvas.FontSize = 12f;
        canvas.DrawString(
            this.view.SupportingText,
            stRect,
            HorizontalAlignment.Left,
            VerticalAlignment.Bottom
        );
    }

    private void DrawRipple(ICanvas canvas, RectF rect)
    {
        if (string.IsNullOrEmpty(this.view.TrailingIconData))
            return;

        canvas.SaveState();

        var drawRect = new PathF();
        drawRect.AppendCircle(rect.Right - 24f, rect.Center.Y, 20f);
        canvas.ClipPath(drawRect);

        if (this.view.RipplePercent is not 0f and not 1f)
            canvas.DrawRipple(
                this.view,
                this.view.LastTouchPoint,
                this.view.RippleSize,
                this.view.RipplePercent
            );

        canvas.RestoreState();
    }

    private void DrawLabelText(ICanvas canvas, RectF rect)
    {
        var percent =
            !string.IsNullOrEmpty(this.view.Text) || this.view.IsFocused
                ? 1 - this.view.LabelAnimationPercent
                : this.view.LabelAnimationPercent;

        var fontSize = 12f + (this.view.FontSize - 12f) * percent;

        canvas.FontColor = this.view.LabelFontColor.MultiplyAlpha(
            this.view.ViewState is ViewState.Disabled ? 0.38f : 1f
        );

        var labelSize = this.view.GetStringSize(this.view.LabelText, fontSize);
        var minLabelSize = this.view.GetStringSize(this.view.LabelText, 12f);
        var maxLabelSize = this.view.GetStringSize(this.view.LabelText);

        var rectLeft =
            rect.Left + (!string.IsNullOrEmpty(this.view.IconData) ? 12f + 24f + 16f : 16f);

        if (this.view.OutlineWidth == 0)
        {
            var minY = rect.Top + 8f;
            var maxY = rect.Center.Y - maxLabelSize.Height / 2;

            var labelRect = new RectF
            {
                Left = rectLeft,
                Top = minY + (maxY - minY) * percent,
                Width = rect.Width - (16f - 24f - 16f),
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
                Left = rectLeft,
                Top = minY + (maxY - minY) * percent,
                Width = rect.Width - (16f - 24f - 16f),
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
                    rectLeft
                    + minLabelSize.Width / 2
                    - (minLabelSize.Width / 2 + 4f) * (1 - percent),
                Top = rect.Top - minLabelSize.Height / 2,
                Width = (minLabelSize.Width + 8f) * (1 - percent),
                Height = minLabelSize.Height
            };

            //clip rect in outline
            canvas.SubtractFromClip(clipRect);
        }
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
}
