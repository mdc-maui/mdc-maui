using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SampleApp.ViewModels;

internal partial class ContextMenuVM : ObservableObject
{

    [ObservableProperty]
    private string testString = "you selected item text: empty";

    [RelayCommand]
    public void OnMenuItemClicked(object commandParameter)
    {
        this.TestString = $"you selected item text: {commandParameter as string}";
    }
}