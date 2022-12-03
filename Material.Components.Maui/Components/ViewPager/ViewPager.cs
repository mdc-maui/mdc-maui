using Material.Components.Maui.Core;
using System.Runtime.Versioning;
using System.Windows.Input;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class ViewPager : View, IVisualTreeElement
{
    private static readonly BindablePropertyKey ItemsPropertyKey =
       BindableProperty.CreateReadOnly(
           nameof(Items),
           typeof(ItemCollection<Page>),
           typeof(ViewPager),
           null,
           defaultValueCreator: bo =>
               new ItemCollection<Page>());

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;
    public ItemCollection<Page> Items
    {
        get => (ItemCollection<Page>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    [AutoBindable(DefaultValue = "0", OnChanged = nameof(OnSelectedIndexChanged))]
    private readonly int selectedIndex;

    [AutoBindable(OnChanged = nameof(OnSelectedItemChanged))]
    private readonly Page selectedItem;

    [AutoBindable(DefaultValue = "true")]
    private readonly bool hasAnimation;

    [SupportedOSPlatform("android")]
    public static readonly BindableProperty UserInputEnabledProperty = BindableProperty.Create(
        nameof(UserInputEnabled),
        typeof(bool),
        typeof(ViewPager),
        true);

    [SupportedOSPlatform("android")]
    public bool UserInputEnabled
    {
        get => (bool)this.GetValue(UserInputEnabledProperty);
        set => this.SetValue(UserInputEnabledProperty, value);
    }

    private void OnSelectedIndexChanged()
    {
        if (this.SelectedIndex < 0 || this.SelectedIndex >= this.Items.Count) return;
        if(this.SelectedItem != this.Items[this.SelectedIndex])
        {
            this.SelectedItem = this.Items[this.SelectedIndex];
        }
    }

    private void OnSelectedItemChanged()
    {
        if (this.SelectedItem != this.Items[this.SelectedIndex])
        {
            this.SelectedIndex = this.Items.IndexOf(this.SelectedItem);
        }
        this.SelectedItemChanged?.Invoke(this, new SelectedItemChangedEventArgs(this.SelectedItem, this.SelectedIndex));
        this.Command?.Execute(this.CommandParameter ?? this.SelectedIndex);
    }

    [AutoBindable]
    private readonly ICommand command;

    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<SelectedItemChangedEventArgs> SelectedItemChanged;

    public ViewPager() : base()
    {
        this.Items.OnAdded += this.OnItemsChanged;
        this.Items.OnRemoved += this.OnItemsChanged;
        this.Items.OnCleared += this.OnItemsCleared;
    }

    private void OnItemsChanged(object sender, ItemsChangedEventArgs<Page> e)
    {
        if (this.Handler != null)
        {
            if (e.EventType is "Add" or "Insert")
            {
                ViewPagerHandler.AddItem((ViewPagerHandler)this.Handler, e.Index, this.Items[e.Index]);
            }
            else
            {
                ViewPagerHandler.RemoveItem((ViewPagerHandler)this.Handler, e.Index);
            }
        }
    }

    private void OnItemsCleared(object sender, EventArgs e)
    {
        if (this.Handler != null)
        {
            ViewPagerHandler.ClearItems((ViewPagerHandler)this.Handler);
        }
    }

    protected override void OnHandlerChanged()
    {
        for (var i = 0; i < this.Items.Count; i++)
        {
            ViewPagerHandler.AddItem((ViewPagerHandler)this.Handler, i, this.Items[i]);
        }
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() => this.Items.ToList();

    public IVisualTreeElement GetVisualParent() => this.Window;
}
