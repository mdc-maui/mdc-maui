using Microsoft.Maui.Layouts;

namespace Material.Components.Maui;

/// <summary>
///  For internal use by Material.Components.
/// </summary>
internal class AutoFillLayout : Layout, IItemsLayout
{
    protected override ILayoutManager CreateLayoutManager()
    {
        return new UniformStackLayoutManager(this);
    }
}

file class UniformStackLayoutManager : LayoutManager
{
    private readonly AutoFillLayout layout;
    private readonly List<Size> childrenSizes = new();

    public UniformStackLayoutManager(AutoFillLayout layout) : base(layout)
    {
        this.layout = layout;
    }

    public override Size Measure(double widthConstraint, double heightConstraint)
    {
        this.childrenSizes.Clear();

        var maxWidth = 0d;
        var maxHeight = 64d;

        foreach (var item in this.layout.Children)
        {
            var size = item.Measure(widthConstraint, heightConstraint);
            maxWidth += size.Width;
            maxHeight = Math.Max(maxHeight, size.Height);
            this.childrenSizes.Add(size);
        }

        maxWidth =
            widthConstraint != double.PositiveInfinity
                ? Math.Max(maxWidth, widthConstraint)
                : maxWidth;
        var childrenWidth = Math.Ceiling(maxWidth / this.layout.Children.Count);
        for (var i = 0; i < this.childrenSizes.Count; i++)
        {
            this.childrenSizes[i] = new Size(childrenWidth, maxHeight);
        }

        return new Size(maxWidth, maxHeight);
    }

    public override Size ArrangeChildren(Rect bounds)
    {
        var x = 0d;
        var index = 0;
        foreach (var item in this.layout.Children)
        {
            var size = this.childrenSizes[index];
            item.Arrange(new Rect(x, 0d, size.Width, size.Height));
            x += size.Width;
            index++;
        }
        return new Size(bounds.Width, bounds.Height);
    }
}
