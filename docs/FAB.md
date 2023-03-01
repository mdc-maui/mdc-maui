# FAB

FABs(floating action button) represents the primary action of a screen.

![](/assets/FABs.png)



## Styles

There are 9 Styles of FABs: 1. Secondary,  2. Surface,  3. Tertiary,  4. SmallSecondary,  5. SmallSurface,  6. SmallTertiary,  7. LargeSecondary,  8. LargeSurface,  9. LargeTertiary.

## Examples

```xml
<mdc:FAB IconKind="Add" Style="{DynamicResource SecondaryFABStyle}" />
<mdc:FAB IconKind="Add" Style="{DynamicResource SurfaceFABStyle}" />
<mdc:FAB IconKind="Add" Style="{DynamicResource TertiaryFABStyle}" />
<mdc:FAB IconKind="Add" Text="Extended" IsExtended="True" Style="{DynamicResource SecondaryFABStyle}" />
<mdc:FAB IconKind="Add" Style="{DynamicResource LargeSecondaryFABStyle}" />
```



## Properties

| name             | type        | defalut | describes                             |
| ---------------- | ----------- | ------- | ------------------------------------- |
| Text             | string      | empty   | FAB's text                            |
| IconKind         | IconKind    | none    | FAB's icon from iconkind.             |
| IconSource       | SkPicture   |         | FAB's icon from file.                 |
| IconData         | string      | empty   | FAB's icon from path data.            |
| IsExtended       | bool        | false   | FAB's extended state                  |
| BackgroundColour | Color       | style   | FAB's background color.               |
| ForegroundColor  | Color       | style   | FAB's foreground color.               |
| FontFamily       | string      |         | font family of the FAB's text.        |
| FontSize         | float       | 14      | font size of the FAB's text.          |
| FontWeight       | int         | 400     | font weight of the FAB's text.        |
| FontItalic       | bool        | false   | enable font italic of the FAB's text. |
| Shape            | Shape       | style   | corner radius of the FAB's border.    |
| Elevation        | int         | style   | FAB's elevation.                      |
| OutlineColor     | Color       | style   | FAB's border color.                   |
| RippleColor      | Color       | style   | FAB's ripple color.                   |
| ContextMenu      | ContextMenu |         | FAB's context menu.                   |
| Style            | Style       | Filled  | FAB's style                           |
| Command          | ICommand    |         | executed when the FAB is clicked.     |
| CommandParameter | object      |         | Command's parameter.                  |



## Events

| name            | type                             |
| --------------- | -------------------------------- |
| ExtendedChanged | EventHandler                     |
| Clicked         | `EventHandler<SKTouchEventArgs>` |
| Pressed         | `EventHandler<SKTouchEventArgs>` |
| Released        | `EventHandler<SKTouchEventArgs>` |
| Moved           | `EventHandler<SKTouchEventArgs>` |
| LongPressed     | `EventHandler<SKTouchEventArgs>` |
| Entered         | `EventHandler<SKTouchEventArgs>` |
| Exited          | `EventHandler<SKTouchEventArgs>` |