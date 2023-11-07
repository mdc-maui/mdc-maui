using System.ComponentModel;
using IPaddingElement = Material.Components.Maui.Interfaces.IPaddingElement;

namespace Material.Components.Maui;

[ContentProperty(nameof(Content))]
public class Card
    : TemplatedView,
        IElement,
        IBackgroundElement,
        IPaddingElement,
        IShapeElement,
        IOutlineElement,
        IElevationElement,
        IVisualTreeElement
{
    ViewState IElement.ViewState => ViewState.Normal;

    void IElement.OnPropertyChanged() { }

    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content),
        typeof(View),
        typeof(Card)
    );

    public static new readonly BindableProperty BackgroundColorProperty =
        IBackgroundElement.BackgroundColorProperty;
    public static new readonly BindableProperty PaddingProperty = IPaddingElement.PaddingProperty;
    public static readonly BindableProperty ShapeProperty = IShapeElement.ShapeProperty;
    public static readonly BindableProperty OutlineWidthProperty =
        IOutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineColorProperty =
        IOutlineElement.OutlineColorProperty;
    public static readonly BindableProperty ElevationProperty = IElevationElement.ElevationProperty;

    public View Content
    {
        get => (View)this.GetValue(ContentProperty);
        set => this.SetValue(ContentProperty, value);
    }

    public new Color BackgroundColor
    {
        get => (Color)this.GetValue(BackgroundColorProperty);
        set => this.SetValue(BackgroundColorProperty, value);
    }

    public new Thickness Padding
    {
        get => (Thickness)this.GetValue(PaddingProperty);
        set => this.SetValue(PaddingProperty, value);
    }

    [TypeConverter(typeof(ShapeConverter))]
    public Shape Shape
    {
        get => (Shape)this.GetValue(ShapeProperty);
        set => this.SetValue(ShapeProperty, value);
    }
    public Color OutlineColor
    {
        get => (Color)this.GetValue(OutlineColorProperty);
        set => this.SetValue(OutlineColorProperty, value);
    }

    public int OutlineWidth
    {
        get => (int)this.GetValue(OutlineWidthProperty);
        set => this.SetValue(OutlineWidthProperty, value);
    }

    public Elevation Elevation
    {
        get => (Elevation)this.GetValue(ElevationProperty);
        set => this.SetValue(ElevationProperty, value);
    }

    Grid PART_Root;

    static Style defaultStyle;

    public Card()
    {
        this.Style = defaultStyle ??= ResourceExtension.MaterialDictionaries
            .First(x => x.GetType() == typeof(CardStyles))
            .FindStyle("FilledCardStyle");
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        this.PART_Root = (Grid)this.GetTemplateChild("PART_Root");
        this.OnChildAdded(this.PART_Root);
        VisualDiagnostics.OnChildAdded(this, this.PART_Root);
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        SetInheritedBindingContext(this.PART_Root, this.BindingContext);
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.PART_Root != null
            ? [this.PART_Root]
            : Array.Empty<IVisualTreeElement>().ToList();

    public IVisualTreeElement GetVisualParent() => null;
}
