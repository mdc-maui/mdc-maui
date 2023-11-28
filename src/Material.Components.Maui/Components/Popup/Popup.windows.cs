using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Grid = Microsoft.Maui.Controls.Grid;
using Page = Microsoft.Maui.Controls.Page;
using WPopup = Microsoft.UI.Xaml.Controls.Primitives.Popup;

namespace Material.Components.Maui;

public partial class Popup
{
    private WPopup container;
    private ContentPanel platformAnchor;
    private FrameworkElement platformOutside;

    private void PlatformShow(Page anchor)
    {
        var context = anchor.Handler.MauiContext;
        var platformContent = this.Content.ToPlatform(context);

        if (this.container == null)
        {
            this.platformAnchor = anchor.ToPlatform(context) as ContentPanel;

            var outside = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
            };

            var root = new AbsoluteLayout
            {
                Parent = this,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.FromArgb("#80808080"),
                Children = { outside, this.Content },
            };

            this.platformOutside = outside.ToPlatform(context);
            var platformRoot = root.ToPlatform(context);

            this.platformOutside.Tapped += this.OnOutsideTapped;

            this.container = new WPopup
            {
                Child = platformRoot,
                IsLightDismissEnabled = false,
                LightDismissOverlayMode = LightDismissOverlayMode.Off,
            };

            this.platformAnchor.Children.Add(this.container);

            this.container.Opened += this.OnContainerOpened;
            this.container.Closed += this.OnContainerClosed;

            outside.SetBinding(
                View.WidthRequestProperty,
                new Binding(
                    "Width",
                    source: new RelativeBindingSource(
                        RelativeBindingSourceMode.FindAncestor,
                        typeof(Page)
                    )
                )
            );
            outside.SetBinding(
                View.HeightRequestProperty,
                new Binding(
                    "Height",
                    source: new RelativeBindingSource(
                        RelativeBindingSourceMode.FindAncestor,
                        typeof(Page)
                    )
                )
            );

            root.SetBinding(
                View.WidthRequestProperty,
                new Binding(
                    "Width",
                    source: new RelativeBindingSource(
                        RelativeBindingSourceMode.FindAncestor,
                        typeof(Page)
                    )
                )
            );
            root.SetBinding(
                View.HeightRequestProperty,
                new Binding(
                    "Height",
                    source: new RelativeBindingSource(
                        RelativeBindingSourceMode.FindAncestor,
                        typeof(Page)
                    )
                )
            );
        }

        platformContent.Measure(
            new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity)
        );
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

    private void OnOutsideTapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
    {
        if (this.DismissOnOutside)
            this.Close();
    }

    private void OnContainerClosed(object sender, object e) => this.Close();

    private void OnContainerOpened(object sender, object e) =>
        this.Opened?.Invoke(this, EventArgs.Empty);

    public void Close(object result = null)
    {
        if (this.container.IsOpen)
            this.container.IsOpen = false;

        if (!this.taskCompletionSource.Task.IsCompleted)
        {
            this.taskCompletionSource.TrySetResult(result);
            this.Closed?.Invoke(this, result);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.Close();
                this.platformOutside.Tapped -= this.OnOutsideTapped;
                this.container.Opened -= this.OnContainerOpened;
                this.container.Closed -= this.OnContainerClosed;
                this.platformAnchor.Children.Remove(this.container);
            }
            this.disposedValue = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
