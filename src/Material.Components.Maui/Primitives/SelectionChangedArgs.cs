namespace Material.Components.Maui.Primitives;

public class SelectionChangedArgs
{
    public TextRange SelectionRange { get; set; }

    public SelectionChangedArgs(TextRange range) => this.SelectionRange = range;

    public SelectionChangedArgs(int start, int end) => this.SelectionRange = new(start, end);
}
