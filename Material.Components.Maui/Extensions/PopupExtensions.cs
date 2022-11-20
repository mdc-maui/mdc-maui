namespace Material.Components.Maui.Extensions;
public static class PopupExtensions
{
    public static void ShowPopup(this Page page, IPopup popup)
    {
        if (popup.Content != null)
        {
            popup.Show(page.Handler.MauiContext);
        }
    }

    public static void ShowPopup(this View view, IPopup popup)
    {
        if (view.Handler != null && popup.Content != null && popup.Anchor != null)
        {
            popup.Show(view.Handler.MauiContext);
        }
    }
}
