namespace Material.Components.Maui;

[ContentProperty(nameof(Content))]
public partial class Popup : Element
{
    [AutoBindable]
    private readonly View content;

    [AutoBindable(DefaultValue = "Microsoft.Maui.Controls.LayoutAlignment.Center")]
    private readonly LayoutAlignment horizontalOptions;

    [AutoBindable(DefaultValue = "Microsoft.Maui.Controls.LayoutAlignment.Center")]
    private readonly LayoutAlignment verticalOptions;

    [AutoBindable]
    private readonly bool dismissOnOutside;

    [AutoBindable]
    private readonly int offsetX;

    [AutoBindable]
    private readonly int offsetY;

    public EventHandler Opened;
    public EventHandler<object> Closed;

    private readonly TaskCompletionSource<object> taskCompletionSource = new();

    public async Task<object> ShowAtAsync(Page anchor)
    {
        this.PlatformShow(anchor);
        return await this.taskCompletionSource.Task;
    }

#if !WINDOWS && !__ANDROID__

    private void PlatformShow(Page anchor)
    {
        throw new NotImplementedException();
    }

    public void Close(object result = null)
    {
        throw new NotImplementedException();
    }
#endif
}
