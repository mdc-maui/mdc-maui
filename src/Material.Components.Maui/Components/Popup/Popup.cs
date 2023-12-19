namespace Material.Components.Maui;

[ContentProperty(nameof(Content))]
public partial class Popup : View, IVisualTreeElement, IDisposable
{
    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content),
        typeof(View),
        typeof(Popup),
        default
    );

    public static new readonly BindableProperty HorizontalOptionsProperty = BindableProperty.Create(
        nameof(HorizontalOptions),
        typeof(LayoutAlignment),
        typeof(Popup),
        LayoutAlignment.Center
    );

    public static new readonly BindableProperty VerticalOptionsProperty = BindableProperty.Create(
        nameof(VerticalOptions),
        typeof(LayoutAlignment),
        typeof(Popup),
        LayoutAlignment.Center
    );

    public static readonly BindableProperty DismissOnOutsideProperty = BindableProperty.Create(
        nameof(DismissOnOutside),
        typeof(bool),
        typeof(Popup),
        default
    );

    public static readonly BindableProperty OffsetXProperty = BindableProperty.Create(
        nameof(OffsetX),
        typeof(int),
        typeof(Popup),
        default
    );

    public static readonly BindableProperty OffsetYProperty = BindableProperty.Create(
        nameof(OffsetY),
        typeof(int),
        typeof(Popup),
        default
    );

    public View Content
    {
        get => (View)this.GetValue(ContentProperty);
        set => this.SetValue(ContentProperty, value);
    }

    public new LayoutAlignment HorizontalOptions
    {
        get => (LayoutAlignment)this.GetValue(HorizontalOptionsProperty);
        set => this.SetValue(HorizontalOptionsProperty, value);
    }

    public new LayoutAlignment VerticalOptions
    {
        get => (LayoutAlignment)this.GetValue(VerticalOptionsProperty);
        set => this.SetValue(VerticalOptionsProperty, value);
    }

    public bool DismissOnOutside
    {
        get => (bool)this.GetValue(DismissOnOutsideProperty);
        set => this.SetValue(DismissOnOutsideProperty, value);
    }

    public int OffsetX
    {
        get => (int)this.GetValue(OffsetXProperty);
        set => this.SetValue(OffsetXProperty, value);
    }

    public int OffsetY
    {
        get => (int)this.GetValue(OffsetYProperty);
        set => this.SetValue(OffsetYProperty, value);
    }

    public EventHandler Opened;
    public EventHandler<object> Closed;

    private readonly TaskCompletionSource<object> taskCompletionSource = new();

    private bool disposedValue;

    public async Task<object> ShowAtAsync(Page anchor = default)
    {
        this.PlatformShow(anchor);
        return await this.taskCompletionSource.Task;
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if (this.Content != null)
            SetInheritedBindingContext(this.Content, this.BindingContext);
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.Content != null ? [this.Content] : Array.Empty<IVisualTreeElement>().ToList();
}
