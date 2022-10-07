using CommunityToolkit.Maui.Markup;
using Material.Components.Maui.Core.NavigationBar;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class NavigationBar : ContentView
{
    private static readonly BindablePropertyKey ItemsPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(Items),
            typeof(NavigationItemCollection),
            typeof(NavigationBar),
            null,
            defaultValueCreator: bo =>
                new NavigationItemCollection { Inner = ((NavigationBar)bo).PART_Bar });

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;

    public NavigationItemCollection Items
    {
        get => (NavigationItemCollection)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    [AutoBindable(DefaultValue = "true", OnChanged = nameof(OnHasLabelChanged))]
    private readonly bool hasLabel;

    [AutoBindable(OnChanged = nameof(OnPositionChanged))]
    private readonly int position;

    private void OnHasLabelChanged()
    {
        this.PART_Bar.HeightRequest = this.HasLabel ? 80 : 65;
        foreach (var item in this.Items)
        {
            item.HasLabel = this.HasLabel;
        }
    }

    private void OnPositionChanged()
    {
        this.Items.ChangePosition(this.Position);
    }

    private readonly Grid PART_Content;
    private readonly Grid PART_Bar;

    public NavigationBar()
    {
        this.PART_Content = new Grid();
        this.PART_Bar = new Grid { HeightRequest = 80 };
        this.PART_Bar.ChildAdded += this.OnItemsAdded;
        this.PART_Bar.ChildRemoved += this.OnItemsRemoved;

        this.Content = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto),
            },
            Children =
            {
                this.PART_Content,
                this.PART_Bar.Row(1)
            }
        };


    }

    private void OnItemsAdded(object sender, ElementEventArgs e)
    {
        if (e.Element is NavigationItem item)
        {
            this.PART_Content.Add(item.Content);
            item.HasLabel = this.HasLabel;
            item.Clicked += (sender, e) =>
            {
                this.Position = this.Items.IndexOf((NavigationItem)sender);
            };
        }
    }

    private void OnItemsRemoved(object sender, ElementEventArgs e)
    {
        if (e.Element is NavigationItem item)
        {
            this.PART_Content.Remove(item.Content);
            this.PART_Bar.ColumnDefinitions.Clear();
            for (int i = 0; i < this.Items.Count; i++)
            {
                this.PART_Bar.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                this.Items[i].SetValue(Grid.ColumnProperty, i);
            }
        }
    }
}