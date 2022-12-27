namespace Material.Components.Maui.Core.Interfaces;

internal static class RippleElement
{
    public static readonly BindableProperty RippleColorProperty = BindableProperty.Create(
        nameof(IRippleElement.RippleColor),
        typeof(Color),
        typeof(IRippleElement),
        null
    );
}
