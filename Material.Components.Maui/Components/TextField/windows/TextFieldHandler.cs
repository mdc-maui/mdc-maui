using ImeSharp;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml;
using SkiaSharp.Views.Maui.Handlers;
using SkiaSharp.Views.Maui.Platform;
using Topten.RichTextKit;
using Topten.RichTextKit.Editor;
using Windows.System;
using Windows.UI.Core;

namespace Material.Components.Maui.Core;

public partial class TextFieldHandler : ViewHandler<TextField, WTextField>
{
    protected override WTextField CreatePlatformView()
    {
        var pv = new WTextField();
        pv.DPadDown += this.OnDPadDown;
        pv.GotFocus += this.OnGotFocus;
        pv.LostFocus += this.OnLostFocus;
        return pv;
    }

    private static void MapInternalFocus(TextFieldHandler handler, TextField view)
    {
        if (view.InternalFocus)
        {
            handler.PlatformView.Focus(FocusState.Programmatic);
        }
    }

    private void OnGotFocus(object sender, RoutedEventArgs e)
    {
        this.editTextManager.Focus = true;
    }

    private void OnLostFocus(object sender, RoutedEventArgs e)
    {
        this.editTextManager.Focus = false;
    }

    private void OnDPadDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        var position = this.editTextManager.CaretPosition;
        var start = this.editTextManager.Range.Start;
        var end = this.editTextManager.Range.End;
        var textLength = this.editTextManager.Document.Length;

        var isCtrlPressed = Microsoft.UI.Input.InputKeyboardSource
            .GetKeyStateForCurrentThread(VirtualKey.Control)
            .HasFlag(CoreVirtualKeyStates.Down);

        var isShiftPressed = Microsoft.UI.Input.InputKeyboardSource
            .GetKeyStateForCurrentThread(VirtualKey.Shift)
            .HasFlag(CoreVirtualKeyStates.Down);

        if (e.Key is VirtualKey.Left)
        {
            if (isCtrlPressed)
                this.editTextManager.Navigate(NavigationKind.WordLeft);
            else if (isShiftPressed)
            {
                if (end > position)
                    this.editTextManager.Range = new TextRange(position, end - 1);
                else
                    this.editTextManager.Range = new TextRange(Math.Max(0, start - 1), position);
            }
            else if (start != end)
                this.editTextManager.CaretPosition = start;
            else
                this.editTextManager.CaretPosition--;
        }
        else if (e.Key is VirtualKey.Right)
        {
            if (isCtrlPressed)
                this.editTextManager.Navigate(NavigationKind.CharacterLeft);
            else if (isShiftPressed)
            {
                if (start < position)
                    this.editTextManager.Range = new TextRange(start + 1, position);
                else
                    this.editTextManager.Range = new TextRange(position, Math.Min(end + 1, textLength - 1));
            }
            else if (start != end)
                this.editTextManager.CaretPosition = end;
            else
                this.editTextManager.CaretPosition++;

        }
        else if (e.Key is VirtualKey.Up)
        {
            if (isShiftPressed)
            {
                var currPosition = end > position ? end : start;
                float? xCoord = 0f;
                var newPosition = this.editTextManager.Document
                    .Navigate(
                        new CaretPosition(currPosition),
                        NavigationKind.LineUp,
                        0,
                        ref xCoord
                    )
                    .CodePointIndex;
                this.editTextManager.Range = new TextRange(newPosition, this.editTextManager.CaretPosition).Normalized;
            }
            else
                this.editTextManager.Navigate(NavigationKind.LineUp);
        }
        else if (e.Key is VirtualKey.Down)
        {
            if (isShiftPressed)
            {
                var currPosition = start < position ? start : end;
                float? xCoord = 0f;
                var newPosition = this.editTextManager.Document
                    .Navigate(
                        new CaretPosition(currPosition),
                        NavigationKind.LineDown,
                        0,
                        ref xCoord
                    )
                    .CodePointIndex;
                this.editTextManager.Range = new TextRange(this.editTextManager.CaretPosition, newPosition).Normalized;
            }
            else
                this.editTextManager.Navigate(NavigationKind.LineDown);
        }
    }

    private void OnTextInput(object sender, IMETextInputEventArgs e)
    {
        if (!this.editTextManager.Focus)
            return;

        switch (e.Character)
        {
            case '\b':
                this.editTextManager.DeleteRangeText();
                break;
            case '\r':
                this.editTextManager.CommitText("\n");
                break;
            //Ctrl + A
            case '\u0001':
                this.editTextManager.SelectAll();
                break;
            //Ctrl + C
            case '\u0003':
                this.editTextManager.CopyRangeTextToClipboard();
                break;
            //Ctrl + V
            case '\u0016':
                this.editTextManager.CopyRangeTextFromClipboard();
                break;
            //Ctrl + X
            case '\u0018':
                this.editTextManager.CutRangeTextToClipboard();

                break;
            //Ctrl + Z
            case '\u001a':
                this.editTextManager.Undo();
                break;
            //Ctrl + Y
            case '\u0019':
                this.editTextManager.Redo();
                break;
            default:
                if (!char.IsControl(e.Character))
                {
                    var text = e.Character.ToString();
                    this.editTextManager.CommitText(text);
                }
                break;
        }
    }

    protected override void ConnectHandler(WTextField platformView)
    {
        base.ConnectHandler(platformView);
        platformView.Canvas.PaintSurface += this.OnPaintSurface;
        InputMethod.TextInput += this.OnTextInput;
        InputMethod.TextComposition += this.OnTextComposition;
    }

    protected override void DisconnectHandler(WTextField platformView)
    {
        this.touchHandler?.Detach(platformView.Canvas);
        this.touchHandler = null;
        platformView.Canvas.PaintSurface -= this.OnPaintSurface;

        InputMethod.TextInput -= this.OnTextInput;
        InputMethod.TextComposition -= this.OnTextComposition;
        base.DisconnectHandler(platformView);
    }

    private void OnTextComposition(object sender, IMETextCompositionEventArgs e)
    {
        if (!this.editTextManager.Focus)
            return;

        var point = this.PlatformView
            .TransformToVisual(null)
            .TransformPoint(new Windows.Foundation.Point());

        var bounds = this.editTextManager.GetCaretBounds();
        InputMethod.SetTextInputRect(
            (int)(point.X + bounds.Left),
            (int)(point.Y + bounds.Top + bounds.Height),
            0,
            0
        );
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
        handler.PlatformView?.Canvas?.Invalidate();
    }

    public static void MapIgnorePixelScaling(TextFieldHandler handler, ISKCanvasView canvasView)
    {
        handler.PlatformView?.Canvas?.UpdateIgnorePixelScaling(canvasView);
    }

    public static void MapEnableTouchEvents(TextFieldHandler handler, ISKCanvasView canvasView)
    {
        if (handler.PlatformView == null)
            return;

        handler.touchHandler ??= new SKTouchHandler(canvasView.OnTouch, handler.OnGetScaledCoord);

        handler.touchHandler?.SetEnabled(handler.PlatformView.Canvas, canvasView.EnableTouchEvents);
    }

    private void OnPaintSurface(object sender, SkiaSharp.Views.Windows.SKPaintSurfaceEventArgs e)
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
        if (this.VirtualView?.IgnorePixelScaling == false && this.PlatformView != null)
        {
            var scale = this.PlatformView.Canvas.Dpi;
            x *= scale;
            y *= scale;
        }

        return new SKPoint((float)x, (float)y);
    }
}
