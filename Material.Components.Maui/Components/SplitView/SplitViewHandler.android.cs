using Android.Views;
using Android.Widget;
using AndroidX.DrawerLayout.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;

namespace Material.Components.Maui.Core;

public partial class SplitViewHandler : ViewHandler<SplitView, DrawerLayout>
{
    private LinearLayout container;
    private AView pane;

    private int openWidth;
    private int closeWidth;

    protected override ASplitView CreatePlatformView()
    {
        this.openWidth = (int)this.Context.ToPixels(240);
        this.closeWidth = (int)this.Context.ToPixels(80);
        var result = new ASplitView(this.Context);
        result.DrawerOpened += (_, _) => this.VirtualView.IsPaneOpen = true;
        result.DrawerClosed += (_, _) => this.VirtualView.IsPaneOpen = false;
        this.container = new LinearLayout(this.Context);
        result.AddView(this.container);
        return result;
    }

    private static void MapPane(SplitViewHandler handler, SplitView view)
    {
        handler.pane = view.Pane.ToPlatform(handler.MauiContext);
        var width =
            view.DisplayMode == DrawerDisplayMode.Popup
                ? handler.openWidth
                : view.IsPaneOpen
                    ? handler.openWidth
                    : handler.closeWidth;
        var lp = new DrawerLayout.LayoutParams(
            width,
            ViewGroup.LayoutParams.WrapContent,
            (int)GravityFlags.Start
        );
        handler.pane.LayoutParameters = lp;
        if (view.DisplayMode == DrawerDisplayMode.Popup)
        {
            handler.PlatformView.AddView(handler.pane);
        }
        else
        {
            handler.container.AddView(handler.pane, 0);
        }
    }

    private static void MapContent(SplitViewHandler handler, SplitView view)
    {
        handler.container.AddView(
            view.Content.ToPlatform(handler.MauiContext),
            ViewGroup.LayoutParams.MatchParent,
            ViewGroup.LayoutParams.MatchParent
        );
    }

    private static void MapIsPaneOpen(SplitViewHandler handler, SplitView view)
    {
        if (handler.pane != null)
        {
            if (view.DisplayMode == DrawerDisplayMode.Popup)
            {
                if (view.IsPaneOpen)
                {
                    handler.PlatformView.OpenDrawer(handler.pane);
                }
                else
                {
                    handler.PlatformView.CloseDrawer(handler.pane);
                }
            }
            else
            {
                var lp = handler.pane.LayoutParameters;
                if (view.IsPaneOpen)
                {
                    lp.Width = handler.openWidth;
                }
                else
                {
                    lp.Width = handler.closeWidth;
                }

                handler.pane.LayoutParameters = lp;
            }
        }
    }
}
