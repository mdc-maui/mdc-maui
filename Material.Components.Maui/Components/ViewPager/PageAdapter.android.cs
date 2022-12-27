using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.ViewPager2.Adapter;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;
using MView = Microsoft.Maui.Controls.View;

namespace Material.Components.Maui.Components.Core;

internal class PageAdapter : FragmentStateAdapter
{
    private readonly List<MView> items = new();
    private readonly IMauiContext context;
    public override int ItemCount => this.items.Count;

    public PageAdapter(FragmentManager fragmentManager, IMauiContext context)
        : base(fragmentManager, (context.Context.GetActivity() as AppCompatActivity).Lifecycle)
    {
        this.context = context;
    }

    public void AddItem(int index, MView item) => this.items.Insert(index, item);

    public void RemoveItem(int index) => this.items.RemoveAt(index);

    public void ClearItems() => this.items.Clear();

    public override Fragment CreateFragment(int position) =>
        new ViewPagerFragment(this.items[position], this.context);
}

internal class ViewPagerFragment : Fragment
{
    private readonly MView view;
    private readonly IMauiContext context;

    public ViewPagerFragment(MView view, IMauiContext context) : base()
    {
        this.view = view;
        this.context = context;
    }

    public override AView OnCreateView(
        LayoutInflater inflater,
        ViewGroup container,
        Bundle savedInstanceState
    )
    {
        var result = this.view.ToPlatform(this.context);
        result.LayoutParameters = new ViewGroup.LayoutParams(
            ViewGroup.LayoutParams.MatchParent,
            ViewGroup.LayoutParams.MatchParent
        );
        return result;
    }
}