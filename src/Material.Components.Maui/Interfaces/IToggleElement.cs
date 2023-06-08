namespace Material.Components.Maui.Interfaces;

public interface IToggleElement : IElement
{
    bool IsToggleEnabled { get; set; }
    bool IsSelected { get; set; }

    void OnToggleStateChanged();

    public static readonly BindableProperty IsToggleEnabledProperty = BindableProperty.Create(
        nameof(IsToggleEnabled),
        typeof(bool),
        typeof(IToggleElement),
        false,
        propertyChanged: (bo, ov, nv) => ((IToggleElement)bo).OnToggleStateChanged()
    );

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected),
        typeof(bool),
        typeof(IToggleElement),
        false,
        propertyChanged: (bo, ov, nv) => ((IToggleElement)bo).OnToggleStateChanged()
    );
}
