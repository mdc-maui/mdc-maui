using Material.Components.Maui.Core;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class RadioButton : ContentView, IVisualTreeElement
{
    private static readonly BindablePropertyKey ItemsPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(Items),
            typeof(ItemCollection<RadioButtonItem>),
            typeof(RadioButton),
            null,
            defaultValueCreator: bo =>
                new ItemCollection<RadioButtonItem>());

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;

    public ItemCollection<RadioButtonItem> Items
    {
        get => (ItemCollection<RadioButtonItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    [AutoBindable(OnChanged = nameof(OnItemsSourceChanged))]
    private readonly IList itemsSource;

    [AutoBindable(DefaultValue = "-1", OnChanged = nameof(OnSelectedIndexChanged))]
    private readonly int selectedIndex;

    [AutoBindable(OnChanged = nameof(OnOrientationChanged))]
    private readonly StackOrientation orientation;

    [AutoBindable(OnChanged = nameof(OnSpacingChanged))]
    private readonly double spacing;

    [AutoBindable]
    private readonly ICommand command;
    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<SelectedIndexChangedEventArgs> SelectedIndexChanged;

    private void OnSelectedIndexChanged()
    {
        for (int i = 0; i < this.Items.Count; i++)
        {
            if (this.Items[i] is RadioButtonItem item)
            {
                item.IsSelected = i == this.SelectedIndex;
            }
        }
        SelectedIndexChanged?.Invoke(this, new SelectedIndexChangedEventArgs(this.SelectedIndex));
        this.Command?.Execute(this.CommandParameter ?? this.SelectedIndex);
    }

    private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is NotifyCollectionChangedAction.Add)
        {
            foreach (var item in e.NewItems)
            {
                this.Items.Add(new RadioButtonItem { Text = item.ToString() });
            }
        }
        else if (e.Action is NotifyCollectionChangedAction.Remove)
        {
            foreach (var item in e.OldItems)
            {
                for (int i = e.OldStartingIndex; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Text == item.ToString())
                    {
                        this.Items.RemoveAt(i);
                        break;
                    }
                }

            }
        }
        else if (e.Action is NotifyCollectionChangedAction.Replace)
        {
            for (int j = 0; j < e.OldItems.Count; j++)
            {
                for (int i = e.OldStartingIndex; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Text == e.OldItems[j].ToString())
                    {
                        this.Items[i] = new RadioButtonItem { Text = e.NewItems[j].ToString() };
                        break;
                    }
                }
            }
        }
        else if (e.Action is NotifyCollectionChangedAction.Reset)
        {
            this.Items.Clear();
        }
    }

    private void OnItemsSourceChanged()
    {
        if (this.ItemsSource != null)
        {
            if (this.ItemsSource is INotifyCollectionChanged ncc)
            {
                ncc.CollectionChanged += this.OnItemsSourceCollectionChanged;
            }

            foreach (var item in this.ItemsSource)
            {
                if (item is RadioButtonItem rbItem)
                {
                    this.Items.Add(rbItem);
                }
                else if (item is string text)
                {
                    this.Items.Add(new RadioButtonItem { Text = text });
                }
            }
        }
    }

    private void OnOrientationChanged()
    {
        this.PART_Content.Orientation = this.Orientation;
    }

    private void OnSpacingChanged()
    {
        this.PART_Content.Spacing = this.Spacing;
    }

    private readonly StackLayout PART_Content;

    public RadioButton()
    {
        this.Items.OnAdded += this.OnItemsAdded;
        this.Items.OnRemoved += this.OnItemsRemoved;
        this.Items.OnCleared += this.OnItemsCleared;
        this.HorizontalOptions = LayoutOptions.Start;
        this.VerticalOptions = LayoutOptions.Start;
        this.PART_Content = new StackLayout { Spacing = this.Spacing };
        this.Content = this.PART_Content;
    }

    private void OnItemsAdded(object sender, ItemsChangedEventArgs<RadioButtonItem> e)
    {
        var index = e.Index;
        var item = this.Items[index];
        this.PART_Content.Insert(index, item);
        item.IsSelected = e.Index == this.SelectedIndex;
        item.Clicked += (sender, e) => this.SelectedIndex = this.Items.IndexOf(sender as RadioButtonItem);
    }

    private void OnItemsRemoved(object sender, ItemsChangedEventArgs<RadioButtonItem> e)
    {
        this.PART_Content.RemoveAt(e.Index);
    }

    private void OnItemsCleared(object sender, EventArgs e)
    {
        this.PART_Content.Clear();
    }


    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() => new List<View> { this.Content };

    public IVisualTreeElement GetVisualParent() => this.Window;
}