using CommunityToolkit.Mvvm.ComponentModel;
using Material.Components.Maui;

namespace SampleApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        try
        {
            this.InitializeComponent();
        }
        catch (Exception)
        {

        }
    }

    private void Chip_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        var chip = sender as Chip;
        chip.IsSelected = !chip.IsSelected;
    }

    private void FAB_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        var fab = sender as FAB;
        fab.IsExtended = !fab.IsExtended;
    }

    private void Button_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        Application.Current.UserAppTheme =
            Application.Current.RequestedTheme == AppTheme.Light ?
            AppTheme.Dark :
            AppTheme.Light;
    }
}

public partial class ViewModel : ObservableObject
{

}

