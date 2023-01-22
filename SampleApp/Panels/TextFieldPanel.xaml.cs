using Material.Components.Maui;

namespace SampleApp.Panels;

public partial class TextFieldPanel : ContentView
{
    public TextFieldPanel()
    {
        this.InitializeComponent();
    }


    private void TextField_TrailingIconClicked(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        var tf = sender as TextField;
        tf.Text = string.Empty;
    }

    private void TextField_TextChanged(object sender, TextChangedEventArgs e)
    {
        var tf = sender as TextField;
        var newText = e.NewTextValue;
        if (newText != "12345678")
        {
            tf.SupportingText = "Incorrect password";
            tf.IsError = true;
        }
        else
        {
            tf.SupportingText = "Supporting text";
            tf.IsError = false;
        }
    }
}