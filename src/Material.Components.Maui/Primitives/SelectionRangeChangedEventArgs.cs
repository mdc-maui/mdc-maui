namespace Material.Components.Maui.Primitives;
public class SelectionRangeChangedEventArgs : EventArgs
{
    public TextRange SelectionRange { get; set; }

    public SelectionRangeChangedEventArgs(TextRange range) => this.SelectionRange = range;
}
