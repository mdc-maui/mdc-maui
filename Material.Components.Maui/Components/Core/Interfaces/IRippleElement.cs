namespace Material.Components.Maui.Core.Interfaces;

public interface IRippleElement
{
    SKPoint TouchPoint { get; set; }
    Color RippleColor { get; }
    float RippleSize { get; }
    float RipplePercent { get; set; }

    void StartRippleEffect();
}
