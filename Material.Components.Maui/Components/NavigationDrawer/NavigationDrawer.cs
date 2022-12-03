using CommunityToolkit.Maui.Markup;
using Material.Components.Maui.Converters;
using Material.Components.Maui.Core;
using Material.Components.Maui.Extensions;
using Microsoft.Maui.Platform;
using ShimSkiaSharp;
using System.ComponentModel;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class NavigationDrawer : ContentView, IVisualTreeElement
{
    private static readonly BindablePropertyKey ItemsPropertyKey = BindableProperty.CreateReadOnly(
        nameof(Items),
        typeof(ItemCollection<View>),
        typeof(NavigationBar),
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
            typeof(NavigationBar),
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

    private void OnDisplayModeChanged()
    {
        if (this.Handler != null)
        {
            throw new Exception(
                "Cannot be change DisplayMode at after Initialize completed!"
            );
        }

        if (this.DisplayMode == DrawerDisplayMode.Popup)
        {
            this.PaneWidth = 240d;
        }
    }

    private void OnIsPaneOpenChanged()
    {
        foreach (var item in Items)
        {
            if (item is NavigationDrawerItem ndi)
            {
                ndi.IsExtended = this.IsPaneOpen;
            }
        }
        foreach (var item in FooterItems)
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

    private void OnSelectedItemChanged()
    {
        if (this.SelectedItem != null)
        {
            foreach (var item in Items)
            {
                if (item is NavigationDrawerItem ndi)
                {
                    ndi.IsActived = this.SelectedItem.Equals(ndi);
                }
            }
            foreach (var item in FooterItems)
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
    }

    private SplitView PART_Root;
    private Card PART_Container;
    private Grid PART_Pane;
    private VerticalStackLayout PART_Items;
    private VerticalStackLayout PART_Footer;
    private Grid PART_ContentContainer;
    private ViewPager PART_Content;

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
            this.PART_Content.Items.Insert(e.Index, item.Content);
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
        if (view is NavigationDrawerItem item)
        {
            this.PART_Content.Items.Remove(item.Content);
        }
    }

    private void OnItemsCleared(object sender, EventArgs e)
    {
        foreach (var item in this.Items)
        {
            if (item is NavigationDrawerItem ndi)
            {
                this.PART_Content.Items.Remove(ndi.Content);
            }
        }
        this.PART_Items.Children.Clear();
    }

    private void OnFooterItemsAdded(object sender, ItemsChangedEventArgs<View> e)
    {
        var view = this.FooterItems[e.Index];
        this.PART_Footer.Children.Insert(e.Index, view);
        if (view is NavigationDrawerItem item)
        {
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
        if (view is NavigationDrawerItem item)
        {
            this.PART_Content.Items.Remove(item.Content);
        }
    }

    private void OnFooterItemsCleared(object sender, EventArgs e)
    {
        foreach (var item in this.FooterItems)
        {
            if (item is NavigationDrawerItem ndi)
            {
                this.PART_Content.Items.Remove(ndi.Content);
            }
        }
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
        this.PART_Content = (ViewPager)this.GetTemplateChild("PART_Content");
        this.PART_Pane.BindingContext = this;
        this.PART_ContentContainer.BindingContext = this;
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        new List<IVisualTreeElement> { this.PART_Root };

    public IVisualTreeElement GetVisualParent() => this.Window;
}
