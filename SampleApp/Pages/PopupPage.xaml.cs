using Material.Components.Maui;
using SampleApp.Views;

namespace SampleApp.Pages;

public partial class PopupPage : ContentPage
{
    public PopupPage()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        var btn = sender as Material.Components.Maui.Button;

        var horizontalOptions = btn.CommandParameter switch
        {
            "left-top"
             or "left-center"
             or "left-bottom"
                => LayoutAlignment.Start,
            "center-top"
            or "center"
            or "center-bottom"
                => LayoutAlignment.Center,
            "right-top"
            or "right-center"
            or "right-bottom"
                => LayoutAlignment.End,
        };

        var verticalOptions = btn.CommandParameter switch
        {
            "left-top"
             or "center-top"
             or "right-top"
                => LayoutAlignment.Start,
            "left-center"
            or "center"
            or "right-center"
                => LayoutAlignment.Center,
            "left-bottom"
            or "center-bottom"
            or "right-bottom"
                => LayoutAlignment.End,
        };

        var popup = new Dialog
        {
            HorizontalOptions = horizontalOptions,
            VerticalOptions = verticalOptions
        };
        var result = await popup.ShowAtAsync(this);
        this.Hint.Text = $"popup result: {result}";
    }
}