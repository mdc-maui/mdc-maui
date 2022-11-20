using Material.Components.Maui.Core;
using Microsoft.Maui.Animations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Material.Components.Maui;

[ContentProperty(nameof(Content))]
public partial class Card : TemplatedView, IView, IShapeElement, IElevationElement, IRippleElement, IBackgroundElement, IStateLayerElement, IOutlineElement, IVisualTreeElement
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
            VisualStateManager.GoToState(this, value switch
            {
                ControlState.Normal => "normal",
                ControlState.Hovered => "hovered",
                ControlState.Pressed => "pressed",
                ControlState.Disabled => "disabled",
                _ => "normal",
            });
            this.controlState = value;
        }
    }
    public void OnPropertyChanged()
    {
        this.PART_Container?.InvalidateSurface();
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

    #region IOutlineElement
    public static readonly BindableProperty OutlineColorProperty = OutlineElement.OutlineColorProperty;
    public static readonly BindableProperty OutlineWidthProperty = OutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineOpacityProperty = OutlineElement.OutlineOpacityProperty;
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
    public float OutlineOpacity
    {
        get => (float)this.GetValue(OutlineOpacityProperty);
        set => this.SetValue(OutlineOpacityProperty, value);
    }
    #endregion

    #region IElevationElement
    public static readonly BindableProperty ElevationProperty = ElevationElement.ElevationProperty;
    public Elevation Elevation
    {
        get => (Elevation)this.GetValue(ElevationProperty);
        set => this.SetValue(ElevationProperty, value);
    }
    #endregion

    #region IShapeElement
    public static readonly BindableProperty ShapeProperty = ShapeElement.ShapeProperty;
    public Shape Shape
    {
        get => (Shape)this.GetValue(ShapeProperty);
        set => this.SetValue(ShapeProperty, value);
    }
    #endregion

    #region IStateLayerElement
    public static readonly BindableProperty StateLayerColorProperty = StateLayerElement.StateLayerColorProperty;
    public static readonly BindableProperty StateLayerOpacityProperty = StateLayerElement.StateLayerOpacityProperty;
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

    [AutoBindable(OnChanged = nameof(OnContentChanged))]
    private readonly View content;

    [AutoBindable(DefaultValue = "true")]
    private readonly bool enableTouchEvents;

    [AutoBindable(HidesUnderlyingProperty = true, OnChanged = nameof(OnHorizontalOptionsChanged))]
    private readonly LayoutOptions horizontalOptions;

    [AutoBindable(HidesUnderlyingProperty = true, OnChanged = nameof(OnVerticalOptionsChanged))]
    private readonly LayoutOptions verticalOptions;

    [AutoBindable(HidesUnderlyingProperty = true, OnChanged = nameof(OnWidthRequestChanged))]
    private readonly double widthRequest;

    [AutoBindable(HidesUnderlyingProperty = true, OnChanged = nameof(OnHeightRequestChanged))]
    private readonly double heightRequest;

    [AutoBindable(HidesUnderlyingProperty = true, OnChanged = nameof(OnPaddingChanged))]
    private readonly Thickness padding;

    private void OnContentChanged() => this.PART_Content.Content = this.Content;

    private void OnHorizontalOptionsChanged()
    {
        base.HorizontalOptions = this.HorizontalOptions;
        if (this.PART_Root != null)
        {
            this.PART_Root.HorizontalOptions = this.HorizontalOptions;
            this.PART_Container?.InvalidateSurface();
        }
    }

    private void OnVerticalOptionsChanged()
    {
        base.VerticalOptions = this.VerticalOptions;
        if (this.PART_Root != null)
        {
            this.PART_Root.VerticalOptions = this.VerticalOptions;
        }
    }

    private void OnWidthRequestChanged()
    {
        base.WidthRequest = this.WidthRequest;
        if (this.PART_Root != null)
        {
            this.PART_Root.WidthRequest = this.WidthRequest;
            this.PART_Container?.InvalidateSurface();
        }
    }

    private void OnHeightRequestChanged()
    {
        base.WidthRequest = this.WidthRequest;
        if (this.PART_Root != null)
        {
            this.PART_Root.HeightRequest = this.HeightRequest;
        }
    }

    private void OnPaddingChanged()
    {
        if (this.PART_Root != null)
        {
            this.PART_Content.Padding = this.Padding;
        }
    }

    private readonly Grid PART_Root;
    private readonly SKTouchCanvasView PART_Container;
    private readonly ContentPresenter PART_Content = new();
    private readonly CardDrawable drawable;
    private IAnimationManager animationManager;

    public Card()
    {
        this.PART_Container = new SKTouchCanvasView
        {
            EnableTouchEvents = true,
            IgnorePixelScaling = true,
        };

        this.PART_Container.PaintSurface += this.OnPaintSurface;
        this.PART_Root = new Grid
        {
            HorizontalOptions = this.HorizontalOptions,
            VerticalOptions = this.VerticalOptions,
            Children =
            {
                this.PART_Container,
                this.PART_Content
            }
        };
        this.ControlTemplate = new ControlTemplate(() => this.PART_Root);
        this.drawable = new CardDrawable(this);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void StartRippleEffect()
    {
        this.animationManager ??= this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();

        var start = 0f;
        var end = 1f;

        this.animationManager?.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
        {
            this.RipplePercent = start.Lerp(end, progress);
            this.PART_Container.InvalidateSurface();
        },
        duration: 0.35f,
        easing: Easing.SinInOut,
        finished: () =>
        {
            if (this.ControlState != ControlState.Pressed)
            {
                this.RipplePercent = 0f;
                this.PART_Container.InvalidateSurface();
            }
        }));
    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        this.RippleSize = e.Info.Rect.GetRippleSize(this.TouchPoint);
        this.drawable.Draw(e.Surface.Canvas, e.Info.Rect);
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName is "IsEnabled")
        {
            this.ControlState = this.IsEnabled ? ControlState.Normal : ControlState.Disabled;
        }
        else if (propertyName is "Padding")
        {
            this.PART_Content.Padding = this.Padding;
        }
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() => new List<View> { this.Content };

    public IVisualTreeElement GetVisualParent() => this.Window;
}
