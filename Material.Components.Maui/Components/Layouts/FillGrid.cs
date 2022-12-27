using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material.Components.Maui;

public class FillGrid : Grid
{
    protected override ILayoutManager CreateLayoutManager()
    {
        return new FillGridLayoutManager(this);
    }
}

file class FillGridLayoutManager : GridLayoutManager
{
    public FillGridLayoutManager(Grid layout) : base(layout) { }

    public override Size Measure(double widthConstraint, double heightConstraint)
    {
        return new Size(widthConstraint, heightConstraint);
    }
}
