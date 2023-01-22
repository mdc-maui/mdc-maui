namespace SampleApp.Panels;

public partial class OverviewPanel : ContentView
{
    public OverviewPanel()
    {
        this.InitializeComponent();
    }

    private void Button_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        Application.Current.UserAppTheme =
            Application.Current.UserAppTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
    }

    private void Chip_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        var chip = sender as Material.Components.Maui.Chip;
        chip.IsChecked = !chip.IsChecked;
    }

    private void FAB_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        var fab = sender as Material.Components.Maui.FAB;
        fab.IsExtended = !fab.IsExtended;
    }
}
