# Popup

Popups Displaying a modal  pop-up on the window.



![](/assets/popups.png)


## Examples

```xml
<md:Popup
    x:Class="Sample.Dialog"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:md="clr-namespace:Material.Components.Maui;assembly=Material.Components.Maui">

    <md:Card
        BackgroundColor="{DynamicResource SurfaceColor}"
        Shape="28"
        WidthRequest="350">
        <Grid Padding="24" RowDefinitions="auto,auto,auto">
            <Label
                FontSize="24"
                HorizontalOptions="Center"
                Text="Base dialog title" />
            <Label
                Grid.Row="1"
                Padding="0,24,0,16"
                Text="A dialog is a type of modal window that appears in front of app content to provide critical information, or ask for a decision."
                TextColor="{DynamicResource OnSurfaceVariantColor}" />
            <HorizontalStackLayout
                Grid.Row="2"
                HorizontalOptions="End"
                Spacing="8">
                <md:Button
                    x:Name="Cancel"
                    FontWeight="Medium"
                    Style="{DynamicResource TextButtonStyle}"
                    Text="Cancel" />
                <md:Button
                    x:Name="Confirm"
                    FontWeight="Medium"
                    Style="{DynamicResource TextButtonStyle}"
                    Text="Confirm" />
            </HorizontalStackLayout>
        </Grid>
    </md:Card>
</md:Popup>
```

```c#
//using on the page
var dlg = new Dialog { Parent = this };
var result = await dlg.ShowAtAsync(this);
```





## Properties

| name              | type            | default |
| ----------------- | --------------- | ------- |
| Content           | View            |         |
| HorizontalOptions | LayoutAlignment | Center  |
| VerticalOptions   | LayoutAlignment | Center  |
| DismissOnOutside  | bool            | false   |
| OffsetX           | int             | 0       |
| OffsetY           | int             | 0       |



## Methods

| name                           |      |
| ------------------------------ | ---- |
| ShowAtAsync(Page anchor)       |      |
| Close(object result = default) |      |



## Events

| name   | type                   |
| ------ | ---------------------- |
| Opened | EventHandler           |
| Closed | `EventHandler<object>` |