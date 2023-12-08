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

file class UniformStackLayoutManager(AutoFillLayout layout) : LayoutManager(layout)
{
    private readonly List<Size> childrenSizes = [];

    public override Size Measure(double widthConstraint, double heightConstraint)
    {
        this.childrenSizes.Clear();

        var childrenWidth = Math.Ceiling(widthConstraint / layout.Children.Count);
        for (var i = 0; i < layout.Children.Count; i++)
        {
            this.childrenSizes.Add(new Size(childrenWidth, heightConstraint));
            layout.Children[i].Measure(childrenWidth, heightConstraint);
        }

        return new Size(widthConstraint, heightConstraint);
    }

    public override Size ArrangeChildren(Rect bounds)
    {
        var x = 0d;
        var index = 0;
        foreach (var item in layout.Children)
        {
            var size = this.childrenSizes[index];
            item.Arrange(new Rect(x, 0d, size.Width, size.Height));
            x += size.Width;
            index++;
        }

        return new Size(bounds.Width, bounds.Height);
    }
}
