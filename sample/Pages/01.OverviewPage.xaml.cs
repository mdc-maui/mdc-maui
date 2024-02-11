using Material.Components.Maui;

namespace SampleApp.Pages;

public partial class OverviewPage : ContentView
{
    public OverviewPage()
    {
        this.InitializeComponent();
    }

    private void TextField_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextField view)
            view.IsError = view.Text != "mdc-maui";
    }

    private void TextField_TrailingIconClicked(object sender, EventArgs e)
    {
        if (sender is TextField view)
            view.Text = string.Empty;
    }
}
