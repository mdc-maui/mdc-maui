using Android.Views;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using SkiaSharp.Views.Maui.Handlers;
using SkiaSharp.Views.Maui.Platform;
using Topten.RichTextKit;

namespace Material.Components.Maui.Core;

public partial class TextFieldHandler : ViewHandler<TextField, ATextField>
{
    protected override ATextField CreatePlatformView()
    {
        var pv = new ATextField(this.Context, this.editTextManager);
        pv.FocusChange += this.OnFocusChange;
        return pv;
    }

    private void OnFocusChange(object sender, Android.Views.View.FocusChangeEventArgs e)
    {
        this.editTextManager.Focus = e.HasFocus;
    }

    private void OnSendKeyEvent(object sender, Keycode e)
    {
        var range = this.editTextManager.Range;
        var start = this.editTextManager.Range.Start;
        var end = this.editTextManager.Range.End;
        var textLength = this.editTextManager.Document.Length;

        switch (e)
        {
            case Keycode.Del:
                this.editTextManager.DeleteRangeText();
                break;
            case Keycode.Enter:
                this.editTextManager.CommitText("\n");
                break;
            case Keycode.DpadLeft:
                if (range.IsRange)
                    this.editTextManager.Range = new TextRange(Math.Max(start - 1, 0), end);
                else
                    this.editTextManager.Range = new TextRange(Math.Max(start - 1, 0));
                break;
            case Keycode.DpadRight:
                if (range.IsRange)
                    this.editTextManager.Range = new TextRange(
                        start,
                        Math.Min(end + 1, textLength - 1)
                    );
                else
                    this.editTextManager.Range = new TextRange(Math.Min(end + 1, textLength - 1));
                break;
            case Keycode.DpadUp:
                this.editTextManager.Navigate(Topten.RichTextKit.Editor.NavigationKind.LineUp);
                break;
            case Keycode.DpadDown:
                this.editTextManager.Navigate(Topten.RichTextKit.Editor.NavigationKind.LineDown);
                break;
        }
    }

    private void OnDeleteRangeText(object sender, TextRange e)
    {
        this.editTextManager.Range = e;
        this.editTextManager.DeleteRangeText();
    }

    private void OnCommitText(object sender, string e)
    {
        this.editTextManager.CommitText(e);
    }

    private static void MapInternalFocus(TextFieldHandler handler, TextField view) { }

    protected override void ConnectHandler(ATextField platformView)
    {
        platformView.PaintSurface += this.OnPaintSurface;

        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(ATextField platformView)
    {
        this.touchHandler?.Detach(platformView);
        this.touchHandler = null;

        platformView.PaintSurface -= this.OnPaintSurface;

        base.DisconnectHandler(platformView);
    }

    private SKSizeI lastCanvasSize;
    private SKTouchHandler touchHandler;

    // Mapper actions / properties

    public static void OnInvalidateSurface(
        TextFieldHandler handler,
        ISKCanvasView canvasView,
        object args
    )
    {
        handler.PlatformView?.Invalidate();
    }

    public static void MapIgnorePixelScaling(TextFieldHandler handler, ISKCanvasView canvasView)
    {
        handler.PlatformView?.UpdateIgnorePixelScaling(canvasView);
    }

    public static void MapEnableTouchEvents(TextFieldHandler handler, ISKCanvasView canvasView)
    {
        if (handler.PlatformView == null)
            return;

        handler.touchHandler ??= new SKTouchHandler(
            canvasView.OnTouch,
            handler.OnGetScaledCoord
        );

        handler.touchHandler?.SetEnabled(handler.PlatformView, canvasView.EnableTouchEvents);
    }

    // helper methods

    private void OnPaintSurface(object sender, SkiaSharp.Views.Android.SKPaintSurfaceEventArgs e)
    {
        var newCanvasSize = e.Info.Size;
        if (this.lastCanvasSize != newCanvasSize)
        {
            this.lastCanvasSize = newCanvasSize;
            (this.VirtualView as ISKCanvasView)?.OnCanvasSizeChanged(newCanvasSize);
        }

        (this.VirtualView as ISKCanvasView)?.OnPaintSurface(
            new SKPaintSurfaceEventArgs(e.Surface, e.Info, e.RawInfo)
        );
    }

    private SKPoint OnGetScaledCoord(double x, double y)
    {
        if (this.VirtualView?.IgnorePixelScaling == true && this.Context != null)
        {
            x = this.Context.FromPixels(x);
            y = this.Context.FromPixels(y);
        }

        return new SKPoint((float)x, (float)y);
    }
}
