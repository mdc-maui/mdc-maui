# Icon packs

 mdc-maui has provided a selection of icon packs in [here](https://github.com/mdc-maui/IconPacks.NET).



## Examples


### Istall nuget packge
```powershell
dotnet add package IconPacks.Material --version 1.0.8732.5-build

```

### Add namespace in xaml file. 

```xml
xmlns:icon="clr-namespace:IconPacks.IconKind;assembly=IconPacks.Material"
```

### Usage

```xml
<md:Chip IconData="{Static icon:Material.Add}" Text="chip" />
```

