# Chip

Chips are compact elements that represent an input, attribute, or action.

![](/assets/chips.png)



## Styles

There are 7 styles of chips: 1. Assist,  2. AssistElevated,  3. Filter,  4. FilterElevated 5. Input,  6. Suggestion,  7. SuggestionElevated.

## Examples

```xml
<mdc:Chip Text="chip" Style="{DynamicResource AssistChipStyle}" />
<mdc:Chip Text="chip" Style="{DynamicResource AssistElevatedChipStyle}" />
<mdc:Chip Text="chip" Style="{DynamicResource FilterChipStyle}" />
<mdc:Chip Text="chip" Style="{DynamicResource FilterElevatedChipStyle}" />
<mdc:Chip Text="chip" Style="{DynamicResource InputChipStyle}" />
<mdc:Chip Text="chip" Style="{DynamicResource SuggestionChipStyle}" />
<mdc:Chip Text="chip" Style="{DynamicResource SuggestionElevatedChipStyle}" />
```





## Properties

| name             | type      | default | describes                                                    |
| ---------------- | --------- | ------- | ------------------------------------------------------------ |
| IsChecked        | bool      | false   | chip's selected state(only support Filter and FilterElevated style). |
| HasCloseIcon     | bool      | false   | show or hide close-icon of the chip.                         |
| Text             | string    | empty   | chip's text.                                                 |
| IconKind         | IconKind  | none    | chip's icon from iconkind.                                   |
| IconSource       | SkPicture |         | chip's icon from file.                                       |
| IconData         | string    | empty   | chip's icon from path data.                                  |
| IconColor        | Color     | style   | chip's icon color                                            |
| BackgroundColour | Color     | style   | chip's background color                                      |
| ForegroundColor  | Color     | style   | chip's foreground color                                      |
| FontFamily       | string    |         | font family of the chip's text.                              |
| FontSize         | float     | 14      | font size of the chip's text.                                |
| FontWeight       | int       | 400     | font weight of the chip's text.                              |
| FontItalic       | bool      | false   | enable font italic of the chip's text.                       |
| Shape            | Shape     | Small   | corner radius of the chip's border.                          |
| Elevation        | int       | style   | chip's elevation.                                            |
| OutlineWidth     | int       | style   | chip's border width.                                         |
| OutlineColor     | Color     | style   | chip's border color.                                         |
| RippleColor      | Color     | style   | chip's ripple color.                                         |
| Style            | Style     | Assist  | chip's style                                                 |
| Command          | ICommand  |         | executed when the chip is clicked.                           |
| CommandParameter | object    |         | Command's parameter.                                         |



## Events

| name           | type                                    |
| -------------- | --------------------------------------- |
| CheckedChanged | `EventHandler<CheckedChangedEventArgs>` |
| Clicked        | `EventHandler<SKTouchEventArgs>`        |
| Pressed        | `EventHandler<SKTouchEventArgs>`        |
| Released       | `EventHandler<SKTouchEventArgs>`        |
| Moved          | `EventHandler<SKTouchEventArgs>`        |
| LongPressed    | `EventHandler<SKTouchEventArgs>`        |
| Entered        | `EventHandler<SKTouchEventArgs>`        |
| Exited         | `EventHandler<SKTouchEventArgs>`        |

