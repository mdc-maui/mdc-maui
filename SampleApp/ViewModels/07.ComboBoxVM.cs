using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SampleApp.ViewModels;

internal partial class ComboBoxVM : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<PropertyDescribe> describes =
        new()
        {
            new PropertyDescribe(
                "Items",
                "ItemCollection<ComboBoxItem>",
                "get or set the list of items to display."
            ),
            new PropertyDescribe(
                "ItemsSource",
                "IList",
                "get or set the source list of items to display."
            ),
            new PropertyDescribe(
                "SelectedIndex",
                "int",
                "get or set the current selected item of the comboBox."
            ),
            new PropertyDescribe(
                "LabelText",
                "string",
                "get or set the label-text of the comboBox."
            ),
            new PropertyDescribe(
                "LabelTextColor",
                "Color",
                "defines the color of the comboBox's label-text."
            ),
            new PropertyDescribe(
                "ActiveIndicatorHeight",
                "int",
                "defines the height of the comboBox's active-indicator."
            ),
            new PropertyDescribe(
                "ActiveIndicatorColor",
                "Color",
                "defines the color of the comboBox's active-indicator."
            ),
            new PropertyDescribe(
                "BackgroundColour",
                "Color",
                "defines the color of the comboBox's background."
            ),
            new PropertyDescribe(
                "ForegroundColor",
                "Color",
                "defines the color of the comboBox's foreground."
            ),
            new PropertyDescribe(
                "FontFamily",
                "string",
                "defines the font family of the comboBox's text."
            ),
            new PropertyDescribe(
                "FontSize",
                "float",
                "defines the font size of the comboBox's text."
            ),
            new PropertyDescribe(
                "FontWeight",
                "int",
                "defines the font weight of the comboBox's text."
            ),
            new PropertyDescribe(
                "FontItalic",
                "bool",
                "defines the font of the chip's comboBox is italic."
            ),
            new PropertyDescribe(
                "Shape",
                "Shape",
                "defines the corner radius of the comboBox's border."
            ),
            new PropertyDescribe(
                "OutlineWidth",
                "int",
                "defines the width of the comboBox's border."
            ),
            new PropertyDescribe(
                "OutlineColor",
                "Color",
                "defines the color of the comboBox's border."
            ),
            new PropertyDescribe(
                "RippleColor",
                "Color",
                "defines the color of the comboBox's ripple effect."
            ),
            new PropertyDescribe(
                "Command",
                "ICommand",
                "defines the command that's executed when the comboBox is clicked."
            ),
            new PropertyDescribe(
                "CommandParameter",
                "object",
                "is the parameter that's passed to Command."
            ),
        };

    [ObservableProperty]
    private ObservableCollection<PropertyDescribe> itemDescribes =
        new()
        {
            new PropertyDescribe(
                "Text",
                "string",
                "defines the text displayed as the content of the comboBoxItem."
            ),
            new PropertyDescribe(
                "BackgroundColour",
                "Color",
                "defines the color of the comboBoxItem's background."
            ),
            new PropertyDescribe(
                "ForegroundColor",
                "Color",
                "defines the color of the comboBoxItem's foreground."
            ),
            new PropertyDescribe(
                "FontFamily",
                "string",
                "defines the font family of the comboBoxItem's text."
            ),
            new PropertyDescribe(
                "FontSize",
                "float",
                "defines the font size of the comboBoxItem's text."
            ),
            new PropertyDescribe(
                "FontWeight",
                "int",
                "defines the font weight of the comboBoxItem's text."
            ),
            new PropertyDescribe(
                "FontItalic",
                "bool",
                "defines the font of the comboBoxItem's text is italic."
            ),
            new PropertyDescribe(
                "RippleColor",
                "Color",
                "defines the color of the button's ripple effect."
            ),
            new PropertyDescribe(
                "Command",
                "ICommand",
                "defines the command that's executed when the button is clicked."
            ),
            new PropertyDescribe(
                "CommandParameter",
                "object",
                "is the parameter that's passed to Command."
            ),
        };

    [ObservableProperty]
    private string testString = "ggggggggggggg";
}
