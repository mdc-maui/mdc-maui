using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class ContextMenu
    : TemplatedView,
        IItemsElement<MenuItem>,
        IElement,
        IBackgroundElement,
        IShapeElement,
        IElevationElement,
        IVisualTreeElement,
        IDisposable
{
    ViewState IElement.ViewState => ViewState.Normal;

    void IElement.OnPropertyChanged() { }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if (this.PART_Root != null)
        {
            SetInheritedBindingContext(this.PART_Root, this.BindingContext);
        }
    }

    public static readonly BindableProperty ItemsProperty = IItemsElement<MenuItem>.ItemsProperty;

    public static readonly BindableProperty ItemsSourceProperty =
        IItemsElement<MenuItem>.ItemsSourceProperty;

    public static readonly new BindableProperty IsEnabledProperty = IElement.IsEnabledProperty;
    public static new readonly BindableProperty BackgroundColorProperty =
        IBackgroundElement.BackgroundColorProperty;
    public static readonly BindableProperty ShapeProperty = IShapeElement.ShapeProperty;
    public static readonly BindableProperty ElevationProperty = IElevationElement.ElevationProperty;

    public ItemCollection<MenuItem> Items
    {
        get => (ItemCollection<MenuItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    public IList ItemsSource
    {
        get => (IList)this.GetValue(ItemsSourceProperty);
        set => this.SetValue(ItemsSourceProperty, value);
    }

    public new Color BackgroundColor
    {
        get => (Color)this.GetValue(BackgroundColorProperty);
        set => this.SetValue(BackgroundColorProperty, value);
    }

    [TypeConverter(typeof(ShapeConverter))]
    public Shape Shape
    {
        get => (Shape)this.GetValue(ShapeProperty);
        set => this.SetValue(ShapeProperty, value);
    }

    public Elevation Elevation
    {
        get => (Elevation)this.GetValue(ElevationProperty);
        set => this.SetValue(ElevationProperty, value);
    }

    void IItemsElement<MenuItem>.OnItemsCollectionChanged(
        object sender,
        NotifyCollectionChangedEventArgs e
    )
    {
        if (e.OldItems != null)
        {
            foreach (MenuItem item in e.OldItems)
            {
                item.Clicked -= this.OnMenuItemClicked;
                this.PART_Stack.Remove(item);
            }
        }

        if (e.NewItems != null)
        {
            var index = e.NewStartingIndex;
            foreach (MenuItem item in e.NewItems)
            {
                this.PART_Stack.Insert(index, item);
                item.Clicked += this.OnMenuItemClicked;
            }
        }
    }

    private void OnMenuItemClicked(object sender, TouchEventArgs e)
    {
        this.Close(this.Items.IndexOf(sender as MenuItem));
    }

    void IItemsElement<MenuItem>.OnItemsSourceCollectionChanged(
        object sender,
        NotifyCollectionChangedEventArgs e
    )
    {
        if (e.OldItems != null)
        {
            var index = e.OldStartingIndex;
            foreach (string item in e.OldItems)
            {
                this.Items.RemoveAt(index);
                index++;
            }
        }

        if (e.NewItems != null)
        {
            var index = e.NewStartingIndex;
            foreach (string item in e.NewItems)
            {
                this.Items.Insert(index, new MenuItem { Text = item });
                index++;
            }
        }
    }

    public object Result { get; private set; }

    public EventHandler<object> Closed;

    private void OnClosed(object sender, object e)
    {
        this.Closed?.Invoke(sender, this.Result ?? e);
    }

    private Grid PART_Root;
    private VerticalStackLayout PART_Stack;

    public ContextMenu() { }

    public void Show(View anchor)
    {
        if (this.DesiredSize == Size.Zero)
            this.MeasureOverride(double.PositiveInfinity, double.PositiveInfinity);

        this.PlatformShow(anchor);
    }

    public void Show(View anchor, Point location)
    {
        if (this.DesiredSize == Size.Zero)
            this.MeasureOverride(double.PositiveInfinity, double.PositiveInfinity);

        this.PlatformShow(anchor, location);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        this.PART_Root = (Grid)this.GetTemplateChild("PART_Root");
        this.PART_Stack = (VerticalStackLayout)this.GetTemplateChild("PART_Stack");

        this.OnChildAdded(this.PART_Root);
        VisualDiagnostics.OnChildAdded(this, this.PART_Root);
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        var result = new Size(this.WidthRequest, this.HeightRequest);
        if (this.WidthRequest == -1d)
        {
            var maxWidth = 0d;
            foreach (var item in this.Items)
            {
                var (w, h) = item.GetDesiredSize();
                maxWidth = Math.Max(maxWidth, w);
            }
            result.Width = maxWidth;
        }

        if (this.HeightRequest == -1d)
            result.Height = this.Items.Count * 48d + 16d;

        this.DesiredSize = result;
        return result;
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.PART_Root != null
            ? new List<IVisualTreeElement> { this.PART_Root }
            : Array.Empty<IVisualTreeElement>().ToList();

    public IVisualTreeElement GetVisualParent() => null;

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                foreach (var item in this.Items)
                {
                    item.Clicked -= this.OnMenuItemClicked;
                }
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

#if !WINDOWS && !__ANDROID__
    private void PlatformShow(View anchor)
    {
        throw new NotImplementedException();
    }

    private void PlatformShow(View anchor, Point location)
    {
        throw new NotImplementedException();
    }

    public void Close(object result = null)
    {
        throw new NotImplementedException();
    }
#endif
}
