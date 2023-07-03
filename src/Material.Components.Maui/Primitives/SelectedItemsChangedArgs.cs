namespace Material.Components.Maui.Primitives;
public class SelectedItemsChangedArgs<T> : EventArgs
{
    public IEnumerable<T> SelectedItems { get; set; }
    public SelectedItemsChangedArgs(IEnumerable<T> selectedItems)
    {
        this.SelectedItems = selectedItems;
    }
}
