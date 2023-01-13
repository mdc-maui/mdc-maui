using Microsoft.Maui.Handlers;
using UIKit;
using MViewPager = Material.Components.Maui.ViewPager;

namespace Material.Components.Maui.Core;

public partial class ViewPagerHandler : ViewHandler<MViewPager, UIKit.UIView>
{
    protected override UIView CreatePlatformView()
    {
        throw new NotImplementedException();
    }

    private static void MapSelectedIndex(ViewPagerHandler handler, MViewPager view)
    {
        throw new NotImplementedException();
    }

    private static void MapUserInputEnabled(ViewPagerHandler handler, MViewPager view)
    {
        throw new NotImplementedException();
    }

    private static void MapHasAnimation(ViewPagerHandler handler, MViewPager view)
    {
        throw new NotImplementedException();
    }

    internal static void AddItem(ViewPagerHandler handler, int index, View item)
    {
        throw new NotImplementedException();
    }

    internal static void RemoveItem(ViewPagerHandler handler, int index)
    {
        throw new NotImplementedException();
    }

    internal static void ClearItems(ViewPagerHandler handler) { }
}
