namespace SampleApp.Pages;

public partial class ContextMenusPage : ContentPage
{
	public ContextMenusPage()
	{
		InitializeComponent();
	}

    private void MenuItem_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        var item = sender as Material.Components.Maui.MenuItem;
        this.Hint.Text = $"you selected item text: {item.Text}";
    }
}