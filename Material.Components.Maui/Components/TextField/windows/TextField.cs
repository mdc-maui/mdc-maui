using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using SkiaSharp.Views.Windows;
using Windows.System;

namespace Material.Components.Maui.Core;

public class WTextField : ButtonBase
{
    public SKXamlCanvas Canvas { get; set; }

    public event EventHandler<KeyRoutedEventArgs> DPadDown;
    public event EventHandler<ValueChangedEventArgs> FocusChanged;

    private bool isArrow = true;

    public WTextField()
    {
        this.IsTabStop = true;
        this.IsHitTestVisible = true;
        this.Content = this.Canvas = new SKXamlCanvas();
    }

    protected override void OnKeyDown(KeyRoutedEventArgs e)
    {
        if (e.Key is VirtualKey.Left or VirtualKey.Right or VirtualKey.Up or VirtualKey.Down)
        {
            DPadDown?.Invoke(this, e);
        }
        e.Handled = true;
    }

    public void SetCursor(bool isArrow)
    {
        if (this.isArrow != isArrow)
        {
            this.ProtectedCursor = InputSystemCursor.Create(
                isArrow ? InputSystemCursorShape.Arrow : InputSystemCursorShape.IBeam
            );
            this.isArrow = isArrow;
        }
    }

    protected override void OnPointerExited(PointerRoutedEventArgs e)
    {
        this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
        this.isArrow = true;
    }
}
