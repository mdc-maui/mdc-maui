using System.Windows.Input;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class NavigationBar : TemplatedView, IVisualTreeElement, ICommandElement
{
    private static readonly BindablePropertyKey ItemsPropertyKey = BindableProperty.CreateReadOnly(
        nameof(Items),
        typeof(ItemCollection<NavigationBarItem>),
        typeof(NavigationBar),
        null,
        defaultValueCreator: bo => new ItemCollection<NavigationBarItem>()
    );

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;

    public ItemCollection<NavigationBarItem> Items
    {
        get => (ItemCollection<NavigationBarItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    [AutoBindable(DefaultValue = "true", OnChanged = nameof(OnHasLabelChanged))]
    private readonly bool hasLabel;

    [AutoBindable(OnChanged = nameof(OnSelectedIndexChanged))]
    private readonly int selectedIndex;

    [AutoBindable(OnChanged = nameof(OnSelectedItemChanged))]
    private readonly NavigationBarItem selectedItem;

    [AutoBindable]
    private readonly ICommand command;

    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<SelectedItemChangedEventArgs> SelectedItemChanged;

    private void OnSelectedIndexChanged()
    {
        this.SelectedItem = this.Items[this.SelectedIndex];
    }

    private void OnSelectedItemChanged()
    {
        foreach (var item in this.Items)
        {
            if (item is NavigationBarItem nbi)
            {
                nbi.IsActived = this.SelectedItem.Equals(nbi);
            }
        }
        this.SelectedItemChanged?.Invoke(
            this,
            new SelectedItemChangedEventArgs(
                this.SelectedItem,
                this.Items.IndexOf(this.SelectedItem)
            )
        );
        this.Command?.Execute(this.CommandParameter ?? this.SelectedItem);
    }

    private void OnHasLabelChanged()
    {
        this.PART_Bar.HeightRequest = this.HasLabel ? 80 : 65;
        foreach (var item in this.Items)
        {
            item.HasLabel = this.HasLabel;
        }
    }

    private Grid PART_Root;
    private AutoFillLayout PART_Bar;

    public NavigationBar()
    {
        this.Items.OnAdded += this.OnItemsAdded;
    }

    private void OnItemsAdded(object sender, ItemsChangedEventArgs<NavigationBarItem> e)
    {
        var index = e.Index;
        var item = this.Items[index];
        this.SelectedItem ??= item;
        item.HasLabel = this.HasLabel;
        item.Clicked += (sender, e) =>
        {
            var nbi = sender as NavigationBarItem;
            this.SelectedItem = nbi;
        };
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        this.PART_Root = (Grid)this.GetTemplateChild("PART_Root");
        this.PART_Bar = (AutoFillLayout)this.GetTemplateChild("PART_Bar");

        this.OnChildAdded(this.PART_Root);
        VisualDiagnostics.OnChildAdded(this, this.PART_Root);
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.PART_Root != null
            ? new List<IVisualTreeElement> { this.PART_Root }
            : Array.Empty<IVisualTreeElement>().ToList();

    public IVisualTreeElement GetVisualParent() => this.Window.Parent;
}
