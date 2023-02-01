# NavigationDrawer

Navigation drawers provide ergonomic access to destinations in an app.

![](/assets/navigation-drawers.png)


## Examples

```xml
<mdc:NavigationDrawer Title="Components">
    <mdc:NavigationDrawerItem Title="Overview" Icon="LayersOutline" ContentType="{x:Type panels:OverviewPanel}" />
	<mdc:NavigationDrawerItem Title="Button" Icon="LayersOutline" ContentType="{x:Type panels:ButtonPanel}" />
</mdc:NavigationDrawer>
```



## Properties

| name                    | type                   | default              | describes                                                  |
| ----------------------- | ---------------------- | -------------------- | ---------------------------------------------------------- |
| Items                   | `ItemCollection<View>` |                      | NavigationDrawer's items.                                  |
| FooterItems             | `ItemCollection<View>` |                      | NavigationDrawer's footer items.                           |
| Title                   | string                 | empty                | NavigationDrawer's title.                                  |
| DisplayMode             | DrawerDisplayMode      | device               | NavigationDrawer's DisplayMode, split or popup.            |
| SelectedItem            | NavigationDrawerItem   | 0                    | NavigationDrawer's selected item.                          |
| IsPaneOpen              | bool                   | false                | open NavigationDrawer's pane.                              |
| HasToolBar              | bool                   | Binding  DisplayMode | enable toolBar of the NavigationDrawer.                    |
| PaneBackGroundColour    | Color                  | style                | NavigationDrawer's pane background color.                  |
| ToolBarBackGroundColour | Color                  | style                | NavigationDrawer's toolBarbackground color.                |
| Command                 | ICommand               |                      | executed when the NavigationDrawer is SelectedItemChanged. |
| CommandParameter        | object                 |                      | Command's parameter.                                       |



## Events

| name                | type                                         |
| ------------------- | -------------------------------------------- |
| SelectedItemChanged | `EventHandler<SelectedItemChangedEventArgs>` |



## NavigationDrawerItem Properties

| name                 | type   | default                 | describes                                              |
| -------------------- | ------ | ----------------------- | ------------------------------------------------------ |
| Title                | string | empty                   | NavigationDrawerItem's title.                          |
| Text                 | string | empty                   | NavigationDrawerItem's Text.                           |
| ContentType          | Type   |                         | NavigationDrawerItem's contain content type.           |
| ActiveIndicatorColor |        | SecondaryContainerColor | NavigationDrawerItem's activeIndicator color.          |
| BackgroundColour     | Color  | Transparent             | NavigationDrawerItem's background color.               |
| ForegroundColor      | Color  | OnSurfaceVariantColor   | NavigationDrawerItem's foreground color.               |
| FontFamily           | string |                         | font family of the NavigationDrawerItem's text.        |
| FontSize             | float  | 14                      | font size of the NavigationDrawerItem's text.          |
| FontWeight           | int    | 500                     | font weight of the NavigationDrawerItem's text.        |
| FontItalic           | bool   | false                   | enable font italic of the NavigationDrawerItem's text. |
| RippleColor          | Color  | SurfaceTintColor        | NavigationDrawerItem's ripple color.                   |



## NavigationDrawerItem Events

| name        | type                             |
| ----------- | -------------------------------- |
| Clicked     | `EventHandler<SKTouchEventArgs>` |
| Pressed     | `EventHandler<SKTouchEventArgs>` |
| Released    | `EventHandler<SKTouchEventArgs>` |
| Moved       | `EventHandler<SKTouchEventArgs>` |
| LongPressed | `EventHandler<SKTouchEventArgs>` |
| Entered     | `EventHandler<SKTouchEventArgs>` |
| Exited      | `EventHandler<SKTouchEventArgs>` |