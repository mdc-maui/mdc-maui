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
        this.container.Width = (int)this.platformContent.Context.ToPixels(this.WidthRequest);
        this.container.Height = (int)this.platformContent.Context.ToPixels(this.HeightRequest);
        this.container.ShowAsDropDown(this.platformAnchor);
    }

    public void Close(object result = null)
    {
        this.Result = result;
        this.container.Dismiss();
    }
}