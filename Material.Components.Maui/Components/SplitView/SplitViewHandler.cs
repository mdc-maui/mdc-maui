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
}
