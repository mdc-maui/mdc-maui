namespace Material.Components.Maui.Core.Interfaces;
internal interface IView
{
    ControlState ControlState { get; set; }
    void OnPropertyChanged();
}
