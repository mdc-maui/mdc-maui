namespace Material.Components.Maui.Platform.Editable;
internal class UndoRedoHelper
{
    private readonly List<EditableCache> caches = new();
    private int position = -1;

    public void Add(EditableCache cache)
    {
        if (this.position < this.caches.Count - 1)
            this.caches.RemoveRange(this.position + 1, this.caches.Count - this.position - 1);

        this.caches.Add(cache);
        this.position = this.caches.Count;
    }

    public void Add(string text, TextRange range)
    {
        this.Add(new EditableCache(text, range));
    }

    public void Add(string text, int positon)
    {
        this.Add(new EditableCache(text, positon));
    }

    public void Add(string text, int startPositon, int endPositon)
    {
        this.Add(new EditableCache(text, startPositon, endPositon));
    }

    public EditableCache? GetUndoCache()
    {
        if (this.caches.Count > 0 && this.position >= 0)
        {
            this.position = Math.Min(this.position, this.caches.Count - 1);
            var cache = this.caches.ElementAt(this.position);
            this.position -= 1;
            return cache;
        }
        return null;
    }

    public EditableCache? GetRedoCache()
    {
        if (this.caches.Count > 0 && this.position < this.caches.Count - 1)
        {
            var cache = this.caches.ElementAt(this.position + 1);
            this.position += 1;
            return cache;
        }
        return null;
    }
}
