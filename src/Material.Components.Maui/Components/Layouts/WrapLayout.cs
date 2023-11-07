using Microsoft.Maui.Layouts;
using MIView = Microsoft.Maui.IView;

namespace Material.Components.Maui;

public partial class WrapLayout : Microsoft.Maui.Controls.Layout, IItemsLayout
{

    public static readonly BindableProperty OrientationProperty = BindableProperty.Create(
        nameof(Orientation),
        typeof(StackOrientation),
        typeof(WrapLayout),
        StackOrientation.Horizontal,
        propertyChanged: (bo, ov, nv) => ((WrapLayout)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty HorizontalSpacingProperty = BindableProperty.Create(
        nameof(HorizontalSpacing),
        typeof(double),
        typeof(WrapLayout),
        0d,
        propertyChanged: (bo, ov, nv) => ((WrapLayout)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty VerticalSpacingProperty = BindableProperty.Create(
        nameof(VerticalSpacing),
        typeof(double),
        typeof(WrapLayout),
        0d,
        propertyChanged: (bo, ov, nv) => ((WrapLayout)bo).OnPropertyChanged()
    );



    public StackOrientation Orientation
    {
        get => (StackOrientation)this.GetValue(OrientationProperty);
        set => this.SetValue(OrientationProperty, value);
    }

    public double HorizontalSpacing
    {
        get => (double)this.GetValue(HorizontalSpacingProperty);
        set => this.SetValue(HorizontalSpacingProperty, value);
    }

    public double VerticalSpacing
    {
        get => (double)this.GetValue(VerticalSpacingProperty);
        set => this.SetValue(VerticalSpacingProperty, value);
    }


    private void OnPropertyChanged()
    {
        if (this.Handler != null)
            this.InvalidateMeasure();
    }

    protected override ILayoutManager CreateLayoutManager()
    {
        return new WrapLayoutManager(this);
    }
}

file class WrapLayoutManager(WrapLayout layout) : LayoutManager(layout)
{
    private readonly List<Rect> childrenBounds = [];

    public override Size Measure(double widthConstraint, double heightConstraint)
    {
        this.childrenBounds.Clear();

        widthConstraint =
            Math.Min(
                Math.Min(widthConstraint, layout.MaximumWidthRequest),
                layout.WidthRequest != -1 ? layout.WidthRequest : double.PositiveInfinity
            ) - layout.Padding.HorizontalThickness;
        heightConstraint =
            Math.Min(
                Math.Min(heightConstraint, layout.MaximumHeightRequest),
                layout.HeightRequest != -1
                    ? layout.HeightRequest
                    : double.PositiveInfinity
            ) - layout.Padding.VerticalThickness;

        var result = layout.Orientation == StackOrientation.Horizontal
            ? (Size)this.HorizontalMeasure(widthConstraint, heightConstraint)
            : (Size)this.VerticalMeasure(widthConstraint, heightConstraint);

        result.Width += layout.Padding.HorizontalThickness;
        result.Height += layout.Padding.VerticalThickness;
        return result;
    }

    private SizeRequest HorizontalMeasure(double widthConstraint, double heightConstraint)
    {
        var width = 0d;
        var height = 0d;
        var rowWidth = 0d;
        var rowHeight = 0d;
        var rowViews = new List<MIView>();

        foreach (var item in layout.Children)
        {
            var size = item.Measure(widthConstraint, heightConstraint);
            if (rowWidth + size.Width > widthConstraint)
            {
                this.UpdateHorizontalChildrenBounds(height, rowHeight, rowViews);
                rowViews.Clear();

                width = Math.Max(width, rowWidth);
                height += rowHeight + layout.VerticalSpacing;

                rowWidth = 0d;
                rowHeight = 0d;
            }

            rowWidth += size.Width + layout.HorizontalSpacing;
            rowHeight = Math.Max(rowHeight, size.Height);
            rowViews.Add(item);
        }

        this.UpdateHorizontalChildrenBounds(height, rowHeight, rowViews);
        width = Math.Max(rowWidth, width) - layout.HorizontalSpacing;
        height = Math.Min(heightConstraint, height + rowHeight);
        return new Size(width, height);
    }

    private void UpdateHorizontalChildrenBounds(double y, double height, List<MIView> views)
    {
        var x = this.Layout.Padding.Left;
        y += this.Layout.Padding.Top;
        foreach (var view in views)
        {
            this.childrenBounds.Add(new Rect(x, y, view.DesiredSize.Width, height));
            x += view.DesiredSize.Width + layout.HorizontalSpacing;
        }
    }

    private SizeRequest VerticalMeasure(double widthConstraint, double heightConstraint)
    {
        var width = 0d;
        var height = 0d;
        var columnWidth = 0d;
        var columnHeight = 0d;
        var columnViews = new List<MIView>();

        foreach (var item in layout.Children)
        {
            var size = item.Measure(widthConstraint, heightConstraint);
            if (size.Height + columnHeight - layout.VerticalSpacing > heightConstraint)
            {
                this.UpdateVerticalChildrenBounds(width, columnWidth, columnViews);
                columnViews.Clear();

                width += columnWidth + layout.HorizontalSpacing;
                height = Math.Max(columnHeight, size.Height);
                columnWidth = 0d;
                columnHeight = 0d;
            }
            columnWidth = Math.Max(columnWidth, size.Width);
            columnHeight += size.Height + layout.VerticalSpacing;
            columnViews.Add(item);
        }
        this.UpdateVerticalChildrenBounds(height, columnWidth, columnViews);

        width = Math.Min(widthConstraint, width + columnWidth);
        height = Math.Max(columnHeight, height) - layout.VerticalSpacing;
        return new Size(width, height);
    }

    private void UpdateVerticalChildrenBounds(double x, double width, List<MIView> views)
    {
        x += this.Layout.Padding.Left;
        var y = this.Layout.Padding.Top;
        foreach (var view in views)
        {
            this.childrenBounds.Add(new Rect(x, y, width, view.DesiredSize.Height));
            y += view.DesiredSize.Height + layout.VerticalSpacing;
        }
    }

    public override Size ArrangeChildren(Rect bounds)
    {
        if (layout.Orientation == StackOrientation.Horizontal)
            this.HorizontalArrangeChildren();
        else
            this.VerticalArrangeChildren();

        return new Size(bounds.Width, bounds.Height);
    }

    private void HorizontalArrangeChildren()
    {
        var index = 0;
        foreach (var item in layout.Children)
        {
            var bounds = this.childrenBounds[index];
            item.Arrange(bounds);
            index++;
        }
    }

    private void VerticalArrangeChildren()
    {
        var index = 0;
        foreach (var item in layout.Children)
        {
            var bounds = this.childrenBounds[index];
            item.Arrange(bounds);
            index++;
        }
    }
}
