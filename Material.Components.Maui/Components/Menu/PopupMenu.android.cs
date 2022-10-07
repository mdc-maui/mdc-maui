#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Google.Crypto.Tink.Prf;
using static Android.Views.ViewGroup;
using Window = Android.Views.Window;

namespace Material.Components.Maui.Core.Menu;
public partial class PopupMenu
{
    private PopupWindow container;
    private Android.Views.View platformAnchor;

    public void Connect(IMauiContext context)
    {
        var content = this.Content.ToPlatform(context);
        this.platformAnchor = this.Anchor.ToPlatform(context);

        this.container = new PopupWindow(content) { OutsideTouchable = true };
        this.container.DismissEvent += this.OnClosed;
    }

    public void Show(int count)
    {
        this.container.Width = this.platformAnchor.Width;
        this.container.Height = (int)this.platformAnchor.Context.ToPixels(Math.Min(240, 48 * count));
        this.container.ShowAsDropDown(this.platformAnchor);
    }

    public void Close(object result = null)
    {
        this.Result = result;
        this.container.Dismiss();
    }
}
#endif