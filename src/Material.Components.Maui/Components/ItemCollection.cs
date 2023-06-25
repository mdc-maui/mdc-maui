using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Material.Components.Maui;

public sealed class ItemCollection<T> : IEnumerable<T>, IList<T>, INotifyCollectionChanged
{
    readonly ObservableCollection<T> inner = new();

    public event NotifyCollectionChangedEventHandler CollectionChanged
    {
        add { ((INotifyCollectionChanged)inner).CollectionChanged += value; }
        remove { ((INotifyCollectionChanged)inner).CollectionChanged -= value; }
    }

    public int Count => inner.Count;

    public bool IsReadOnly => ((IList<T>)inner).IsReadOnly;

    public T this[int index]
    {
        get => inner[index];
        set => inner[index] = value;
    }

    public void Add(T item) => inner.Add(item);

    public void Clear() => inner.Clear();

    public bool Contains(T item) => inner.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => inner.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => inner.GetEnumerator();

    public int IndexOf(T item) => inner.IndexOf(item);

    public void Insert(int index, T item) => inner.Insert(index, item);

    public bool Remove(T item) => inner.Remove(item);

    public void RemoveAt(int index) => inner.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.inner).GetEnumerator();
}
