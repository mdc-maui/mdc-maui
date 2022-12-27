using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SampleApp.ViewModels;

internal partial class ChipVM : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<PropertyDescribe> describes =
        new()
        {
            new PropertyDescribe(
                "IsChecked",
                "bool",
                "get or set isChecked of the chip(only support FilterChipStyle and FilterElevatedChipStyle)."
            ),
            new PropertyDescribe(
                "HasCloseIcon",
                "bool",
                "get or set show or hide close-icon of the chip."
            ),
            new PropertyDescribe(
                "Text",
                "string",
                "defines the text displayed as the content of the chip."
            ),
            new PropertyDescribe(
                "Icon",
                "IconKind",
                "specifies a icon to display as the content of the chip."
            ),
            new PropertyDescribe(
                "IconSource",
                "SkPicture",
                "specifies a svg to display as the content of the chip."
            ),
            new PropertyDescribe("IconColor", "Color", "defines the color of the chip's icon."),
            new PropertyDescribe(
                "BackgroundColour",
                "Color",
                "defines the color of the chip's background."
            ),
            new PropertyDescribe(
                "ForegroundColor",
                "Color",
                "defines the color of the chip's foreground."
            ),
            new PropertyDescribe(
                "FontFamily",
                "string",
                "defines the font family of the chip's text."
            ),
            new PropertyDescribe("FontSize", "float", "defines the font size of the chip's text."),
            new PropertyDescribe(
                "FontWeight",
                "int",
                "defines the font weight of the chip's text."
            ),
            new PropertyDescribe(
                "FontItalic",
                "bool",
                "defines the font of the chip's text is italic."
            ),
            new PropertyDescribe(
                "Shape",
                "Shape",
                "defines the corner radius of the chip's border."
            ),
            new PropertyDescribe("Elevation", "int", "defines the elevation height of the chip."),
            new PropertyDescribe("OutlineWidth", "int", "defines the width of the chip's border."),
            new PropertyDescribe(
                "OutlineColor",
                "Color",
                "defines the color of the chip's border."
            ),
            new PropertyDescribe(
                "RippleColor",
                "Color",
                "defines the color of the chip's ripple effect."
            ),
            new PropertyDescribe(
                "Command",
                "ICommand",
                "defines the command that's executed when the chip is clicked."
            ),
            new PropertyDescribe(
                "CommandParameter",
                "object",
                "is the parameter that's passed to Command."
            ),
        };
}
