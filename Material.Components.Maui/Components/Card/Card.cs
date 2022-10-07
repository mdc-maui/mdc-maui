using Material.Components.Maui.Core.Card;
using Material.Components.Maui.Core;
using Microsoft.Maui.Animations;
using System.Runtime.CompilerServices;

namespace Material.Components.Maui;

[ContentProperty(nameof(Content))]
public partial class Card : TemplatedView, IView, IShapeElement, IElevationElement, IRippleElement, IBackgroundElement, IStateLayerElement, IOutlineElement
{
    #region interface

    #region IView

    private ControlState controlState = ControlState.Normal;
    public ControlState ControlState
    {
        get => this.controlState;
        private set
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
        this.PART_Card?.InvalidateSurface();
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
    public float RippleSize { get; private set; } = 0f;
    public float RipplePercent { get; private set; } = 0f;
    public SKPoint TouchPoint { get; private set; } = new SKPoint(-1, -1);

    #endregion

    #endregion


    [AutoBindable(OnChanged = nameof(OnContentChanged))]
    private readonly View content;

    [AutoBindable(DefaultValue = "true")]
    private readonly bool enableTouchEvents;

    [AutoBindable(HidesUnderlyingProperty = true, DefaultValue = "LayoutOptions.Center", OnChanged = nameof(OnHorizontalOptionsChanged))]
    private readonly LayoutOptions horizontalOptions;

    [AutoBindable(HidesUnderlyingProperty = true, DefaultValue = "LayoutOptions.Center", OnChanged = nameof(OnVerticalOptionsChanged))]
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
        if (this.PART_Root is not null)
        {
            this.PART_Root.HorizontalOptions = this.HorizontalOptions;
            this.PART_Card?.InvalidateSurface();
        }
    }

    private void OnVerticalOptionsChanged()
    {
        base.VerticalOptions = this.VerticalOptions;
        if (this.PART_Root is not null)
        {
            this.PART_Root.VerticalOptions = this.VerticalOptions;
        }
    }

    private void OnWidthRequestChanged()
    {
        base.WidthRequest = this.WidthRequest;
        if (this.PART_Root is not null)
        {
            this.PART_Root.WidthRequest = this.WidthRequest;
            this.PART_Card?.InvalidateSurface();
        }
    }

    private void OnHeightRequestChanged()
    {
        base.WidthRequest = this.WidthRequest;
        if (this.PART_Root is not null)
        {
            this.PART_Root.HeightRequest = this.HeightRequest;
        }
    }

    private void OnPaddingChanged()
    {
        if (this.PART_Root is not null)
        {
            this.PART_Content.Padding = this.Padding;
        }
    }

    private readonly Grid PART_Root;
    private readonly SKCanvasView PART_Card;
    private readonly ContentPresenter PART_Content;
    private readonly CardDrawable drawable;
    private IAnimationManager animationManager;

    public Card()
    {
        this.PART_Card = new SKCanvasView
        {
            EnableTouchEvents = true,
            IgnorePixelScaling = true,
        };

        this.PART_Card.Touch += this.OnTouch;
        this.PART_Card.PaintSurface += this.OnPaintSurface;
        this.PART_Content = new ContentPresenter();
        this.PART_Root = new Grid
        {
            HorizontalOptions = this.HorizontalOptions,
            VerticalOptions = this.VerticalOptions,
            Children =
            {
                this.PART_Card,
                this.PART_Content
            }
        };

        this.ControlTemplate = new ControlTemplate(() => this.PART_Root);
        this.drawable = new CardDrawable(this);
    }

    private void OnTouch(object sender, SKTouchEventArgs e)
    {
        if (this.ControlState != ControlState.Disabled)
        {
            if (e.ActionType == SKTouchAction.Pressed)
            {
                this.ControlState = ControlState.Pressed;
                this.TouchPoint = e.Location;
                this.StartRippleEffect();
            }
            else if (e.ActionType == SKTouchAction.Released)
            {
#if WINDOWS || MACCATALYST
                this.ControlState = ControlState.Hovered;
#else
                this.ControlState = ControlState.Normal;
#endif

                if (this.RipplePercent == 1f)
                {
                    this.RipplePercent = 0f;
                }
                this.PART_Card.InvalidateSurface();
            }
            else if (e.ActionType == SKTouchAction.Entered)
            {
                this.ControlState = ControlState.Hovered;
                this.PART_Card.InvalidateSurface();
            }
            else if (e.ActionType == SKTouchAction.Cancelled || e.ActionType == SKTouchAction.Exited)
            {
                this.ControlState = ControlState.Normal;
                this.PART_Card.InvalidateSurface();
            }
            e.Handled = true;
        }
    }

    private void StartRippleEffect()
    {
        this.animationManager ??= this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();

        var start = 0f;
        var end = 1f;

        this.animationManager?.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
        {
            this.RipplePercent = start.Lerp(end, progress);
            this.PART_Card.InvalidateSurface();
        },
        duration: 0.35f,
        easing: Easing.SinInOut,
        finished: () =>
        {
            if (this.ControlState != ControlState.Pressed)
            {
                this.RipplePercent = 0;
                this.PART_Card.InvalidateSurface();
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
        if (propertyName == "IsEnabled")
        {
            this.ControlState = this.IsEnabled ? ControlState.Normal : ControlState.Disabled;
            this.PART_Card.InvalidateSurface();
        }
    }
}
