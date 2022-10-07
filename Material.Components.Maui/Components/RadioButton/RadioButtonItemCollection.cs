using System.Collections;

namespace Material.Components.Maui.Core.RadioButton;
public class RadioButtonItemCollection : IList<RadioButtonItem>
{
    private StackLayout inner;
    internal StackLayout Inner
    {
        get => this.inner;
        set
        {
            if (this.inner != null)
                throw new ArgumentException("Inner can only be set once");

            this.inner = value;
        }
    }

    public RadioButtonItem this[int index]
    {
        get => (RadioButtonItem)this.Inner[index];
        set => this.Inner[index] = value;
    }

    public int Count => this.Inner.Count;

    public bool IsReadOnly => this.Inner.IsReadOnly;

    public void Add(RadioButtonItem item) => this.Inner.Add(item);

    public void Clear() => this.Inner.Clear();

    public bool Contains(RadioButtonItem item) => this.Inner.Contains(item);

    public void CopyTo(RadioButtonItem[] array, int arrayIndex) => this.Inner.CopyTo(array, arrayIndex);

    public int IndexOf(RadioButtonItem item) => this.Inner.IndexOf(item);

    public void Insert(int index, RadioButtonItem item) => this.Inner.Insert(index, item);

    public bool Remove(RadioButtonItem item) => this.Inner.Remove(item);

    public void RemoveAt(int index) => this.Inner.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() => this.Inner.Children.GetEnumerator();

    public IEnumerator<RadioButtonItem> GetEnumerator()
    {
        return (IEnumerator<RadioButtonItem>)this.Inner.Children.GetEnumerator();
    }
}
