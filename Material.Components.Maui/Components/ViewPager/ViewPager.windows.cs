using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.System;

namespace Material.Components.Maui.Core;
public class WViewPager : FlipView
{
    public WViewPager()
    {
        base.Loaded += this.WViewPager_Loaded;
    }

    private void WViewPager_Loaded(object sender, RoutedEventArgs e)
    {
        var grid = (Microsoft.UI.Xaml.Controls.Grid)VisualTreeHelper.GetChild(this, 0);
        for (int i = grid.Children.Count - 1; i >= 0; i--)
        {
            if (grid.Children[i] is Microsoft.UI.Xaml.Controls.Button)
            {
                grid.Children.RemoveAt(i);
            }
        }
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
        return new WViewPagerItem();
    }
}

public class WViewPagerItem : FlipViewItem
{
    protected override void OnKeyDown(KeyRoutedEventArgs e)
    {
        if (e.Key is VirtualKey.Left || e.Key is VirtualKey.Right || e.Key is VirtualKey.Up || e.Key is VirtualKey.Down)
        {
            e.Handled = true;
        }
        base.OnKeyDown(e);
    }
}
