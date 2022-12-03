using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material.Components.Maui;

[ContentProperty(nameof(Pane))]
public partial class SplitView : View, IVisualTreeElement
{
    [AutoBindable]
    private readonly DrawerDisplayMode displayMode;

    [AutoBindable]
    private readonly Page pane;

    [AutoBindable]
    private readonly Page content;

    [AutoBindable(DefaultValue = "true")]
    private readonly bool isPaneOpen;


    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        new List<IVisualTreeElement> { this.Pane, this.Content };

    public IVisualTreeElement GetVisualParent() => this.Window;
}
