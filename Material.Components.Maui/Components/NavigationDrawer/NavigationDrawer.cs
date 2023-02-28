using System.Windows.Input;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class NavigationDrawer : TemplatedView, ICommandElement, IVisualTreeElement
{
    private static readonly BindablePropertyKey ItemsPropertyKey = BindableProperty.CreateReadOnly(
        nameof(Items),
        typeof(ItemCollection<View>),
        typeof(NavigationDrawer),
        null,
        defaultValueCreator: bo => new ItemCollection<View>()
    );

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;

    public ItemCollection<View> Items
    {
        get => (ItemCollection<View>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    private static readonly BindablePropertyKey FooterItemsPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(FooterItems),
            typeof(ItemCollection<View>),
            typeof(NavigationDrawer),
            null,
            defaultValueCreator: bo => new ItemCollection<View>()
        );

    public static readonly BindableProperty FooterItemsProperty =
        FooterItemsPropertyKey.BindableProperty;

    public ItemCollection<View> FooterItems
    {
        get => (ItemCollection<View>)this.GetValue(FooterItemsProperty);
        set => this.SetValue(FooterItemsProperty, value);
    }

    [AutoBindable(OnChanged = nameof(OnDisplayModeChanged))]
    private readonly DrawerDisplayMode displayMode;

    [AutoBindable(OnChanged = nameof(OnSelectedItemChanged))]
    private readonly NavigationDrawerItem selectedItem;

    [AutoBindable(OnChanged = nameof(OnIsPaneOpenChanged))]
    private readonly bool isPaneOpen;

    [AutoBindable]
    private readonly Color paneBackGroundColour;

    [AutoBindable]
    private readonly Color toolBarBackGroundColour;

    [AutoBindable(DefaultValue = "80d")]
    private readonly double paneWidth;

    [AutoBindable]
    private readonly string title;

    [AutoBindable]
    private readonly IconKind switchIcon;

    [AutoBindable]
    private readonly bool hasToolBar;

    [AutoBindable]
    private readonly ICommand command;

    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<SelectedItemChangedEventArgs> SelectedItemChanged;

    private void OnDisplayModeChanged()
    {
        if (this.Handler != null)
        {
            throw new Exception("Cannot be change DisplayMode at after Initialize completed!");
        }

        if (this.DisplayMode == DrawerDisplayMode.Popup)
        {
            this.PaneWidth = 240d;
        }
    }

    private void OnIsPaneOpenChanged()
    {
        foreach (var item in this.Items)
        {
            if (item is NavigationDrawerItem ndi)
            {
                ndi.IsExtended = this.IsPaneOpen;
            }
        }
        foreach (var item in this.FooterItems)
        {
            if (item is NavigationDrawerItem ndi)
            {
                ndi.IsExtended = this.IsPaneOpen;
            }
        }
        this.SwitchIcon = this.IsPaneOpen ? IconKind.MenuOpen : IconKind.Menu;
        if (this.DisplayMode == DrawerDisplayMode.Split)
        {
            this.PaneWidth = this.IsPaneOpen ? 240d : 80d;
        }
    }

    private void OnSelectedItemChanged(NavigationDrawerItem oldValue, NavigationDrawerItem newValue)
    {
        if (this.SelectedItem != null)
        {
            foreach (var item in this.Items)
            {
                if (item is NavigationDrawerItem ndi)
                {
                    ndi.IsActived = this.SelectedItem.Equals(ndi);
                }
            }
            foreach (var item in this.FooterItems)
            {
                if (item is NavigationDrawerItem ndi)
                {
                    ndi.IsActived = this.SelectedItem.Equals(ndi);
                }
            }
        }
        if (this.DisplayMode == DrawerDisplayMode.Popup)
        {
            this.IsPaneOpen = false;
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

    private SplitView PART_Root;
    private Grid PART_Pane;
    private VerticalStackLayout PART_Items;
    private VerticalStackLayout PART_Footer;
    private Grid PART_ContentContainer;

    public NavigationDrawer()
    {
        this.Items.OnAdded += this.OnItemsAdded;
        this.Items.OnRemoved += this.OnItemsRemoved;
        this.Items.OnCleared += this.OnItemsCleared;
        this.FooterItems.OnAdded += this.OnFooterItemsAdded;
        this.FooterItems.OnRemoved += this.OnFooterItemsRemoved;
        this.FooterItems.OnCleared += this.OnFooterItemsCleared;
    }

    private void OnItemsAdded(object sender, ItemsChangedEventArgs<View> e)
    {
        var view = this.Items[e.Index];
        this.PART_Items.Children.Insert(e.Index, view);
        if (view is NavigationDrawerItem item)
        {
            this.SelectedItem ??= item;
            item.IsExtended = this.IsPaneOpen;
            item.Clicked += (sender, e) =>
            {
                var ndi = sender as NavigationDrawerItem;
                this.SelectedItem = ndi;
            };
        }
    }

    private void OnItemsRemoved(object sender, ItemsChangedEventArgs<View> e)
    {
        var view = this.Items[e.Index];
        this.PART_Items.Children.Remove(view);
    }

    private void OnItemsCleared(object sender, EventArgs e)
    {
        this.PART_Items.Children.Clear();
    }

    private void OnFooterItemsAdded(object sender, ItemsChangedEventArgs<View> e)
    {
        var view = this.FooterItems[e.Index];
        this.PART_Footer.Children.Insert(e.Index, view);
        if (view is NavigationDrawerItem item)
        {
            item.IsExtended = this.IsPaneOpen;
            item.Clicked += (sender, e) =>
            {
                var ndi = sender as NavigationDrawerItem;
                this.SelectedItem = ndi;
            };
        }
    }

    private void OnFooterItemsRemoved(object sender, ItemsChangedEventArgs<View> e)
    {
        var view = this.FooterItems[e.Index];
        this.PART_Footer.Children.Remove(view);
    }

    private void OnFooterItemsCleared(object sender, EventArgs e)
    {
        this.PART_Footer.Children.Clear();
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        this.PART_Root = (SplitView)this.GetTemplateChild("PART_Root");
        this.PART_Pane = (Grid)this.GetTemplateChild("PART_Pane");
        this.PART_Items = (VerticalStackLayout)this.GetTemplateChild("PART_Items");
        this.PART_Footer = (VerticalStackLayout)this.GetTemplateChild("PART_Footer");
        this.PART_ContentContainer = (Grid)this.GetTemplateChild("PART_ContentContainer");

        this.OnChildAdded(this.PART_Root);
        VisualDiagnostics.OnChildAdded(this, this.PART_Root);
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.PART_Root != null
            ? new List<IVisualTreeElement> { this.PART_Root }
            : Array.Empty<IVisualTreeElement>().ToList();

    public IVisualTreeElement GetVisualParent() => this.Window.Parent;
}
