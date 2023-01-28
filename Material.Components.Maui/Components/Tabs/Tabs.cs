using System.Windows.Input;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class Tabs : TemplatedView, IVisualTreeElement, ICommandElement
{
    private static readonly BindablePropertyKey ItemsPropertyKey = BindableProperty.CreateReadOnly(
        nameof(Items),
        typeof(ItemCollection<TabItem>),
        typeof(Tabs),
        null,
        defaultValueCreator: bo => new ItemCollection<TabItem>()
    );

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;

    public ItemCollection<TabItem> Items
    {
        get => (ItemCollection<TabItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    [AutoBindable(DefaultValue = "true", OnChanged = nameof(OnBarPropertyChanged))]
    private readonly bool hasLabel;

    [AutoBindable(DefaultValue = "true", OnChanged = nameof(OnBarPropertyChanged))]
    private readonly bool hasIcon;

    [AutoBindable(OnChanged = nameof(OnSelectedIndexChanged))]
    private readonly int selectedIndex;

    [AutoBindable(OnChanged = nameof(OnSelectedItemChanged))]
    private readonly TabItem selectedItem;

    [AutoBindable(OnChanged = nameof(OnIndicatorShapeChanged))]
    private readonly Shape activeIndicatorShape;

    [AutoBindable(OnChanged = nameof(OnIndicatorColorChanged))]
    private readonly Color activeIndicatorColor;

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
        this.SelectedIndex = this.Items.IndexOf(this.SelectedItem);

        foreach (var item in this.Items)
        {
            if (item is TabItem ti)
            {
                ti.IsActived = this.SelectedItem.Equals(ti);
            }
        }
        this.SelectedItemChanged?.Invoke(
            this,
            new SelectedItemChangedEventArgs(this.SelectedItem, this.SelectedIndex)
        );
        this.Command?.Execute(this.CommandParameter ?? this.SelectedItem);
    }

    private void OnBarPropertyChanged()
    {
        foreach (var item in this.Items)
        {
            item.HasLabel = this.HasLabel;
            item.HasIcon = this.HasIcon;
        }
    }

    private void OnIndicatorShapeChanged()
    {
        for (int i = 0; i < this.Items.Count; i++)
        {
            this.Items[i].ActiveIndicatorShape = this.ActiveIndicatorShape;
        }
    }

    private void OnIndicatorColorChanged()
    {
        for (int i = 0; i < this.Items.Count; i++)
        {
            this.Items[i].ActiveIndicatorColor = this.ActiveIndicatorColor;
        }
    }

    private Grid PART_Root;
    private Layout PART_Bar;
    private ScrollView PART_Scroller;

    public Tabs()
    {
        this.Items.OnAdded += this.OnItemsAdded;
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        this.PART_Root = (Grid)this.GetTemplateChild("PART_Root");
        this.PART_Bar = (Layout)this.GetTemplateChild("PART_Bar");
        this.PART_Scroller = this.GetTemplateChild("PART_Scroller") as ScrollView;
        this.OnChildAdded(this.PART_Root);
        VisualDiagnostics.OnChildAdded(this, this.PART_Root);
    }

    private void OnItemsAdded(object sender, ItemsChangedEventArgs<TabItem> e)
    {
        var index = e.Index;
        var item = this.Items[index];
        this.SelectedItem ??= item;
        item.IsActived = index == this.SelectedIndex;
        item.HasIcon = this.HasIcon;
        item.HasLabel = this.HasLabel;
        if (this.ActiveIndicatorShape != Shape.None)
            item.ActiveIndicatorShape = this.ActiveIndicatorShape;
        if (this.ActiveIndicatorColor != null)
            item.ActiveIndicatorColor = this.ActiveIndicatorColor;

        item.Clicked += (sender, e) => this.SelectedItem = sender as TabItem;
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.PART_Root != null
            ? new List<IVisualTreeElement> { this.PART_Root }
            : Array.Empty<IVisualTreeElement>().ToList();

    public IVisualTreeElement GetVisualParent() => this.Window.Parent;
}
