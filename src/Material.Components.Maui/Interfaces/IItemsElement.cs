using System.Collections;
using System.Collections.Specialized;

namespace Material.Components.Maui.Interfaces;

public interface IItemsElement<T>
{
    ItemCollection<T> Items { get; set; }

    IList ItemsSource { get; set; }

    void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);

    void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);

    private static readonly BindablePropertyKey ItemsPropertyKey = BindableProperty.CreateReadOnly(
        nameof(Items),
        typeof(ItemCollection<T>),
        typeof(IItemsElement<T>),
        null,
        defaultValueCreator: bo =>
        {
            var result = new ItemCollection<T>();
            result.CollectionChanged += ((IItemsElement<T>)bo).OnItemsCollectionChanged;
            return result;
        }
    );

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource),
        typeof(IList),
        typeof(IItemsElement<T>),
        default,
        propertyChanged: (bo, ov, nv) =>
        {
            if (nv != null)
            {
                if (nv is INotifyCollectionChanged ncc)
                {
                    ncc.CollectionChanged += ((IItemsElement<T>)bo).OnItemsSourceCollectionChanged;
                }
            }
        }
    );
}
