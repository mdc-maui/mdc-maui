
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IconPacks.Material;

namespace SampleApp.ViewModels;
public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = "astator1";

    [ObservableProperty]
    private IconKind themeIcon;

    public MainPageViewModel()
    {
        this.ThemeIcon = Application.Current.RequestedTheme is AppTheme.Dark
                ? IconKind.LightMode
                : IconKind.DarkMode;

        Application.Current.RequestedThemeChanged += (s, e) =>
        {
            this.ThemeIcon = Application.Current.RequestedTheme is AppTheme.Dark
                 ? IconKind.LightMode
                 : IconKind.DarkMode;
        };
    }

    [RelayCommand]
    public static void ThemeChanged()
    {
        Application.Current.UserAppTheme =
            Application.Current.RequestedTheme is AppTheme.Dark
                ? AppTheme.Light
                : AppTheme.Dark;
    }
}
