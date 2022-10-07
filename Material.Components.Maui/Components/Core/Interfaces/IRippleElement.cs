namespace Material.Components.Maui.Core.Interfaces;
internal interface IRippleElement
{
    SKPoint TouchPoint { get; }
    Color RippleColor { get; }
    float RippleSize { get; }
    float RipplePercent { get; }
}
