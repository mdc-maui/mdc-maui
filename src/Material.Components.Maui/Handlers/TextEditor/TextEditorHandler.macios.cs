using Microsoft.Maui.Handlers;

namespace Material.Components.Maui.Handlers;
public partial class TextEditorHandler : ViewHandler<BaseTextEditor, PlatformTextEditor>
{
    protected override PlatformTextEditor CreatePlatformView() => new();

    public static void MapDrawable(TextEditorHandler handler, BaseTextEditor virtualView)
    {
        handler.PlatformView.Drawable = virtualView.Drawable;
    }

    public static void MapInvalidate(TextEditorHandler handler, BaseTextEditor virtualView, object arg)
    {
        handler.PlatformView?.InvalidateDrawable();
    }

    private static void MapText(TextEditorHandler handler, BaseTextEditor virtualView)
    {
        throw new NotImplementedException();
    }

    private static void MapSelectionRange(TextEditorHandler handler, BaseTextEditor virtualView)
    {
        throw new NotImplementedException();
    }
    private static void MapFontSize(TextEditorHandler handler, BaseTextEditor virtualView)
    {
        throw new NotImplementedException();
    }
}
