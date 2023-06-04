using System.ComponentModel;

namespace Material.Components.Maui;

[ContentProperty(nameof(Content))]
public class Card
    : TemplatedView,
        IBackgroundElement,
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

    public static readonly BindableProperty BackgroundColourProperty =
        IBackgroundElement.BackgroundColourProperty;
    public static readonly BindableProperty BackgroundOpacityProperty =
        IBackgroundElement.BackgroundOpacityProperty;
    public static readonly BindableProperty ShapeProperty = IShapeElement.ShapeProperty;
    public static readonly BindableProperty OutlineWidthProperty =
        IOutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineColorProperty =
        IOutlineElement.OutlineColorProperty;
    public static readonly BindableProperty OutlineOpacityProperty =
        IOutlineElement.OutlineOpacityProperty;
    public static readonly BindableProperty ElevationProperty = IElevationElement.ElevationProperty;

    public View Content
    {
        get => (View)this.GetValue(ContentProperty);
        set => this.SetValue(ContentProperty, value);
    }

    public Color BackgroundColour
    {
        get => (Color)this.GetValue(BackgroundColourProperty);
        set => this.SetValue(BackgroundColourProperty, value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float BackgroundOpacity
    {
        get => (float)this.GetValue(BackgroundOpacityProperty);
        set => this.SetValue(BackgroundOpacityProperty, value);
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

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float OutlineOpacity
    {
        get => (float)this.GetValue(OutlineOpacityProperty);
        set => this.SetValue(OutlineOpacityProperty, value);
    }
    public Elevation Elevation
    {
        get => (Elevation)this.GetValue(ElevationProperty);
        set => this.SetValue(ElevationProperty, value);
    }

    Grid PART_Root;

    public Card() { }

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
            ? new List<IVisualTreeElement> { this.PART_Root }
            : Array.Empty<IVisualTreeElement>().ToList();

    public IVisualTreeElement GetVisualParent() => null;
}
