using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Handlers;

namespace Material.Components.Maui.Handlers;

public partial class TextFieldHandler : ViewHandler<TextField, PlatformTextField>
{
    protected override PlatformTextField CreatePlatformView()
    {
        return new(this.Context);
    }

    public static void MapDrawable(TextFieldHandler handler, TextField virtualView)
    {
        handler.PlatformView?.UpdateDrawable(virtualView.Drawable);
    }

    public static void MapInvalidate(TextFieldHandler handler, TextField virtualView, object arg)
    {
        handler.PlatformView?.Invalidate();
    }

    protected override void ConnectHandler(PlatformTextField platformView)
    {
        platformView.Connect(this.VirtualView);
        platformView.TextChanged += this.OnTextChanged;
        platformView.SelectionChanged += this.OnSelectionChanged;
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(PlatformTextField platformView)
    {
        platformView.Disconnect();
        platformView.TextChanged -= this.OnTextChanged;
        platformView.SelectionChanged -= this.OnSelectionChanged;
        base.DisconnectHandler(platformView);
    }

    private void OnTextChanged(object sender, EditTextChangedEventArgs e)
    {
        this.VirtualView.Text = e.Text;
        this.VirtualView.UpdateSelectionRange(e.SelectionRange);
    }

    private void OnSelectionChanged(object sender, SelectionChangedArgs e)
    {
        this.VirtualView.UpdateSelectionRange(e.SelectionRange);
    }

    static partial void MapText(TextFieldHandler handler, TextField virtualView)
    {
        if (handler.PlatformView.Text != virtualView.Text)
            handler.PlatformView?.UpdateText(virtualView.Text);
    }

    static partial void MapSelectionRange(TextFieldHandler handler, TextField virtualView)
    {
        var pv = handler.PlatformView;
        var start = virtualView.SelectionRange.Start;
        var end = virtualView.SelectionRange.End;

        if (start != pv.SelectionStart || end != pv.SelectionEnd)
            pv?.UpdateSelection(start, end);
    }

    static partial void MapFontSize(TextFieldHandler handler, TextField virtualView)
    {
        handler.PlatformView?.SetTextSize(Android.Util.ComplexUnitType.Dip, virtualView.FontSize);
    }

    static partial void MapFontAttributes(TextFieldHandler handler, TextField virtualView)
    {
        var weight = (int)virtualView.FontWeight;
        var style = virtualView.FontIsItalic ? FontStyleType.Italic : FontStyleType.Normal;
        var font = new Microsoft.Maui.Graphics.Font(virtualView.FontFamily, weight, style);
        var typeface = font.ToTypeface();
        handler.PlatformView?.UpdateTypeface(typeface);
    }

    static partial void MapTextAlignment(TextFieldHandler handler, TextField virtualView)
    {
        var alignment =
            virtualView.TextAlignment == TextAlignment.End
                ? Android.Views.TextAlignment.TextEnd
                : virtualView.TextAlignment == TextAlignment.Center
                    ? Android.Views.TextAlignment.Center
                    : Android.Views.TextAlignment.TextStart;

        handler.PlatformView?.UpdateTextAlignment(alignment);
    }

    static partial void MapEditablePadding(TextFieldHandler handler, TextField virtualView)
    {
        handler.PlatformView?.UpdatePadding(virtualView.EditablePadding);
    }

    static partial void MapInputType(TextFieldHandler handler, TextField virtualView)
    {
        handler.PlatformView?.UpdateInputType(virtualView.InputType);
        MapFontAttributes(handler, virtualView);
        MapFontSize(handler, virtualView);
    }
}
