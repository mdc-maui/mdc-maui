# RadioButton

Radio buttons let people select one option from a set of options.



- Use radio buttons (not switches) when only one item can be selected from a list.
- Label should be scannable.
- Selected items are more prominent than unselected items.



![](/assets/radio-buttons.png)





## Examples

```xml
<VerticalStackLayout Padding="20" Spacing="20">
    <md:RadioButton Orientation="Horizontal">
        <md:RadioItem Text="item 1" />
        <md:RadioItem Text="item 2" />
        <md:RadioItem Text="item 3" />
    </md:RadioButton>
    <md:RadioButton Orientation="Vertical">
        <md:RadioItem Text="item 1" />
        <md:RadioItem Text="item 2" />
        <md:RadioItem Text="item 3" />
    </md:RadioButton>
</VerticalStackLayout>
```





## Properties

| name              | type                              | default    |
| ----------------- | --------------------------------- | ---------- |
| Items             | `ObservableCollection<RadioItem>` |            |
| SelectedIndex     | int                               | -1         |
| SelectItem        | RadioItem                         |            |
| Orientation       | StackOrientation                  | Horizontal |
| Spacing           | double                            | 0          |
| HorizontalSpacing | double                            | 0          |
| VerticalSpacing   | double                            | 0          |
| Command           | ICommand                          |            |
| CommandParameter  | object                            |            |



## Events

| name                 | type                                         |
| -------------------- | -------------------------------------------- |
| SelectedIndexChanged | `EventHandler<SelectedItemChangedEventArgs>` |



## RadioItem Properties

| name            | type       | default               |
| --------------- | ---------- | --------------------- |
| ActivedColor    | Color      | style                 |
| IsSelected      | bool       | false                 |
| Text            | string     | empty                 |
| FontColor       | Color      | OnSurfaceVariantColor |
| FontSize        | float      | 16                    |
| FontFamily      | string     |                       |
| FontWeight      | FontWeight | Medium                |
| FontIsItalic    | bool       | false                 |
| StateLayerColor | Color      | PrimaryColor          |



## RadioItem Events

| name                          | type                           |
| ----------------------------- | ------------------------------ |
| Clicked                       | `EventHandler<TouchEventArgs>` |
| Pressed                       | `EventHandler<TouchEventArgs>` |
| Released                      | `EventHandler<TouchEventArgs>` |
| LongPressed                   | `EventHandler<TouchEventArgs>` |
| RightClicked ( desktop only ) | `EventHandler<TouchEventArgs>` |
