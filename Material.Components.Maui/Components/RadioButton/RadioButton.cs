using System.Windows.Input;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class RadioButton : WrapLayout, IVisualTreeElement, ICommandElement
{
    private static readonly BindablePropertyKey ItemsPropertyKey = BindableProperty.CreateReadOnly(
        nameof(Items),
        typeof(ItemCollection<RadioButtonItem>),
        typeof(RadioButton),
        null,
        defaultValueCreator: bo => new ItemCollection<RadioButtonItem>()
    );

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;

    public ItemCollection<RadioButtonItem> Items
    {
        get => (ItemCollection<RadioButtonItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    [AutoBindable(DefaultValue = "-1", OnChanged = nameof(OnSelectedIndexChanged))]
    private readonly int selectedIndex;

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

    public RadioButton()
    {
        this.Items.OnAdded += this.OnItemsAdded;
        this.Items.OnRemoved += this.OnItemsRemoved;
        this.Items.OnCleared += this.OnItemsCleared;
        this.HorizontalOptions = LayoutOptions.Start;
        this.VerticalOptions = LayoutOptions.Start;
    }

    private void OnItemsAdded(object sender, ItemsChangedEventArgs<RadioButtonItem> e)
    {
        var index = e.Index;
        var item = this.Items[index];
        this.Insert(index, item);
        item.IsSelected = e.Index == this.SelectedIndex;
        item.Clicked += (sender, e) =>
            this.SelectedIndex = this.Items.IndexOf(sender as RadioButtonItem);
    }

    private void OnItemsRemoved(object sender, ItemsChangedEventArgs<RadioButtonItem> e)
    {
        this.RemoveAt(e.Index);
    }

    private void OnItemsCleared(object sender, EventArgs e)
    {
        this.Clear();
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() => this.Items?.ToList();

    public IVisualTreeElement GetVisualParent() => this.Window.Parent;
}
