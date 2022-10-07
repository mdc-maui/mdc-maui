using System.Collections;

namespace Material.Components.Maui.Core.ComboBox;
public class ComboBoxItemCollection : IList<ComboBoxItem>
{
    private VerticalStackLayout inner;
    internal VerticalStackLayout Inner
    {
        get => this.inner;
        set
        {
            if (this.inner != null)
                throw new ArgumentException("Inner can only be set once");

            this.inner = value;
        }
    }

    public ComboBoxItem this[int index]
    {
        get => (ComboBoxItem)this.Inner[index];
        set => this.Inner[index] = value;
    }

    public int Count => this.Inner.Count;

    public bool IsReadOnly => this.Inner.IsReadOnly;

    public void Add(ComboBoxItem item) => this.Inner.Add(item);

    public void Clear() => this.Inner.Clear();

    public bool Contains(ComboBoxItem item) => this.Inner.Contains(item);

    public void CopyTo(ComboBoxItem[] array, int arrayIndex) => this.Inner.Children.CopyTo(array, arrayIndex);

    public int IndexOf(ComboBoxItem item) => this.Inner.IndexOf(item);

    public void Insert(int index, ComboBoxItem item) => this.Inner.Insert(index, item);

    public bool Remove(ComboBoxItem item) => this.Inner.Remove(item);

    public void RemoveAt(int index) => this.Inner.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() => this.Inner.GetEnumerator();

    public IEnumerator<ComboBoxItem> GetEnumerator()
    {
        return (IEnumerator<ComboBoxItem>)this.Inner.Children.GetEnumerator();
    }
}
