using Material.Components.Maui.Converters;
using Microsoft.Maui.Animations;
using System.ComponentModel;
using Topten.RichTextKit;

namespace Material.Components.Maui;

public partial class MenuItem
    : SKTouchCanvasView,
        IView,
        IBackgroundElement,
        IForegroundElement,
        IIconElement,
        ITextElement,
        IRippleElement,
        IStateLayerElement
{
    #region interface
    #region IView
    private bool isVisualStateChanging;
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
        this.isVisualStateChanging = true;
        var state = this.ControlState switch
        {
            ControlState.Normal => "normal",
            ControlState.Hovered => "hovered",
            ControlState.Pressed => "pressed",
            ControlState.Disabled => "disabled",
            _ => "normal",
        };
        VisualStateManager.GoToState(this, state);
        this.isVisualStateChanging = false;

        if (!this.IsFocused)
            this.InvalidateSurface();
    }

    public void OnPropertyChanged()
    {
        if (this.Handler != null && !this.isVisualStateChanging)
        {
            this.InvalidateSurface();
        }
    }
    #endregion

    #region ITextElement
    public static readonly BindableProperty TextProperty = TextElement.TextProperty;
    public static readonly BindableProperty FontFamilyProperty = TextElement.FontFamilyProperty;
    public static readonly BindableProperty FontSizeProperty = TextElement.FontSizeProperty;
    public static readonly BindableProperty FontWeightProperty = TextElement.FontWeightProperty;
    public static readonly BindableProperty FontItalicProperty = TextElement.FontItalicProperty;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public TextBlock InternalText { get; set; } = new();

    [EditorBrowsable(EditorBrowsableState.Never)]
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

    void ITextElement.OnChanged()
    {
        this.OnPropertyChanged();
    }

    #endregion
    #region IIconElement
    public static readonly BindableProperty IconKindProperty = IconElement.IconKindProperty;
    public static readonly BindableProperty IconDataProperty = IconElement.IconDataProperty;
    public static readonly BindableProperty IconSourceProperty = IconElement.IconSourceProperty;

    public IconKind IconKind
    {
        get => (IconKind)this.GetValue(IconKindProperty);
        set => this.SetValue(IconKindProperty, value);
    }

    public string IconData
    {
        get => (string)this.GetValue(IconDataProperty);
        set => this.SetValue(IconDataProperty, value);
    }

    [TypeConverter(typeof(IconSourceConverter))]
    public SKPicture IconSource
    {
        get => (SKPicture)this.GetValue(IconSourceProperty);
        set => this.SetValue(IconSourceProperty, value);
    }
    #endregion

    #region ITrailingIconElement
    public static readonly BindableProperty TrailingIconKindProperty = TrailingIconElement.TrailingIconKindProperty;
    public static readonly BindableProperty TrailingIconDataProperty = TrailingIconElement.TrailingIconDataProperty;
    public static readonly BindableProperty TrailingIconSourceProperty = TrailingIconElement.TrailingIconSourceProperty;

    public IconKind TrailIconKind
    {
        get => (IconKind)this.GetValue(TrailingIconKindProperty);
        set => this.SetValue(TrailingIconKindProperty, value);
    }

    public string TrailIconData
    {
        get => (string)this.GetValue(TrailingIconDataProperty);
        set => this.SetValue(TrailingIconDataProperty, value);
    }

    [TypeConverter(typeof(IconSourceConverter))]
    public SKPicture TrailIconSource
    {
        get => (SKPicture)this.GetValue(TrailingIconSourceProperty);
        set => this.SetValue(TrailingIconSourceProperty, value);
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

    [EditorBrowsable(EditorBrowsableState.Never)]
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

    [EditorBrowsable(EditorBrowsableState.Never)]
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

    [EditorBrowsable(EditorBrowsableState.Never)]
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
        this.Clicked += (sender, e) => this.Command?.Execute(this.CommandParameter ?? e);
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
            (!string.IsNullOrEmpty(this.IconData) || this.IconSource != null ? 48d : 12d)
            + (string.IsNullOrEmpty(this.TrailIconData) || this.TrailIconSource != null ? 48d : 12d);
        this.InternalText.MaxWidth = (float)(maxWidth - 24d - offsetWidth);
        this.InternalText.MaxHeight = 48f;
        return Math.Max(
            minWidth,
            Math.Min(maxWidth, this.InternalText.MeasuredWidth + 24d + offsetWidth)
        );
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
    }
}
