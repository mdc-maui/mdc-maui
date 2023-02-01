# ProgressIndicator

Progress indicators inform users about the status of ongoing processes.



![](/assets/progress-indicators.png)



## Styles

There are 2 Styles of :  1. Circular,  2. Linear.



## Examples

```xml
<mdc:ProgressIndicator Percent="25" Style="{DynamicResource CircularProgressIndicatorStyle}" />
<mdc:ProgressIndicator Percent="25" Style="{DynamicResource LinearProgressIndicatorStyle}" />
```





## Properties

| name                 | type     | default | describes                                              |
| -------------------- | -------- | ------- | ------------------------------------------------------ |
| Percent              | float    | -1      | ProgressIndicator's progress.                          |
| AnimationDuration    | float    | 1.5     | ProgressIndicator's animation duration.                |
| ActiveIndicatorColor | Color    | style   | ProgressIndicator's activeIndicator color.             |
| BackgroundColour     | Color    | style   | ProgressIndicator's background color.                  |
| Command              | ICommand |         | executed when the ProgressIndicator is PercentChanged. |
| CommandParameter     | object   |         | Command's parameter.                                   |



## Events

| name           | type                                  |
| -------------- | ------------------------------------- |
| PercentChanged | `EventHandler<ValueChangedEventArgs>` |