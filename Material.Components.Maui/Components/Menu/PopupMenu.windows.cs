#if WINDOWS
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Diagnostics;

namespace Material.Components.Maui.Core.Menu;

public partial class PopupMenu
{
    private Popup container;
    private LayoutPanel platformAnchor;


    public void Connect(IMauiContext context)
    {
        this.platformAnchor = this.Anchor.ToPlatform(context) as LayoutPanel;
        this.container = new Popup
        {
            IsLightDismissEnabled = true,
            Child = this.Content.ToPlatform(context),
        };
        this.container.Closed += this.OnClosed;
        this.platformAnchor.Children.Add(this.container);
    }

    public void Show(int count)
    {
        var height = Math.Min(240, 48 * count);
        var anchorPosition = this.platformAnchor.TransformToVisual(this.platformAnchor.XamlRoot.Content).TransformPoint(new Windows.Foundation.Point(0, 0));

        var x = 0;
        var y = this.platformAnchor.ActualHeight;
        if (anchorPosition.Y + this.Anchor.Height + height > this.platformAnchor.XamlRoot.Content.ActualSize.Y)
        {
            y = -height;
        }
        this.container.HorizontalOffset = x;
        this.container.VerticalOffset = y;

        this.container.IsOpen = true;
    }

    public void Close(object result = null)
    {
        this.Result = result;
        this.container.IsOpen = false;
    }
}
#endif