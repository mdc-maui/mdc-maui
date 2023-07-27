namespace Material.Components.Maui.Interfaces;

public interface IPaddingElement : IElement
{
    Thickness Padding { get; set; }

    public static readonly BindableProperty PaddingProperty = BindableProperty.Create(
       nameof(Padding),
       typeof(Thickness),
       typeof(IPaddingElement),
       Thickness.Zero,
       propertyChanged: (bo, ov, nv) => ((IElement)bo).InvalidateMeasure()
   );
}
