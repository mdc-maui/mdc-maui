using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Material.Components.Maui.Interfaces;

public interface IItemsElement<T>
{
    ObservableCollection<T> Items { get; set; }

    void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);

    private static readonly BindablePropertyKey ItemsPropertyKey = BindableProperty.CreateReadOnly(
        nameof(Items),
        typeof(ObservableCollection<T>),
        typeof(IItemsElement<T>),
        null,
        defaultValueCreator: bo =>
        {
            var result = new ObservableCollection<T>();
            result.CollectionChanged += ((IItemsElement<T>)bo).OnItemsCollectionChanged;
            return result;
        }
    );

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;
}
