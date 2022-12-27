namespace SampleApp.Panels;

public partial class TabsPanel : ContentView
{
    public TabsPanel()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        var btn = (Material.Components.Maui.Button)sender;
        btn.Text += "~~~";
    }
}
