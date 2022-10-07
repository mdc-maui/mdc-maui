using System.Collections;

namespace Material.Components.Maui.Core.NavigationBar;
public partial class NavigationItemCollection : IList<NavigationItem>
{
    private Grid inner;
    internal Grid Inner
    {
        get => this.inner;
        set
        {
            if (this.inner != null)
                throw new ArgumentException("Inner can only be set once");

            this.inner = value;
        }
    }

    private int currPosition;

    public NavigationItem this[int index]
    {
        get => (NavigationItem)this.Inner[index];
        set => this.Inner[index] = value;
    }

    public int Count => this.Inner.Count;

    public bool IsReadOnly => this.Inner.IsReadOnly;

    public void ChangePosition(int index)
    {
        this.currPosition = index;
        if (index >= 0 && this.Inner is not null && index < this.Inner.Count)
        {
            for (int i = 0; i < this.Inner.Count; i++)
            {
                ((NavigationItem)this.Inner[i]).IsActived = i == index;
            }
        }
    }

    public void Add(NavigationItem item)
    {
        var index = this.Inner.Count;
        item.IsActived = item.Content.IsVisible = index == this.currPosition;
        this.Inner.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        this.Inner.Add(item, column: index);
    }

    public void Clear() => this.Inner.Clear();

    public bool Contains(NavigationItem item) => this.Inner.Contains(item);

    public void CopyTo(NavigationItem[] array, int arrayIndex) => this.Inner.CopyTo(array, arrayIndex);

    public int IndexOf(NavigationItem item) => this.Inner.IndexOf(item);

    public void Insert(int index, NavigationItem item) => this.Inner.Insert(index, item);

    public bool Remove(NavigationItem item)
    {
        this.Inner.Remove(item);
        return !this.Inner.Contains(item);
    }

    public void RemoveAt(int index) => this.Inner.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() => this.Inner.GetEnumerator();

    public IEnumerator<NavigationItem> GetEnumerator()
    {
        return this.Inner.Cast<NavigationItem>().GetEnumerator();
    }
}
