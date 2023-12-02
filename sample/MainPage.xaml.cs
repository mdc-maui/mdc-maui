namespace Sample;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        this.InitializeComponent();
    }

    private void Button_Clicked(object sender, TouchEventArgs e)
    {
        var dlg = new Dialog { Parent = this };

        _ = dlg.ShowAtAsync(this);
    }
}
