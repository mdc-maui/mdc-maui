# CheckBox

Checkboxes let users select one or more items from a list, or turn an item on or off.



- Use checkboxes if multiple options can be selected from a list.
- Label should be scannable.
- Selected items are more prominent than unselected items.

![](/assets/check-boxs.png)

## Examples

```xml
<md:CheckBox Text="checkbox" />
```





## Properties

| name             | type     | default |
| ---------------- | -------- | ------- |
| IsChecked        | bool     | false   |
| IconData         | string     | mark           |
| IconColor        | Color      | OnPrimaryColor |
| BackgroundColor  | Color      | PrimaryColor   |
| Shape           | Shape      | 2    |
| OutlineWidth    | int        | 2   |
| OutlineColor    | Color      | OnSurfaceColor |
| StateLayerColor | Color      | OnSurfaceColor |
| RippleDuration  | float      | 0.25      |
| RippleEasing    | Easing     | SinInOut |
| Command          | ICommand |         |
| CommandParameter | object   |         |



## Events

| name                          | type                           |
| ----------------------------- | ------------------------------ |
| Clicked                       | `EventHandler<TouchEventArgs>` |
| Pressed                       | `EventHandler<TouchEventArgs>` |
| Released                      | `EventHandler<TouchEventArgs>` |
| LongPressed                   | `EventHandler<TouchEventArgs>` |
| RightClicked ( desktop only ) | `EventHandler<TouchEventArgs>` |