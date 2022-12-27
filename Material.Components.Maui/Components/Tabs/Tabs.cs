using System.Runtime.Versioning;
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

    [AutoBindable(OnChanged = nameof(OnIndicatorShapeChanged))]
    private readonly Shape activeIndicatorShape;

    [AutoBindable(OnChanged = nameof(OnIndicatorColorChanged))]
    private readonly Color activeIndicatorColor;

    [SupportedOSPlatform("android")]
    public static readonly BindableProperty UserInputEnabledProperty = BindableProperty.Create(
        nameof(UserInputEnabled),
        typeof(bool),
        typeof(Tabs),
        true
    );

    [SupportedOSPlatform("android")]
    public bool UserInputEnabled
    {
        get => (bool)this.GetValue(UserInputEnabledProperty);
        set => this.SetValue(UserInputEnabledProperty, value);
    }

    [AutoBindable]
    private readonly ICommand command;

    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<SelectedIndexChangedEventArgs> SelectedIndexChanged;

    private void OnSelectedIndexChanged()
    {
        for (int i = 0; i < this.Items.Count; i++)
        {
            this.Items[i].IsActived = i == this.SelectedIndex;
        }
        if (this.PART_Content.SelectedIndex != this.SelectedIndex)
        {
            this.PART_Content.SelectedIndex = this.SelectedIndex;
        }
        SelectedIndexChanged?.Invoke(this, new SelectedIndexChangedEventArgs(this.SelectedIndex));
        this.Command?.Execute(this.CommandParameter ?? this.SelectedIndex);

        this.PART_Scroller?.ScrollToAsync(
           this.Items[this.SelectedIndex],
           ScrollToPosition.Center,
           true
       );
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
    private ViewPager PART_Content;
    private ScrollView PART_Scroller;

    public Tabs()
    {
        this.Items.OnAdded += this.OnItemsAdded;
        this.Items.OnRemoved += this.OnItemsRemoved;
        this.Items.OnCleared += this.OnItemsCleared;
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        this.PART_Root = (Grid)this.GetTemplateChild("PART_Root");
        this.PART_Bar = (Layout)this.GetTemplateChild("PART_Bar");
        this.PART_Scroller = this.GetTemplateChild("PART_Scroller") as ScrollView;
        this.PART_Content = (ViewPager)this.GetTemplateChild("PART_Content");
        this.PART_Content.SelectedItemChanged += (sender, e) =>
        {
            this.SelectedIndex = e.SelectedItemIndex;
            for (int i = 0; i < this.Items.Count; i++)
            {
                this.Items[i].IsActived = i == e.SelectedItemIndex;
            }
        };
        this.OnChildAdded(PART_Root);
        VisualDiagnostics.OnChildAdded(this, PART_Root);
    }

    private void OnItemsAdded(object sender, ItemsChangedEventArgs<TabItem> e)
    {
        var index = e.Index;
        var item = this.Items[index];
        item.IsActived = index == this.SelectedIndex;
        item.HasIcon = this.HasIcon;
        item.HasLabel = this.HasLabel;
        if (this.ActiveIndicatorShape != Shape.None)
            item.ActiveIndicatorShape = this.ActiveIndicatorShape;
        if (this.ActiveIndicatorColor != null)
            item.ActiveIndicatorColor = this.ActiveIndicatorColor;
      
        if (item.Content != null)
        {
            this.PART_Content.Items.Insert(index, item.Content);
        }
        else
        {
            item.ContentChanged += (s, e) =>
            {
                this.PART_Content.Items.Insert(index, e);
            };
        }
        item.Clicked += (sender, e) => this.SelectedIndex = this.Items.IndexOf(sender as TabItem);
    }

    private void OnItemsRemoved(object sender, ItemsChangedEventArgs<TabItem> e)
    {
        var item = this.Items[e.Index];
        this.PART_Content.Items.Remove(item.Content);
    }

    private void OnItemsCleared(object sender, EventArgs e)
    {
        this.PART_Content.Items.Clear();
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.PART_Root != null
            ? new List<IVisualTreeElement> { this.PART_Root }
            : Array.Empty<IVisualTreeElement>().ToList();

    public IVisualTreeElement GetVisualParent() => this.Window.Parent;
}
