using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Material.Components.Maui.Core;

public class ItemsChangedEventArgs<IView>
{
    public string EventType;
    public int Index;
}

public class ItemCollection<T> : ObservableCollection<T>
{
    public event EventHandler<ItemsChangedEventArgs<T>> OnAdded;
    public event EventHandler<ItemsChangedEventArgs<T>> OnRemoved;
    public event EventHandler OnCleared;

    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        OnAdded?.Invoke(this, new ItemsChangedEventArgs<T> { EventType = "Insert", Index = index });
    }

    protected override void RemoveItem(int index)
    {
        base.RemoveItem(index);
        OnRemoved?.Invoke(this, new ItemsChangedEventArgs<T> { EventType = "Remove", Index = index });
    }

    protected override void ClearItems()
    {
        base.ClearItems();
        OnCleared?.Invoke(this, default);
    }
}
