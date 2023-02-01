using Material.Components.Maui.Converters;
using Microsoft.Maui.Animations;
using System.ComponentModel;
using Topten.RichTextKit;

namespace Material.Components.Maui;

public partial class NavigationDrawerItem
    : SKTouchCanvasView,
        IView,
        ITextElement,
        IIconElement,
        IForegroundElement,
        IBackgroundElement,
        IStateLayerElement,
        IRippleElement
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
            ControlState.Normal => this.IsActived ? "normal:actived" : "normal",
            ControlState.Hovered => this.IsActived ? "hovered:actived" : "hovered",
            ControlState.Pressed => this.IsActived ? "pressed:actived" : "pressed",
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
        this.InvalidateSurface();
    }
    #endregion

    #region IIconElement
    public static readonly BindableProperty IconProperty = IconElement.IconProperty;
    public static readonly BindableProperty IconSourceProperty = IconElement.IconSourceProperty;
    public IconKind Icon
    {
        get => (IconKind)this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    [TypeConverter(typeof(IconSourceConverter))]
    public SKPicture IconSource
    {
        get => (SKPicture)this.GetValue(IconSourceProperty);
        set => this.SetValue(IconSourceProperty, value);
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

    [AutoBindable(DefaultValue = "")]
    private readonly string title;

    [AutoBindable]
    private readonly Type contentType;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    public bool isActived;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    public bool isExtended;

    [AutoBindable]
    private readonly Color activeIndicatorColor;

    public float ChangingPercent { get; private set; } = 1f;

    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content),
        typeof(View),
        typeof(NavigationDrawerItem),
        null
    );

    public View Content
    {
        get
        {
            var result = (View)this.GetValue(ContentProperty);
            if (result == null && this.ContentType != null)
            {
                result = (View)Activator.CreateInstance(this.ContentType);
                this.SetValue(ContentProperty, result);
            }
            return result;
        }
    }

    private readonly NavigationDrawerItemDrawable drawable;
    private IAnimationManager animationManager;

    public NavigationDrawerItem()
    {
        this.drawable = new NavigationDrawerItemDrawable(this);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void StartRippleEffect()
    {
        this.animationManager ??=
            this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        var start = 0f;
        var end = 1f;
        this.animationManager?.Add(
            new Microsoft.Maui.Animations.Animation(
                callback: (progress) =>
                {
                    this.RipplePercent = start.Lerp(end, progress);
                    this.InvalidateSurface();
                },
                duration: 0.35f,
                easing: Easing.SinInOut,
                finished: () =>
                {
                    if (this.ControlState != ControlState.Pressed)
                    {
                        this.RipplePercent = 0;
                        this.InvalidateSurface();
                    }
                }
            )
        );
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        var bounds = e.Info.Rect;
        if (!this.IsExtended)
        {
            bounds = new SKRectI(4, 0, 52, 48);
        }
        this.RippleSize = bounds.GetRippleSize(this.TouchPoint);
        this.drawable.Draw(e.Surface.Canvas, bounds);
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == "IsEnabled")
        {
            this.ControlState = this.IsEnabled ? ControlState.Normal : ControlState.Disabled;
        }
    }
}
