using Material.Components.Maui.Core;
using Microsoft.Maui.Animations;
using System.ComponentModel;

namespace Material.Components.Maui;
public class CardContainer : SKTouchCanvasView, IView
{
    #region IView
    private ControlState controlState = ControlState.Normal;
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ControlState ControlState
    {
        get => this.controlState;
        set
        {
            VisualStateManager.GoToState(this, value switch
            {
                ControlState.Normal => "normal",
                ControlState.Hovered => "hovered",
                ControlState.Pressed => "pressed",
                ControlState.Disabled => "disabled",
                _ => "normal",
            });
            this.controlState = value;
            this.PART_Parent.ControlState = value;
        }
    }
    public void OnPropertyChanged()
    {
        this.InvalidateSurface();
    }
    #endregion

    private readonly Card PART_Parent;
    private readonly CardDrawable drawable;
    private IAnimationManager animationManager;

    public CardContainer(Card parent)
    {
        this.IgnorePixelScaling = true;
        this.PART_Parent = parent;
        this.Pressed += (s, e) =>
        {
            this.PART_Parent.TouchPoint = e.Location;
            this.StartRippleEffect();
        };
        this.drawable = new CardDrawable(parent);
    }


    [EditorBrowsable(EditorBrowsableState.Never)]
    public void StartRippleEffect()
    {
        this.animationManager ??= this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        var start = 0f;
        var end = 1f;
        this.animationManager?.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
        {
            this.PART_Parent.RipplePercent = start.Lerp(end, progress);
            this.InvalidateSurface();
        },
        duration: 0.35f,
        easing: Easing.SinInOut,
        finished: () =>
        {
            if (this.ControlState != ControlState.Pressed)
            {
                this.PART_Parent.RipplePercent = 0f;
                this.InvalidateSurface();
            }
        }));
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        this.PART_Parent.RippleSize = e.Info.Rect.GetRippleSize(this.PART_Parent.TouchPoint);
        this.drawable.Draw(e.Surface.Canvas, e.Info.Rect);
    }
}
