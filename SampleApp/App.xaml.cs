namespace SampleApp;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
        var root = new NavigationPage(new MainPage())
        {
            BarBackgroundColor = Material.Components.Maui.Tokens.MaterialColors.Primary,
            BarTextColor = Material.Components.Maui.Tokens.MaterialColors.OnPrimary,
        };

        this.MainPage = root;
    }
}
