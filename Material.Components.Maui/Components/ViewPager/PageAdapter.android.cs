using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.ViewPager2.Adapter;
using AView = Android.Views.View;

namespace Material.Components.Maui.Components.Core;

internal class PageAdapter : FragmentStateAdapter
{
    private readonly List<AView> items;
    public override int ItemCount => this.items.Count;

    public PageAdapter(FragmentActivity activity, List<AView> items) : base(activity)
    {
        this.items = items;
    }

    public override AndroidX.Fragment.App.Fragment CreateFragment(int position)
    {
        return new ViewPagerFragment(this.items[position]);
    }
}

internal class ViewPagerFragment : AndroidX.Fragment.App.Fragment
{
    private readonly AView view;

    public ViewPagerFragment(AView view) : base()
    {
        this.view = view;
    }

    public override AView OnCreateView(
        LayoutInflater inflater,
        ViewGroup container,
        Bundle savedInstanceState
    )
    {
        return this.view;
    }
}
