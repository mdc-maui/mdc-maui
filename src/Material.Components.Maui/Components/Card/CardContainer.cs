using System.ComponentModel;

namespace Material.Components.Maui;

public class CardContainer
    : GraphicsView,
        IBackgroundElement,
        IShapeElement,
        IOutlineElement,
        IElevationElement
{
    ViewState IElement.ViewState => ViewState.Normal;

    void IElement.OnPropertyChanged()
    {
        if (this.Handler != null)
            this.Invalidate();
    }

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

    public CardContainer()
    {
        this.Drawable = new CardContainerDrawable(this);
    }
}
