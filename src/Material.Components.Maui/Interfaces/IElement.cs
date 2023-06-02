namespace Material.Components.Maui.Interfaces;

public interface IElement
{
    ViewState ViewState { get; set; }

    void OnPropertyChanged();
}
