using SampleApp.Views;

namespace SampleApp.Panels;

public partial class PopupPanel : ContentView
{
    public PopupPanel()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        var btn = sender as Material.Components.Maui.Button;

        var horizontalOptions = btn.CommandParameter switch
        {
            "left-top" or "left-center" or "left-bottom" => LayoutAlignment.Start,
            "right-top" or "right-center" or "right-bottom" => LayoutAlignment.End,
            _ => LayoutAlignment.Center,
        };

        var verticalOptions = btn.CommandParameter switch
        {
            "left-top" or "center-top" or "right-top" => LayoutAlignment.Start,
            "left-bottom" or "center-bottom" or "right-bottom" => LayoutAlignment.End,
            _ => LayoutAlignment.Center,
        };

        var popup = new Dialog
        {
            HorizontalOptions = horizontalOptions,
            VerticalOptions = verticalOptions
        };

        var parent = this.Parent;
        while (parent is not Page)
        {
            parent = parent.Parent;
        }
        var result = await popup.ShowAtAsync((Page)parent);
        this.Hint.Text = $"popup result: {result}";
    }
}
