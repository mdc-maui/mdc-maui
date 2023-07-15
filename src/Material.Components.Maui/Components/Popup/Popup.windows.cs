using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using WPopup = Microsoft.UI.Xaml.Controls.Primitives.Popup;

namespace Material.Components.Maui;

public partial class Popup
{
    private WPopup container;
    private ContentPanel platformAnchor;

    private void PlatformShow(Microsoft.Maui.Controls.Page anchor)
    {
        var context = anchor.Handler.MauiContext;
        var platformContent = this.Content.ToPlatform(context);
        if (this.container == null)
        {
            this.platformAnchor = anchor.ToPlatform(context) as ContentPanel;
            var layout = new AbsoluteLayout
            {
                Parent = anchor,
                BackgroundColor = Color.FromArgb("#80ffffff"),
                WidthRequest = this.platformAnchor.ActualWidth,
                HeightRequest = this.platformAnchor.ActualHeight,
                Children = { this.Content },
            };

            this.container = new WPopup
            {
                Child = layout.ToPlatform(context),
                IsLightDismissEnabled = this.DismissOnOutside,
                LightDismissOverlayMode = LightDismissOverlayMode.Off,
            };

            this.platformAnchor.Children.Add(this.container);
            this.container.Opened += (s, e) => this.Opened?.Invoke(this, EventArgs.Empty);
            this.container.Closed += (s, e) => this.Close();
            platformContent.Measure(
                new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity)
            );
        }
        var size = platformContent.DesiredSize;
        var x =
            this.OffsetX
            + this.HorizontalOptions switch
            {
                LayoutAlignment.Start => 0,
                LayoutAlignment.End => this.platformAnchor.ActualWidth - size.Width,
                _ => (this.platformAnchor.ActualWidth - size.Width) / 2
            };
        var y =
            this.OffsetY
            + this.VerticalOptions switch
            {
                LayoutAlignment.Start => 0,
                LayoutAlignment.End => this.platformAnchor.ActualHeight - size.Height,
                _ => (this.platformAnchor.ActualHeight - size.Height) / 2
            };
        this.Content.SetValue(
            AbsoluteLayout.LayoutBoundsProperty,
            new Rect(x, y, size.Width, size.Height)
        );
        this.container.IsOpen = true;
    }

    public void Close(object result = null)
    {
        if (this.container.IsOpen)
        {
            this.container.IsOpen = false;
            this.platformAnchor.Children.Remove(this.container);
        }
        if (!this.taskCompletionSource.Task.IsCompleted)
        {
            this.taskCompletionSource.TrySetResult(result);
            this.Closed?.Invoke(this, result);
        }
    }
}
