using Material.Components.Maui.Components.Core;
using Material.Components.Maui.Core;
using Microsoft.Maui.Platform;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Topten.RichTextKit;

namespace Material.Components.Maui;

public partial class Label : SKTouchCanvasView, IView, ITextElement, IForegroundElement, IBackgroundElement, IPaddingElement
{
    #region interface
    #region IView
    private ControlState controlState = ControlState.Normal;
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ControlState ControlState
    {
        get => this.controlState;
        set
        {
            this.controlState = value;
            this.ChangeVisualState();
        }
    }
    protected override void ChangeVisualState()
    {
        var state = this.ControlState switch
        {
            ControlState.Normal => "normal",
            ControlState.Disabled => "disabled",
            _ => "normal",
        };
        VisualStateManager.GoToState(this, state);
    }
    public void OnPropertyChanged()
    {
        this.InvalidateSurface();
    }
    #endregion

    #region ITextElement
    public static readonly BindableProperty TextProperty = TextElement.TextProperty;
    public static readonly BindableProperty FontFamilyProperty = TextElement.FontFamilyProperty;
    public static readonly BindableProperty FontSizeProperty = TextElement.FontSizeProperty;
    public static readonly BindableProperty FontWeightProperty = TextElement.FontWeightProperty;
    public static readonly BindableProperty FontItalicProperty = TextElement.FontItalicProperty;
    [EditorBrowsable(EditorBrowsableState.Never)]
    public TextBlock TextBlock { get; set; } = new();
    public TextStyle TextStyle { get; set; } = FontMapper.DefaultStyle.Modify();
    public string Text
    {
        get => (string)this.GetValue(TextProperty);
        set => this.SetValue(TextProperty, value);
    }
    public string FontFamily
    {
        get => (string)this.GetValue(FontFamilyProperty);
        set => this.SetValue(FontFamilyProperty, value);
    }
    public float FontSize
    {
        get => (float)this.GetValue(FontSizeProperty);
        set => this.SetValue(FontSizeProperty, value);
    }
    public int FontWeight
    {
        get => (int)this.GetValue(FontWeightProperty);
        set => this.SetValue(FontWeightProperty, value);
    }
    public bool FontItalic
    {
        get => (bool)this.GetValue(FontItalicProperty);
        set => this.SetValue(FontItalicProperty, value);
    }
    void ITextElement.OnTextBlockChanged()
    {
        this.AllocateSize(this.MeasureOverride(this.widthConstraint, this.heightConstraint));
        this.InvalidateSurface();
    }
    #endregion

    #region IForegroundElement
    public static readonly BindableProperty ForegroundColorProperty = ForegroundElement.ForegroundColorProperty;
    public static readonly BindableProperty ForegroundOpacityProperty = ForegroundElement.ForegroundOpacityProperty;
    public Color ForegroundColor
    {
        get => (Color)this.GetValue(ForegroundColorProperty);
        set => this.SetValue(ForegroundColorProperty, value);
    }
    public float ForegroundOpacity
    {
        get => (float)this.GetValue(ForegroundOpacityProperty);
        set => this.SetValue(ForegroundOpacityProperty, value);
    }
    #endregion

    #region IBackgroundElement
    public static readonly BindableProperty BackgroundColourProperty = BackgroundElement.BackgroundColourProperty;
    public static readonly BindableProperty BackgroundOpacityProperty = BackgroundElement.BackgroundOpacityProperty;
    public Color BackgroundColour
    {
        get => (Color)this.GetValue(BackgroundColourProperty);
        set => this.SetValue(BackgroundColourProperty, value);
    }
    public float BackgroundOpacity
    {
        get => (float)this.GetValue(BackgroundOpacityProperty);
        set => this.SetValue(BackgroundOpacityProperty, value);
    }
    #endregion

    #region IPaddingElement
    public static readonly BindableProperty PaddingProperty = PaddingElement.PaddingProperty;
    public Thickness Padding
    {
        get => (Thickness)this.GetValue(PaddingProperty);
        set => this.SetValue(PaddingProperty, value);
    }
    #endregion
    #endregion

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly TextAlignment horizontalTextAlignment;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly TextAlignment verticalTextAlignment;

    private readonly LabelDrawable drawable;

    private double widthConstraint = double.PositiveInfinity;
    private double heightConstraint = double.PositiveInfinity;

    public Label()
    {
        this.drawable = new LabelDrawable(this);
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        this.drawable.Draw(e.Surface.Canvas, e.Info.Rect);
    }

    public override string ToString() => this.Text;

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName is "IsEnabled")
        {
            this.ControlState = this.IsEnabled ? ControlState.Normal : ControlState.Disabled;
        }
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        var maxWidth = Math.Min(Math.Min(widthConstraint, this.MaximumWidthRequest), this.WidthRequest != -1 ? this.WidthRequest : double.PositiveInfinity) - this.Padding.HorizontalThickness;
        var maxHeight = Math.Min(Math.Min(heightConstraint, this.MaximumHeightRequest), this.HeightRequest != -1 ? this.HeightRequest : double.PositiveInfinity) - this.Padding.VerticalThickness;
        this.TextBlock.MaxWidth = (float)maxWidth;
        this.TextBlock.MaxHeight = (float)maxHeight;
        var width = this.HorizontalOptions.Alignment == LayoutAlignment.Fill
            ? maxWidth
            : this.Margin.HorizontalThickness
            + this.Padding.HorizontalThickness
            + Math.Max(this.MinimumWidthRequest, this.WidthRequest == -1
                ? Math.Min(maxWidth, this.TextBlock.MeasuredWidth)
                : this.WidthRequest);
        var height = this.VerticalOptions.Alignment == LayoutAlignment.Fill
            ? maxHeight
            : this.Margin.VerticalThickness
            + this.Padding.VerticalThickness
            + Math.Max(this.MinimumHeightRequest, this.HeightRequest == -1
                ? Math.Min(maxHeight, this.TextBlock.MeasuredHeight)
                : this.HeightRequest);
        var result = new Size(Math.Ceiling(width), Math.Ceiling(height));
        this.DesiredSize = result;
        return result;
    }

    protected override Size ArrangeOverride(Rect bounds)
    {
        this.widthConstraint = bounds.Width;
        this.heightConstraint = bounds.Height;
        return base.ArrangeOverride(bounds);
    }

}
