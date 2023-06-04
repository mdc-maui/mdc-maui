using Material.Components.Maui.Extensions;
using System.ComponentModel;

namespace Material.Components.Maui;

public class Button
    : TouchGraphicView,
        IElement,
        ITextElement,
        IBackgroundElement,
        IShapeElement,
        IStateLayerElement,
        IRippleElement,
        IContextMenuElement,
        IIconElement,
        IOutlineElement,
        IElevationElement
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

        if (!this.IsFocused)
            this.Invalidate();
    }

    public static readonly BindableProperty TextProperty = ITextElement.TextProperty;
    public static readonly BindableProperty TextColorProperty = ITextElement.TextColorProperty;
    public static readonly BindableProperty TextOpacityProperty = ITextElement.TextOpacityProperty;
    public static readonly BindableProperty FontSizeProperty = ITextElement.FontSizeProperty;
    public static readonly BindableProperty FontFamilyProperty = ITextElement.FontFamilyProperty;
    public static readonly BindableProperty FontSlantProperty = ITextElement.FontSlantProperty;
    public static readonly BindableProperty FontWeightProperty = ITextElement.FontWeightProperty;

    public static readonly BindableProperty IconDataroperty = IIconElement.IconDataProperty;
    public static readonly BindableProperty IconColorProperty = IIconElement.IconColorProperty;
    public static readonly BindableProperty IconOpacityProperty = IIconElement.IconOpacityProperty;

    public static readonly BindableProperty OutlineWidthProperty =
        IOutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineColorProperty =
        IOutlineElement.OutlineColorProperty;
    public static readonly BindableProperty OutlineOpacityProperty =
        IOutlineElement.OutlineOpacityProperty;
    public static readonly BindableProperty ElevationProperty = IElevationElement.ElevationProperty;

    public string Text
    {
        get => (string)this.GetValue(TextProperty);
        set => this.SetValue(TextProperty, value);
    }
    public Color TextColor
    {
        get => (Color)this.GetValue(TextColorProperty);
        set => this.SetValue(TextColorProperty, value);
    }
    public float TextOpacity
    {
        get => (float)this.GetValue(TextOpacityProperty);
        set => this.SetValue(TextOpacityProperty, value);
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
    public FontSlant FontSlant
    {
        get => (FontSlant)this.GetValue(FontSlantProperty);
        set => this.SetValue(FontSlantProperty, value);
    }
    public FontWeight FontWeight
    {
        get => (FontWeight)this.GetValue(FontWeightProperty);
        set => this.SetValue(FontWeightProperty, value);
    }

    public string IconData
    {
        get => (string)this.GetValue(IconDataroperty);
        set => this.SetValue(IconDataroperty, value);
    }

    PathF IIconElement.IconPath { get; set; }

    public Color IconColor
    {
        get => (Color)this.GetValue(IconColorProperty);
        set => this.SetValue(IconColorProperty, value);
    }

    public float IconOpacity
    {
        get => (float)this.GetValue(IconOpacityProperty);
        set => this.SetValue(IconOpacityProperty, value);
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

    protected IFontManager fontManager;

    public Button()
    {
        this.Drawable = new ButtonDrawable(this);
    }

    public override SizeRequest Measure(
        double widthConstraint,
        double heightConstraint,
        MeasureFlags flags = MeasureFlags.None
    )
    {
        return base.Measure(widthConstraint, heightConstraint, flags);
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

        var scale = (this.HeightRequest != -1 ? this.HeightRequest : Math.Min(40, maxHeight)) / 40;
        var iconSize = (!string.IsNullOrEmpty(this.IconData) ? 18 : 0) * scale;
        var textSize = this.GetStringSize();
        //18 + iconSize + 6 + textSize.Width + 24
        var needWidth = 48 * scale + iconSize + textSize.Width;
        //8 + iconSize + 8;
        var needHeight = 16 * scale + Math.Max(iconSize, 24);

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
