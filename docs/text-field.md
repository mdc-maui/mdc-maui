# TextField

Text fields allow users to enter text into a UI. 



![](/assets/text-fields.png)



## Styles

There are 2 Styles of text fields: 1. Filled,  2. Outlined.



## Examples

```xml
<mdc:TextField Icon="Search" WidthRequest="250" Style="{DynamicResource FilledTextFieldStyle}"/>
<mdc:TextField Icon="Password" IsError="True" WidthRequest="300" SupportingText="Incorrect password" TextChanged="OnTextChanged" TrailingIcon="Remove" TrailingIconClicked="OnTrailingIconClicked" Style="{DynamicResource OutlinedTextFieldStyle}" />
```





## Properties

| name                  | type      | default         | describes                                     |
| --------------------- | --------- | --------------- | --------------------------------------------- |
| Text                  | string    | empty           | TextField's text.                             |
| IsError               | bool      |                 | TextField's verified state.                   |
| CaretPosition         | int       |                 | TextField's caret position.                   |
| CaretColor            | Color     | style           | TextField's caret and cursor color.           |
| Icon                  | IconKind  | none            | TextField's icon.                             |
| IconSource            | SkPicture |                 | TextField's icon.                             |
| TrailingIcon          | IconKind  | none            | TextField's trailing icon.                    |
| TrailingIconSource    | SkPicture |                 | TextField's trailing icon.                    |
| LabelText             | string    | Label text      | TextField's label text.                       |
| LabelTextColor        | Color     | style           | TextField's label text color.                 |
| SupportingText        | string    | Supporting text | TextField's supporting text.                  |
| SupportingTextColor   | Color     | style           | TextField's supporting text color.            |
| ActiveIndicatorHeight | int       | style           | TextField's active indicator height.          |
| ActiveIndicatorColor  | Color     | style           | TextField's active indicator color.           |
| BackgroundColour      | Color     | style           | TextField's background color.                 |
| ForegroundColor       | Color     | style           | TextField's foreground color.                 |
| FontFamily            | string    |                 | font family of the TextField's text.          |
| FontSize              | float     | 16              | font size of the TextField's text.            |
| FontWeight            | int       | 400             | font weight of the TextField's text.          |
| FontItalic            | bool      | false           | enable font italic of the TextField's text.   |
| Shape                 | Shape     | style           | corner radius of the TextField's border.      |
| OutlineWidth          | int       | style           | TextField's border width.                     |
| OutlineColor          | Color     | style           | TextField's border color.                     |
| RippleColor           | Color     | style           | TextField's ripple color.                     |
| Style                 | Style     | Filled          | TextField's style                             |
| Command               | ICommand  |                 | executed when the TextField's is TextChanged. |
| CommandParameter      | object    |                 | Command's parameter.                          |



## Events

| name                | type                                 |
| ------------------- | ------------------------------------ |
| TextChanged         | `EventHandler<TextChangedEventArgs>` |
| TrailingIconClicked | `EventHandler<SKTouchEventArgs>`     |
| Clicked             | `EventHandler<SKTouchEventArgs>`     |
| Pressed             | `EventHandler<SKTouchEventArgs>`     |
| Released            | `EventHandler<SKTouchEventArgs>`     |
| Moved               | `EventHandler<SKTouchEventArgs>`     |
| LongPressed         | `EventHandler<SKTouchEventArgs>`     |
| Entered             | `EventHandler<SKTouchEventArgs>`     |
| Exited              | `EventHandler<SKTouchEventArgs>`     |