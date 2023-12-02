# WrapLayout

WrapLayout is a layout container that lets you position items in rows or columns, based on the orientation property. When the space is filled, the container automatically wraps items onto a new row or column.

![](/assets/wrap-layout.png)



## Examples

```xml
 <md:WrapLayout Orientation="Horizontal">
     <md:Chip IconData="{Static icon:Material.Add}" Text="chip" />
     <md:Chip IconData="{Static icon:Material.Add}" Text="chip" />
     <md:Chip IconData="{Static icon:Material.Add}" Text="chip" />
     <md:Chip IconData="{Static icon:Material.Add}" Text="chip" />
     <md:Chip IconData="{Static icon:Material.Add}" Text="chip" />
     <md:Chip IconData="{Static icon:Material.Add}" Text="chip" />
     <md:Chip IconData="{Static icon:Material.Add}" Text="chip" />
 </md:WrapLayout>
```





## Properties

| name              | type             | default    |
| ----------------- | ---------------- | ---------- |
| Orientation       | StackOrientation | Horizontal |
| Spacing           | double           | 0          |
| HorizontalSpacing | double           | 0          |
| VerticalSpacing   | double           | 0          |
