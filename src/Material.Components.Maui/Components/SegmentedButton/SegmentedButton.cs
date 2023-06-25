using System.Collections;
using System.Collections.Specialized;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public class SegmentedButton
    : TouchGraphicView,
        IOutlineElement,
        IVisualTreeElement,
        IElement,
        IBackgroundElement,
        IShapeElement,
        IStateLayerElement,
        IRippleElement,
        IDisposable
{
    protected override void ChangeVisualState()
    {
        if (this.ViewState is ViewState.Hovered or ViewState.Pressed)
        {
            var index = (int)(this.LastTouchPoint.X / (this.Bounds.Width / this.Items.Count));
            for (var i = 0; i < this.Items.Count; i++)
            {
                var item = this.Items[i];
                if (i == index)
                    item.ViewState = this.ViewState;
                else
                    item.ViewState = ViewState.Normal;
            }
        }
        else
        {
            foreach (var item in this.Items)
                item.ViewState = this.ViewState;
        }
    }

    private static readonly BindablePropertyKey ItemsPropertyKey = BindableProperty.CreateReadOnly(
        nameof(Items),
        typeof(ItemCollection<SegmentedItem>),
        typeof(SegmentedButton),
        null,
        defaultValueCreator: bo => new ItemCollection<SegmentedItem>()
    );

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource),
        typeof(IList),
        typeof(SegmentedButton),
        default,
        propertyChanged: (bo, ov, nv) =>
        {
            if (nv != null)
            {
                if (nv is INotifyCollectionChanged ncc)
                {
                    ncc.CollectionChanged += ((SegmentedButton)bo).OnItemsSourceCollectionChanged;
                }
            }
        }
    );

    public static readonly BindableProperty MultiSelectModeProperty = BindableProperty.Create(
        nameof(MultiSelectMode),
        typeof(bool),
        typeof(SegmentedButton),
        default
    );

    public static readonly BindableProperty OutlineWidthProperty =
        IOutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineColorProperty =
        IOutlineElement.OutlineColorProperty;

    public ItemCollection<SegmentedItem> Items
    {
        get => (ItemCollection<SegmentedItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    public IList ItemsSource
    {
        get => (IList)this.GetValue(ItemsSourceProperty);
        set => this.SetValue(ItemsSourceProperty, value);
    }

    public bool MultiSelectMode
    {
        get => (bool)this.GetValue(MultiSelectModeProperty);
        set => this.SetValue(MultiSelectModeProperty, value);
    }

    public Color OutlineColor
    {
        get => (Color)this.GetValue(OutlineColorProperty);
        set => this.SetValue(OutlineColorProperty, value);
    }

    public int OutlineWidth
    {
        get => (int)this.GetValue(OutlineWidthProperty);
        set => this.SetValue(OutlineWidthProperty, value);
    }

    public SegmentedButton()
    {
        this.EndInteraction += this.OnEndInteraction;
        this.MoveHoverInteraction += this.OnMoveHoverInteraction;
        this.Items.CollectionChanged += this.OnItemsCollectionChanged;
        this.Drawable = new SegmentedButtonDrawable(this);
    }

    private void OnEndInteraction(object sender, TouchEventArgs e)
    {
        var index = (int)(this.LastTouchPoint.X / (this.Bounds.Width / this.Items.Count));
        this.Items[index].IsSelected = !this.Items[index].IsSelected;
    }

    private void OnMoveHoverInteraction(object sender, TouchEventArgs e)
    {
        this.LastTouchPoint = e.Touches[0];
        this.ChangeVisualState();
    }

    private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (SegmentedItem item in e.NewItems)
            {
                item.Parent = this;
            }
        }
    }

    private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            var index = e.NewStartingIndex;
            foreach (string item in e.NewItems)
            {
                this.Items.Insert(index, new SegmentedItem { Text = item });
                index++;
            }
        }

        if (e.OldItems != null)
        {
            var index = e.OldStartingIndex;
            foreach (string item in e.OldItems)
            {
                this.Items.RemoveAt(index);
                index++;
            }
        }
    }

    internal void OnSelectedItemChanged(SegmentedItem item)
    {
        if (item.IsSelected && !this.MultiSelectMode)
        {
            foreach (var _item in this.Items)
                if (!_item.Equals(item))
                {
                    _item.IsSelected = false;
                }
        }
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        var maxWidth = Math.Min(
            Math.Min(widthConstraint, this.MaximumWidthRequest),
            this.WidthRequest != -1 ? this.WidthRequest : double.PositiveInfinity
        );
        var maxHeight = Math.Min(
            Math.Min(heightConstraint, this.MaximumHeightRequest),
            this.HeightRequest != -1 ? this.HeightRequest : double.PositiveInfinity
        );

        var scale =
            (this.HeightRequest != -1 ? this.HeightRequest : Math.Min(40f, maxHeight)) / 40f;

        var maxItemWidth = 0d;
        var needHeight = 40f * scale;

        foreach (var item in this.Items)
        {
            var iconSize = 18f * scale;
            var textSize = item.GetStringSize();
            //16 + iconSize + 8 + textSize.Width + 16
            if (textSize == Size.Zero)
                maxItemWidth = Math.Max(
                    maxItemWidth,
                    iconSize + iconSize + (16f + 8f + 16f) * scale
                );
            else
                maxItemWidth = Math.Max(
                    maxItemWidth,
                    iconSize + 16f + textSize.Width + (8f + 16f) * scale
                );
        }

        var width =
            this.HorizontalOptions.Alignment == LayoutAlignment.Fill
                ? maxWidth
                : this.Margin.HorizontalThickness
                    + Math.Max(
                        this.MinimumWidthRequest,
                        this.WidthRequest == -1
                            ? Math.Min(maxWidth, maxItemWidth * this.Items.Count)
                            : this.WidthRequest
                    );
        var height =
            this.VerticalOptions.Alignment == LayoutAlignment.Fill
                ? maxHeight
                : this.Margin.VerticalThickness
                    + Math.Max(
                        this.MinimumHeightRequest,
                        this.HeightRequest == -1
                            ? Math.Min(maxHeight, needHeight)
                            : this.HeightRequest
                    );

        this.DesiredSize = new Size(Math.Ceiling(width), Math.Ceiling(height));
        return this.DesiredSize;
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() => this.Items.ToList();

    public IVisualTreeElement GetVisualParent() => null;

    protected override void Dispose(bool disposing)
    {
        if (!this.disposedValue && disposing)
        {
            this.EndInteraction -= this.OnEndInteraction;
            this.MoveHoverInteraction -= this.OnMoveHoverInteraction;
            this.Items.CollectionChanged -= this.OnItemsCollectionChanged;
            if (this.ItemsSource is INotifyCollectionChanged ncc)
            {
                ncc.CollectionChanged -= this.OnItemsSourceCollectionChanged;
            }
            ((IIconElement)this).IconPath?.Dispose();
        }
        base.Dispose(disposing);
    }
}
