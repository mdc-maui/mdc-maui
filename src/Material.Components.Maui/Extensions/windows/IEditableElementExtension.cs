using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI.ViewManagement;
using WinRT.Interop;

namespace Material.Components.Maui.Extensions.Platform;

internal static class IEditableElementExtension
{
    public static SizeF GetLayoutSize<TElement>(this TElement element, float maxWidth)
        where TElement : IEditableElement, IFontElement
    {
        maxWidth -= (float)element.EditablePadding.HorizontalThickness;

        var layout = element.CreateCanvasTextLayout(maxWidth);
        var size = new SizeF((float)layout.LayoutBounds.Width, (float)layout.LayoutBounds.Height);

        return new SizeF(MathF.Ceiling(size.Width), MathF.Ceiling(size.Height));
    }

    public static CaretInfo GetCaretInfo<TElement>(
        this TElement element,
        float maxWidth,
        PointF point
    ) where TElement : IEditableElement, IFontElement
    {
        maxWidth -= (float)element.EditablePadding.HorizontalThickness;
        point.X -= (float)element.EditablePadding.Left;
        point.Y -= (float)element.EditablePadding.Top;

        var layout = element.CreateCanvasTextLayout(maxWidth);
        layout.HitTest(point.X, point.Y, out var region);

        return point.X > region.LayoutBounds.Left + region.LayoutBounds.Width / 2
            ? new CaretInfo
            {
                Position = Math.Min(region.CharacterIndex + 1, element.Text?.Length ?? 0),
                X = (float)region.LayoutBounds.Right,
                Y = (float)region.LayoutBounds.Top,
                Width = (float)region.LayoutBounds.Width,
                Height = (float)region.LayoutBounds.Height
            }
            : new CaretInfo
            {
                Position = region.CharacterIndex,
                X = (float)region.LayoutBounds.Left,
                Y = (float)region.LayoutBounds.Top,
                Width = (float)region.LayoutBounds.Width,
                Height = (float)region.LayoutBounds.Height
            };
    }

    public static CaretInfo GetCaretInfo<TElement>(
        this TElement element,
        float maxWidth,
        int location
    ) where TElement : IEditableElement, IFontElement
    {
        maxWidth -= (float)element.EditablePadding.HorizontalThickness;

        var layout = element.CreateCanvasTextLayout(maxWidth);
        layout.GetCaretPosition(location, false, out var region);

        return new CaretInfo
        {
            Position = region.CharacterIndex,
            X = (float)region.LayoutBounds.Left,
            Y = (float)region.LayoutBounds.Top,
            Width = (float)region.LayoutBounds.Width,
            Height = (float)region.LayoutBounds.Height
        };
    }

    public static (RectF, RectF) GetSelectionRect<TElement>(this TElement element, float maxWidth)
        where TElement : IEditableElement, IFontElement
    {
        maxWidth -= (float)element.EditablePadding.HorizontalThickness;

        var layout = element.CreateCanvasTextLayout(maxWidth);
        var range = element.SelectionRange.Normalized();

        layout.GetCaretPosition(range.Start, false, out var startRegion);

        var startRect = new RectF
        {
            X = (float)startRegion.LayoutBounds.Left,
            Y = (float)startRegion.LayoutBounds.Top,
            Height = (float)startRegion.LayoutBounds.Height
        };

        layout.GetCaretPosition(range.End, false, out var endRegion);

        var endRect = new RectF
        {
            X = (float)endRegion.LayoutBounds.Left,
            Y = (float)endRegion.LayoutBounds.Top,
            Height = (float)endRegion.LayoutBounds.Height
        };

        return (startRect, endRect);
    }

    public static CaretInfo NavigateUp<TElement>(this TElement element, float maxWidth)
        where TElement : IEditableElement, IFontElement
    {
        maxWidth -= (float)element.EditablePadding.HorizontalThickness;

        var layout = element.CreateCanvasTextLayout(maxWidth);
        var range = element.SelectionRange.Normalized();

        layout.GetCaretPosition(range.Start, false, out var region);
        var y = region.LayoutBounds.Top - region.LayoutBounds.Height / 2;
        if (y > 0)
        {
            layout.HitTest(
                (float)(region.LayoutBounds.Left + region.LayoutBounds.Width / 2),
                (float)y,
                out var navRegion
            );

            return new CaretInfo { Position = navRegion.CharacterIndex, };
        }
        return new CaretInfo { Position = region.CharacterIndex, };
    }

    public static CaretInfo NavigateDown<TElement>(this TElement element, float maxWidth)
        where TElement : IEditableElement, IFontElement
    {
        maxWidth -= (float)element.EditablePadding.HorizontalThickness;

        var layout = element.CreateCanvasTextLayout(maxWidth);
        var range = element.SelectionRange.Normalized();

        layout.GetCaretPosition(range.End, false, out var region);
        var y = region.LayoutBounds.Bottom + region.LayoutBounds.Height / 2;
        if (y < layout.LayoutBounds.Height)
        {
            layout.HitTest(
                (float)(region.LayoutBounds.Left + region.LayoutBounds.Width / 2),
                (float)y,
                out var navRegion
            );

            return new CaretInfo
            {
                Position = navRegion.CharacterIndex,
                X = (float)navRegion.LayoutBounds.Left,
                Y = (float)navRegion.LayoutBounds.Top,
                Width = (float)navRegion.LayoutBounds.Width,
                Height = (float)navRegion.LayoutBounds.Height
            };
        }

        return new CaretInfo
        {
            Position = region.CharacterIndex,
            X = (float)region.LayoutBounds.Left,
            Y = (float)region.LayoutBounds.Top,
            Width = (float)region.LayoutBounds.Width,
            Height = (float)region.LayoutBounds.Height
        };
    }

    public static CanvasTextLayout CreateCanvasTextLayout<TElement>(
        this TElement element,
        float maxWidth
    ) where TElement : IEditableElement, IFontElement
    {
        var text = element.InputType is InputType.Password ? new string('•', element.Text.Length) : element.Text;
        var fontSize = element.FontSize;

        var weight = (int)element.FontWeight;
        var style = element.FontIsItalic ? FontStyleType.Italic : FontStyleType.Normal;

        var font = new Microsoft.Maui.Graphics.Font(element.FontFamily, weight, style);

        var format = new CanvasTextFormat
        {
            FontFamily = font.Name,
            FontSize = fontSize,
            FontWeight = new Windows.UI.Text.FontWeight { Weight = (ushort)font.Weight },
            FontStyle =
                font.StyleType is FontStyleType.Italic
                    ? Windows.UI.Text.FontStyle.Italic
                    : Windows.UI.Text.FontStyle.Normal,
            WordWrapping = CanvasWordWrapping.Wrap
        };

        var device = CanvasDevice.GetSharedDevice();
        var textLayout = new CanvasTextLayout(device, text, format, maxWidth, 0f)
        {
            HorizontalAlignment = element.TextAlignment == TextAlignment.End ? CanvasHorizontalAlignment.Right : element.TextAlignment == TextAlignment.Center ? CanvasHorizontalAlignment.Center : CanvasHorizontalAlignment.Left,
            VerticalAlignment = CanvasVerticalAlignment.Top
        };

        return textLayout;
    }

    static IntPtr GetWindowHwnd(this Element element)
    {
        if (element is View view)
        {
            return view.Window != null
                ? view.Window.Handler?.PlatformView != null
                    ? WindowNative.GetWindowHandle(view.Window.Handler.PlatformView)
                    : IntPtr.Zero
                : view.Parent is View parent
                    ? parent.GetWindowHwnd()
                    : IntPtr.Zero;
        }
        else if (element is Page page)
        {
            return page.Window != null
                ? page.Window.Handler?.PlatformView != null
                    ? WindowNative.GetWindowHandle(page.Window.Handler.PlatformView)
                    : IntPtr.Zero
                : IntPtr.Zero;
        }

        return IntPtr.Zero;
    }

    public static bool ShowKeyboard(this TextField editor)
    {
        if (editor.Handler?.PlatformView is PlatformTextField pv)
        {
            var hwnd = editor.GetWindowHwnd();
            if (hwnd != IntPtr.Zero)
            {
                pv.EditContext.NotifyFocusEnter();
                var inputPane = InputPaneInterop.GetForWindow(hwnd);
                return inputPane.TryShow();
            }
        }
        return false;
    }

    public static bool CheckKeyboard(this TextField editor)
    {
        if (editor.Handler?.PlatformView is PlatformTextField)
        {
            var hwnd = editor.GetWindowHwnd();
            if (hwnd != IntPtr.Zero)
            {
                var inputPane = InputPaneInterop.GetForWindow(hwnd);
                return inputPane.Visible;
            }
        }
        return false;
    }

    public static bool HideKeyboard(this TextField editor)
    {
        if (editor.Handler?.PlatformView is PlatformTextField pv)
        {
            var hwnd = editor.GetWindowHwnd();
            if (hwnd != IntPtr.Zero)
            {
                pv.EditContext.NotifyFocusLeave();
                var inputPane = InputPaneInterop.GetForWindow(hwnd);
                return inputPane.TryHide();
            }
        }
        return false;
    }
}
