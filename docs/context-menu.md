# ContextMenu

ContextMenu display a list of choices on a temporary surface, It can be included in the component that has the touch event.



![](/assets/context-menus.png)


## Examples

```xml
<mdc:FAB Icon="Settings">
	<mdc:FAB.ContextMenu>
        <mdc:ContextMenu>
            <mdc:MenuItem Icon="About" Text="item 1" />
            <mdc:MenuItem Icon="About" Text="item 2" />
            <mdc:MenuItem Icon="About" Text="item 3" />
    	</mdc:ContextMenu>
	</mdc:FAB.ContextMenu>
</mdc:FAB>
```



## Properties

| name             | type                       | default | describes                         |
| ---------------- | -------------------------- | ------- | --------------------------------- |
| Items            | `ItemCollection<MenuItem>` |         | ContextMenu's items.              |
| Result           | object                     |         | ContextMenu's result.             |
| BackgroundColour | Color                      | style   | ContextMenu's background color.   |
| RippleColor      | Color                      | style   | ContextMenu's items ripple color. |



## MenuItem Properties

| name             | type      | default | describes                                  |
| ---------------- | --------- | ------- | ------------------------------------------ |
| Text             | string    | empty   | MenuItem's text.                           |
| IconKind         | IconKind  | none    | MenuItem's icon from iconkind.             |
| IconSource       | SkPicture |         | MenuItem's icon from file.                 |
| IconData         | string    | empty   | MenuItem's icon from path data.            |
| BackgroundColour | Color     | style   | MenuItem's background color.               |
| ForegroundColor  | Color     | style   | MenuItem's foreground color.               |
| FontFamily       | string    |         | font family of the MenuItem's text.        |
| FontSize         | float     | 16      | font size of the MenuItem's text.          |
| FontWeight       | int       | 500     | font weight of the MenuItem's text.        |
| FontItalic       | bool      | false   | enable font italic of the MenuItem's text. |
| RippleColor      | Color     | style   | MenuItem's items ripple color.             |
| Command          | ICommand  |         | executed when the MenuItem is clicked.     |
| CommandParameter | object    |         | Command's parameter.                       |



## MenuItem Events

| name        | type                             |
| ----------- | -------------------------------- |
| Clicked     | `EventHandler<SKTouchEventArgs>` |
| Pressed     | `EventHandler<SKTouchEventArgs>` |
| Released    | `EventHandler<SKTouchEventArgs>` |
| Moved       | `EventHandler<SKTouchEventArgs>` |
| LongPressed | `EventHandler<SKTouchEventArgs>` |
| Entered     | `EventHandler<SKTouchEventArgs>` |
| Exited      | `EventHandler<SKTouchEventArgs>` |