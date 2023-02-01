# ComboBox

ComboBox displays a short list of items, from which the user can select an item.

![](/assets/combo-boxs.png)



## Styles

There are 2 styles of comboBoxs: 1. Filled.  2. Outlined.

## Examples

```xml
<mdc:ComboBox Style="{DynamicResource FilledComboBoxStyle}">
	<mdc:ComboBoxItem Text="item 1" />
	<mdc:ComboBoxItem Text="item 2" />
</mdc:ComboBox>
<mdc:ComboBox Style="{DynamicResource OutlinedComboBoxStyle}">
	<mdc:ComboBoxItem Text="item 1" />
	<mdc:ComboBoxItem Text="item 2" />
</mdc:ComboBox>
```





## Properties

| name                  | type                           | default    | describes                                             |
| --------------------- | ------------------------------ | ---------- | ----------------------------------------------------- |
| Items                 | `ItemCollection<ComboBoxItem>` |            | comboBox's items.                                     |
| ItemsSource           | IList                          |            | comboBox's items.                                     |
| SelectedIndex         | int                            |            | comboBox's items selected index.                      |
| LabelText             | string                         | Label text | comboBox's label text.                                |
| LabelTextColor        | Color                          | style      | comboBox's label text color.                          |
| ActiveIndicatorHeight | int                            | style      | comboBox's active indicator height.                   |
| ActiveIndicatorColor  | Color                          | style      | comboBox's active indicator color.                    |
| BackgroundColour      | Color                          | style      | comboBox's background color.                          |
| ForegroundColor       | Color                          | style      | comboBox's foreground color.                          |
| FontFamily            | string                         |            | font family of the comboBox's text.                   |
| FontSize              | float                          | 14         | font size of the comboBox's text.                     |
| FontWeight            | int                            | 400        | font weight of the comboBox's text.                   |
| FontItalic            | bool                           | false      | enable font italic of the comboBox's text.            |
| Shape                 | Shape                          | Small      | corner radius of the comboBox's border.               |
| OutlineWidth          | int                            | style      | comboBox's border width.                              |
| OutlineColor          | Color                          | style      | comboBox's border color.                              |
| RippleColor           | Color                          | style      | comboBox's ripple color.                              |
| Style                 | Style                          | Filled     | comboBox's style                                      |
| Command               | ICommand                       |            | executed when the comboBox's is SelectedindexChanged. |
| CommandParameter      | object                         |            | Command's parameter.                                  |





## Events

| name                 | type                                          |
| -------------------- | --------------------------------------------- |
| SelectedindexChanged | `EventHandler<SelectedIndexChangedEventArgs>` |
| Clicked              | `EventHandler<SKTouchEventArgs>`              |
| Pressed              | `EventHandler<SKTouchEventArgs>`              |
| Released             | `EventHandler<SKTouchEventArgs>`              |
| Moved                | `EventHandler<SKTouchEventArgs>`              |
| LongPressed          | `EventHandler<SKTouchEventArgs>`              |
| Entered              | `EventHandler<SKTouchEventArgs>`              |
| Exited               | `EventHandler<SKTouchEventArgs>`              |

