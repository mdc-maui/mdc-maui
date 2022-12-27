using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using MViewPager = Material.Components.Maui.ViewPager;

namespace Material.Components.Maui.Core;

public partial class ViewPagerHandler : ViewHandler<MViewPager, WViewPager>
{
    protected override WViewPager CreatePlatformView()
    {
        var platformView = new WViewPager();
        platformView.SelectionChanged += (sender, e) =>
        {
            this.VirtualView.SelectedIndex = platformView.SelectedIndex;
        };
        platformView.UseTouchAnimationsForAllNavigation = true;
        return platformView;
    }

    private static void MapSelectedIndex(ViewPagerHandler handler, MViewPager view)
    {
        handler.PlatformView.SelectedIndex = view.SelectedIndex;
    }

    private static void MapUserInputEnabled(ViewPagerHandler handler, MViewPager view) { }

    private static void MapHasAnimation(ViewPagerHandler handler, MViewPager view)
    {
        handler.PlatformView.UseTouchAnimationsForAllNavigation = view.HasAnimation;
    }

    internal static void AddItem(ViewPagerHandler handler, int index, View item)
    {
        handler.PlatformView.Items.Insert(index, item.ToPlatform(handler.MauiContext));
    }

    internal static void RemoveItem(ViewPagerHandler handler, int index)
    {
        handler.PlatformView.Items.RemoveAt(index);
    }

    internal static void ClearItems(ViewPagerHandler handler)
    {
        handler.PlatformView.Items.Clear();
    }
}
