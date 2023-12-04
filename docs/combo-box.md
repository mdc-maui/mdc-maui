# ComboBox

ComboBox displays a short list of items, from which the user can select an item.



- Five styels: FilledComboBoxStyle and OutlinedComboBoxStyle.

![](/assets/combo-boxs.png)

## Examples

```xml
<md:ComboBox Style="{DynamicResource FilledComboBoxStyle}">
	<md:MenuItem Text="item 1" />
	<md:MenuItem Text="item 2" />
</md:ComboBox>
<md:ComboBox Style="{DynamicResource OutlinedComboBoxStyle}">
	<md:MenuItem Text="item 1" />
	<md:MenuItem Text="item 2" />
</md:ComboBox>
```





## Properties

| name                  | type                    | default    |
| --------------------- | ----------------------- | ---------- |
| Items                 | `ObservableCollection<MenuItem>` |            |
| ItemsSource           | IList                   |            |
| SelectedIndex         | int                     | -1         |
| SelectedItem          | MenuItem                |            |
| LabelText             | string                  | Label text |
| LabelFontColor        | Color                   | style      |
| ActiveIndicatorHeight | int                     | style      |
| ActiveIndicatorColor  | Color                   | style      |
| BackgroundColor       | Color                   | style      |
| lFontColor            | Color                   | style      |
| FontFamily            | string                  |            |
| FontSize              | float                   | 14         |
| FontWeight            | int                     | 400        |
| FontIsItalic          | bool                    | false      |
| OutlineWidth          | int                     | style      |
| OutlineColor          | Color                   | style      |
| StateLayerColor  | Color       | style    |
| RippleDuration   | float       | 0.5      |
| RippleEasing     | Easing      | SinInOut |
| ContextMenu      | ContextMenu |          |
| Style                 | Style                   | Filled     |
| Command               | ICommand                |            |
| CommandParameter      | object                  |            |





## Events

| name            | type                                         |
| --------------- | -------------------------------------------- |
| SelectedChanged | `EventHandler<SelectedItemChangedEventArgs>` |
| Clicked                     | `EventHandler<TouchEventArgs>` |
| Pressed                     | `EventHandler<TouchEventArgs>` |
| Released                    | `EventHandler<TouchEventArgs>` |
| LongPressed                 | `EventHandler<TouchEventArgs>` |
| RightClicked ( desktop only ) | `EventHandler<TouchEventArgs>` |

