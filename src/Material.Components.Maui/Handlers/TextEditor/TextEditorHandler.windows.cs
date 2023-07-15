
using Material.Components.Maui.Platform.Editable;
using Microsoft.Maui.Handlers;

namespace Material.Components.Maui.Handlers;
public partial class TextEditorHandler : ViewHandler<BaseTextEditor, PlatformTextEditor>
{
    EditableHandler editableHandler;
    protected override PlatformTextEditor CreatePlatformView()
    {
        this.editableHandler = new EditableHandler(this.VirtualView);
        return new PlatformTextEditor(this.editableHandler);

    }

    public static void MapDrawable(TextEditorHandler handler, BaseTextEditor virtualView)
    {
        handler.PlatformView?.UpdateDrawable(virtualView);
    }

    public static void MapInvalidate(TextEditorHandler handler, BaseTextEditor virtualView, object arg)
    {
        handler.PlatformView?.Invalidate();
    }

    private static void MapText(TextEditorHandler handler, BaseTextEditor virtualView)
    {
        handler.editableHandler.UpdateText(virtualView.Text);
    }

    private static void MapSelectionRange(TextEditorHandler handler, BaseTextEditor virtualView)
    {
        if (!virtualView.SelectionRange.Equals(handler.editableHandler.SelectionRange)
        )
            handler.editableHandler.SelectionRange = virtualView.SelectionRange;
    }

    protected override void ConnectHandler(PlatformTextEditor platformView)
    {
        platformView.Connect(this.VirtualView);
        base.ConnectHandler(platformView);
    }
    protected override void DisconnectHandler(PlatformTextEditor platformView)
    {
        platformView.Disconnect();
        base.DisconnectHandler(platformView);
    }

    private static void MapFontSize(TextEditorHandler handler, BaseTextEditor virtualView)
    {

    }

}
