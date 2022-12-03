using Material.Components.Maui.Converters;
using Material.Components.Maui.Core;
using Microsoft.Maui.Animations;
using System.ComponentModel;
using Topten.RichTextKit;

namespace Material.Components.Maui;

public partial class MenuItem
    : SKTouchCanvasView,
        IView,
        IBackgroundElement,
        IForegroundElement,
        IImageElement,
        ITextElement,
        IRippleElement,
        IStateLayerElement
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
            VisualStateManager.GoToState(
                this,
                value switch
                {
                    ControlState.Normal => "normal",
                    ControlState.Hovered => "hovered",
                    ControlState.Pressed => "pressed",
                    ControlState.Disabled => "disabled",
                    _ => "normal",
                }
            );
            this.controlState = value;
        }
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
        this.OnPropertyChanged();
    }

    #endregion
    #region IImageElement
    public static readonly BindableProperty IconProperty = ImageElement.IconProperty;
    public static readonly BindableProperty ImageProperty = ImageElement.ImageProperty;
    public static readonly BindableProperty TrailIconProperty = BindableProperty.Create(
        nameof(TrailIcon),
        typeof(IconKind),
        typeof(MenuItem),
        IconKind.None,
        propertyChanged: OnTrailIconChanged
    );
    public static readonly BindableProperty TrailImageProperty = BindableProperty.Create(
        nameof(TrailImage),
        typeof(SKPicture),
        typeof(MenuItem),
        null,
        propertyChanged: OnTrailIconChanged
    );

    public static void OnTrailIconChanged(BindableObject bo, object oldValue, object newValue)
    {
        ((IView)bo).OnPropertyChanged();
    }

    public IconKind Icon
    {
        get => (IconKind)this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    [TypeConverter(typeof(ImageConverter))]
    public SKPicture Image
    {
        get => (SKPicture)this.GetValue(ImageProperty);
        set => this.SetValue(ImageProperty, value);
    }
    public IconKind TrailIcon
    {
        get => (IconKind)this.GetValue(TrailIconProperty);
        set => this.SetValue(TrailIconProperty, value);
    }

    [TypeConverter(typeof(ImageConverter))]
    public SKPicture TrailImage
    {
        get => (SKPicture)this.GetValue(TrailImageProperty);
        set => this.SetValue(TrailImageProperty, value);
    }
    #endregion

    #region IForegroundElement
    public static readonly BindableProperty ForegroundColorProperty =
        ForegroundElement.ForegroundColorProperty;
    public static readonly BindableProperty ForegroundOpacityProperty =
        ForegroundElement.ForegroundOpacityProperty;
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
    public static readonly BindableProperty BackgroundColourProperty =
        BackgroundElement.BackgroundColourProperty;
    public static readonly BindableProperty BackgroundOpacityProperty =
        BackgroundElement.BackgroundOpacityProperty;
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
    #region IStateLayerElement
    public static readonly BindableProperty StateLayerColorProperty =
        StateLayerElement.StateLayerColorProperty;
    public static readonly BindableProperty StateLayerOpacityProperty =
        StateLayerElement.StateLayerOpacityProperty;
    public Color StateLayerColor
    {
        get => (Color)this.GetValue(StateLayerColorProperty);
        set => this.SetValue(StateLayerColorProperty, value);
    }
    public float StateLayerOpacity
    {
        get => (float)this.GetValue(StateLayerOpacityProperty);
        set => this.SetValue(StateLayerOpacityProperty, value);
    }
    #endregion

    #region IRippleElement
    public static readonly BindableProperty RippleColorProperty = RippleElement.RippleColorProperty;
    public Color RippleColor
    {
        get => (Color)this.GetValue(RippleColorProperty);
        set => this.SetValue(RippleColorProperty, value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float RippleSize { get; private set; } = 0f;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float RipplePercent { get; set; } = 0f;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public SKPoint TouchPoint { get; set; } = new SKPoint(-1, -1);
    #endregion
    #endregion

    private readonly MenuItemDrawable drawable;
    private IAnimationManager animationManager;

    public MenuItem()
    {
        this.Clicked += (sender, e) =>
        {
            this.Command?.Execute(this.CommandParameter ?? e);
        };
        this.drawable = new MenuItemDrawable(this);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void StartRippleEffect()
    {
        this.animationManager ??=
            this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        var start = 0f;
        var end = 1f;

        this.animationManager.Add(
            new Microsoft.Maui.Animations.Animation(
                callback: (progress) =>
                {
                    if (this.ControlState == ControlState.Pressed)
                    {
                        this.RipplePercent = start.Lerp(end, progress);
                        this.InvalidateSurface();
                    }
                },
                duration: 0.15f,
                easing: Easing.SinInOut,
                finished: () =>
                {
                    if (this.ControlState != ControlState.Pressed)
                    {
                        this.RipplePercent = 0f;
                        this.InvalidateSurface();
                    }
                }
            )
        );
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        this.RippleSize = e.Info.Rect.GetRippleSize(this.TouchPoint);
        this.drawable.Draw(e.Surface.Canvas, e.Info.Rect);
    }

    internal double GetDesiredWidth()
    {
        var minWidth = this.MinimumWidthRequest;
        var maxWidth = this.MaximumWidthRequest;
        var offsetWidth =
            (this.Icon != IconKind.None || this.Image != null ? 48d : 12d)
            + (this.TrailIcon != IconKind.None || this.TrailImage != null ? 48d : 12d);
        this.TextBlock.MaxWidth = (float)(maxWidth - 24d - offsetWidth);
        this.TextBlock.MaxHeight = 48f;
        return Math.Max(
            minWidth,
            Math.Min(maxWidth, this.TextBlock.MeasuredWidth + 24d + offsetWidth)
        );
    }
}
