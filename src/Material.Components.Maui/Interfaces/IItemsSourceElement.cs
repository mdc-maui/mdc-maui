using System.Collections;
using System.Collections.Specialized;

namespace Material.Components.Maui.Interfaces;

public interface IItemsSourceElement<T>
{
    IList ItemsSource { get; set; }
    void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource),
        typeof(IList),
        typeof(IItemsSourceElement<T>),
        default,
        propertyChanged: (bo, ov, nv) =>
        {
            if (nv != null)
            {
                if (nv is INotifyCollectionChanged ncc)
                {
                    ncc.CollectionChanged += (
                        (IItemsSourceElement<T>)bo
                    ).OnItemsSourceCollectionChanged;
                }
            }
        }
    );
}
