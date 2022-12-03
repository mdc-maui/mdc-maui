using Android.Content;
using AndroidX.DrawerLayout.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Views.View;

namespace Material.Components.Maui.Core;
public class ASplitView : DrawerLayout
{
    public ASplitView(Context context) : base(context)
    {
    }

    protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
    {
        widthMeasureSpec = MeasureSpec.MakeMeasureSpec(MeasureSpec.GetSize(widthMeasureSpec), Android.Views.MeasureSpecMode.Exactly);
        heightMeasureSpec = MeasureSpec.MakeMeasureSpec(MeasureSpec.GetSize(heightMeasureSpec), Android.Views.MeasureSpecMode.Exactly);
        base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
    }
}
