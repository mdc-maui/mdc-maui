using Material.Components.Maui.Core;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class ContextMenu : Card, IVisualTreeElement
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

    [AutoBindable(DefaultValue = "-1")]
    private readonly int visibleItemCount;

    public object Result { get; private set; }

    public EventHandler<object> Closed;

    private void OnClosed(object sender, object e)
    {
        this.Closed?.Invoke(this, this.Result ?? e);
    }

    private readonly VerticalStackLayout PART_Container = new();

    public ContextMenu()
    {
        this.Items.OnAdded += this.OnItemsAdded;
        this.Items.OnRemoved += this.OnItemsRemoved;
        this.Items.OnCleared += this.OnItemsCleared;
        this.Content = new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
            VerticalScrollBarVisibility = ScrollBarVisibility.Never,
            Content = PART_Container
        };
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

    public new IReadOnlyList<IVisualTreeElement> GetVisualChildren() => this.Items.ToList();

    public new IVisualTreeElement GetVisualParent() => this.Window;

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
