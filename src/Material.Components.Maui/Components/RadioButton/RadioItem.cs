namespace Material.Components.Maui;

public class RadioItem
    : TouchGraphicsView,
        ITextElement,
        IFontElement,
        IStateLayerElement,
        IRippleElement
{
    public static readonly BindableProperty TextProperty = ITextElement.TextProperty;
    public static readonly BindableProperty FontColorProperty = IFontElement.FontColorProperty;
    public static readonly BindableProperty FontSizeProperty = IFontElement.FontSizeProperty;
    public static readonly BindableProperty FontFamilyProperty = IFontElement.FontFamilyProperty;
    public static readonly BindableProperty FontWeightProperty = IFontElement.FontWeightProperty;
    public static readonly BindableProperty FontIsItalicProperty =
        IFontElement.FontIsItalicProperty;

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected),
        typeof(bool),
        typeof(RadioItem),
        false,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public static readonly BindableProperty ActivedColorProperty = BindableProperty.Create(
        nameof(ActivedColor),
        typeof(Color),
        typeof(RadioItem),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

    public string Text
    {
        get => (string)this.GetValue(TextProperty);
        set => this.SetValue(TextProperty, value);
    }
    public Color FontColor
    {
        get => (Color)this.GetValue(FontColorProperty);
        set => this.SetValue(FontColorProperty, value);
    }
    public float FontSize
    {
        get => (float)this.GetValue(FontSizeProperty);
        set => this.SetValue(FontSizeProperty, value);
    }
    public string FontFamily
    {
        get => (string)this.GetValue(FontFamilyProperty);
        set => this.SetValue(FontFamilyProperty, value);
    }
    public FontWeight FontWeight
    {
        get => (FontWeight)this.GetValue(FontWeightProperty);
        set => this.SetValue(FontWeightProperty, value);
    }
    public bool FontIsItalic
    {
        get => (bool)this.GetValue(FontIsItalicProperty);
        set => this.SetValue(FontIsItalicProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)this.GetValue(IsSelectedProperty);
        set => this.SetValue(IsSelectedProperty, value);
    }

    public Color ActivedColor
    {
        get => (Color)this.GetValue(ActivedColorProperty);
        set => this.SetValue(ActivedColorProperty, value);
    }

    public RadioItem()
    {
        this.Drawable = new RadioItemDrawable(this);
        this.Clicked += this.OnClicked;
    }

    private void OnClicked(object sender, TouchEventArgs e)
    {
        var rb = this.GetParentElement<RadioButton>();
        rb.SelectedItem = this;
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        var maxWidth = Math.Min(
            Math.Min(widthConstraint, this.MaximumWidthRequest),
            this.WidthRequest != -1 ? this.WidthRequest : double.PositiveInfinity
        );
        var maxHeight = Math.Min(
            Math.Min(heightConstraint, this.MaximumHeightRequest),
            this.HeightRequest != -1 ? this.HeightRequest : double.PositiveInfinity
        );

        var scale =
            (this.HeightRequest != -1 ? this.HeightRequest : Math.Min(40f, maxHeight)) / 40f;
        var textSize = this.GetStringSize();

        var needWidth = (40f + 30f) * scale + textSize.Width;
        var needHeight = 40f * scale;

        var width =
            this.Margin.HorizontalThickness
            + Math.Max(
                this.MinimumWidthRequest,
                this.WidthRequest == -1 ? Math.Min(maxWidth, needWidth) : this.WidthRequest
            );
        var height =
            this.Margin.VerticalThickness
            + Math.Max(
                this.MinimumHeightRequest,
                this.HeightRequest == -1 ? Math.Min(maxHeight, needHeight) : this.HeightRequest
            );

        this.DesiredSize = new Size(Math.Ceiling(width), Math.Ceiling(height));
        return this.DesiredSize;
    }
}
