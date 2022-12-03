using CommunityToolkit.Maui.Markup;
using Material.Components.Maui.Core;
using System.Runtime.Versioning;
using System.Windows.Input;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class Tabs : ContentView, IVisualTreeElement
{
    private static readonly BindablePropertyKey ItemsPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(Items),
            typeof(ItemCollection<TabItem>),
            typeof(Tabs),
            null,
            defaultValueCreator: bo => new ItemCollection<TabItem>());

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;

    public ItemCollection<TabItem> Items
    {
        get => (ItemCollection<TabItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    [AutoBindable(DefaultValue = "true", OnChanged = nameof(OnBarPropertyChanged))]
    private readonly bool hasLabel;

    [AutoBindable(DefaultValue = "true", OnChanged = nameof(OnBarPropertyChanged))]
    private readonly bool hasIcon;

    [AutoBindable(OnChanged = nameof(OnSelectedIndexChanged))]
    private readonly int selectedIndex;

    [AutoBindable(OnChanged = nameof(OnIndicatorShapeChanged))]
    private readonly Shape activeIndicatorShape;

    [AutoBindable(OnChanged = nameof(OnIndicatorColorChanged))]
    private readonly Color activeIndicatorColor;


    [SupportedOSPlatform("android")]
    public static readonly BindableProperty UserInputEnabledProperty = BindableProperty.Create(
        nameof(UserInputEnabled),
        typeof(bool),
        typeof(Tabs),
        true,
         propertyChanged: OnUserInputEnabledChanged);

    [SupportedOSPlatform("android")]
    public bool UserInputEnabled
    {
        get => (bool)this.GetValue(UserInputEnabledProperty);
        set => this.SetValue(UserInputEnabledProperty, value);
    }

    [SupportedOSPlatform("android")]
    private static void OnUserInputEnabledChanged(BindableObject bo, object oldValue, object NewValue)
    {
        ((Tabs)bo).PART_Content.UserInputEnabled = (bool)NewValue;
    }

    [AutoBindable]
    private readonly ICommand command;
    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<SelectedIndexChangedEventArgs> SelectedIndexChanged;

    private async void OnSelectedIndexChanged()
    {
        for (int i = 0; i < this.Items.Count; i++)
        {
            this.Items[i].IsActived = i == this.SelectedIndex;
        }
        if (this.PART_Content.SelectedIndex != this.SelectedIndex)
        {
            this.PART_Content.SelectedIndex = this.SelectedIndex;
        }
        await this.PART_Scroller.ScrollToAsync(this.Items[this.SelectedIndex], ScrollToPosition.MakeVisible, true);
        SelectedIndexChanged?.Invoke(this, new SelectedIndexChangedEventArgs(this.SelectedIndex));
        this.Command?.Execute(this.CommandParameter ?? this.SelectedIndex);
    }

    private void OnBarPropertyChanged()
    {
        this.PART_Bar.HeightRequest = this.HasLabel && this.HasIcon ? 64 : 48;
        foreach (var item in this.Items)
        {
            item.HasLabel = this.HasLabel;
            item.HasIcon = this.HasIcon;
        }
    }

    private void OnIndicatorShapeChanged()
    {
        for (int i = 0; i < this.Items.Count; i++)
        {
            this.Items[i].ActiveIndicatorShape = this.ActiveIndicatorShape;
        }
    }

    private void OnIndicatorColorChanged()
    {
        for (int i = 0; i < this.Items.Count; i++)
        {
            this.Items[i].ActiveIndicatorColor = this.ActiveIndicatorColor;
        }
    }

    private readonly ViewPager PART_Content;
    private readonly ScrollView PART_Scroller;
    private readonly Grid PART_Bar;

    public Tabs()
    {
        this.Items.OnAdded += this.OnItemsAdded;
        this.Items.OnRemoved += this.OnItemsRemoved;
        this.Items.OnCleared += this.OnItemsCleared;
        this.PART_Content = new ViewPager();
        this.PART_Content.SelectedItemChanged += (sender, e) =>
        {
            this.SelectedIndex = e.SelectedItemIndex;
            for (int i = 0; i < this.Items.Count; i++)
            {
                this.Items[i].IsActived = i == e.SelectedItemIndex;
            }
        };
        this.PART_Scroller = new ScrollView
        {
            Orientation = ScrollOrientation.Horizontal,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
            Content = this.PART_Bar = new Grid { HeightRequest = 64 }
        };
        this.Content = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
            },
            Children =
            {
                this.PART_Scroller ,
                this.PART_Content.Row(1),
            }
        };
    }

    private void OnItemsAdded(object sender, ItemsChangedEventArgs<TabItem> e)
    {
        var index = e.Index;
        var item = this.Items[index];
        item.HasIcon = this.HasIcon;
        item.HasLabel = this.hasLabel;
        if (this.ActiveIndicatorShape != Shape.None)
            item.ActiveIndicatorShape = this.ActiveIndicatorShape;
        if (this.ActiveIndicatorColor != null)
            item.ActiveIndicatorColor = this.ActiveIndicatorColor;
        item.IsActived = index == this.SelectedIndex;
        this.PART_Bar.ColumnDefinitions.Insert(index, new ColumnDefinition(GridLength.Star));
        item.Column(index);
        this.PART_Bar.Insert(index, item);
        if (item.Content != null)
        {
            this.PART_Content.Items.Insert(index, item.Content);
        }
        else
        {
            item.ContentChanged += (s, e) =>
            {
                this.PART_Content.Items.Insert(index, e);
            };
        }
        item.HasLabel = this.HasLabel;
        item.Clicked += (sender, e) => this.SelectedIndex = this.Items.IndexOf(sender as TabItem);

        if (e.EventType is "Insert")
        {
            for (int i = index + 1; i < this.PART_Bar.ColumnDefinitions.Count; i++)
            {
                this.Items[i].Column(i);
            }
        }
    }

    private void OnItemsRemoved(object sender, ItemsChangedEventArgs<TabItem> e)
    {
        var item = this.Items[e.Index];
        this.PART_Bar.Remove(item);
        this.PART_Content.Items.Remove(item.Content);
        this.PART_Bar.ColumnDefinitions.Clear();
        for (int i = 0; i < this.Items.Count; i++)
        {
            this.PART_Bar.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            this.Items[i].Column(i);
        }
    }

    private void OnItemsCleared(object sender, EventArgs e)
    {
        this.PART_Bar.Clear();
        this.PART_Content.Items.Clear();
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() => this.Items.ToList();

    public IVisualTreeElement GetVisualParent() => this.Window;
}
