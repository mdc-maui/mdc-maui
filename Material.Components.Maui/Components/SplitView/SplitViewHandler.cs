using Microsoft.Maui.Handlers;

namespace Material.Components.Maui.Core;

public partial class SplitViewHandler
{
    public static PropertyMapper<SplitView, SplitViewHandler> Mapper =
        new(ViewHandler.ViewMapper)
        {
            [nameof(SplitView.Pane)] = MapPane,
            [nameof(SplitView.Content)] = MapContent,
            [nameof(SplitView.IsPaneOpen)] = MapIsPaneOpen,
        };

    public SplitViewHandler(PropertyMapper mapper) : base(mapper) { }

    public SplitViewHandler() : base(Mapper) { }

#if !ANDROID && !WINDOWS
    private static void MapPane(ViewPagerHandler handler, SplitView view) { }

    private static void MapContent(ViewPagerHandler handler, SplitView view) { }

    private static void MapIsPaneOpen(SplitViewHandler handler, SplitView view) { }
#endif
}
