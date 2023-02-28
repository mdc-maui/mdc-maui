namespace Material.Components.Maui.Core.Interfaces;

public interface IView
{
    ControlState ControlState { get; set; }
    void OnPropertyChanged();
}
