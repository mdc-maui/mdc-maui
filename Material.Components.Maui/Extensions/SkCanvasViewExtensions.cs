using Microsoft.Maui.Platform;

namespace Material.Components.Maui.Extensions;

public static class SkCanvasViewExtensions
{

    public static void AllocateSize(this SKCanvasView view, double width, double height)
    {
        if (view.Handler != null)
        {
            width -= view.Margin.HorizontalThickness;
            height -= view.Margin.VerticalThickness;

#if ANDROID
            var platformView = (Android.Views.View)view.Handler.PlatformView;
            var lp = platformView.LayoutParameters;
            lp.Width = (int)platformView.Context.ToPixels(width);
            lp.Height = (int)platformView.Context.ToPixels(height);
            platformView.LayoutParameters = lp;
#elif WINDOWS     
            ((Microsoft.UI.Xaml.FrameworkElement)view.Handler.PlatformView).Width = width;
            ((Microsoft.UI.Xaml.FrameworkElement)view.Handler.PlatformView).Height = height;
#endif
        }
    }
    public static void AllocateSize(this SKCanvasView view, Size size)
    {
        view.AllocateSize(size.Width, size.Height);
    }
}