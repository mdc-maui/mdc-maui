# Button

Buttons allow users to take actions, and make choices, with a single tap.

![](/assets/buttons.png)



## Styles

There are 5 Styles of buttons: 1. Elevated, 2. Filled,  3. FilledTonal , 4. Outlined, 5. Text.

## Examples

```xml
<mdc:Button Style="{DynamicResource ElevatedButtonStyle}" Text="Elevated" />
<mdc:Button Style="{DynamicResource FilledButtonStyle}" Text="Filled" />
<mdc:Button Style="{DynamicResource FilledTonalButtonStyle}" Text="FilledTonal" />
<mdc:Button Style="{DynamicResource OutlinedButtonStyle}" Text="Outlined" />
<mdc:Button Style="{StaticResource TextButtonStyle}" Text="Text" />
```



## Properties

| name             | type        | defalut | describes                                |
| ---------------- | ----------- | ------- | ---------------------------------------- |
| Text             | string      | empty   | button's text.                           |
| IconKind         | IconKind    | none    | button's icon from iconkind.             |
| IconSource       | SkPicture   |         | button's icon from file.                 |
| IconData         | string      | empty   | button's icon from path data.            |
| BackgroundColour | Color       | style   | button's background color.               |
| ForegroundColor  | Color       | style   | button's foreground color.               |
| FontFamily       | string      |         | font family of the button's text.        |
| FontSize         | float       | 14      | font size of the button's text.          |
| FontWeight       | int         | 400     | font weight of the button's text.        |
| FontItalic       | bool        | false   | enable font italic of the button's text. |
| Shape            | Shape       | style   | corner radius of the button's border.    |
| Elevation        | int         | style   | button's elevation.                      |
| OutlineColor     | Color       | style   | button's border color.                   |
| RippleColor      | Color       | style   | button's ripple color.                   |
| ContextMenu      | ContextMenu |         | button's context menu.                   |
| Style            | Style       | Filled  | button's style                           |
| Command          | ICommand    |         | executed when the button is clicked.     |
| CommandParameter | object      |         | Command's parameter.                     |



## Events

| name        | type                             |
| ----------- | -------------------------------- |
| Clicked     | `EventHandler<SKTouchEventArgs>` |
| Pressed     | `EventHandler<SKTouchEventArgs>` |
| Released    | `EventHandler<SKTouchEventArgs>` |
| Moved       | `EventHandler<SKTouchEventArgs>` |
| LongPressed | `EventHandler<SKTouchEventArgs>` |
| Entered     | `EventHandler<SKTouchEventArgs>` |
| Exited      | `EventHandler<SKTouchEventArgs>` |

