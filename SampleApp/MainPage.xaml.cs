using CommunityToolkit.Mvvm.ComponentModel;
using Material.Components.Maui;

namespace SampleApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        this.InitializeComponent();
    }

    private void FAB_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        var fab = sender as FAB;
        fab.IsExtended = !fab.IsExtended;
    }

    private void Button_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        Application.Current.UserAppTheme =
           Application.Current.RequestedTheme is AppTheme.Light ?
           AppTheme.Dark :
           AppTheme.Light;
    }

    private void Chip_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        var chip = sender as Chip;
        chip.IsChecked = !chip.IsChecked;
    }
}

public partial class ViewModel : ObservableObject
{

}

