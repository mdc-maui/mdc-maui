using System.Collections.ObjectModel;
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

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
        nameof(SelectedItem),
        typeof(NavigationBarItem),
        typeof(NavigationBar),
        propertyChanged: (bo, ov, nv) =>
        {
            var navBar = bo as NavigationBar;
            var item = (NavigationBarItem)nv;

            foreach (var navItem in navBar.Items)
                navItem.IsActived = item.Equals(navItem);

            navBar.SelectedItemChanged?.Invoke(navBar, new(item));

            if (navBar.Command?.CanExecute(navBar.CommandParameter ?? item) is true)
                navBar.Command?.Execute(navBar.CommandParameter ?? item);
        }
    );

    public static readonly BindableProperty CommandProperty = IICommandElement.CommandProperty;
    public static readonly BindableProperty CommandParameterProperty =
        IICommandElement.CommandParameterProperty;

    public ObservableCollection<NavigationBarItem> Items
    {
        get => (ObservableCollection<NavigationBarItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    void IItemsElement<NavigationBarItem>.OnItemsCollectionChanged(
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

    public NavigationBarItem SelectedItem
    {
        get => (NavigationBarItem)this.GetValue(SelectedItemProperty);
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

    public event EventHandler<SelectedItemChangedArgs<NavigationBarItem>> SelectedItemChanged;

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
        this.Items != null ? this.Items : Array.Empty<IVisualTreeElement>().ToList();
}
