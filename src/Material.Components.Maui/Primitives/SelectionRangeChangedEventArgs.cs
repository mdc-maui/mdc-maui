namespace Material.Components.Maui.Primitives;
public class SelectionRangeChangedEventArgs(TextRange range) : EventArgs
{
    public TextRange SelectionRange { get; set; } = range;
}
