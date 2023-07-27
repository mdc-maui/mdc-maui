using Microsoft.Maui.Handlers;

namespace Material.Components.Maui.Handlers;

public partial class TextFieldHandler : ViewHandler<TextField, PlatformTextField>
{
    protected override PlatformTextField CreatePlatformView() => new();

    public static void MapDrawable(TextFieldHandler handler, TextField virtualView)
    {
        handler.PlatformView.Drawable = virtualView.Drawable;
    }

    public static void MapInvalidate(TextFieldHandler handler, TextField virtualView, object arg)
    {
        handler.PlatformView?.InvalidateDrawable();
    }

    static partial void MapText(TextFieldHandler handler, TextField virtualView)
    {
        throw new NotImplementedException();
    }

    static partial void MapSelectionRange(TextFieldHandler handler, TextField virtualView)
    {
        throw new NotImplementedException();
    }

    static partial void MapFontSize(TextFieldHandler handler, TextField virtualView)
    {
        throw new NotImplementedException();
    }

    static partial void MapFontAttributes(TextFieldHandler handler, TextField virtualView)
    {
        throw new NotImplementedException();
    }

    static partial void MapTextAlignment(TextFieldHandler handler, TextField virtualView)
    {
        throw new NotImplementedException();
    }

    static partial void MapEditablePadding(TextFieldHandler handler, TextField virtualView)
    {
        throw new NotImplementedException();
    }

    static partial void MapInputType(TextFieldHandler handler, TextField virtualView)
    {
        throw new NotImplementedException();
    }
}
