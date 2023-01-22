using Microsoft.Maui.Handlers;

namespace Material.Components.Maui.Core;
public partial class TextFieldHandler : ViewHandler<TextField, IOSTextField>
{
    protected override IOSTextField CreatePlatformView()
    {
        throw new NotImplementedException();
    }

    private static void MapInternalFocus(TextFieldHandler handler, TextField view)
    {
        throw new NotImplementedException();
    }

    private static void MapIgnorePixelScaling(TextFieldHandler arg1, TextField arg2)
    {
        throw new NotImplementedException();
    }

    private static void MapEnableTouchEvents(TextFieldHandler arg1, TextField arg2)
    {
        throw new NotImplementedException();
    }

    public static void OnInvalidateSurface(
       TextFieldHandler handler,
       ISKCanvasView canvasView,
       object args
   )
    {
        throw new NotImplementedException();
    }
}
