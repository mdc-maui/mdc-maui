using System.Diagnostics;

namespace Sample;

public partial class App : Application
{
    public App()
    {
        try
        {
            this.InitializeComponent();
            //this.UserAppTheme = AppTheme.Dark;
            this.MainPage = new MainPage();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
}
