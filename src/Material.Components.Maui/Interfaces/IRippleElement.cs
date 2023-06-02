namespace Material.Components.Maui.Interfaces;

public interface IRippleElement : IStateLayerElement
{
    float RippleDuration { get; set; }
    Easing RippleEasing { get; set; }

    public static readonly BindableProperty RippleDurationProperty = BindableProperty.Create(
        nameof(RippleDuration),
        typeof(float),
        typeof(IRippleElement),
        0.5f
    );

    public static readonly BindableProperty RippleEasingProperty = BindableProperty.Create(
        nameof(RippleEasing),
        typeof(Easing),
        typeof(IRippleElement),
        Easing.SinInOut
    );
}
