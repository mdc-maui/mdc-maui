using System.Windows.Input;

namespace Material.Components.Maui;

[ContentProperty(nameof(Pane))]
public partial class SplitView : View, IVisualTreeElement, ICommandElement
{
    [AutoBindable]
    private readonly DrawerDisplayMode displayMode;

    [AutoBindable]
    private readonly Page pane;

    [AutoBindable(OnChanged = nameof(OnContentChanged))]
    private readonly Page content;

    [AutoBindable(DefaultValue = "true")]
    private readonly bool isPaneOpen;

    [AutoBindable]
    private readonly ICommand command;

    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<ValueChangedEventArgs> ContentChanged;

    private void OnContentChanged()
    {
        this.ContentChanged?.Invoke(this, new ValueChangedEventArgs(this.Content));
        this.Command?.Execute(this.CommandParameter ?? this.Content);
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        new List<IVisualTreeElement> { this.Pane, this.Content };

    public IVisualTreeElement GetVisualParent() => this.Window;
}
