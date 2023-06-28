namespace Material.Components.Maui.Interfaces;

public interface ITrailingIconElement : IElement, IDisposable
{
    string TrailingIconData { get; set; }
    PathF TrailingIconPath { get; set; }
    Color TrailingIconColor { get; set; }

    public static readonly BindableProperty TrailingIconDataProperty = BindableProperty.Create(
        nameof(TrailingIconData),
        typeof(string),
        typeof(ITrailingIconElement),
        default,
        propertyChanged: (bo, ov, nv) =>
        {
            ((ITrailingIconElement)bo).TrailingIconPath = PathBuilder.Build((string)nv);
            ((IElement)bo).InvalidateMeasure();
        }
    );

    public static readonly BindableProperty TrailingIconColorProperty = BindableProperty.Create(
        nameof(TrailingIconColor),
        typeof(Color),
        typeof(ITrailingIconElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}
