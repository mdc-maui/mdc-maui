using Microsoft.Maui.Layouts;
using ILayout = Microsoft.Maui.ILayout;

namespace Material.Components.Maui;

public partial class WrapLayout : Layout
{
    [AutoBindable(
        DefaultValue = "StackOrientation.Horizontal",
        OnChanged = nameof(OnOrientationChanged)
    )]
    private readonly StackOrientation orientation;

    [AutoBindable(DefaultValue = "0d", OnChanged = nameof(OnSpacingChanged))]
    private readonly double spacing;

    private void OnOrientationChanged() => this.InvalidateMeasure();

    private void OnSpacingChanged() => this.InvalidateMeasure();

    protected override ILayoutManager CreateLayoutManager()
    {
        return new WrapLayoutManager(this);
    }

}

public class WrapLayoutManager : LayoutManager
{
    private readonly WrapLayout layout;

    public WrapLayoutManager(WrapLayout layout) : base(layout)
    {
        this.layout = layout;
    }

    public override Size Measure(double widthConstraint, double heightConstraint)
    {
        widthConstraint =
            Math.Min(
                Math.Min(widthConstraint, layout.MaximumWidthRequest),
                this.layout.WidthRequest != -1 ? this.layout.WidthRequest : double.PositiveInfinity
            ) - this.layout.Padding.HorizontalThickness;
        heightConstraint =
            Math.Min(
                Math.Min(heightConstraint, this.layout.MaximumHeightRequest),
                this.layout.HeightRequest != -1
                    ? this.layout.HeightRequest
                    : double.PositiveInfinity
            ) - this.layout.Padding.VerticalThickness;

        Size result;
        if (this.layout.Orientation == StackOrientation.Horizontal)
            result = HorizontalMeasure(widthConstraint, heightConstraint);
        else
            result = VerticalMeasure(widthConstraint, heightConstraint);
        result.Width += this.layout.Padding.HorizontalThickness;
        result.Height += this.layout.Padding.VerticalThickness;
        return result;
    }

    private SizeRequest HorizontalMeasure(double widthConstraint, double heightConstraint)
    {
        var width = 0d;
        var height = 0d;
        var rowWidth = 0d;
        var rowHeight = 0d;
        foreach (var item in this.layout.Children)
        {
            var size = item.Measure(widthConstraint, double.PositiveInfinity);
            if (size.Width + rowWidth > widthConstraint)
            {
                width = Math.Max(rowWidth, size.Width);
                height += rowHeight + this.layout.Spacing;
                rowWidth = 0d;
                rowHeight = 0d;
            }
            rowWidth += size.Width + this.layout.Spacing;
            rowHeight = Math.Max(rowHeight, size.Height);
        }
        width = Math.Max(rowWidth, width) - this.layout.Spacing;
        height = Math.Min(heightConstraint, height + rowHeight);
        return new Size(width, height);
    }

    private SizeRequest VerticalMeasure(double widthConstraint, double heightConstraint)
    {
        var width = 0d;
        var height = 0d;
        var columnWidth = 0d;
        var columnHeight = 0d;
        foreach (var item in layout.Children)
        {
            var size = item.Measure(double.PositiveInfinity, heightConstraint);
            if (size.Height + columnHeight > heightConstraint)
            {
                width += columnWidth + this.layout.Spacing;
                height = Math.Max(columnHeight, size.Height);
                columnWidth = 0d;
                columnHeight = 0d;
            }
            columnWidth = Math.Max(columnWidth, size.Width);
            columnHeight += size.Height + this.layout.Spacing;
        }
        width = Math.Min(widthConstraint, width + columnWidth);
        height = Math.Max(columnHeight, height) - this.layout.Spacing;
        return new Size(width, height);
    }

    public override Size ArrangeChildren(Rect bounds)
    {
        bounds.Left += this.layout.Padding.Left;
        bounds.Top += this.layout.Padding.Top;
        bounds.Right -= this.layout.Padding.Right;
        bounds.Bottom -= this.layout.Padding.Bottom;

        Size result;
        if (this.layout.Orientation == StackOrientation.Horizontal)
            result = HorizontalArrangeChildren(bounds.Left, bounds.Top, bounds.Right);
        else
            result = VerticalArrangeChildren(bounds.Left, bounds.Top, bounds.Bottom);

        result.Width += this.layout.Padding.HorizontalThickness;
        result.Height += this.layout.Padding.VerticalThickness;
        return result;
    }

    private Size HorizontalArrangeChildren(double left, double top, double right)
    {
        var row = 0;
        var xPos = left;
        var yPos = top;
        var maxWidth = 0d;
        var rowHeight = 0d;

        var rowHeights = new List<double>();
        var rowRects = new List<List<Rect>>() { new List<Rect>() };
        foreach (var item in this.layout.Children)
        {
            var size = item.DesiredSize.IsZero
                ? item.Measure(right - left, double.PositiveInfinity)
                : item.DesiredSize;
            if (size.Width + xPos > right)
            {
                maxWidth = Math.Max(xPos - this.layout.Spacing, maxWidth);
                row++;
                xPos = left;
                yPos += rowHeight + this.layout.Spacing;
                rowRects.Add(new List<Rect>());
                rowHeights.Add(rowHeight);
                rowHeight = 0d;
            }
            var bounds = new Rect(xPos, yPos, size.Width, size.Height);
            rowRects[row].Add(bounds);
            xPos += size.Width + this.layout.Spacing;
            rowHeight = Math.Max(size.Height, rowHeight);
        }
        rowHeights.Add(rowHeight);

        var rects = new List<Rect>();
        for (int i = 0; i < rowRects.Count; i++)
        {
            var height = rowHeights[i];
            for (int j = 0; j < rowRects[i].Count; j++)
            {
                var _bounds = rowRects[i][j];
                _bounds.Height = height;
                rects.Add(_bounds);
            }
        }

        var index = 0;
        foreach (var item in this.layout.Children)
        {
            item.Arrange(rects[index]);
            index++;
        }

        return new Size(maxWidth, yPos + rowHeight);
    }

    private Size VerticalArrangeChildren(double left, double top, double bottom)
    {
        var xPos = left;
        var yPos = top;
        var columnWidth = 0d;
        var maxHeight = 0d;
        foreach (var item in layout.Children)
        {
            var size = item.DesiredSize.IsZero
               ? item.Measure(double.PositiveInfinity, bottom - top)
               : item.DesiredSize;
            if (size.Height + yPos > bottom)
            {
                maxHeight = Math.Max(yPos - this.layout.Spacing, maxHeight);
                xPos += columnWidth + this.layout.Spacing;
                yPos = top;
                columnWidth = 0d;
            }
            var bounds = new Rect(xPos, yPos, size.Width, size.Height);
            item.Arrange(bounds);
            yPos += size.Height + this.layout.Spacing;
            columnWidth = Math.Max(size.Width, columnWidth);
        }
        return new Size(xPos + columnWidth, maxHeight);
    }
}
