namespace Material.Components.Maui;

[ContentProperty(nameof(Content))]
public class NavigationDrawerItem
    : TouchGraphicsView,
        ITextElement,
        IFontElement,
        IIconElement,
        IElement,
        IBackgroundElement,
        IShapeElement,
        IRippleElement,
        IStateLayerElement,
        IICommandElement,
        IDisposable
{
    protected override void ChangeVisualState()
    {
        this.IsVisualStateChanging = true;
        var state = this.ViewState switch
        {
            ViewState.Normal => this.IsActived ? "actived_normal" : "normal",
            ViewState.Hovered => this.IsActived ? "actived_hovered" : "hovered",
            ViewState.Pressed => this.IsActived ? "actived_pressed" : "pressed",
            ViewState.Disabled => "disabled",
            _ => "normal",
        };

        VisualStateManager.GoToState(this, state);
        this.IsVisualStateChanging = false;

        this.Invalidate();
    }

    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content),
        typeof(View),
        typeof(NavigationDrawerItem)
    );

    public static readonly BindableProperty IsActivedProperty = BindableProperty.Create(
        nameof(IsActived),
        typeof(bool),
        typeof(NavigationBarItem),
        propertyChanged: (bo, ov, nv) =>
        {
            var ndi = (NavigationDrawerItem)bo;
            ndi.ChangeVisualState();
            if (nv is true)
            {
                var navDrawer = ndi.GetParentElement<NavigationDrawer>();
                if (navDrawer is not null)
                    navDrawer.SelectedItem = ndi;
            }
        }
    );

    public static readonly BindableProperty TextProperty = ITextElement.TextProperty;
    public static readonly BindableProperty FontColorProperty = IFontElement.FontColorProperty;
    public static readonly BindableProperty FontSizeProperty = IFontElement.FontSizeProperty;
    public static readonly BindableProperty FontFamilyProperty = IFontElement.FontFamilyProperty;
    public static readonly BindableProperty FontWeightProperty = IFontElement.FontWeightProperty;
    public static readonly BindableProperty FontIsItalicProperty =
        IFontElement.FontIsItalicProperty;

    public static readonly BindableProperty IconDataProperty = IIconElement.IconDataProperty;
    public static readonly BindableProperty IconColorProperty = IIconElement.IconColorProperty;

    public View Content
    {
        get => (View)this.GetValue(ContentProperty);
        set => this.SetValue(ContentProperty, value);
    }

    public bool IsActived
    {
        get => (bool)this.GetValue(IsActivedProperty);
        set => this.SetValue(IsActivedProperty, value);
    }

    public string Text
    {
        get => (string)this.GetValue(TextProperty);
        set => this.SetValue(TextProperty, value);
    }
    public Color FontColor
    {
        get => (Color)this.GetValue(FontColorProperty);
        set => this.SetValue(FontColorProperty, value);
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
    public FontWeight FontWeight
    {
        get => (FontWeight)this.GetValue(FontWeightProperty);
        set => this.SetValue(FontWeightProperty, value);
    }
    public bool FontIsItalic
    {
        get => (bool)this.GetValue(FontIsItalicProperty);
        set => this.SetValue(FontIsItalicProperty, value);
    }

    public string IconData
    {
        get => (string)this.GetValue(IconDataProperty);
        set => this.SetValue(IconDataProperty, value);
    }

    PathF IIconElement.IconPath { get; set; }

    public Color IconColor
    {
        get => (Color)this.GetValue(IconColorProperty);
        set => this.SetValue(IconColorProperty, value);
    }

    public NavigationDrawerItem()
    {
        this.Drawable = new NavigationDrawerItemDrawable(this);
        this.Clicked += this.OnClicked;
    }

    private void OnClicked(object sender, TouchEventArgs e)
    {
        var navDrawer = this.GetParentElement<NavigationDrawer>();
        navDrawer.SelectedItem = this;
    }

    protected override void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.Clicked -= this.OnClicked;
            }
        }
        base.Dispose(disposing);
    }
}
