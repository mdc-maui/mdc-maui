# ContextMenu

ContextMenu display a list of choices on a temporary surface, It can be included in the component that has the touch event.



![](/assets/context-menus.png)


## Examples

```xml
<...
	xmlns:icon="clr-namespace:IconPacks.IconKind;assembly=IconPacks.Material"
...>

<md:FAB IconData="{Static icon:Material.Settings}">
	<md:FAB.ContextMenu>
        <md:ContextMenu>
            <md:MenuItem Text="item 1" />
            <md:MenuItem Text="item 2" />
            <md:MenuItem Text="item 3" />
    	</md:ContextMenu>
	</md:FAB.ContextMenu>
</md:FAB>
```



## Properties

| name            | type                             | default               |
| --------------- | -------------------------------- | --------------------- |
| Items           | `ObservableCollection<MenuItem>` |                       |
| ItemsSource     | IList                            |                       |
| Result          | object                           |                       |
| BackgroundColor | Color                            | SurfaceContainerColor |
| Shape           | Shape                            | 4                     |
| Elevation       | Elevation                        | Level2                |



## Methods

| name        | args                            |
| ----------- | -------------------------------- |
| Close     | result |



## Events

| name        | type                             |
| ----------- | -------------------------------- |
| Closed     | `EventHandler<object>` |

