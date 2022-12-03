namespace Material.Components.Maui.Core;

public class SelectedIndexChangedEventArgs : EventArgs
{
    public int SelectedIndex { get; init; }

    public SelectedIndexChangedEventArgs(int index)
    {
        this.SelectedIndex = index;
    }
}
