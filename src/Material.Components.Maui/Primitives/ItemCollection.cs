using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Material.Components.Maui.Primitives;

public sealed class ItemCollection<T> : IEnumerable<T>, IList<T>, INotifyCollectionChanged
{
    readonly ObservableCollection<T> inner = new();

    public event NotifyCollectionChangedEventHandler CollectionChanged
    {
        add { ((INotifyCollectionChanged)this.inner).CollectionChanged += value; }
        remove { ((INotifyCollectionChanged)this.inner).CollectionChanged -= value; }
    }

    public int Count => this.inner.Count;

    public bool IsReadOnly => ((IList<T>)this.inner).IsReadOnly;

    public T this[int index]
    {
        get => this.inner[index];
        set => this.inner[index] = value;
    }

    public void Add(T item) => this.inner.Add(item);

    public void Clear() => this.inner.Clear();

    public bool Contains(T item) => this.inner.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => this.inner.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => this.inner.GetEnumerator();

    public int IndexOf(T item) => this.inner.IndexOf(item);

    public void Insert(int index, T item) => this.inner.Insert(index, item);

    public bool Remove(T item) => this.inner.Remove(item);

    public void RemoveAt(int index) => this.inner.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.inner).GetEnumerator();
}
