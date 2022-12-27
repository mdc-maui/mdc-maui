namespace Material.Components.Maui;

internal class ContentPresenter : Microsoft.Maui.Controls.ContentPresenter, IVisualTreeElement
{
    public static new readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content),
        typeof(View),
        typeof(ContentPresenter),
        null,
        propertyChanged: (bo, oldValue, newValue) =>
            ((ContentPresenter)bo).OnContentChanged((View)oldValue, (View)newValue)
    );

    public new View Content
    {
        get => (View)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    private void OnContentChanged(View oldValue, View newValue)
    {
        if (oldValue != null)
        {
            this.OnChildRemoved(oldValue, 0);
            VisualDiagnostics.OnChildRemoved(this, oldValue, 0);
        }
        if (newValue != null)
        {
            this.OnChildAdded(newValue);
            VisualDiagnostics.OnChildAdded(this, newValue);
        }
        base.Content = newValue;
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.Content != null
            ? new List<IVisualTreeElement> { this.Content }
            : Array.Empty<IVisualTreeElement>().ToList();

    public IVisualTreeElement GetVisualParent() => this.Window.Parent;
}
