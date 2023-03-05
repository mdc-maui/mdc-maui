namespace SampleApp;

public partial class MainPage : ContentPage, IVisualTreeElement
{
    public MainPage()
    {
        try
        {
            this.InitializeComponent();

            this.ThemeSwitch.IconKind =
                Application.Current.RequestedTheme == AppTheme.Light
                    ? IconPacks.Material.IconKind.LightMode
                    : IconPacks.Material.IconKind.DarkMode;

            Application.Current.RequestedThemeChanged += (s, e) =>
            {
                this.ThemeSwitch.IconKind =
                    e.RequestedTheme == AppTheme.Light
                        ? IconPacks.Material.IconKind.LightMode
                        : IconPacks.Material.IconKind.DarkMode;
            };
        }
        catch (Exception ex) { }
    }

    private void ThemeSwitch_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        Application.Current.UserAppTheme =
            Application.Current.RequestedTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.Content != null
            ? new List<IVisualTreeElement> { this.Content }
            : Array.Empty<IVisualTreeElement>().ToList();

    public IVisualTreeElement GetVisualParent() => null;
}
