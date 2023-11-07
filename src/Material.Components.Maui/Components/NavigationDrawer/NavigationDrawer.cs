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
    public static readonly BindableProperty ItemsProperty =
        IItemsElement<View>.ItemsProperty;

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
        if (e.Action is NotifyCollectionChangedAction.Add && this.Items[e.NewStartingIndex] is NavigationDrawerItem ndi)
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

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.Items != null
            ? [this.PART_Root]
            : Array.Empty<IVisualTreeElement>().ToList();

    public IVisualTreeElement GetVisualParent() => null;
}
