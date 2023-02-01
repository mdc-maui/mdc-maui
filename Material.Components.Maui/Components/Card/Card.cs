using System.ComponentModel;
using System.Windows.Input;

namespace Material.Components.Maui;

//[ContentProperty(nameof(Inner))]
public partial class Card
    : ContentView,
        IView,
        IShapeElement,
        IElevationElement,
        IBackgroundElement,
        IStateLayerElement,
        IOutlineElement,
        ICommandElement,
        IVisualTreeElement
{
    #region interface
    #region IView
    public static readonly BindableProperty ControlStateProperty = BindableProperty.Create(
        nameof(IView.ControlState),
        typeof(ControlState),
        typeof(IView)
    );

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ControlState ControlState
    {
        get => (ControlState)this.GetValue(ControlStateProperty);
        set
        {
            this.SetValue(ControlStateProperty, value);
            this.ChangeVisualState();
        }
    }

    internal void ChangeVisualState(ControlState value) => this.ControlState = value;

    protected override void ChangeVisualState()
    {
        var state = this.ControlState switch
        {
            ControlState.Normal => "normal",
            ControlState.Hovered => "hovered",
            ControlState.Pressed => "pressed",
            ControlState.Disabled => "disabled",
            _ => "normal",
        };
        VisualStateManager.GoToState(this, state);
    }

    public void OnPropertyChanged() { }
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

    #region IOutlineElement
    public static readonly BindableProperty OutlineColorProperty =
        OutlineElement.OutlineColorProperty;
    public static readonly BindableProperty OutlineWidthProperty =
        OutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineOpacityProperty =
        OutlineElement.OutlineOpacityProperty;
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

    public static readonly BindableProperty RippleColorProperty = BindableProperty.Create(
        nameof(RippleColor),
        typeof(Color),
        typeof(Card),
        null
    );
    public Color RippleColor
    {
        get => (Color)this.GetValue(RippleColorProperty);
        set => this.SetValue(RippleColorProperty, value);
    }
    #endregion

    [AutoBindable]
    private readonly bool enableTouchEvents;

    [AutoBindable]
    private readonly ICommand command;
    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<SKTouchEventArgs> Pressed;
    public event EventHandler<SKTouchEventArgs> Moved;
    public event EventHandler<SKTouchEventArgs> Released;
    public event EventHandler<SKTouchEventArgs> LongPressed;
    public event EventHandler<SKTouchEventArgs> Clicked;
    public event EventHandler<SKTouchEventArgs> Entered;
    public event EventHandler<SKTouchEventArgs> Exited;


    private Grid PART_Root;

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        this.PART_Root = (Grid)this.GetTemplateChild("PART_Root");
        var PART_Container = (CardContainer)this.GetTemplateChild("PART_Container");
        PART_Container.ParentCard = this;

        PART_Container.Clicked += (s, e) =>
        {
            this.Clicked?.Invoke(this, e);
            this.Command?.Execute(this.CommandParameter ?? e);
        };
        PART_Container.Pressed += (s, e) => this.Pressed?.Invoke(this, e);
        PART_Container.Moved += (s, e) => this.Moved?.Invoke(this, e);
        PART_Container.Released += (s, e) => this.Released?.Invoke(this, e);
        PART_Container.LongPressed += (s, e) => this.LongPressed?.Invoke(this, e);
        PART_Container.Clicked += (s, e) => this.Clicked?.Invoke(this, e);
        PART_Container.Entered += (s, e) => this.Entered?.Invoke(this, e);
        PART_Container.Exited += (s, e) => this.Exited?.Invoke(this, e);

        this.OnChildAdded(this.PART_Root);
        VisualDiagnostics.OnChildAdded(this, this.PART_Root);
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        new List<View> { this.PART_Root };

    public IVisualTreeElement GetVisualParent() => this.Window.Parent;
}
