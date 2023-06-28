namespace Material.Components.Maui.Interfaces;

public interface IContextMenuElement
{
    ContextMenu ContextMenu { get; set; }

    void OnContextMenuChanged(object oldValue, object newValue);

    public static readonly BindableProperty ContextMenuProperty = BindableProperty.Create(
        nameof(ContextMenu),
        typeof(ContextMenu),
        typeof(IContextMenuElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IContextMenuElement)bo).OnContextMenuChanged(ov, nv)
    );
}
