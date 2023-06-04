namespace Material.Components.Maui.Interfaces;

public interface IElement
{
    ViewState ViewState { get; }

    void OnPropertyChanged();
}
