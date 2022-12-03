namespace Material.Components.Maui.Components.Core;

public class SelectedChangedEventArgs : EventArgs
{
    public bool IsSelected { get; set; }

    public SelectedChangedEventArgs(bool isSelected)
    {
        this.IsSelected = isSelected;
    }
}
