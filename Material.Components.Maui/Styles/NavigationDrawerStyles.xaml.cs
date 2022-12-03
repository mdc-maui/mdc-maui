namespace Material.Components.Maui.Styles;

public partial class NavigationDrawerStyles : ResourceDictionary
{
    public NavigationDrawerStyles()
    {
        InitializeComponent();
    }

    private void MenuBtn_Clicked(object sender, SKTouchEventArgs e)
    {
        var btn = sender as ITouchElement;
        btn.CommandParameter = !(bool)btn.CommandParameter;
    }

    private void ToolBarContextMenuBtn_Clicked(object sender, SKTouchEventArgs e)
    {
        var element = sender as IContextMenu;
        element.ContextMenu?.Show(sender as View);
    }
}
