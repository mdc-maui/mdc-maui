namespace Material.Components.Maui.Platform.Editable;
internal struct EditableCache
{
    public string Text;
    public TextRange SelectionRange;

    public EditableCache(string text, TextRange range)
    {
        this.Text = text;
        this.SelectionRange = range;
    }

    public EditableCache(string text, int positon)
    {
        this.Text = text;
        this.SelectionRange = new TextRange(positon);
    }

    public EditableCache(string text, int start, int end)
    {
        this.Text = text;
        this.SelectionRange = new TextRange(start, end);
    }
}
