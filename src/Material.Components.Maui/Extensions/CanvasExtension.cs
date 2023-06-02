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
        canvas.FillColor = element.BackgroundColour.MultiplyAlpha(element.BackgroundOpacity);
        canvas.FillRectangle(rect);
    }

    internal static void DrawIcon(
        this ICanvas canvas,
        IIconElement element,
        RectF rect,
        float scale
    )
    {
        canvas.FillColor = element.IconColor.MultiplyAlpha(element.IconOpacity);
        var path = element.IconPath.AsScaledPath(scale);
        var sx = rect.Center.X - 12 * scale;
        var sy = rect.Center.Y - 12 * scale;
        path.Move(sx, sy);
        canvas.FillPath(path);
    }

    internal static void DrawOutline(this ICanvas canvas, IOutlineElement element, RectF rect)
    {
        if (element.OutlineWidth == 0)
            return;

        canvas.StrokeColor = element.OutlineColor.MultiplyAlpha(element.OutlineOpacity);
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
        if (element.Elevation.Value != 0)
        {
            canvas.FillColor = MaterialColors.SurfaceTint.MultiplyAlpha(
                element.Elevation.Value * 0.05f
            );
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
#if !__MOBILE__
        if (viewState is ViewState.Hovered or ViewState.Pressed)
        {
            canvas.FillColor = element.StateLayerColor.MultiplyAlpha(StateLayerOpacity.Hovered);
            canvas.FillRectangle(rect);
        }

#endif
    }

    internal static void DrawRipple(
        this ICanvas canvas,
        IRippleElement element,
        PointF point,
        float size,
        float percent
    )
    {
        canvas.FillColor = element.StateLayerColor.MultiplyAlpha(StateLayerOpacity.Pressed);
        canvas.FillCircle(point, 0f.Lerp(size, percent));
    }
}
