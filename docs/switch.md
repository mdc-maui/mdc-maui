# Switch



![](/assets/switchs.png)



## Examples

```xml
<mdc:Switch />
<mdc:Switch HasIcon="False" />
<mdc:Switch IsChecked="True" />
```





## Properties

| name             | type     | default | describes                                   |
| ---------------- | -------- | ------- | ------------------------------------------- |
| IsChecked        | bool     | false   | Switch's selected state.                    |
| HasIcon          | bool     | true    | enable check-mark icon of the Switch.       |
| TrackColor       | Color    | style   | Switch's track color.                       |
| ThumbColor       | Color    | style   | Switch's thumb color.                       |
| Shape            | Shape    | style   | corner radius of the Switch's border.       |
| OutlineColor     | Color    | style   | Switch's border color.                      |
| RippleColor      | Color    | style   | Switch's ripple color.                      |
| Command          | ICommand |         | executed when the Switch is CheckedChanged. |
| CommandParameter | object   |         | Command's parameter.                        |



## Events

| name           | type                                    |
| -------------- | --------------------------------------- |
| CheckedChanged | `EventHandler<CheckedChangedEventArgs>` |
| Clicked        | `EventHandler<SKTouchEventArgs>`        |
| Pressed        | `EventHandler<SKTouchEventArgs>`        |
| Released       | `EventHandler<SKTouchEventArgs>`        |
| Moved          | `EventHandler<SKTouchEventArgs>`        |
| LongPressed    | `EventHandler<SKTouchEventArgs>`        |
| Entered        | `EventHandler<SKTouchEventArgs>`        |
| Exited         | `EventHandler<SKTouchEventArgs>`        |