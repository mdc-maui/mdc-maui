# Popup

Popups Displaying a modal  pop-up on the window.



![](/assets/popups.png)


## Examples

```xml
<mdc:Popup
    x:Class="SampleApp.Views.Dialog"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:mdc="clr-namespace:Material.Components.Maui;assembly=Material.Components.Maui">
    <mdc:Card
        BackgroundColour="{DynamicResource SurfaceColor}"
        Elevation="3"
        EnableTouchEvents="False"
        Shape="28"
        WidthRequest="350">
        <Grid RowDefinitions="auto,auto,auto" 
        Padding="24">
            <mdc:Label FontSize="24" Text="Base dialog title" />
            <mdc:Label
                Grid.Row="1"
                Padding="0,24,0,16"
                ForegroundColor="{DynamicResource OnSurfaceVariantColor}"
                Text="A dialog is a type of modal window that appears in front of app content to provide critical information, or ask for a decision." />
            <HorizontalStackLayout
                Grid.Row="2"
                HorizontalOptions="End"
                Spacing="8">
                <mdc:Button
                    x:Name="Cancel"
                    Clicked="Cancel_Clicked"
                    FontWeight="500"
                    Style="{DynamicResource TextButtonStyle}"
                    Text="Cancel" />
                <mdc:Button
                    x:Name="Confirm"
                    Clicked="Confirm_Clicked"
                    FontWeight="500"
                    Style="{DynamicResource TextButtonStyle}"
                    Text="Confirm" />
            </HorizontalStackLayout>
        </Grid>
    </mdc:Card>
</mdc:Popup>

...

//using on the page
var popup = new Dialog();
var result = await popup.ShowAtAsync(this);
```





## Properties

| name              | type            | default | describes                                             |
| ----------------- | --------------- | ------- | ----------------------------------------------------- |
| Content           | View            |         | Popup's contain content.                              |
| HorizontalOptions | LayoutAlignment | Center  | Popup's horizontalOptions.                            |
| VerticalOptions   | LayoutAlignment | Center  | Popup's verticalOptions.                              |
| DismissOnOutside  | bool            | false   | close popup when touched outside the window's bounds. |
| OffsetX           | int             | 0       | Popup's offset x-coordinate based on alignment.       |
| OffsetY           | int             | 0       | Popup's offset y-coordinate based on alignment.       |



## Methods

| name                           | describes               |      |
| ------------------------------ | ----------------------- | ---- |
| ShowAtAsync(Page anchor)       | show popup on the page. |      |
| Close(object result = default) | close popup.            |      |



## Events

| name   | type                   |
| ------ | ---------------------- |
| Opened | EventHandler           |
| Closed | `EventHandler<object>` |