namespace Material.Components.Maui.Primitives;
public class EditTextChangedEventArgs : EventArgs
{
    public string Text { get; set; }
    public TextRange SelectionRange { get; set; }
    public EditTextChangedEventArgs(string text, TextRange range)
    {
        this.Text = text;
        this.SelectionRange = range;
    }
}
