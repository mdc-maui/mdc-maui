namespace Material.Components.Maui.Primitives;
public class SelectedItemsChangedArgs<T>(IEnumerable<T> selectedItems) : EventArgs
{
    public IEnumerable<T> SelectedItems { get; set; } = selectedItems;
}
