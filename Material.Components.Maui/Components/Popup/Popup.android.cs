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
            this.container.Window.DecorView.SetBackgroundColor(Colors.Transparent.ToPlatform());
        }
        var window = this.container.Window;
        window.DecorView.Measure((int)MeasureSpecMode.Unspecified, (int)MeasureSpecMode.Unspecified);
        var width = window.DecorView.MeasuredWidth;
        var height = window.DecorView.MeasuredHeight;
        var x = this.OffsetX + this.HorizontalOptions switch
        {
            LayoutAlignment.Start => 0,
            LayoutAlignment.End => this.platformAnchor.Width - width,
            _ => (this.platformAnchor.Width - width) / 2
        };
        var y = this.OffsetY + this.VerticalOptions switch
        {
            LayoutAlignment.Start => 0,
            LayoutAlignment.End => this.platformAnchor.Height - height,
            _ => (this.platformAnchor.Height - height) / 2
        };
        window.SetGravity(GravityFlags.Start | GravityFlags.Top);
        window.Attributes.X = x;
        window.Attributes.Y = y;
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