namespace SampleApp.Pages;

public partial class ChipsPage : ContentPage
{
	public ChipsPage()
	{
		InitializeComponent();
	}

    private void Chip_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        var chip = sender as Material.Components.Maui.Chip;
        chip.IsChecked = !chip.IsChecked;
    }
}