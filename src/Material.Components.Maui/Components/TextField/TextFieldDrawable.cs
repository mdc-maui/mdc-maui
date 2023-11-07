namespace Material.Components.Maui;

internal class TextFieldDrawable(TextField view) : IDrawable
{
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
            Left = rect.Left + (float)view.EditablePadding.Left,
            Top = rect.Top + (float)view.EditablePadding.Top,
            Width = rect.Width - (float)view.EditablePadding.HorizontalThickness,
            Height = rect.Height - (float)view.EditablePadding.VerticalThickness
        };

        canvas.DrawBackground(view, containerRect);

        this.DrawIcon(canvas, containerRect);
        this.DrawTrailingIcon(canvas, containerRect);
        this.DrawSelection(canvas, editableRect);
        this.DrawText(canvas, editableRect);
        this.DrawCaret(canvas, editableRect);
        this.DrawActiveIndicator(canvas, containerRect);
        this.DrawSupportingText(canvas, rect);
        this.DrawLabelText(canvas, containerRect);
        this.DrawRipple(canvas, containerRect);
        canvas.DrawOutline(view, containerRect);
    }

    private void DrawIcon(ICanvas canvas, RectF rect)
    {
        if (string.IsNullOrEmpty(view.IconData))
            return;

        canvas.FillColor = view.IconColor.WithAlpha(
            view.ViewState is ViewState.Disabled ? 0.38f : 1f
        );

        using var path = ((IIconElement)view).IconPath.AsScaledPath(1f);
        var sx = rect.Left + 12f;
        var sy = rect.Center.Y - 12f;
        path.Move(sx, sy);
        canvas.FillPath(path);
    }

    private void DrawTrailingIcon(ICanvas canvas, RectF rect)
    {
        if (string.IsNullOrEmpty(view.TrailingIconData))
            return;

        canvas.FillColor = view.TrailingIconColor.WithAlpha(
            view.ViewState is ViewState.Disabled ? 0.38f : 1f
        );

        using var path = ((ITrailingIconElement)view).TrailingIconPath.AsScaledPath(1f);
        var sx = rect.Right - 12f - 24f;
        var sy = rect.Center.Y - 12f;
        path.Move(sx, sy);
        canvas.FillPath(path);
    }

    private void DrawText(ICanvas canvas, RectF rect)
    {
        if (string.IsNullOrEmpty(view.Text))
            return;

        var weight = (int)view.FontWeight;
        var style = view.FontIsItalic ? FontStyleType.Italic : FontStyleType.Normal;

        var font = new Microsoft.Maui.Graphics.Font(view.FontFamily, weight, style);

        canvas.Font = font;
        canvas.FontColor = view.FontColor.WithAlpha(
            view.ViewState is ViewState.Disabled ? 0.38f : 1f
        );
        canvas.FontSize = view.FontSize;
        var horizontal =
            view.TextAlignment == TextAlignment.End
                ? HorizontalAlignment.Right
                : view.TextAlignment == TextAlignment.Center
                    ? HorizontalAlignment.Center
                    : HorizontalAlignment.Left;

        var text =
            view.InputType is InputType.Password
                ? new string('•', view.Text.Length)
                : view.Text;
        canvas.DrawString(text, rect, horizontal, VerticalAlignment.Center);
    }

    public void DrawSelection(ICanvas canvas, RectF rect)
    {
        if (!view.SelectionRange.IsRange)
            return;

        canvas.FillColor = view.CaretColor;
        var (startRect, endRect) = view.GetSelectionRect(
            rect.Width + (float)view.EditablePadding.HorizontalThickness
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
        if (view.SelectionRange.IsRange || !view.IsDrawCaret)
            return;

        canvas.FillColor = view.CaretColor;
        canvas.FillRectangle(
            rect.Left + view.CaretInfo.X,
            rect.Top + view.CaretInfo.Y,
            2,
            view.CaretInfo.Height
        );
    }

    private void DrawSupportingText(ICanvas canvas, RectF rect)
    {
        if (string.IsNullOrEmpty(view.SupportingText))
            return;

        var stRect = new Rect
        {
            Left = rect.Left + 16f,
            Top = rect.Bottom - 16f,
            Width = rect.Width - 16f,
            Height = 16f
        };

        var weight = (int)view.FontWeight;
        var style = view.FontIsItalic ? FontStyleType.Italic : FontStyleType.Normal;

        var font = new Microsoft.Maui.Graphics.Font(view.FontFamily, weight, style);

        canvas.Font = font;
        canvas.FontColor = view.SupportingFontColor.WithAlpha(
            view.ViewState is ViewState.Disabled ? 0.38f : 1f
        );
        canvas.FontSize = 12f;
        canvas.DrawString(
            view.SupportingText,
            stRect,
            HorizontalAlignment.Left,
            VerticalAlignment.Bottom
        );
    }

    private void DrawRipple(ICanvas canvas, RectF rect)
    {
        if (string.IsNullOrEmpty(view.TrailingIconData))
            return;

        canvas.SaveState();

        var drawRect = new PathF();
        drawRect.AppendCircle(rect.Right - 24f, rect.Center.Y, 20f);
        canvas.ClipPath(drawRect);

        if (view.RipplePercent is not 0f and not 1f)
            canvas.DrawRipple(
                view,
                view.LastTouchPoint,
                view.RippleSize,
                view.RipplePercent
            );

        canvas.RestoreState();
    }

    private void DrawLabelText(ICanvas canvas, RectF rect)
    {
        var percent =
            !string.IsNullOrEmpty(view.Text) || view.IsFocused && !view.IsReadOnly
                ? 1 - view.LabelAnimationPercent
                : view.LabelAnimationPercent;

        var fontSize = 12f + (view.FontSize - 12f) * percent;

        canvas.FontColor = view.LabelFontColor.MultiplyAlpha(
            view.ViewState is ViewState.Disabled ? 0.38f : 1f
        );

        var labelSize = view.GetStringSize(view.LabelText, fontSize);
        var minLabelSize = view.GetStringSize(view.LabelText, 12f);
        var maxLabelSize = view.GetStringSize(view.LabelText);

        var rectLeft =
            rect.Left + (!string.IsNullOrEmpty(view.IconData) ? 12f + 24f + 16f : 16f);

        if (view.OutlineWidth == 0)
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
                view,
                view.LabelText,
                view.LabelFontColor,
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
                view,
                view.LabelText,
                view.LabelFontColor,
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
        if (view.ActiveIndicatorHeight == 0)
            return;

        canvas.FillColor = view.ActiveIndicatorColor.MultiplyAlpha(
            view.ViewState is ViewState.Disabled ? 0.38f : 1f
        );

        canvas.FillRectangle(
            rect.Left,
            rect.Bottom - view.ActiveIndicatorHeight,
            rect.Width,
            view.ActiveIndicatorHeight
        );
    }
}
