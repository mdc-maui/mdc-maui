# MenuItem



![](/assets/context-menus.png)


## Examples

```xml
<mdc:FAB IconKind="Settings">
	<mdc:FAB.ContextMenu>
        <mdc:ContextMenu>
            <mdc:MenuItem Text="item 1" />
            <mdc:MenuItem Text="item 2" />
            <mdc:MenuItem Text="item 3" />
    	</mdc:ContextMenu>
	</mdc:FAB.ContextMenu>
</mdc:FAB>
```





## Properties

| name              | type       | default               |
| ----------------- | ---------- | --------------------- |
| Text              | string     | empty                 |
| IconData          | string     |                       |
| IconColor         | Color      | OnSurfaceVariantColor |
| TrailingIconData  | string     |                       |
| TrailingIconColor | Color      | OnSurfaceVariantColor |
| BackgroundColor   | Color      | Transparent           |
| FontColor         | Color      | OnSurfaceColor        |
| FontFamily        | string     |                       |
| FontSize          | float      | 16                    |
| FontWeight        | FontWeight | Medium                |
| FontItalic        | bool       | false                 |
| StateLayerColor   | Color      | OnSurfaceColor        |
| RippleDuration    | float      | 0.5                   |
| RippleEasing      | Easing     | SinInOut              |
| Command           | ICommand   |                       |
| CommandParameter  | object     |                       |



## Events



| name                          | type                           |
| ----------------------------- | ------------------------------ |
| Clicked                       | `EventHandler<TouchEventArgs>` |
| Pressed                       | `EventHandler<TouchEventArgs>` |
| Released                      | `EventHandler<TouchEventArgs>` |
| LongPressed                   | `EventHandler<TouchEventArgs>` |
| RightClicked ( desktop only ) | `EventHandler<TouchEventArgs>` |