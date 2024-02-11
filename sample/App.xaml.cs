using SampleApp.Pages;

namespace SampleApp;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();

        this.MainPage = new MainPage();
    }
}
