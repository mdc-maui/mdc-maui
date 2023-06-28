using Android.Widget;
using Microsoft.Maui.Platform;

namespace Material.Components.Maui;

public partial class ContextMenu
{
    private PopupWindow container;
    private Android.Views.View platformContent;
    private Android.Views.View platformAnchor;

    private void PlatformShow(View anchor)
    {
        var context = anchor.Handler.MauiContext;
        if (this.container == null)
        {
            this.platformContent = this.ToPlatform(context);
            this.platformAnchor = anchor.ToPlatform(context);
            this.container = new PopupWindow(this.platformContent) { OutsideTouchable = true };
            this.container.DismissEvent += this.OnClosed;
        }
        this.container.Width = (int)this.platformContent.Context.ToPixels(this.DesiredSize.Width);
        this.container.Height = (int)this.platformContent.Context.ToPixels(this.DesiredSize.Height);
        this.container.ShowAsDropDown(this.platformAnchor);
    }

    private void PlatformShow(View anchor, Point point)
    {
        var context = anchor.Handler.MauiContext;
        if (this.container == null)
        {
            this.platformContent = this.ToPlatform(context);
            this.platformAnchor = anchor.ToPlatform(context);
            this.container = new PopupWindow(this.platformContent) { OutsideTouchable = true };
            this.container.DismissEvent += this.OnClosed;
        }
        this.container.Width = (int)this.platformContent.Context.ToPixels(this.DesiredSize.Width);
        this.container.Height = (int)this.platformContent.Context.ToPixels(this.DesiredSize.Height);

        var location = new int[2];
        this.platformAnchor.GetLocationOnScreen(location);
        this.container.ShowAtLocation(
            this.platformAnchor,
            Android.Views.GravityFlags.Left | Android.Views.GravityFlags.Top,
            location[0] + (int)this.platformContent.Context.ToPixels(point.X),
            location[1] + (int)this.platformContent.Context.ToPixels(point.Y)
        );
    }

    public void Close(object result = null)
    {
        this.Result = result;
        this.container.Dismiss();
    }

    public bool IsShowing() => this.container?.IsShowing ?? false;
}
