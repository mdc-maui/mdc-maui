using Material.Components.Maui.Components.Core;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using static Android.Views.ViewGroup;
using AViewPager = AndroidX.ViewPager2.Widget.ViewPager2;
using MViewPager = Material.Components.Maui.ViewPager;

namespace Material.Components.Maui.Core;

public partial class ViewPagerHandler : ViewHandler<MViewPager, AViewPager>
{
    private PageAdapter adapter;
    private bool hasAnimation;

    protected override AViewPager CreatePlatformView()
    {
        this.adapter = new PageAdapter(this.Context.GetFragmentManager(), this.MauiContext);
        var platformView = new AViewPager(this.Context)
        {
            LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent),
            UserInputEnabled = true,
            Orientation = AViewPager.OrientationHorizontal,
            Adapter = adapter,
        };

        platformView.RegisterOnPageChangeCallback(
            new OnPageChangeCallback(positon =>
            {
                this.VirtualView.SelectedIndex = positon;
            })
        );
        return platformView;
    }

    private static void MapSelectedIndex(ViewPagerHandler handler, MViewPager view)
    {
        handler.PlatformView.SetCurrentItem(view.SelectedIndex, handler.hasAnimation);
    }

    private static void MapUserInputEnabled(ViewPagerHandler handler, MViewPager view)
    {
        handler.PlatformView.UserInputEnabled = view.UserInputEnabled;
    }

    private static void MapHasAnimation(ViewPagerHandler handler, MViewPager view)
    {
        handler.hasAnimation = view.HasAnimation;
    }

    internal static void AddItem(ViewPagerHandler handler, int index, View item)
    {
        handler.adapter.AddItem(index, item);
    }

    internal static void RemoveItem(ViewPagerHandler handler, int index)
    {
        handler.adapter.RemoveItem(index);
    }

    internal static void ClearItems(ViewPagerHandler handler)
    {
        handler.adapter.ClearItems();
    }
}

internal class OnPageChangeCallback : AViewPager.OnPageChangeCallback
{
    private readonly Action<int> callback;

    public OnPageChangeCallback(Action<int> callback)
    {
        this.callback = callback;
    }

    public override void OnPageSelected(int position)
    {
        this.callback.Invoke(position);
    }
}
