# IconButton

Icon buttons help users take supplementary actions with a single tap.

![](/assets/icon-buttons.png)



## Styles

There are 4 Styles of icon buttons: 1. Filled, 2. FilledTonal , 3. Outlined, 4. Standard.

## Examples

```xml
<mdc:IconButton Icon="Pencil" Style="{DynamicResource FilledIconButtonStyle}" />
<mdc:IconButton Icon="Pencil" Style="{DynamicResource FilledTonalIconButtonStyle}" />
<mdc:IconButton Icon="Pencil" Style="{DynamicResource OutlinedIconButtonStyle}" />
<mdc:IconButton Icon="Pencil" Style="{DynamicResource StandardIconButtonStyle}" />
```



## Properties

| name             | type        | defalut | describes                             |
| ---------------- | ----------- | ------- | ------------------------------------- |
| IconKind         | IconKind    | none    | button's icon from iconkind.          |
| IconSource       | SkPicture   |         | button's icon from file.              |
| IconData         | string      | empty   | button's icon from path data.         |
| BackgroundColour | Color       | style   | button's background color.            |
| ForegroundColor  | Color       | style   | button's foreground color.            |
| Shape            | Shape       | style   | corner radius of the button's border. |
| Elevation        | int         | style   | button's elevation.                   |
| OutlineColor     | Color       | style   | button's border color.                |
| RippleColor      | Color       | style   | button's ripple color.                |
| ContextMenu      | ContextMenu |         | button's context menu.                |
| Style            | Style       | Filled  | button's style                        |
| Command          | ICommand    |         | executed when the button is clicked.  |
| CommandParameter | object      |         | Command's parameter.                  |



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