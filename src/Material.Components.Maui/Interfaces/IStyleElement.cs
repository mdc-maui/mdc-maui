namespace Material.Components.Maui.Interfaces;

public interface IStyleElement
{
    public string DynamicStyle { get; set; }

    public static readonly BindableProperty DynamicStyleProperty = BindableProperty.Create(
        nameof(DynamicStyle),
        typeof(string),
        typeof(IStateLayerElement),
        default,
        propertyChanged: (bo, ov, nv) =>
            ((VisualElement)bo).SetDynamicResource(VisualElement.StyleProperty, (string)nv)
    );
}
