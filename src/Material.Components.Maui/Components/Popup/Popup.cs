namespace Material.Components.Maui;

[ContentProperty(nameof(Content))]
public partial class Popup : Element, IVisualTreeElement
{
    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content),
        typeof(View),
        typeof(Popup),
        default
    );

    public static readonly BindableProperty HorizontalOptionsProperty = BindableProperty.Create(
        nameof(HorizontalOptions),
        typeof(LayoutAlignment),
        typeof(Popup),
        LayoutAlignment.Center
    );

    public static readonly BindableProperty VerticalOptionsProperty = BindableProperty.Create(
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


    public LayoutAlignment HorizontalOptions
    {
        get => (LayoutAlignment)this.GetValue(HorizontalOptionsProperty);
        set => this.SetValue(HorizontalOptionsProperty, value);
    }

    public LayoutAlignment VerticalOptions
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

    public async Task<object> ShowAtAsync(Page anchor = default)
    {
        this.PlatformShow(anchor);
        return await this.taskCompletionSource.Task;
    }
}
