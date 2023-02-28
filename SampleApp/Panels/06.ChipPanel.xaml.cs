namespace SampleApp.Panels;

public partial class ChipPanel : ContentView
{
    public ChipPanel()
    {
        this.InitializeComponent();
    }

    private void Chip_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        var chip = sender as Material.Components.Maui.Chip;
        chip.IsChecked = !chip.IsChecked;
    }
}
