using Material.Components.Maui.Extensions;
using Material.Components.Maui.Styles;
using System.Diagnostics;

namespace Material.Components.Maui;

public class IconButton
    : TouchGraphicView,
        IToggleElement,
        IElement,
        IBackgroundElement,
        IShapeElement,
        IStateLayerElement,
        IRippleElement,
        IContextMenuElement,
        IIconElement,
        IOutlineElement,
        IElevationElement,
        IVisualTreeElement
{
    protected override void ChangeVisualState()
    {
        this.IsVisualStateChanging = true;
        var state = this.ViewState switch
        {
            ViewState.Normal
                => this.IsToggleEnabled
                    ? this.IsSelected
                        ? "selected_normal"
                        : "unselected_normal"
                    : "normal",
            ViewState.Hovered
                => this.IsToggleEnabled
                    ? this.IsSelected
                        ? "selected_hovered"
                        : "unselected_hovered"
                    : "hovered",
            ViewState.Pressed
                => this.IsToggleEnabled
                    ? this.IsSelected
                        ? "selected_pressed"
                        : "unselected_pressed"
                    : "pressed",
            ViewState.Disabled
                => this.IsToggleEnabled
                    ? this.IsSelected
                        ? "selected_disabled"
                        : "unselected_disabled"
                    : "disabled",
            _ => "normal",
        };
        Debug.WriteLine(state);
        VisualStateManager.GoToState(this, state);
        this.IsVisualStateChanging = false;

        if (!this.IsFocused)
            this.Invalidate();
    }

    public static readonly BindableProperty IconDataroperty = IIconElement.IconDataProperty;
    public static readonly BindableProperty IconColorProperty = IIconElement.IconColorProperty;

    public static readonly BindableProperty IsToggleEnabledProperty =
        IToggleElement.IsToggleEnabledProperty;
    public static readonly BindableProperty IsSelectedProperty = IToggleElement.IsSelectedProperty;

    public static readonly BindableProperty OutlineWidthProperty =
        IOutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineColorProperty =
        IOutlineElement.OutlineColorProperty;
    public static readonly BindableProperty ElevationProperty = BindableProperty.Create(
        nameof(Elevation),
        typeof(Elevation),
        typeof(IconButton),
        Elevation.Level0,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );

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

    public bool IsToggleEnabled
    {
        get => (bool)this.GetValue(IsToggleEnabledProperty);
        set => this.SetValue(IsToggleEnabledProperty, value);
    }
    public bool IsSelected
    {
        get => (bool)this.GetValue(IsSelectedProperty);
        set => this.SetValue(IsSelectedProperty, value);
    }

    void IToggleElement.OnToggleStateChanged() => this.ChangeVisualState();

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

    //[TypeConverter(typeof(ElevationConverter))]
    public Elevation Elevation
    {
        get => (Elevation)this.GetValue(ElevationProperty);
        set => this.SetValue(ElevationProperty, value);
    }

    static Style defaultStyle;

    public IconButton()
    {
        this.Style = defaultStyle ??= ResourceExtension.MaterialDictionaries
            .First(x => x.GetType() == typeof(IconButtonStyles))
            .FindStyle("FilledIconButtonStyle");

        this.Drawable = new IconButtonDrawable(this);
        this.EndInteraction += this.OnToggleStateChanged;
    }

    private void OnToggleStateChanged(object sender, TouchEventArgs e)
    {
        if (this.IsToggleEnabled)
            this.IsSelected = !this.IsSelected;
    }
}
