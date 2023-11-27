using Grid = Microsoft.Maui.Controls.Grid;

namespace Material.Components.Maui.Styles;

public partial class NavigationDrawerStyles : ResourceDictionary
{
    public NavigationDrawerStyles()
    {
        this.InitializeComponent();
    }

    Popup popup;

    private async void OnSwiped(object sender, SwipedEventArgs e)
    {
        var grid = sender as Grid;

        this.popup ??= NavigationDrawer.CreatePopup(grid);

        await this.popup.ShowAtAsync(grid.GetParentElement<ContentPage>());
    }
}
