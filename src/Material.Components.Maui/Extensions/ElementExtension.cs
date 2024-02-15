namespace Material.Components.Maui.Extensions;

public static class ElementExtension
{
    public static T GetParentElement<T>(this Element view)
        where T : Element
    {
        return view.Parent is null
            ? null
            : view.Parent is T
                ? view.Parent as T
                : view.Parent.GetParentElement<T>();
    }
}
