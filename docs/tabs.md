# Tabs

Tabs organize content across different screens, data sets, and other interactions.



![](/assets/tabs.png)



## Styles

There are 2 Styles of : 1. Filled, 2. Scrolled.



## Examples

```xml
<mdc:Tabs Style="{DynamicResource FilledTabsStyle}">
	<md:TabItem IconKind="Add" Text="Item 1">
    	...
    </md:TabItem>
	<md:TabItem IconKind="Add" Text="Item 2">
    	...
    </md:TabItem>
	<md:TabItem IconKind="Add" Text="Item 3">
    	...
    </md:TabItem>
	<md:TabItem IconKind="Add" Text="Item 4">
    	...
    </md:TabItem>
</mdc:Tabs>
```





## Properties

| name                 | type                      | default | describes                                      |
| -------------------- | ------------------------- | ------- | ---------------------------------------------- |
| Items                | `ItemCollection<TabItem>` |         | Tabs's Items                                   |
| HasIcon              | bool                      | true    | enable  icon of the Tabs.                      |
| HasLabel             | bool                      | true    | enable  label of the Tabs.                     |
| SelectedIndex        | int                       | 0       | Tabs's selected index.                         |
| SelectedItem         | TabItem                   |         | Tabs's selected item.                          |
| ActiveIndicatorShape | Shape                     | style   | Tabs's active indicator shape.                 |
| ActiveIndicatorColor | Color                     | style   | Tabs's active indicator color.                 |
| Command              | ICommand                  |         | executed when the Tabs is SelectedItemChanged. |
| CommandParameter     | object                    |         | Command's parameter.                           |



## Events

| name                | type                                         |
| ------------------- | -------------------------------------------- |
| SelectedItemChanged | `EventHandler<SelectedItemChangedEventArgs>` |



## TabItem Properties

| name                 | type      | default | describes                                 |
| -------------------- | --------- | ------- | ----------------------------------------- |
| Content              | View      |         | TabItem's contain content.                |
| IsActived            | bool      | false   | TabItem's selected state.                 |
| Text                 | string    | empty   | TabItem's Text.                           |
| IconKind             | IconKind  | none    | TabItem's icon from iconkind.             |
| IconSource           | SkPicture |         | TabItem's icon from file.                 |
| IconData             | string    | empty   | TabItem's icon from path data.            |
| ActiveIndicatorColor | Color     | style   | TabItem's activeIndicator color.          |
| BackgroundColour     | Color     | style   | TabItem's background color.               |
| ForegroundColor      | Color     | style   | TabItem's foreground color.               |
| FontFamily           | string    |         | font family of the TabItem's text.        |
| FontSize             | float     | 14      | font size of the TabItem's text.          |
| FontWeight           | int       | 500     | font weight of the TabItem's text.        |
| FontItalic           | bool      | false   | enable font italic of the TabItem's text. |
| RippleColor          | Color     | style   | TabItem's ripple color.                   |



## TabItem Events

| name        | type                             |
| :---------- | :------------------------------- |
| Clicked     | `EventHandler<SKTouchEventArgs>` |
| Pressed     | `EventHandler<SKTouchEventArgs>` |
| Released    | `EventHandler<SKTouchEventArgs>` |
| Moved       | `EventHandler<SKTouchEventArgs>` |
| LongPressed | `EventHandler<SKTouchEventArgs>` |
| Entered     | `EventHandler<SKTouchEventArgs>` |
| Exited      | `EventHandler<SKTouchEventArgs>` |