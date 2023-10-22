using System.Collections;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public class NavigationBar
    : TemplatedView,
        IItemsElement<NavigationBarItem>,
        IICommandElement,
        IVisualTreeElement
{
    public static readonly BindableProperty ItemsProperty =
        IItemsElement<NavigationBarItem>.ItemsProperty;

    public static readonly BindableProperty ItemsSourceProperty =
        IItemsElement<NavigationBarItem>.ItemsSourceProperty;

    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
        nameof(SelectedIndex),
        typeof(int),
        typeof(NavigationBar),
        -1,
        propertyChanged: (bo, ov, nv) =>
        {
            var navBar = bo as NavigationBar;
            var index = (int)nv;
            if (index >= 0 && index < navBar.Items.Count)
                navBar.SelectedItem = navBar.Items[index];

            navBar.Command?.Execute(navBar.CommandParameter ?? index);
            navBar.SelectedChanged?.Invoke(navBar, new(navBar.Items[index], index));
        }
    );

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
        nameof(SelectedItem),
        typeof(NavigationBarItem),
        typeof(NavigationBar),
        propertyChanged: (bo, ov, nv) =>
        {
            var navBar = bo as NavigationBar;
            var item = (NavigationBarItem)nv;
            if (navBar.Items.Contains(item))
                navBar.SelectedIndex = navBar.Items.IndexOf(item);

            foreach (var navItem in navBar.Items)
                navItem.IsActived = item.Equals(navItem);
        }
    );

    public static readonly BindableProperty CommandProperty = IICommandElement.CommandProperty;
    public static readonly BindableProperty CommandParameterProperty =
        IICommandElement.CommandParameterProperty;

    public ItemCollection<NavigationBarItem> Items
    {
        get => (ItemCollection<NavigationBarItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    public IList ItemsSource
    {
        get => (IList)this.GetValue(ItemsSourceProperty);
        set => this.SetValue(ItemsSourceProperty, value);
    }

    void IItemsElement<NavigationBarItem>.OnItemsCollectionChanged(
        object sender,
        NotifyCollectionChangedEventArgs e
    )
    {
        if (this.SelectedIndex != -1 && this.SelectedItem == null && this.SelectedIndex == e.NewStartingIndex)
            this.SelectedItem = this.Items[e.NewStartingIndex];
    }

    void IItemsElement<NavigationBarItem>.OnItemsSourceCollectionChanged(
        object sender,
        NotifyCollectionChangedEventArgs e
    )
    {
        if (e.OldItems != null)
        {
            var index = e.OldStartingIndex;
            foreach (string item in e.OldItems)
            {
                this.Items.RemoveAt(index);
                index++;
            }
        }

        if (e.NewItems != null)
        {
            var index = e.NewStartingIndex;
            foreach (string item in e.NewItems)
            {
                this.Items.Insert(index, new NavigationBarItem { Text = item });
                index++;
            }
        }
    }

    public int SelectedIndex
    {
        get => (int)this.GetValue(SelectedIndexProperty);
        set => this.SetValue(SelectedIndexProperty, value);
    }

    public NavigationBarItem SelectedItem
    {
        get => (NavigationBarItem)this.GetValue(SelectedItemProperty);
        set => this.SetValue(SelectedItemProperty, value);
    }

    public new bool IsEnabled
    {
        get => (bool)this.GetValue(IsEnabledProperty);
        set => this.SetValue(IsEnabledProperty, value);
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

    public event EventHandler<SelectedItemChangedEventArgs> SelectedChanged;

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
