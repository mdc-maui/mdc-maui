using CommunityToolkit.Mvvm.ComponentModel;
using Material.Components.Maui;
using Microsoft.Maui.Controls;
using SampleApp.Pages;
using SkiaSharp.Views.Maui;

namespace SampleApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        this.InitializeComponent();
    }

    private void Btn_Clicked(object sender, SKTouchEventArgs e)
    {
        var btn = sender as Material.Components.Maui.Button;
        Page page = btn.Text switch
        {
            "Buttons" => new ButtonsPage(),
            "Cards" => new CardsPage(),
            "CheckBoxs" => new CheckBoxsPage(),
            "Chips" => new ChipsPage(),
            "ComboBoxs" => new ComboBoxsPage(),
            "ContextMenus" => new ContextMenusPage(),
            "FABs" => new FABsPage(),
            "NavigtionBar" => new NavigtionBarPage(),
            "Popup" => new PopupPage(),
            "ProgressIndicators" => new ProgressIndicatorsPage(),
            "RadioButtons" => new RadioButtonsPage(),
            "Switchs" => new SwitchsPage(),
            "Tabs" => new TabsPage(),
            _ => null,
        };

        if (page != null)
        {
            this.Navigation.PushAsync(page, true);
        }
    }
}

