using Material.Components.Maui;

namespace SampleApp.Pages;

public partial class PopupPage : ContentPage
{
    public PopupPage()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        var btn1 = new Material.Components.Maui.Button
        {
            Text = "result abc"
        };
        var btn2 = new Material.Components.Maui.Button
        {
            Text = "result 123"
        };

        var popup = new Popup
        {
            Content = new Card
            {
                Content = new HorizontalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                    Padding = new Thickness(100,200,100,50),
                    Spacing = 80,
                    Children =
                    {
                        btn1,
                        btn2
                    }
                }
            }
        };

        btn1.Clicked += (s, e) => popup.Close("abc");
        btn2.Clicked += (s, e) => popup.Close("123");

        var result = await popup.ShowAtAsync(this);
        this.Hint.Text = $"popup result: {result}";
    }
}