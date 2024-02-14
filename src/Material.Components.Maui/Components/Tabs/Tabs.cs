using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public class Tabs
    : TemplatedView,
        IItemsElement<TabItem>,
        IICommandElement,
        IVisualTreeElement,
        IStyleElement
{
    public static readonly BindableProperty ItemsProperty = IItemsElement<TabItem>.ItemsProperty;

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
        nameof(SelectedItem),
        typeof(TabItem),
        typeof(Tabs),
        propertyChanged: (bo, ov, nv) =>
        {
            var tabs = bo as Tabs;
            var item = (TabItem)nv;

            foreach (var tabItem in tabs.Items)
            {
                var s = item.Equals(tabItem);
                tabItem.IsActived = item.Equals(tabItem);
            }

            tabs.SelectedItemChanged?.Invoke(tabs, new(item));

            if (tabs.Command?.CanExecute(tabs.CommandParameter ?? item) is true)
                tabs.Command?.Execute(tabs.CommandParameter ?? item);
        }
    );

    public static readonly BindableProperty CommandProperty = IICommandElement.CommandProperty;
    public static readonly BindableProperty CommandParameterProperty =
        IICommandElement.CommandParameterProperty;

    public static readonly BindableProperty ItemStyleProperty = BindableProperty.Create(
        nameof(ItemStyle),
        typeof(ItemStyle),
        typeof(Tabs)
    );

    public static readonly BindableProperty DynamicStyleProperty =
        IStyleElement.DynamicStyleProperty;

    public string DynamicStyle
    {
        get => (string)this.GetValue(DynamicStyleProperty);
        set => this.SetValue(DynamicStyleProperty, value);
    }

    public ObservableCollection<TabItem> Items
    {
        get => (ObservableCollection<TabItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    void IItemsElement<TabItem>.OnItemsCollectionChanged(
        object sender,
        NotifyCollectionChangedEventArgs e
    )
    {
        if (e.Action is NotifyCollectionChangedAction.Add)
        {
            if (this.Items[e.NewStartingIndex].IsActived)
                this.SelectedItem = this.Items[e.NewStartingIndex];

            this.SelectedItem ??= this.Items[e.NewStartingIndex];
        }
    }

    public TabItem SelectedItem
    {
        get => (TabItem)this.GetValue(SelectedItemProperty);
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

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ItemStyle ItemStyle
    {
        get => (ItemStyle)this.GetValue(ItemStyleProperty);
        set => this.SetValue(ItemStyleProperty, value);
    }

    public event EventHandler<SelectedItemChangedArgs<TabItem>> SelectedItemChanged;

    private Grid PART_Root;

    public Tabs()
    {
        this.SetDynamicResource(StyleProperty, "PrimaryTabsStyle");
    }

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
        this.Items != null ? this.Items : Array.Empty<IVisualTreeElement>().ToList();
}
