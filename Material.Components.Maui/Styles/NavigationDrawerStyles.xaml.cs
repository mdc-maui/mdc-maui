namespace Material.Components.Maui.Styles;

public partial class NavigationDrawerStyles : ResourceDictionary
{
    public NavigationDrawerStyles()
    {
        this.InitializeComponent();
    }

    private void MenuBtn_Clicked(object sender, SKTouchEventArgs e)
    {
        var parent = sender as Element;
        while (parent != null)
        {
            if (parent is NavigationDrawer nd)
            {
                nd.IsPaneOpen = !nd.IsPaneOpen;
                break;
            }
            parent = parent.Parent;
        }
    }
}
