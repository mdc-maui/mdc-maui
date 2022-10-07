namespace Material.Components.Maui.Extensions;
public static class PopupExtensions
{
    public static void ShowPopup(this Page page, IPopup popup)
    {
        if (popup.Content is not null)
        {
            popup.Show(page.Handler.MauiContext);
        }
    }

    public static void ShowPopup(this View view, IPopup popup)
    {
        if (view.Handler is not null && popup.Content is not null && popup.Anchor is not null)
        {
            popup.Show(view.Handler.MauiContext);
        }
    }
}
