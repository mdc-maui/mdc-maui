using Grid = Microsoft.Maui.Controls.Grid;

namespace Material.Components.Maui.Styles;

public partial class NavigationDrawerStyles : ResourceDictionary
{
    public NavigationDrawerStyles()
    {
        this.InitializeComponent();
    }

    Popup drawer;

    private Point startPosition = Point.Zero;

    private void OnPointPressed(object sender, PointerEventArgs e)
    {
        this.startPosition = e.GetPosition(sender as View).Value;
    }

    private void OnPointReleased(object sender, PointerEventArgs e)
    {
        var endPosition = e.GetPosition(sender as View).Value;
        var startX = this.startPosition.X;
        var startY = this.startPosition.Y;
        var endX = endPosition.X;
        var endY = endPosition.Y;

        if (startX < 100 && endX - startX > 100 && Math.Abs(endY - startY) < 50)
        {
            var grid = sender as Grid;

            this.drawer ??= NavigationDrawer.CreateDrawer(grid);

            _ = this.drawer.ShowAtAsync(grid.GetParentElement<ContentPage>());
        }
    }
}
