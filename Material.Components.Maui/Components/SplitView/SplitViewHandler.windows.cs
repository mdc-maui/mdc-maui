using Material.Components.Maui.Core;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WSplitView = Microsoft.UI.Xaml.Controls.SplitView;

namespace Material.Components.Maui.Core;

public partial class SplitViewHandler : ViewHandler<SplitView, WSplitView>
{
    protected override WSplitView CreatePlatformView()
    {
        var reuslt = new WSplitView
        {
            OpenPaneLength = 240,
            CompactPaneLength = 80,
            IsPaneOpen = this.VirtualView.IsPaneOpen,
            DisplayMode = this.VirtualView.DisplayMode switch
            {
                DrawerDisplayMode.Popup => Microsoft.UI.Xaml.Controls.SplitViewDisplayMode.Overlay,
                _ => Microsoft.UI.Xaml.Controls.SplitViewDisplayMode.CompactInline,
            },
            LightDismissOverlayMode = Microsoft.UI.Xaml.Controls.LightDismissOverlayMode.On,
            PaneBackground = new SolidColorBrush(Colors.Transparent).ToBrush()
        };
        reuslt.PaneOpened += (_, _) => this.VirtualView.IsPaneOpen = true;
        reuslt.PaneClosed += (_, _) => this.VirtualView.IsPaneOpen = false;
        return reuslt;
    }

    private static void MapPane(SplitViewHandler handler, SplitView view)
    {
        handler.PlatformView.Pane = view.Pane.ToPlatform(handler.MauiContext);
    }

    private static void MapContent(SplitViewHandler handler, SplitView view)
    {
        handler.PlatformView.Content = view.Content.ToPlatform(handler.MauiContext);
    }

    private static void MapDisplayMode(SplitViewHandler handler, SplitView view)
    {
        handler.PlatformView.DisplayMode = view.DisplayMode switch
        {
            DrawerDisplayMode.Popup => Microsoft.UI.Xaml.Controls.SplitViewDisplayMode.Overlay,
            _ => Microsoft.UI.Xaml.Controls.SplitViewDisplayMode.CompactInline,
        };
    }

    private static void MapIsPaneOpen(SplitViewHandler handler, SplitView view)
    {
        handler.PlatformView.IsPaneOpen = view.IsPaneOpen;
    }
}
