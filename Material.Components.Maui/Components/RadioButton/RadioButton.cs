using Material.Components.Maui.Core.RadioButton;
using System.Collections;
using System.Collections.Specialized;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class RadioButton : ContentView
{
    private static readonly BindablePropertyKey ItemsPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(Items),
            typeof(RadioButtonItemCollection),
            typeof(RadioButton),
            null,
            defaultValueCreator: bo =>
                new RadioButtonItemCollection { Inner = ((RadioButton)bo).PART_Content });

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;

    public RadioButtonItemCollection Items
    {
        get => (RadioButtonItemCollection)this.GetValue(ItemsProperty);
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

    private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (var item in e.NewItems)
            {
                this.Items.Add(new RadioButtonItem { Text = item.ToString() });
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
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
        else if (e.Action == NotifyCollectionChangedAction.Replace)
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
        else if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            this.Items.Clear();
        }
    }

    private void OnItemsSourceChanged()
    {
        if (this.ItemsSource is not null)
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

    private void OnSelectedIndexChanged()
    {
        for (int i = 0; i < this.Items.Count; i++)
        {
            if (this.Items[i] is RadioButtonItem item)
            {
                item.IsSelected = i == this.SelectedIndex;
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
        this.HorizontalOptions = LayoutOptions.Start;
        this.VerticalOptions = LayoutOptions.Start;
        this.PART_Content = new StackLayout { Spacing = this.Spacing };
        this.PART_Content.ChildAdded += this.OnItemsAdded;
        this.Content = this.PART_Content;
    }

    private void OnItemsAdded(object sender, ElementEventArgs e)
    {
        if (e.Element is RadioButtonItem item)
        {
            item.IsSelected = this.Items.IndexOf(item) == this.SelectedIndex;
            item.SelectedChanged += (sender, e) =>
            {
                this.SelectedIndex = this.Items.IndexOf((RadioButtonItem)sender);
            };
        }
    }
}