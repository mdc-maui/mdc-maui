# Button

Common buttons prompt most actions in a UI.



- Can contain an optional leading icon.

- Five styels: FilledButtonStyle, ElevatedButtonStyle, FilledTonalButtonStyle, OutlinedButtonStyle, and TextButtonStyle.

- Keep labels concise and in sentence-case.

- Containers have fully rounded corners and are wide enough to fit label text.

  

![](/assets/buttons.png)



## Examples

```xml
<md:Button Style="{DynamicResource ElevatedButtonStyle}" Text="Elevated" />
<md:Button Style="{DynamicResource FilledButtonStyle}" Text="Filled" />
<md:Button Style="{DynamicResource FilledTonalButtonStyle}" Text="FilledTonal" />
<md:Button Style="{DynamicResource OutlinedButtonStyle}" Text="Outlined" />
<md:Button Style="{StaticResource TextButtonStyle}" Text="Text" />
```



## Properties

| name             | type        | default  |
| ---------------- | ----------- | -------- |
| Text             | string      | empty    |
| IconData         | string      | empty    |
| BackgroundColor  | Color       | style    |
| FontColor        | Color       | style    |
| FontSize         | float       | 14       |
| FontFamily       | string      |          |
| FontWeight       | FontWeight  | Regular  |
| FontIsItalic     | bool        | false    |
| Shape            | Shape       | style    |
| Elevation        | Elevation   | style    |
| OutlineWidth     | int         | style    |
| OutlineColor     | Color       | style    |
| StateLayerColor  | Color       | style    |
| RippleDuration   | float       | 0.5      |
| RippleEasing     | Easing      | SinInOut |
| ContextMenu      | ContextMenu |          |
| Style            | Style       | Filled   |
| Command          | ICommand    |          |
| CommandParameter | object      |          |




## Events

| name                        | type                           |
| --------------------------- | ------------------------------ |
| Clicked                     | `EventHandler<TouchEventArgs>` |
| Pressed                     | `EventHandler<TouchEventArgs>` |
| Released                    | `EventHandler<TouchEventArgs>` |
| LongPressed                 | `EventHandler<TouchEventArgs>` |
| RightClicked ( desktop only ) | `EventHandler<TouchEventArgs>` |

