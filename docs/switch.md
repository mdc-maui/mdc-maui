# Switch

Switches toggle the selection of an item on or off.



- Use switches (not radio buttons) if the items in a list can be independently controlled.
- Switches are the best way to let users adjust settings.
- Make sure the switch’s selection (on or off) is visible at a glance.
- Two styles: DefaultSwitchStyle and MarkSwitchStyle.




![](/assets/switchs.png)



## Examples

```xml
<md:Switch />
<md:Switch IsSelected="True" />
<md:Switch IsSelected="True" Style="{DynamicResource MarkSwitchStyle}" />
```





## Properties

| name             | type     | default                      |
| ---------------- | -------- | ---------------------------- |
| IsSelected       | bool     | false                        |
| ThumbColor       | Color    | OutlineColor                 |
| IconData         | string   |                              |
| IconColor        | Color    | SurfaceContainerHighestColor |
| BackgroundColor  | Color    | SurfaceContainerHighestColor |
| Shape            | Shape    | full                         |
| OutlineColor     | Color    | OutlineColor                 |
| StateLayerColor  | Color    | style                        |
| Command          | ICommand |                              |
| CommandParameter | object   |                              |



## Events

| name           | type                                  |
| -------------- | ------------------------------------- |
| SelectedChanged | `EventHandler<CheckedChangedEventArgs>` |
| Clicked                       | `EventHandler<TouchEventArgs>`          |
| Pressed                       | `EventHandler<TouchEventArgs>`          |
| Released                      | `EventHandler<TouchEventArgs>`          |
| LongPressed                   | `EventHandler<TouchEventArgs>`          |
| RightClicked ( desktop only ) | `EventHandler<TouchEventArgs>`          |
