# Material.Components.Maui

English | [中文](README_zh.md)

Material3 Components for .NET MAUI, powered by SkiaSharp

![](assets/preview.gif)



## Getting Started

- Clone this repo and ref it to your maui project
- UseMaterialComponents in your "MauiProgram.cs"

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
                //generally, we needs add 5 types of font families
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

- Use Material colors&styles in  your "App.xaml"

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
| button    | 😄 | 😄 | 🤔 |
| IconButton | 😄 | 😄 |🤔|
| Card | 😄 | 😄 |🤔|
| CheckBox | 😄 | 😄 |🤔|
| Chip | 😄 | 😄 |🤔|
| ComboBox | 😄 | 😄 |😭|
| FAB | 😄 | 😄 |🤔|
| Label | 😄 | 😄 |🤔|
| NavigationBar | 😄 | 😄 |🤔|
| RadioButton | 😄 | 😄 |🤔|
| Switch | 😄 | 😄 |🤔|

# Contributing

Plan on contributing to the repository? We're glad to have you



## License

MIT



## Documentation

TODO





