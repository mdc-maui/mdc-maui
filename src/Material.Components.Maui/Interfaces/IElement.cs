namespace Material.Components.Maui.Interfaces;

public interface IElement
{
    ViewState ViewState { get; }

    bool IsEnabled { get; set; }

    void OnPropertyChanged();

    public static readonly BindableProperty IsEnabledProperty = BindableProperty.Create(
        nameof(IsEnabled),
        typeof(bool),
        typeof(IElement),
        true,
        propertyChanged: (bo, ov, nv) =>
        {
            ((TouchGraphicView)bo).ViewState = ViewState.Disabled;
            ((IElement)bo).OnPropertyChanged();
        }
    );
}
