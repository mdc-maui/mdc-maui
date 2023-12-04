# NavigationBar

Navigation bars let people switch between UI views on smaller devices.



- Can contain 3-5 destinations of equal importance.

- Destinations don't change. They should be consistent across app screens.

- Used to be named bottom navigation.

  

![](/assets/navigation-bars.png)


## Examples

```xml
<...
	xmlns:icon="clr-namespace:IconPacks.IconKind;assembly=IconPacks.Material"
...>

<md:FAB IconData="{Static icon:Material.Star}" Style="{DynamicResource SecondaryFABStyle}" />

<md:NavigationBar>
    <md:NavigationBarItem IconData="{Static icon:Material.Star}" Text="label 1">
		...
	</md:NavigationBarItem>
    <md:NavigationBarItem IconData="{Static icon:Material.Star}" Text="label 2">
		...
	</md:NavigationBarItem>
</md:NavigationBar>
```





## Properties

| name             | type                                      | default |
| ---------------- | ----------------------------------------- | ------- |
| Items            | `ObservableCollection<NavigationBarItem>` |         |
| SelectedItem     | NavigationBarItem                         |         |
| Command          | ICommand                                  |         |
| CommandParameter | object                                    |         |



## Events

| name                | type                                                       |
| ------------------- | ---------------------------------------------------------- |
| SelectedItemChanged | `EventHandler<SelectedItemChangedArgs<NavigationBarItem>>` |





## NavigationBarItem Properties

| name                  | type      | default |
| --------------------- | --------- | ------- |
| Content               | View      |         |
| IsActived             | bool      |         |
| Text                  | string    | empty   |
| IconData              | string    | empty   |
| IconColor             | Color     |         |
| ActiveIndicatorHeight | int       | 32 |
| ActiveIndicatorColor  | Color     | SecondaryContainerColor |
| BackgroundColor      | Color     | SurfaceContainerColor |
| FontColor        | Color       | OnSurfaceVariantColor |
| FontSize         | float       | 14       |
| FontFamily       | string      |          |
| FontWeight       | FontWeight  | Medium |
| FontIsItalic     | bool        | false    |
| StateLayerColor  | Color       | OnSurfaceVariantColor |
| RippleDuration   | float       | 0.5      |
| RippleEasing     | Easing      | SinInOut |



## NavigationBarItem Events

| name                        | type                           |
| --------------------------- | ------------------------------ |
| Clicked                     | `EventHandler<TouchEventArgs>` |
| Pressed                     | `EventHandler<TouchEventArgs>` |
| Released                    | `EventHandler<TouchEventArgs>` |
| LongPressed                 | `EventHandler<TouchEventArgs>` |
| RightClicked ( desktop only ) | `EventHandler<TouchEventArgs>` |

