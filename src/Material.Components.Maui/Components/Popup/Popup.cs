namespace Material.Components.Maui;

[ContentProperty(nameof(Content))]
public partial class Popup : Element
{
    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content),
        typeof(View),
        typeof(Popup),
        default
    );

    public View Content
    {
        get => (View)this.GetValue(ContentProperty);
        set => this.SetValue(ContentProperty, value);
    }

    public EventHandler<object> Closed;

    private readonly TaskCompletionSource<object> taskCompletionSource = new();

    public async Task<object> ShowAtAsync(Page page)
    {
        //#if MACCATALYST || IOS
        //        this.PlatformShow(page);
        //#endif
        return await this.taskCompletionSource.Task;
    }
}
