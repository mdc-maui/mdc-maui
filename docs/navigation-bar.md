# NavigationBar

Navigation bars offer a persistent and convenient way to switch between primary destinations in an app.

![](/assets/navigation-bars.png)


## Examples

```xml
<mdc:NavigationBar>
    <mdc:NavigationBarItem ActivedIcon="Star" Icon="Globe" Text="label 1">
		...
	</mdc:NavigationBarItem>
        <mdc:NavigationBarItem ActivedIcon="Star" Icon="Globe" Text="label 2">
		...
	</mdc:NavigationBarItem>
</mdc:NavigationBar>
```





## Properties

| name             | type                                | default | describes                                               |
| ---------------- | ----------------------------------- | ------- | ------------------------------------------------------- |
| Items            | `ItemCollection<NavigationBarItem>` |         | navigationBar's Items.                                  |
| HasLabel         | bool                                | true    | has label of the navigationBar's Item.                  |
| SelectedIndex    | int                                 | 0       | navigationBar's selected index.                         |
| SelectedItem     | NavigationBarItem                   |         | navigationBar's selected item.                          |
| Command          | ICommand                            |         | executed when the navigationBar is SelectedItemChanged. |
| CommandParameter | object                              |         | Command's parameter.                                    |



## Events

| name                | type                                         |
| ------------------- | -------------------------------------------- |
| SelectedItemChanged | `EventHandler<SelectedItemChangedEventArgs>` |





## NavigationBarItem Properties

| name                 | type      | default | describes                                             |
| -------------------- | --------- | ------- | ----------------------------------------------------- |
| Content              | View      |         | NavigationBarItem's contain content.                  |
| Text                 | string    | empty   | NavigationBarItem's text.                             |
| IconKind             | IconKind  | none    | NavigationBarItem's icon from iconkind.               |
| IconSource           | SkPicture |         | NavigationBarItem's icon from file.                   |
| IconData             | string    | empty   | NavigationBarItem's icon from path data.              |
| ActivedIconKind      | IconKind  | none    | NavigationBarItem's icon from iconkind when actived.  |
| ActivedIconSource    | SkPicture |         | NavigationBarItem's icon from file when actived.      |
| ActivedIconData      | string    | empty   | NavigationBarItem's icon from path data when actived. |
| ActiveIndicatorColor | Color     | style   | NavigationBarItem's activeIndicator color.            |
| IconColor            | Color     | style   | NavigationBarItem's icon color.                       |
| BackgroundColour     | Color     | style   | NavigationBarItem's background color.                 |
| ForegroundColor      | Color     | style   | NavigationBarItem's foreground color.                 |
| FontFamily           | string    |         | font family of the NavigationBarItem's text.          |
| FontSize             | float     | 14      | font size of the NavigationBarItem's text.            |
| FontWeight           | int       | 400     | font weight of the NavigationBarItem's text.          |
| FontItalic           | bool      | false   | enable font italic of the NavigationBarItem's text.   |
| RippleColor          | Color     | style   | NavigationBarItem's ripple color.                     |



## NavigationBarItem Events

| name        | type                             |
| ----------- | -------------------------------- |
| Clicked     | `EventHandler<SKTouchEventArgs>` |
| Pressed     | `EventHandler<SKTouchEventArgs>` |
| Released    | `EventHandler<SKTouchEventArgs>` |
| Moved       | `EventHandler<SKTouchEventArgs>` |
| LongPressed | `EventHandler<SKTouchEventArgs>` |
| Entered     | `EventHandler<SKTouchEventArgs>` |
| Exited      | `EventHandler<SKTouchEventArgs>` |
