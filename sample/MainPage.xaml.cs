using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IconPacks.Material;
using Material.Components.Maui.FluentExtensions;
using Material.Components.Maui.Tokens;
using System.ComponentModel;

namespace SampleApp;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    public MainPage()
    {
        try
        {
            this.InitializeComponent();
            var vm = new MainViewModel();
            this.BindingContext = vm;
            var button = new Material.Components.Maui.Button()
                .BindText("Text")
                .TextColor(Colors.Blue)
                .IconData(IconKind.Add)
                .Shape(Shape.LargeTop)
                .BindCommand("ClickCommand");

            this.Stack.Add(button);
        }
        catch (Exception ex) { }
    }
}

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    string text = "View model binding";

    public MainViewModel()
    {
        Task.Run(async () =>
        {
            await Task.Delay(5000);
            this.Text = "变身就好哈哈哈变身就好哈哈哈变身就好哈哈哈";
        });
    }

    [RelayCommand]
    void Click()
    {
        this.Text = "我被点了";
    }
}
