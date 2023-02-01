# WrapLayout

WrapLayout is a layout container that lets you position items in rows or columns, based on the orientation property. When the space is filled, the container automatically wraps items onto a new row or column.

![](/assets/wrap-layout.png)



## Examples

```xml
<mdc:WrapLayout Orientation="Horizontal">
	<mdc:Chip Text="chip" Icon="Add" />
	<mdc:Chip Text="chip" Icon="Add" />
	<mdc:Chip Text="chip" Icon="Add" />
	<mdc:Chip Text="chip" Icon="Add" />
	<mdc:Chip Text="chip" Icon="Add" />
	<mdc:Chip Text="chip" Icon="Add" />
	<mdc:Chip Text="chip" Icon="Add" />
	<mdc:Chip Text="chip" Icon="Add" />
</mdc:WrapLayout>
```





## Properties

| name              | type             | default    | describes                                                    |
| ----------------- | ---------------- | ---------- | ------------------------------------------------------------ |
| Orientation       | StackOrientation | Horizontal | WrapLayout's item orientation.                               |
| Spacing           | double           | 0          | Adjustment WrapLayout's HorizontalSpacing and VerticalSpacing. |
| HorizontalSpacing | double           | 0          | WrapLayout's horizontal spacing .                            |
| VerticalSpacing   | double           | 0          | WrapLayout's vertical spacing.                               |
