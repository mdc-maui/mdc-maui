# Chips

Chips help people enter information, make selections, filter content, or trigger actions.



- Use chips to show options for a specific context.
- Four types: AssistChipStyle, FilterChipStyle, InputChipStyle, and SuggestionChipStyle.
- Chip elevation defaults to 0 but can be elevated if they need more visual separation.

![](/assets/chips.png)



## Examples

```xml
<md:Chip Text="chip" Style="{DynamicResource AssistChipStyle}" />
<md:Chip Text="chip" Style="{DynamicResource FilterChipStyle}" />
<md:Chip Text="chip" Style="{DynamicResource InputChipStyle}" />
<md:Chip Text="chip" Style="{DynamicResource SuggestionChipStyle}" />
```





## Properties

| name             | type        | default  |
| ---------------- | ----------- | -------- |
| IsSelected       | bool        | false    |
| HasCloseButton   | bool        | style    |
| Text             | string      | empty    |
| IconData         | string      | empty    |
| IconColor        | Color       | style    |
| BackgroundColor  | Color       | style    |
| FontColor        | Color       | style    |
| FontFamily       | string      |          |
| FontSize         | float       | 14       |
| FontWeight       | FontWeight  | Regular  |
| FontIsItalic     | bool        | false    |
| Shape            | Shape       | Small    |
| Elevation        | Elevation   | style    |
| OutlineWidth     | int         | style    |
| OutlineColor     | Color       | style    |
| StateLayerColor  | Color       | style    |
| RippleDuration   | float       | 0.5      |
| RippleEasing     | Easing      | SinInOut |
| ContextMenu      | ContextMenu |          |
| Style            | Style       | Assist   |
| Command          | ICommand    |          |
| CommandParameter | object      |          |



## Events

| name                          | type                                    |
| ----------------------------- | --------------------------------------- |
| SelectedChanged               | `EventHandler<CheckedChangedEventArgs>` |
| Closed                        | `EventHandler<EventArgs>`               |
| Clicked                       | `EventHandler<TouchEventArgs>`          |
| Pressed                       | `EventHandler<TouchEventArgs>`          |
| Released                      | `EventHandler<TouchEventArgs>`          |
| LongPressed                   | `EventHandler<TouchEventArgs>`          |
| RightClicked ( desktop only ) | `EventHandler<TouchEventArgs>`          |

