# ExtendedFAB

Extended floating action buttons (extended FABs) help people take primary actions.



- Use an extended FAB for the most common or important action on a screen.

- Contains both an icon and label text.

- The most prominent type of button.

- Use when a regular FAB (with just an icon) may not be clear.

- Three styles: SecondaryExtendedFABStyle, SurfaceExtendedFABStyle and TertiaryExtendedFABStyle.

  

![](/assets/FABs.png)



## Examples

```xml
<...
	xmlns:icon="clr-namespace:IconPacks.IconKind;assembly=IconPacks.Material"
...>

<mdc:ExtendedFAB IconData="{Static icon:Material.Add}" Text="Extended" Style="{DynamicResource SecondaryExtendedFABStyle}" />
<mdc:ExtendedFAB IconData="{Static icon:Material.Add}" Text="Extended" Style="{DynamicResource SurfaceExtendedFABStyle }" />
<mdc:ExtendedFAB IconData="{Static icon:Material.Add}" Text="Extended" Style="{DynamicResource TertiaryExtendedFABStyle}" />
```



## Properties

| name             | type        | defalut  |
| ---------------- | ----------- | -------- |
| Text             | string      |          |
| IconData         | string      |          |
| BackgroundColor  | Color       | style    |
| FontColor        | Color       | style    |
| FontSize         | float       | 14       |
| FontFamily       | string      |          |
| FontWeight       | FontWeight  | Medium   |
| FontIsItalic     | bool        | false    |
| Shape            | Shape       | Large    |
| Elevation        | Elevation   | Level3   |
| StateLayerColor  | Color       | style    |
| RippleDuration   | float       | 0.5      |
| RippleEasing     | Easing      | SinInOut |
| ContextMenu      | ContextMenu |          |
| Style            | Style       | Surface  |
| Command          | ICommand    |          |
| CommandParameter | object      |          |



## Events

| name                          | type                           |
| ----------------------------- | ------------------------------ |
| Clicked                       | `EventHandler<TouchEventArgs>` |
| Pressed                       | `EventHandler<TouchEventArgs>` |
| Released                      | `EventHandler<TouchEventArgs>` |
| LongPressed                   | `EventHandler<TouchEventArgs>` |
| RightClicked ( desktop only ) | `EventHandler<TouchEventArgs>` |
