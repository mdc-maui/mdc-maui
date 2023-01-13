using Microsoft.Maui.Handlers;
using MViewPager = Material.Components.Maui.ViewPager;

namespace Material.Components.Maui.Core;

public partial class ViewPagerHandler
{
    public static PropertyMapper<MViewPager, ViewPagerHandler> Mapper =
        new(ViewHandler.ViewMapper)
        {
            [nameof(MViewPager.SelectedIndex)] = MapSelectedIndex,
            [nameof(MViewPager.HasAnimation)] = MapHasAnimation,
#if ANDROID
            [nameof(MViewPager.UserInputEnabled)] = MapUserInputEnabled,
#endif
        };

    public ViewPagerHandler(PropertyMapper mapper) : base(mapper) { }

    public ViewPagerHandler() : base(Mapper) { }


}
