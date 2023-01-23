
# Material.Components.Maui
[![version](https://img.shields.io/nuget/vpre/Material.Components.Maui?style=for-the-badge)](https://www.nuget.org/packages/Material.Components.Maui/0.1.0-beta) 
[![downloads](https://img.shields.io/nuget/dt/Material.Components.Maui?style=for-the-badge)](https://www.nuget.org/packages/Material.Components.Maui/0.1.0-beta) 

English | [中文](README_zh.md)

![](assets/preview.png)



## Getting Started

- Add UseMaterialComponents in MauiProgram.cs

```C#
using Material.Components.Maui.Extensions;

namespace SampleApp;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMaterialComponents(new List<string>
            {
                //generally, we needs add 6 types of font families
                "xxx-Regular.ttf",
                "xxx-Italic.ttf",
                "xxx-Medium.ttf",
                "xxx-MediumItalic.ttf",
                "xxx-Bold.ttf",
                "xxx-BoldItalic.ttf",
            });
        return builder.Build();
    }
}
```

- Add Material colors & styles in App.xaml

```xaml
<?xml version="1.0" encoding="UTF-8" ?>
<Application
    x:Class="SampleApp.App"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:SampleApp"
    xmlns:mds="clr-namespace:Material.Components.Maui.Styles;assembly=Material.Components.Maui">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Resources/Styles/Colors.xaml" />
                <ResourceDictionary Source="Resources/Styles/Styles.xaml" />
                <mds:MaterialStyles />
                <!--or use seendColor
                <mds:MaterialStyles Dark="DarkBlue" Light="Green" />-->
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

- Let's go



## The available controls

> 😄: ready    🤔: unverified    😭: needs help

| control   | android    | windows   |  ios&mac   |
| ---- | ---- | ---- |----|
| button    | 😄 | 😄 | 😄 |
| IconButton | 😄 | 😄 |😄|
| Card | 😄 | 😄 |😄|
| CheckBox | 😄 | 😄 |😄|
| Chip | 😄 | 😄 |😄|
| ComboBox | 😄 | 😄 |😭|
| ContextMenu | 😄 | 😄 |😭|
| FAB | 😄 | 😄 |😄|
| Label | 😄 | 😄 |😄|
| NavigationBar | 😄 | 😄 |😭|
| NavigationDrawer | 😄 | 😄 |😭|
| Popup | 😄 | 😄 |😭|
| ProgressIndicator | 😄 | 😄 |😄|
| RadioButton | 😄 | 😄 |😄|
| SplitView | 😄 | 😄 |😭|
| Switch | 😄 | 😄 |😄|
| Tabs | 😄 | 😄 |😭|
| TextField | 😄 | 😄 |😭|
| WrapLayout | 😄 | 😄 |😄|



## Contributing

Plan on contributing to the repository? We're glad to have you



## License

MIT



## Documentation

TODO





