using CommunityToolkit.Maui.Markup;
using Material.Components.Maui.Core;
using Material.Components.Maui.Core.Interfaces;
using System.Runtime.Versioning;
using System.Windows.Input;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class NavigationBar : ContentView, IVisualTreeElement, ICommandElement
{
    private static readonly BindablePropertyKey ItemsPropertyKey = BindableProperty.CreateReadOnly(
        nameof(Items),
        typeof(ItemCollection<NavigationBarItem>),
        typeof(NavigationBar),
        null,
        defaultValueCreator: bo => new ItemCollection<NavigationBarItem>()
    );

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;

    public ItemCollection<NavigationBarItem> Items
    {
        get => (ItemCollection<NavigationBarItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    [AutoBindable(DefaultValue = "true", OnChanged = nameof(OnHasLabelChanged))]
    private readonly bool hasLabel;

    [AutoBindable(OnChanged = nameof(OnSelectedIndexChanged))]
    private readonly int selectedIndex;

    [SupportedOSPlatform("android")]
    public static readonly BindableProperty UserInputEnabledProperty = BindableProperty.Create(
        nameof(UserInputEnabled),
        typeof(bool),
        typeof(NavigationBar),
        false,
        propertyChanged: OnUserInputEnabledChanged
    );

    [SupportedOSPlatform("android")]
    public bool UserInputEnabled
    {
        get => (bool)this.GetValue(UserInputEnabledProperty);
        set => this.SetValue(UserInputEnabledProperty, value);
    }

    [SupportedOSPlatform("android")]
    private static void OnUserInputEnabledChanged(
        BindableObject bo,
        object oldValue,
        object NewValue
    )
    {
        ((NavigationBar)bo).PART_Content.UserInputEnabled = (bool)NewValue;
    }

    [AutoBindable]
    private readonly ICommand command;

    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<SelectedIndexChangedEventArgs> SelectedIndexChanged;

    private void OnSelectedIndexChanged()
    {
        for (int i = 0; i < this.Items.Count; i++)
        {
            this.Items[i].IsActived = i == this.SelectedIndex;
        }
        this.PART_Content.SelectedIndex = this.SelectedIndex;
        SelectedIndexChanged?.Invoke(this, new SelectedIndexChangedEventArgs(this.SelectedIndex));
        this.Command?.Execute(this.CommandParameter ?? this.SelectedIndex);
    }

    private void OnHasLabelChanged()
    {
        this.PART_Bar.HeightRequest = this.HasLabel ? 80 : 65;
        foreach (var item in this.Items)
        {
            item.HasLabel = this.HasLabel;
        }
    }

    private readonly ViewPager PART_Content;
    private readonly Grid PART_Bar;

    public NavigationBar()
    {
        this.Items.OnAdded += this.OnItemsAdded;
        this.Items.OnRemoved += this.OnItemsRemoved;
        this.Items.OnCleared += this.OnItemsCleared;

        this.PART_Content = new ViewPager();
        this.PART_Content.SelectedItemChanged += (sender, e) =>
        {
            this.SetValue(SelectedIndexProperty, e.SelectedItemIndex);
            for (int i = 0; i < this.Items.Count; i++)
            {
                this.Items[i].IsActived = i == e.SelectedItemIndex;
            }
        };
        this.PART_Bar = new Grid { HeightRequest = 80, BackgroundColor = Colors.Green };

        this.Content = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto),
            },
            Children = { this.PART_Content, this.PART_Bar.Row(1) }
        };
    }

    private void OnItemsAdded(object sender, ItemsChangedEventArgs<NavigationBarItem> e)
    {
        var index = e.Index;
        var item = this.Items[index];
        item.IsActived = index == this.SelectedIndex;
        this.PART_Bar.ColumnDefinitions.Insert(index, new ColumnDefinition(GridLength.Star));
        item.Column(index);
        this.PART_Bar.Insert(index, item);
        this.PART_Content.Items.Insert(index, item.Content);
        item.HasLabel = this.HasLabel;
        item.Clicked += (sender, e) =>
        {
            this.SelectedIndex = this.Items.IndexOf((NavigationBarItem)sender);
        };

        if (e.EventType is "Insert")
        {
            for (int i = index + 1; i < this.PART_Bar.ColumnDefinitions.Count; i++)
            {
                this.Items[i].Column(i);
            }
        }
    }

    private void OnItemsRemoved(object sender, ItemsChangedEventArgs<NavigationBarItem> e)
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
