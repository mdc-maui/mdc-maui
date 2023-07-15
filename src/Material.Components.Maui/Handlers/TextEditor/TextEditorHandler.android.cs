using Microsoft.Maui.Handlers;

namespace Material.Components.Maui.Handlers;

public partial class TextEditorHandler : ViewHandler<BaseTextEditor, PlatformTextEditor>
{
    protected override PlatformTextEditor CreatePlatformView()
    {
        return new(this.Context);
    }

    public static void MapDrawable(TextEditorHandler handler, BaseTextEditor virtualView)
    {
        handler.PlatformView.Drawable = virtualView.Drawable;
    }

    public static void MapInvalidate(
        TextEditorHandler handler,
        BaseTextEditor virtualView,
        object arg
    )
    {
        handler.PlatformView?.Invalidate();
    }

    protected override void ConnectHandler(PlatformTextEditor platformView)
    {
        platformView.Connect(this.VirtualView);
        platformView.TextChanged += this.OnTextChanged;
        platformView.SelectionChanged += this.OnSelectionChanged;
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(PlatformTextEditor platformView)
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

    private static void MapText(TextEditorHandler handler, BaseTextEditor virtualView)
    {
        if (handler.PlatformView.Text != virtualView.Text)
            handler.PlatformView.Text = virtualView.Text;
    }

    private static void MapSelectionRange(TextEditorHandler handler, BaseTextEditor virtualView)
    {
        var pv = handler.PlatformView;
        var start = virtualView.SelectionRange.Start;
        var end = virtualView.SelectionRange.End;

        if (start != pv.SelectionStart || end != pv.SelectionEnd)
            pv.SetSelection(start, end);
    }

    private static void MapFontSize(TextEditorHandler handler, BaseTextEditor virtualView)
    {
        handler.PlatformView.SetTextSize(Android.Util.ComplexUnitType.Dip, virtualView.FontSize);
    }
}
