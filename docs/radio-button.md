# RadioButton

Radio buttons allow users to select one option from a set.



![](/assets/radio-buttons.png)



## Styles

There are 2 Styles of radio buttons: 1. Horizontal,  2. Vertical.



## Examples

```xml
<mdc:RadioButton Orientation="Horizontal">
	<mdc:RadioButtonItem Text="item 1" />
	<mdc:RadioButtonItem Text="item 2" />
	<mdc:RadioButtonItem Text="item 3" />
</mdc:RadioButton>
<mdc:RadioButton Orientation="Vertical">
	<mdc:RadioButtonItem Text="item 1" />
	<mdc:RadioButtonItem Text="item 2" />
	<mdc:RadioButtonItem Text="item 3" />
</mdc:RadioButton>
```





## Properties

| name              | type                              | default    | describes                                                    |
| ----------------- | --------------------------------- | ---------- | ------------------------------------------------------------ |
| Items             | `ItemCollection<RadioButtonItem>` |            | RadioButton's items.                                         |
| selectedIndex     | int                               | -1         | RadioButton's selected index.                                |
| Orientation       | StackOrientation                  | Horizontal | RadioButton's item orientation.                              |
| Spacing           | double                            | 0          | Adjustment RadioButton's HorizontalSpacing and VerticalSpacing. |
| HorizontalSpacing | double                            | 0          | RadioButton's horizontal spacing .                           |
| VerticalSpacing   | double                            | 0          | RadioButton's vertical spacing.                              |
| Command           | ICommand                          |            | executed when the button is SelectedIndexChanged.            |
| CommandParameter  | object                            |            | Command's parameter.                                         |



## Events

| name                 | type                                          |
| -------------------- | --------------------------------------------- |
| SelectedIndexChanged | `EventHandler<SelectedIndexChangedEventArgs>` |



## RadioButtonItem Properties

| name             | type   | default | describes                                         |
| ---------------- | ------ | ------- | ------------------------------------------------- |
| OnColor          | Color  | style   | RadioButtonItem's actived color.                  |
| IsSelected       | bool   | false   | RadioButtonItem's selected state.                 |
| Text             | string | empty   | RadioButtonItem's text.                           |
| BackgroundColour | Color  | style   | RadioButtonItem's background color.               |
| ForegroundColor  | Color  | style   | RadioButtonItem's foreground color.               |
| FontFamily       | string |         | font family of the RadioButtonItem's text.        |
| FontSize         | float  | 14      | font size of the RadioButtonItem's text.          |
| FontWeight       | int    | 400     | font weight of the RadioButtonItem's text.        |
| FontItalic       | bool   | false   | enable font italic of the RadioButtonItem's text. |
| RippleColor      | Color  | style   | RadioButtonItem's items ripple color.             |



## RadioButtonItem Events

| name            | type                                     |
| --------------- | ---------------------------------------- |
| SelectedChanged | `EventHandler<SelectedChangedEventArgs>` |
| Clicked         | `EventHandler<SKTouchEventArgs>`         |
| Pressed         | `EventHandler<SKTouchEventArgs>`         |
| Released        | `EventHandler<SKTouchEventArgs>`         |
| Moved           | `EventHandler<SKTouchEventArgs>`         |
| LongPressed     | `EventHandler<SKTouchEventArgs>`         |
| Entered         | `EventHandler<SKTouchEventArgs>`         |
| Exited          | `EventHandler<SKTouchEventArgs>`         |