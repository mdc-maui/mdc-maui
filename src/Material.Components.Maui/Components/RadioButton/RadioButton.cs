using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Material.Components.Maui;

public class RadioButton
    : WrapLayout,
        IItemsElement<RadioItem>,
        IItemsSourceElement<RadioItem>,
        IICommandElement,
        IVisualTreeElement
{
    public static readonly BindableProperty ItemsProperty = IItemsElement<RadioItem>.ItemsProperty;

    public static readonly BindableProperty ItemsSourceProperty =
        IItemsSourceElement<RadioItem>.ItemsSourceProperty;

    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
        nameof(SelectedIndex),
        typeof(int),
        typeof(RadioButton),
        -1,
        propertyChanged: (bo, ov, nv) =>
        {
            var view = bo as RadioButton;
            var index = (int)nv;
            if (index >= 0 && index < view.Items.Count)
                view.SelectedItem = view.Items[index];

            for (var i = 0; i < view.Items.Count; i++)
                view.Items[i].IsSelected = i == index;

            view.Command?.Execute(view.CommandParameter ?? index);
            view.SelectedChanged?.Invoke(view, new(view.Items[index], index));
        }
    );

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
        nameof(SelectedItem),
        typeof(RadioItem),
        typeof(RadioButton),
        propertyChanged: (bo, ov, nv) =>
        {
            var view = bo as RadioButton;
            var item = (RadioItem)nv;
            if (view.Items.Contains(item))
                view.SelectedIndex = view.Items.IndexOf(item);

            foreach (var ri in view.Items)
                ri.IsSelected = item.Equals(ri);
        }
    );

    public static readonly BindableProperty CommandProperty = IICommandElement.CommandProperty;
    public static readonly BindableProperty CommandParameterProperty =
        IICommandElement.CommandParameterProperty;

    public ObservableCollection<RadioItem> Items
    {
        get => (ObservableCollection<RadioItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    public IList ItemsSource
    {
        get => (IList)this.GetValue(ItemsSourceProperty);
        set => this.SetValue(ItemsSourceProperty, value);
    }

    void IItemsElement<RadioItem>.OnItemsCollectionChanged(
        object sender,
        NotifyCollectionChangedEventArgs e
    ) { }

    void IItemsSourceElement<RadioItem>.OnItemsSourceCollectionChanged(
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
                this.Items.Insert(index, new RadioItem { Text = item });
                index++;
            }
        }
    }

    public int SelectedIndex
    {
        get => (int)this.GetValue(SelectedIndexProperty);
        set => this.SetValue(SelectedIndexProperty, value);
    }

    public RadioItem SelectedItem
    {
        get => (RadioItem)this.GetValue(SelectedItemProperty);
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

    public event EventHandler<SelectedItemChangedEventArgs> SelectedChanged;

    public RadioButton()
    {
        this.ChildAdded += this.OnChildAdded;
        this.ChildRemoved += this.OnChildRemoved;
    }

    private void OnChildAdded(object sender, ElementEventArgs e)
    {
        this.Items.Add((RadioItem)e.Element);
    }

    private void OnChildRemoved(object sender, ElementEventArgs e)
    {
        this.Items.Remove((RadioItem)e.Element);
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() => this.Items?.ToList();

    public IVisualTreeElement GetVisualParent() => null;
}
