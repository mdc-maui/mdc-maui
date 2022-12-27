using CommunityToolkit.Mvvm.ComponentModel;

namespace SampleApp.ViewModels;

internal partial class PropertyDescribe : ObservableObject
{
    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string type;

    [ObservableProperty]
    private string describe;

    public PropertyDescribe(string name, string type, string describe)
    {
        this.Name = name;
        this.Type = type;
        this.Describe = describe;
    }
}
