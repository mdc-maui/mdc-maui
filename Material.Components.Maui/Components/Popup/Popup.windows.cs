using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using WPopup = Microsoft.UI.Xaml.Controls.Primitives.Popup;

namespace Material.Components.Maui;

public partial class Popup
{
    private static readonly Microsoft.UI.Xaml.Style flyoutStyle;
    private WPopup container;
    private ContentPanel platformAnchor;

    private void PlatformShow(Microsoft.Maui.Controls.Page anchor)
    {
        var context = anchor.Handler.MauiContext;
        var platformContent = this.Content.ToPlatform(context);
        if (this.container == null)
        {
            this.platformAnchor = anchor.ToPlatform(context) as ContentPanel;
            this.container = new WPopup
            {
                Child = platformContent,
                IsLightDismissEnabled = this.DismissOnOutside,
                LightDismissOverlayMode = LightDismissOverlayMode.On,
            };
            this.platformAnchor.Children.Add(this.container);
            this.container.Opened += (s, e) => this.Opened?.Invoke(this, EventArgs.Empty);
            this.container.Closed += (s, e) => this.Close();
            var size = this.Content.Measure(double.PositiveInfinity, double.PositiveInfinity);
            var x =
                this.OffsetX
                + this.HorizontalOptions switch
                {
                    LayoutAlignment.Start => 0,
                    LayoutAlignment.End => this.platformAnchor.ActualWidth - size.Request.Width,
                    _ => (this.platformAnchor.ActualWidth - size.Request.Width) / 2
                };
            var y =
                this.OffsetY
                + this.VerticalOptions switch
                {
                    LayoutAlignment.Start => 0,
                    LayoutAlignment.End => this.platformAnchor.ActualHeight - size.Request.Height,
                    _ => (this.platformAnchor.ActualHeight - size.Request.Height) / 2
                };
            this.container.HorizontalOffset = x;
            this.container.VerticalOffset = y;
        }
        this.container.IsOpen = true;
    }

    public void Close(object result = null)
    {
        if (this.container.IsOpen)
        {
            this.container.IsOpen = false;
        }
        if (!this.taskCompletionSource.Task.IsCompleted)
        {
            this.taskCompletionSource.TrySetResult(result);
            this.Closed?.Invoke(this, result);
        }
    }
}
