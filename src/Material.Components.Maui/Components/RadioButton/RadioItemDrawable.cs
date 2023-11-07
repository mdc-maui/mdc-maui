namespace Material.Components.Maui;

internal class RadioItemDrawable(RadioItem view) : IDrawable
{
    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;

        var scale = rect.Height / 40f;
        this.DrawCircle(canvas, rect, scale);

        canvas.DrawText(
            view,
            new RectF(50, 0, rect.Width - 50, rect.Height),
            HorizontalAlignment.Left
        );
        canvas.ResetState();
    }

    void DrawCircle(ICanvas canvas, RectF rect, float scale)
    {
        var color = view.IsSelected ? view.ActivedColor : view.FontColor;

        if (view.IsSelected)
        {
            canvas.FillColor = color;
            canvas.FillCircle(rect.Left + 20, rect.Center.Y, 6 * scale);
        }

        canvas.StrokeColor = color;
        canvas.StrokeSize = 2f * scale;
        canvas.DrawCircle(rect.Left + 20, rect.Center.Y, 10 * scale);

        //canvas.DrawPath(path);
    }
}
