# FAB

Floating action buttons (FABs) help people take primary actions.



- Use a FAB for the most common or important action on a screen.

- Make sure the icon in a FAB is clear and understandable.

- FABs persist on the screen when content is scrolling.

- Three styles: SecondaryFABStyle, SurfaceFABStyle and TertiaryFABStyle.

  

![](/assets/FABs.png)



## Examples

```xml
<...
	xmlns:icon="clr-namespace:IconPacks.IconKind;assembly=IconPacks.Material"
...>

<md:FAB IconData="{Static icon:Material.Add}" Style="{DynamicResource SecondaryFABStyle}" />
<md:FAB IconData="{Static icon:Material.Add}" Style="{DynamicResource SurfaceFABStyle}" />
<md:FAB IconData="{Static icon:Material.Add}" Style="{DynamicResource TertiaryFABStyle}" />
```



## Properties

| name                             | type        | default |
| -------------------------------- | ----------- | ------- |
| IconData                         | string      |         |
| BackgroundColor                  | Color       | style   |
| Shape                            | Shape       | Large   |
| Elevation                        | Elevation         | Level3   |
| StateLayerColor                      | Color       | style   |
| RippleDuration   | float       | 0.5      |
| RippleEasing     | Easing      | SinInOut |
| ContextMenu                      | ContextMenu |         |
| Style                            | Style       | Surface  |
| Command                          | ICommand    |         |
| CommandParameter                 | object      |         |



## Events

| name                        | type                           |
| --------------------------- | ------------------------------ |
| Clicked                     | `EventHandler<TouchEventArgs>` |
| Pressed                     | `EventHandler<TouchEventArgs>` |
| Released                    | `EventHandler<TouchEventArgs>` |
| LongPressed                 | `EventHandler<TouchEventArgs>` |
| RightClicked ( desktop only ) | `EventHandler<TouchEventArgs>` |