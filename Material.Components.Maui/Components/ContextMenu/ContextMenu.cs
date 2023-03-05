namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class ContextMenu : ContentView, IVisualTreeElement
{
    private static readonly BindablePropertyKey ItemsPropertyKey = BindableProperty.CreateReadOnly(
        nameof(Items),
        typeof(ItemCollection<MenuItem>),
        typeof(ContextMenu),
        null,
        defaultValueCreator: bo => new ItemCollection<MenuItem>()
    );

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;

    public ItemCollection<MenuItem> Items
    {
        get => (ItemCollection<MenuItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    [AutoBindable]
    private readonly Color backgroundColour;

    [AutoBindable]
    private readonly Color rippleColor;

    [AutoBindable(DefaultValue = "-1")]
    private readonly int visibleItemCount;

    public object Result { get; private set; }

    public EventHandler<object> Closed;

    private void OnClosed(object sender, object e)
    {
        this.Closed?.Invoke(this, this.Result ?? e);
    }

    private Grid PART_Root;
    private VerticalStackLayout PART_Container;

    public ContextMenu()
    {
        this.Items.OnAdded += this.OnItemsAdded;
        this.Items.OnRemoved += this.OnItemsRemoved;
        this.Items.OnCleared += this.OnItemsCleared;
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        this.PART_Root = (Grid)this.GetTemplateChild("PART_Root");
        this.Content = this.PART_Container = new VerticalStackLayout
        {
            Padding = new Thickness(0, 8)
        };
        this.OnChildAdded(this.PART_Root);
        VisualDiagnostics.OnChildAdded(this, this.PART_Root);
    }

    private void OnItemsAdded(object sender, ItemsChangedEventArgs<MenuItem> e)
    {
        var index = e.Index;
        var item = this.Items[index];
        this.PART_Container.Insert(index, item);
        item.Clicked += (sender, e) => this.Close(this.Items.IndexOf(sender as MenuItem));
    }

    private void OnItemsRemoved(object sender, ItemsChangedEventArgs<MenuItem> e)
    {
        this.PART_Container.RemoveAt(e.Index);
    }

    private void OnItemsCleared(object sender, EventArgs e)
    {
        this.PART_Container.Clear();
    }

    public void Show(View anchor)
    {
        if (this.WidthRequest is 0d or -1d)
        {
            var desiredWidth = -1d;
            foreach (var item in this.Items)
            {
                desiredWidth = Math.Max(this.WidthRequest, item.GetDesiredWidth());
            }
            foreach (var item in this.Items)
            {
                item.WidthRequest = desiredWidth;
            }
            this.WidthRequest = desiredWidth;
        }
        this.HeightRequest =
            (
                (
                    this.VisibleItemCount > 0
                        ? Math.Min(this.Items.Count, this.VisibleItemCount)
                        : this.Items.Count
                ) * 48
            ) + 16d;
        this.PlatformShow(anchor);
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.PART_Root != null
            ? new List<IVisualTreeElement> { this.PART_Root }
            : Array.Empty<IVisualTreeElement>().ToList();

    public IVisualTreeElement GetVisualParent() => null;

#if !WINDOWS && !__ANDROID__
    private void PlatformShow(View anchor)
    {
        throw new NotImplementedException();
    }

    public void Close(object result = null)
    {
        throw new NotImplementedException();
    }
#endif
}
