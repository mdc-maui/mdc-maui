using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SampleApp.ViewModels;

internal partial class CheckBoxVM : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<PropertyDescribe> describes =
        new()
        {
            new PropertyDescribe(
                "IsChecked",
                "bool",
                "defines indicates whether the checkBox is checked."
            ),
            new PropertyDescribe(
                "Text",
                "string",
                "defines the text displayed as the content of the button."
            ),
            new PropertyDescribe(
                "OnColor",
                "Color",
                "defines the color of the checkBox when checked."
            ),
            new PropertyDescribe(
                "MarkColor",
                "Color",
                "defines the color of the checkBox's checkMark."
            ),
            new PropertyDescribe(
                "BackgroundColour",
                "Color",
                "defines the color of the checkBox's background."
            ),
            new PropertyDescribe(
                "ForegroundColor",
                "Color",
                "defines the color of the checkBox's foreground."
            ),
            new PropertyDescribe(
                "Command",
                "ICommand",
                "defines the command that's executed when the button is checkchanged."
            ),
            new PropertyDescribe(
                "CommandParameter",
                "object",
                "is the parameter that's passed to Command."
            ),
        };
}
