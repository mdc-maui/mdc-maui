using Material.Components.Maui.Platform.Editable;
using Microsoft.Maui.Handlers;

namespace Material.Components.Maui.Handlers;

public partial class TextFieldHandler : ViewHandler<TextField, PlatformTextField>
{
    EditableHandler editableHandler;

    protected override PlatformTextField CreatePlatformView()
    {
        this.editableHandler = new EditableHandler(this.VirtualView);
        return new PlatformTextField(this.editableHandler);
    }

    protected override void ConnectHandler(PlatformTextField platformView)
    {
        platformView.Connect(this.VirtualView);
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(PlatformTextField platformView)
    {
        platformView.Disconnect();
        base.DisconnectHandler(platformView);
    }

    public static void MapDrawable(TextFieldHandler handler, TextField virtualView)
    {
        handler.PlatformView?.UpdateDrawable(virtualView);
    }

    public static void MapInvalidate(TextFieldHandler handler, TextField virtualView, object arg)
    {
        handler.PlatformView?.Invalidate();
    }

    static partial void MapText(TextFieldHandler handler, TextField virtualView)
    {
        handler.editableHandler.UpdateText(virtualView.Text);
    }

    static partial void MapFontAttributes(TextFieldHandler handler, TextField virtualView) { }

    static partial void MapSelectionRange(TextFieldHandler handler, TextField virtualView)
    {
        if (!virtualView.SelectionRange.Equals(handler.editableHandler.SelectionRange))
            handler.editableHandler.SelectionRange = virtualView.SelectionRange;
    }

    static partial void MapFontSize(TextFieldHandler handler, TextField virtualView) { }

    static partial void MapTextAlignment(TextFieldHandler handler, TextField virtualView) { }

    static partial void MapEditablePadding(TextFieldHandler handler, TextField virtualView) { }

    static partial void MapInputType(TextFieldHandler handler, TextField virtualView)
    {
        handler.PlatformView?.UpdateInputType(virtualView.InputType);
    }

    static partial void MapIsReadOnly(TextFieldHandler handler, TextField virtualView) { }
}
