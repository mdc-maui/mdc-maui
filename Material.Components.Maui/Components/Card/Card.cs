using Material.Components.Maui.Core;
using Microsoft.Maui.Animations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Material.Components.Maui;

[ContentProperty(nameof(Content))]
public partial class Card : TemplatedView,IView, IShapeElement, IElevationElement, IRippleElement, IBackgroundElement, IStateLayerElement, IOutlineElement, IVisualTreeElement
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
    public float RippleSize { get; internal set; } = 0f;
    [EditorBrowsable(EditorBrowsableState.Never)]
    public float RipplePercent { get; set; } = 0f;
    [EditorBrowsable(EditorBrowsableState.Never)]
    public SKPoint TouchPoint { get; set; } = new SKPoint(-1, -1);
    #endregion
    #endregion

    [AutoBindable(OnChanged = nameof(OnEnableTouchEventsChanged))]
    private readonly bool enableTouchEvents;

    private void OnEnableTouchEventsChanged()
    {
        if(this.PART_Container != null)
        {
            this.PART_Container.EnableTouchEvents = this.EnableTouchEvents;
        }
    }

    [AutoBindable(OnChanged = nameof(OnContentChanged))]
    private readonly View content;

    [AutoBindable(HidesUnderlyingProperty = true, OnChanged = nameof(OnPaddingChanged))]
    private readonly Thickness padding;

    private void OnContentChanged() => this.PART_Content.Content = this.Content;

    private void OnPaddingChanged()
    {
        if (this.PART_Content != null)
        {
            this.PART_Content.Padding = this.Padding;
        }
    }

    private readonly Grid PART_Root;
    private readonly CardContainer PART_Container;
    private readonly ContentPresenter PART_Content = new();

    public Card()
    {
        this.PART_Container = new CardContainer(this)
        {
            EnableTouchEvents = this.EnableTouchEvents
        };
        this.PART_Root = new Grid
        {
            Children =
            {
                this.PART_Container,
                this.PART_Content
            }
        };
        this.ControlTemplate = new ControlTemplate(() => this.PART_Root);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void StartRippleEffect() 
    {

    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() => new List<View> { this.Content };

    public IVisualTreeElement GetVisualParent() => this.Window;
}
