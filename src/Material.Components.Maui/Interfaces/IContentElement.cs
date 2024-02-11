namespace Material.Components.Maui.Interfaces;

public interface IContentElement : IElement
{
    View Content { get; }

    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content),
        typeof(View),
        typeof(IContentElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}
