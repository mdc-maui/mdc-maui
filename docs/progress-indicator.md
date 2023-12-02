# ProgressIndicator

Progress indicators show the status of a process in real time.



- Use the same progress indicator for all instances of a process (like loading).
- Two styles: CircularProgressIndicatorStyle and LinearProgressIndicatorStyle.
- Never use them as decoration.
- They capture attention through motion.



![](/assets/progress-indicators.png)



## Examples

```xml
<md:ProgressIndicator Style="{DynamicResource CircularProgressIndicatorStyle}" />
<md:ProgressIndicator Style="{DynamicResource LinearProgressIndicatorStyle}" />
```





## Properties

| name                  | type     | default      |
| --------------------- | -------- | ------------ |
| Percent               | float    | -1           |
| AnimationDuration     | float    | 1.5          |
| ActiveIndicatorHeight | int      | 4            |
| ActiveIndicatorColor  | Color    | PrimaryColor |
| BackgroundColor       | Color    | style        |
| Command               | ICommand |              |
| CommandParameter      | object   |              |



## Events

| name           | type                                  |
| -------------- | ------------------------------------- |
| PercentChanged | `EventHandler<ValueChangedEventArgs>` |