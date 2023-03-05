using System.Windows.Input;

namespace Material.Components.Maui;

[ContentProperty(nameof(Pane))]
public partial class SplitView : View, ICommandElement, IVisualTreeElement
{
    [AutoBindable]
    private readonly DrawerDisplayMode displayMode;

    [AutoBindable(OnChanged = nameof(OnPaneChanged))]
    private readonly View pane;

    [AutoBindable(OnChanged = nameof(OnContentChanged))]
    private readonly View content;

    [AutoBindable(DefaultValue = "true")]
    private readonly bool isPaneOpen;

    [AutoBindable]
    private readonly ICommand command;

    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<ValueChangedEventArgs> ContentChanged;

    private void OnPaneChanged()
    {
        this.OnChildAdded(this.Pane);
        VisualDiagnostics.OnChildAdded(this, this.Pane, 0);

        if (this.BindingContext != null)
        {
            SetInheritedBindingContext(this.Pane, this.BindingContext);
        }
    }

    private void OnContentChanged(View oldValue, View newValue)
    {
        if (oldValue != null)
        {
            this.OnChildRemoved(oldValue, 1);
            VisualDiagnostics.OnChildRemoved(this, oldValue, 1);
        }

        if (newValue != null)
        {
            this.OnChildAdded(newValue);
            VisualDiagnostics.OnChildAdded(this, newValue, 1);
            if (this.BindingContext != null)
            {
                SetInheritedBindingContext(newValue, this.BindingContext);
            }
        }

        this.ContentChanged?.Invoke(this, new ValueChangedEventArgs(this.Content));
        this.Command?.Execute(this.CommandParameter ?? this.Content);
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren()
    {
        var result = new List<IVisualTreeElement> { this.Pane };
        if (this.Content != null)
        {
            result.Add(this.Content);
        }
        return result;
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        SetInheritedBindingContext(this.Pane, this.BindingContext);
        SetInheritedBindingContext(this.Content, this.BindingContext);
    }

    public IVisualTreeElement GetVisualParent() => null;
}
