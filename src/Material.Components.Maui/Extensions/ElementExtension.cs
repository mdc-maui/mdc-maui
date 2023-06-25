namespace Material.Components.Maui.Extensions;
internal static class ElementExtension
{
    internal static T GetParentElement<T>(this Element view) where T : Element
    {
        if (view.Parent is null)
            return null;
        else if (view.Parent is T)
            return view.Parent as T;
        else
            return view.Parent.GetParentElement<T>();
    }
}
