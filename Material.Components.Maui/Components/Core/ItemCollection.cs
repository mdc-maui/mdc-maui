using System.Collections;

namespace Material.Components.Maui.Core;

public class ItemsChangedEventArgs<IView>
{
    public string EventType;
    public int Index;
}

public class ItemCollection<T> : IList<T>
{
    private readonly List<T> inner = new();

    public T this[int index]
    {
        get => this.inner[index];
        set => this.inner[index] = value;
    }

    public int Count => this.inner.Count;
    public bool IsReadOnly => true;

    public event EventHandler<ItemsChangedEventArgs<T>> OnAdded;
    public event EventHandler<ItemsChangedEventArgs<T>> OnRemoved;
    public event EventHandler OnCleared;

    public void Add(T item)
    {
        this.inner.Add(item);
        this.OnAdded?.Invoke(this, new ItemsChangedEventArgs<T>
        {
            EventType = "Add",
            Index = this.inner.Count - 1
        });
    }

    public void Insert(int index, T item)
    {
        this.inner.Insert(index, item);
        OnAdded?.Invoke(this, new ItemsChangedEventArgs<T>
        {
            EventType = "Insert",
            Index = index
        });
    }

    public bool Remove(T item)
    {
        var index = this.inner.IndexOf(item);
        OnRemoved?.Invoke(this, new ItemsChangedEventArgs<T>
        {
            EventType = "Remove",
            Index = index
        });
        return this.inner.Remove(item);
    }

    public void RemoveAt(int index)
    {
        var item = this.inner[index];
        OnRemoved?.Invoke(this, new ItemsChangedEventArgs<T>
        {
            EventType = "Remove",
            Index = index
        });
        this.inner.RemoveAt(index);
    }

    public void Clear()
    {
        this.inner.Clear();
        OnCleared?.Invoke(this, default);
    }

    public bool Contains(T item) => this.inner.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => this.inner.CopyTo(array as T[], arrayIndex);

    public int IndexOf(T item) => this.inner.IndexOf(item);

    IEnumerator IEnumerable.GetEnumerator() => this.inner.GetEnumerator();

    public IEnumerator<T> GetEnumerator()
    {
        return this.inner.Cast<T>().GetEnumerator();
    }
}
