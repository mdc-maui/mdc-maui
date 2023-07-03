using Android.App;
using Android.Views;
using Microsoft.Maui.Platform;

namespace Material.Components.Maui;

public partial class Popup
{
    private Dialog container;
    private Android.Views.View platformContent;
    private Android.Views.View platformAnchor;

    public void PlatformShow(Page anchor)
    {
        var context = anchor.Handler.MauiContext;
        if (this.container == null)
        {
            this.platformContent = this.Content.ToPlatform(context);
            this.platformAnchor = anchor.ToPlatform(context);
            this.container = new Dialog(this.platformAnchor.Context);
            this.container.ShowEvent += (s, e) => this.Opened?.Invoke(this, EventArgs.Empty);
            this.container.DismissEvent += (s, e) => this.Close();

            this.container.SetCanceledOnTouchOutside(this.DismissOnOutside);
            this.container.SetContentView(this.platformContent);
            this.container.Window.DecorView.SetPadding(0, 0, 0, 0);
            this.container.Window.DecorView.SetBackgroundColor(Colors.Transparent.ToPlatform());
        }
        var window = this.container.Window;
        window.DecorView.Measure(
            (int)MeasureSpecMode.Unspecified,
            (int)MeasureSpecMode.Unspecified
        );
        window.Attributes.Width = window.DecorView.MeasuredWidth;
        window.Attributes.Height = window.DecorView.MeasuredHeight;

        var gravity = this.HorizontalOptions switch
        {
            LayoutAlignment.Start => GravityFlags.Left,
            LayoutAlignment.End => GravityFlags.Right,
            _ => GravityFlags.Center
        };
        gravity |= this.VerticalOptions switch
        {
            LayoutAlignment.Start => GravityFlags.Top,
            LayoutAlignment.End => GravityFlags.Bottom,
            _ => GravityFlags.Center
        };

        window.SetGravity(gravity);
        window.Attributes.X = this.OffsetX;
        window.Attributes.Y = this.OffsetY;
        this.container.Show();
    }

    public void Close(object result = default)
    {
        if (this.container.IsShowing)
        {
            this.container.Dismiss();
        }
        if (!this.taskCompletionSource.Task.IsCompleted)
        {
            this.taskCompletionSource.TrySetResult(result);
            this.Closed?.Invoke(this, result);
        }
    }
}
