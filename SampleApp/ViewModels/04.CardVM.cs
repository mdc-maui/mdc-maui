using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SampleApp.ViewModels;

internal partial class CardVM : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<PropertyDescribe> describes =
        new()
        {
            new PropertyDescribe("Content", "View", "defines the content view of the card."),
            new PropertyDescribe("EnableTouchEvents", "bool", "enable touch events of the card."),
            new PropertyDescribe(
                "BackgroundColour",
                "Color",
                "defines the color of the card's background."
            ),
            new PropertyDescribe(
                "Shape",
                "Shape",
                "defines the corner radius of the card's border."
            ),
            new PropertyDescribe("Elevation", "int", "defines the elevation height of the card."),
            new PropertyDescribe("OutlineWidth", "int", "defines the width of the card's border."),
            new PropertyDescribe(
                "OutlineColor",
                "Color",
                "defines the color of the card's border."
            ),
            new PropertyDescribe(
                "RippleColor",
                "Color",
                "defines the color of the card's ripple effect."
            ),
        };
}
