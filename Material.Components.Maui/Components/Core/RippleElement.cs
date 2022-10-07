namespace Material.Components.Maui.Core;

internal static class RippleElement
{
    public static readonly BindableProperty RippleColorProperty = BindableProperty.Create(
        nameof(IRippleElement.RippleColor),
        typeof(Color),
        typeof(IRippleElement),
        null);
}
