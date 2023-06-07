using Microsoft.Maui.Animations;
using Shape = Material.Components.Maui.Tokens.Shape;

namespace Material.Components.Maui.Extensions;

internal static class CanvasExtension
{
    internal static Shape GetShape(this IShapeElement element, float width, float height)
    {
        if (
            element.Shape.TopLeft is -1
            && element.Shape.TopRight is -1
            && element.Shape.BottomLeft is -1
            && element.Shape.BottomRight is -1
        )
        {
            return Math.Min(width, height) / 2;
        }
        return element.Shape;
    }

    internal static double[] GetRadii(this IShapeElement view, float width, float height)
    {
        var radius = view.GetShape(width, height);
        return new double[]
        {
            radius.TopLeft,
            radius.TopRight,
            radius.BottomLeft,
            radius.BottomRight,
        };
    }

    internal static PathF GetClipPath(this IShapeElement element, RectF rect)
    {
        var radii = element.GetRadii(rect.Width, rect.Height);
        var path = new PathF();
        path.AppendRoundedRectangle(
            new RectF(rect.X, rect.Y, rect.Width, rect.Height),
            (float)radii[0],
            (float)radii[1],
            (float)radii[2],
            (float)radii[3],
            true
        );
        return path;
    }

    internal static void DrawBackground(this ICanvas canvas, IBackgroundElement element, RectF rect)
    {
        if (element.BackgroundColor != Colors.Transparent)
        {
            canvas.FillColor = element.BackgroundColor.MultiplyAlpha(
                element.ViewState is ViewState.Disabled ? 0.12f : 1f
            );
            canvas.FillRectangle(rect);
        }
    }

    internal static void DrawIcon(
        this ICanvas canvas,
        IIconElement element,
        RectF rect,
        int defaultSize,
        float scale
    )
    {
        if (element.IconPath == null)
            return;

        canvas.FillColor = element.IconColor.WithAlpha(
            element.ViewState is ViewState.Disabled ? 0.38f : 1f
        );

        var path = element.IconPath.AsScaledPath(defaultSize / 24f * scale);
        var sx = rect.Center.X - defaultSize / 2 * scale;
        var sy = rect.Center.Y - defaultSize / 2 * scale;
        path.Move(sx, sy);
        canvas.FillPath(path);
    }

    internal static void DrawOutline(this ICanvas canvas, IOutlineElement element, RectF rect)
    {
        if (element.OutlineWidth == 0)
            return;

        canvas.StrokeColor = element.OutlineColor.WithAlpha(
            element.ViewState is ViewState.Disabled ? 0.12f : 1f
        );
        canvas.StrokeSize = element.OutlineWidth;
        var radii = element.GetRadii(rect.Width, rect.Height);

        var path = new PathF();
        path.AppendRoundedRectangle(
            new RectF(rect.X, rect.Y, rect.Width, rect.Height),
            (float)radii[0],
            (float)radii[1],
            (float)radii[2],
            (float)radii[3],
            true
        );
        canvas.DrawPath(path);
    }

    internal static void DrawOverlayLayer(
        this ICanvas canvas,
        IElevationElement element,
        RectF rect
    )
    {
        if (element.Elevation != 0)
        {
            canvas.FillColor = MaterialColors.SurfaceTint.WithAlpha(element.Elevation.GetOpacity());
            canvas.FillRectangle(rect);
        }
    }

    internal static void DrawStateLayer(
        this ICanvas canvas,
        IStateLayerElement element,
        RectF rect,
        ViewState viewState
    )
    {
        if (viewState is ViewState.Hovered)
        {
            canvas.FillColor = element.StateLayerColor.WithAlpha(StateLayerOpacity.Hovered);
            canvas.FillRectangle(rect);
        }
        else if (viewState is ViewState.Pressed)
        {
            canvas.FillColor = element.StateLayerColor.WithAlpha(StateLayerOpacity.Pressed);
            canvas.FillRectangle(rect);
        }
    }

    internal static void DrawRipple(
        this ICanvas canvas,
        IRippleElement element,
        PointF point,
        float size,
        float percent
    )
    {
        canvas.FillColor = element.StateLayerColor.WithAlpha(StateLayerOpacity.Pressed);
        canvas.FillCircle(point, 0f.Lerp(size, percent));
    }

    internal static void DrawText(this ICanvas canvas, ITextElement element, RectF rect)
    {
        var font = string.IsNullOrEmpty(element.FontFamily)
            ? Microsoft.Maui.Graphics.Font.Default
            : new Microsoft.Maui.Graphics.Font(
                element.FontFamily,
                (int)element.FontWeight,
                (FontStyleType)element.FontSlant
            );
        canvas.Font = font;
        canvas.FontColor = element.TextColor.WithAlpha(
            element.ViewState is ViewState.Disabled ? 0.38f : 1f
        );
        canvas.FontSize = element.FontSize;
        canvas.DrawString(element.Text, rect, HorizontalAlignment.Center, VerticalAlignment.Center);
    }
}
