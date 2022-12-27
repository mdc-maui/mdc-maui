using Material.Components.Maui;

namespace SampleApp.Views;

public partial class Dialog : Popup
{
    public Dialog()
    {
        InitializeComponent();
    }

    private void Cancel_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        this.Close("Cancel");
    }

    private void Confirm_Clicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        this.Close("Confirm");
    }
}
