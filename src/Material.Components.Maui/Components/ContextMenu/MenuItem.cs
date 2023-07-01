namespace Material.Components.Maui;

public partial class MenuItem
    : TouchGraphicsView,
        IElement,
        IBackgroundElement,
        ITextElement,
        IFontElement,
        IIconElement,
        ITrailingIconElement,
        IStateLayerElement,
        IRippleElement
{
    protected override void ChangeVisualState()
    {
        this.IsVisualStateChanging = true;
        var state = this.ViewState switch
        {
            ViewState.Normal => "normal",
            ViewState.Hovered => "hovered",
            ViewState.Pressed => "pressed",
            ViewState.Disabled => "disabled",
            _ => "normal",
        };

        VisualStateManager.GoToState(this, state);
        this.IsVisualStateChanging = false;

        this.Invalidate();
    }

    public static readonly BindableProperty TextProperty = ITextElement.TextProperty;
    public static readonly BindableProperty FontColorProperty = IFontElement.FontColorProperty;
    public static readonly BindableProperty FontSizeProperty = IFontElement.FontSizeProperty;
    public static readonly BindableProperty FontFamilyProperty = IFontElement.FontFamilyProperty;
    public static readonly BindableProperty FontAttributesProperty =
        IFontElement.FontAttributesProperty;

    public static readonly BindableProperty IconDataProperty = IIconElement.IconDataProperty;
    public static readonly BindableProperty IconColorProperty = IIconElement.IconColorProperty;

    public static readonly BindableProperty TrailingIconDataProperty =
        ITrailingIconElement.TrailingIconDataProperty;
    public static readonly BindableProperty TrailingIconColorProperty =
        ITrailingIconElement.TrailingIconColorProperty;

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
    public FontAttributes FontAttributes
    {
        get => (FontAttributes)this.GetValue(FontAttributesProperty);
        set => this.SetValue(FontAttributesProperty, value);
    }

    public string IconData
    {
        get => (string)this.GetValue(IconDataProperty);
        set => this.SetValue(IconDataProperty, value);
    }

    PathF IIconElement.IconPath { get; set; }

    public Color IconColor
    {
        get => (Color)this.GetValue(IconColorProperty);
        set => this.SetValue(IconColorProperty, value);
    }

    public string TrailingIconData
    {
        get => (string)this.GetValue(TrailingIconDataProperty);
        set => this.SetValue(TrailingIconDataProperty, value);
    }

    PathF ITrailingIconElement.TrailingIconPath { get; set; }

    public Color TrailingIconColor
    {
        get => (Color)this.GetValue(TrailingIconColorProperty);
        set => this.SetValue(TrailingIconColorProperty, value);
    }

    public MenuItem()
    {
        this.Drawable = new MenuItemDrawable(this);
    }

    internal SizeF GetDesiredSize()
    {
        var iconSize = !string.IsNullOrEmpty(this.IconData) ? 24f : 0f;
        var TrailingIconSize = !string.IsNullOrEmpty(this.TrailingIconData) ? 24f : 0f;
        var textSize = this.GetStringSize();
        //12 + iconSize + 12 + textSize.Width + 12 + trailingIconSize + 12;
        var needWidth = 12f + iconSize + 12f + textSize.Width + 12f + TrailingIconSize + 12f;
        var needHeight = 48f;

        return new Size(needWidth, needHeight);
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

        var (needWidth, needHeight) = this.GetDesiredSize();

        var width =
            this.HorizontalOptions.Alignment == LayoutAlignment.Fill
                ? maxWidth
                : this.Margin.HorizontalThickness
                    + Math.Max(
                        this.MinimumWidthRequest,
                        this.WidthRequest == -1 ? Math.Min(maxWidth, needWidth) : this.WidthRequest
                    );
        var height =
            this.VerticalOptions.Alignment == LayoutAlignment.Fill
                ? maxHeight
                : this.Margin.VerticalThickness
                    + Math.Max(
                        this.MinimumHeightRequest,
                        this.HeightRequest == -1
                            ? Math.Min(maxHeight, needHeight)
                            : this.HeightRequest
                    );

        this.DesiredSize = new Size(Math.Ceiling(width), Math.Ceiling(height));
        return this.DesiredSize;
    }
}
