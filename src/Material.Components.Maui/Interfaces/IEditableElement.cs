namespace Material.Components.Maui.Interfaces;
public interface IEditableElement : IElement
{
    string Text { get; set; }
    TextRange SelectionRange { get; set; }
    Color CaretColor { get; set; }
    TextAlignment TextAlignment { get; set; }


    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(IEditableElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).InvalidateMeasure()
    );


    public static readonly BindableProperty SelectionRangeProperty = BindableProperty.Create(
        nameof(SelectionRange),
        typeof(TextRange),
        typeof(IEditableElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty CaretColorProperty = BindableProperty.Create(
        nameof(CaretColor),
        typeof(Color),
        typeof(IEditableElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty TextAlignmentProperty = BindableProperty.Create(
        nameof(TextAlignment),
        typeof(TextAlignment),
        typeof(IEditableElement),
        TextAlignment.Start,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}
