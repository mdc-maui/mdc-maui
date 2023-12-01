using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public class NavigationDrawer
    : TemplatedView,
        IItemsElement<View>,
        IICommandElement,
        IVisualTreeElement
{
    public static readonly BindableProperty ItemsProperty = IItemsElement<View>.ItemsProperty;

    private static readonly BindablePropertyKey FooterItemsPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(FooterItems),
            typeof(ObservableCollection<View>),
            typeof(NavigationDrawer),
            null,
            defaultValueCreator: bo => new ObservableCollection<View>()
        );

    public static readonly BindableProperty FooterItemsProperty =
        FooterItemsPropertyKey.BindableProperty;

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
        nameof(SelectedItem),
        typeof(View),
        typeof(NavigationDrawer),
        propertyChanged: (bo, ov, nv) =>
        {
            var navDrawer = bo as NavigationDrawer;

            if (nv is NavigationDrawerItem ndi)
            {
                foreach (var navItem in navDrawer.Items)
                {
                    if (navItem is NavigationDrawerItem item)
                    {
                        item.IsActived = item.Equals(ndi);
                    }
                }

                foreach (var navItem in navDrawer.FooterItems)
                {
                    if (navItem is NavigationDrawerItem item)
                    {
                        item.IsActived = item.Equals(ndi);
                    }
                }

                navDrawer.SelectedItemChanged?.Invoke(navDrawer, new(ndi));
                navDrawer.Command?.Execute(navDrawer.CommandParameter ?? ndi);
            }
        }
    );

    public static readonly BindableProperty CommandProperty = IICommandElement.CommandProperty;
    public static readonly BindableProperty CommandParameterProperty =
        IICommandElement.CommandParameterProperty;

    public ObservableCollection<View> Items
    {
        get => (ObservableCollection<View>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    public ObservableCollection<View> FooterItems
    {
        get => (ObservableCollection<View>)this.GetValue(FooterItemsProperty);
        set => this.SetValue(FooterItemsProperty, value);
    }

    void IItemsElement<View>.OnItemsCollectionChanged(
        object sender,
        NotifyCollectionChangedEventArgs e
    )
    {
        if (
            e.Action is NotifyCollectionChangedAction.Add
            && this.Items[e.NewStartingIndex] is NavigationDrawerItem ndi
        )
        {
            if (ndi.IsActived)
                this.SelectedItem = ndi;

            this.SelectedItem ??= ndi;
        }
    }

    public NavigationDrawerItem SelectedItem
    {
        get => (NavigationDrawerItem)this.GetValue(SelectedItemProperty);
        set => this.SetValue(SelectedItemProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)this.GetValue(CommandProperty);
        set => this.SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => this.GetValue(CommandParameterProperty);
        set => this.SetValue(CommandParameterProperty, value);
    }

    public event EventHandler<SelectedItemChangedArgs<NavigationDrawerItem>> SelectedItemChanged;

    private Grid PART_Root;

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        this.PART_Root = (Grid)this.GetTemplateChild("PART_Root");

        this.OnChildAdded(this.PART_Root);
        VisualDiagnostics.OnChildAdded(this, this.PART_Root);
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        SetInheritedBindingContext(this.PART_Root, this.BindingContext);
    }

    internal static Popup CreateDrawer(Grid grid)
    {
        var navDrawer = grid.GetParentElement<NavigationDrawer>();

        var itemsLayout = new WrapLayout { Orientation = StackOrientation.Vertical };

        foreach (var item in navDrawer.Items)
            itemsLayout.Add(item);

        var footerItemsLayout = new WrapLayout
        {
            Orientation = StackOrientation.Vertical,
            Padding = new(0, 12, 0, 0),
            VerticalOptions = LayoutOptions.End,
        };

        foreach (var item in navDrawer.FooterItems)
            footerItemsLayout.Add(item);

        var scrollView = new ScrollView
        {
            HorizontalScrollBarVisibility = Microsoft.Maui.ScrollBarVisibility.Never,
            Orientation = ScrollOrientation.Vertical,
            VerticalScrollBarVisibility = Microsoft.Maui.ScrollBarVisibility.Never,
            Content = itemsLayout
        };

        scrollView.SetValue(Grid.RowProperty, 0);
        footerItemsLayout.SetValue(Grid.RowProperty, 1);

        var content = new Card
        {
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start,
            Elevation = Elevation.Level0,
            Shape = new(0, 16, 0, 16),
            Padding = new(12),
            WidthRequest = 360,
            Content = new Grid
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Fill,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Auto }
                },
                Children = { scrollView, footerItemsLayout }
            },
        };

        content.SetDynamicResource(Card.BackgroundColorProperty, "SurfaceColor");
        content.SetBinding(
            HeightRequestProperty,
            new Binding(
                "Height",
                source: new RelativeBindingSource(
                    RelativeBindingSourceMode.FindAncestor,
                    typeof(NavigationDrawer)
                )
            )
        );

        var popup = new Popup
        {
            DismissOnOutside = true,
            HorizontalOptions = LayoutAlignment.Start,
            VerticalOptions = LayoutAlignment.Start,
            Content = content,
            Parent = navDrawer
        };

        return popup;
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.Items != null
            ? this.Items.Concat(this.FooterItems).Append(this.PART_Root).ToArray()
            : Array.Empty<IVisualTreeElement>().ToList();
}
