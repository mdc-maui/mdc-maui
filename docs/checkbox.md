# CheckBox

CheckBox allow the user to select options.

![](/assets/check-boxs.png)

## Examples

```xml
<mdc:CheckBox Text="checkbox" />
```





## Properties

| name             | type     | default | describes                                   |
| ---------------- | -------- | ------- | ------------------------------------------- |
| IsChecked        | bool     | false   | checkBox's selected state.                  |
| Text             | string   | empty   | checkBox's text.                            |
| OnColor          | Color    | style   | checkBox's box color when checked.          |
| MarkColor        | Color    | style   | checkBox's checkmark color when checked.    |
| BackgroundColour | Color    | style   | checkBox's background color.                |
| ForegroundColor  | Color    | style   | checkBox's foreground color.                |
| FontFamily       | string   |         | font family of the checkBox's text.         |
| FontSize         | float    | 14      | font size of the checkBox's text.           |
| FontWeight       | int      | 400     | font weight of the checkBox's text.         |
| FontItalic       | bool     | false   | enable font italic of the checkBox's text.  |
| RippleColor      | Color    | style   | button's ripple color.                      |
| Command          | ICommand |         | executed when the checkBox is checkchanged. |
| CommandParameter | object   |         | Command's parameter.                        |



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
