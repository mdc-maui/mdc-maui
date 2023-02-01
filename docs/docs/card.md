# Card

Cards contain content about a single subject.

![](/assets/cards.png)



## Styles

There are 3 Styles of cards: 1. Elevated, 2. Filled,  3. Outlined.

## Examples

```xml
<mdc:Card Style="{DynamicResource FilledCardStyle}">
    ...
</mdc:Card>
<mdc:Card Style="{DynamicResource ElevatedCardStyle}">
    ...
</mdc:Card>
<mdc:Card Style="{DynamicResource OutlinedCardStyle}">
    ...
</mdc:Card>
```



## Properties

| name              | type        | defalut | describes                           |
| ----------------- | ----------- | ------- | ----------------------------------- |
| Content           | View        |         | card's contain content.             |
| BackgroundColour  | Color       | style   | card's background color.            |
| Shape             | Shape       | style   | corner radius of the card's border. |
| Elevation         | int         | style   | card's elevation.                   |
| OutlineColor      | Color       | style   | card's border color.                |
| RippleColor       | Color       | style   | card's ripple color.                |
| ContextMenu       | ContextMenu |         | card's context menu.                |
| enableTouchEvents | bool        | false   | enable touch events of the card     |
| Style             | Style       | Filled  | card's style                        |
| Command           | ICommand    |         | executed when the card is clicked.  |
| CommandParameter  | object      |         | Command's parameter.                |



## Events

| name        | type                             |
| ----------- | -------------------------------- |
| Clicked     | `EventHandler<SKTouchEventArgs>` |
| Pressed     | `EventHandler<SKTouchEventArgs>` |
| Released    | `EventHandler<SKTouchEventArgs>` |
| Moved       | `EventHandler<SKTouchEventArgs>` |
| LongPressed | `EventHandler<SKTouchEventArgs>` |
| Entered     | `EventHandler<SKTouchEventArgs>` |
| Exited      | `EventHandler<SKTouchEventArgs>` |
