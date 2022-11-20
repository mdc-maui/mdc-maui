using Microsoft.Maui.Handlers;
using MViewPager = Material.Components.Maui.ViewPager;

namespace Material.Components.Maui.Core;
public partial class ViewPagerHandler
{
    public static PropertyMapper<MViewPager, ViewPagerHandler> Mapper = new(ViewHandler.ViewMapper)
    {
        [nameof(MViewPager.SelectedIndex)] = MapSelectedIndex,
        [nameof(MViewPager.UserInputEnabled)] = MapUserInputEnabled,
    };

    public ViewPagerHandler(PropertyMapper mapper) : base(mapper)
    {

    }

    public ViewPagerHandler() : base(Mapper)
    {

    }

#if !WINDOWS && !ANDROID
    private static void MapSelectedIndex(ViewPagerHandler handler, MViewPager view)
    {

    }

    private static void MapUserInputEnabled(ViewPagerHandler handler, MViewPager view)
    {

    }

    internal static void AddItem(ViewPagerHandler handler, int index, Page item)
    {
       
    }

    internal static void RemoveItem(ViewPagerHandler handler, int index)
    {
       
    }

    internal static void ClearItems(ViewPagerHandler handler)
    {
       
    }
#endif
}
