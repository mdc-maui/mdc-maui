namespace Material.Components.Maui;

[ContentProperty(nameof(Content))]
public class NavigationBarItem
    : TouchGraphicsView,
        ITextElement,
        IFontElement,
        IIconElement,
        IActiveIndicatorElement,
        IElevationElement,
        IElement,
        IBackgroundElement,
        IStateLayerElement,
        IRippleElement,
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
        typeof(NavigationBarItem)
    );

    public static readonly BindableProperty IsActivedProperty = BindableProperty.Create(
        nameof(IsActived),
        typeof(bool),
        typeof(NavigationBarItem),
        propertyChanged: (bo, ov, nv) =>
        {
            var nbi = (NavigationBarItem)bo;
            nbi.ChangeVisualState();
            if (nv is true)
            {
                var navBar = nbi.GetParentElement<NavigationBar>();
                if (navBar is not null)
                    navBar.SelectedItem = nbi;
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

    public static readonly BindableProperty ActiveIndicatorHeightProperty =
        IActiveIndicatorElement.ActiveIndicatorHeightProperty;
    public static readonly BindableProperty ActiveIndicatorColorProperty =
        IActiveIndicatorElement.ActiveIndicatorColorProperty;

    public static readonly BindableProperty OutlineWidthProperty =
        IOutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineColorProperty =
        IOutlineElement.OutlineColorProperty;
    public static readonly BindableProperty ElevationProperty = IElevationElement.ElevationProperty;

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

    public int ActiveIndicatorHeight
    {
        get => (int)this.GetValue(ActiveIndicatorHeightProperty);
        set => this.SetValue(ActiveIndicatorHeightProperty, value);
    }

    public Color ActiveIndicatorColor
    {
        get => (Color)this.GetValue(ActiveIndicatorColorProperty);
        set => this.SetValue(ActiveIndicatorColorProperty, value);
    }

    public Elevation Elevation
    {
        get => (Elevation)this.GetValue(ElevationProperty);
        set => this.SetValue(ElevationProperty, value);
    }

    public NavigationBarItem()
    {
        this.Drawable = new NavigationBarItemDrawable(this);
        this.Clicked += this.OnClicked;
    }

    private void OnClicked(object sender, TouchEventArgs e)
    {
        var navBar = this.GetParentElement<NavigationBar>();
        navBar.SelectedItem = this;
    }

    protected override float GetRippleSize()
    {
        var bounds = new Rect(this.Bounds.Center.X - 32, this.Bounds.Top + 12, 64, 32);
        var sx = Math.Clamp(this.LastTouchPoint.X, bounds.Left, bounds.Right);
        var sy = Math.Clamp(this.LastTouchPoint.Y, bounds.Top, bounds.Bottom);
        this.LastTouchPoint = new Point(sx, sy);

        var points = new PointF[4];
        points[0].X = points[2].X = this.LastTouchPoint.X;
        points[0].Y = points[1].Y = this.LastTouchPoint.Y;
        points[1].X = points[3].X = this.LastTouchPoint.X - (float)64f;
        points[2].Y = points[3].Y = this.LastTouchPoint.Y - (float)32f;
        var maxSize = 0f;
        foreach (var point in points)
        {
            var size = MathF.Pow(
                MathF.Pow(point.X - this.LastTouchPoint.X, 2f)
                    + MathF.Pow(point.Y - this.LastTouchPoint.Y, 2f),
                0.5f
            );
            if (size > maxSize)
            {
                maxSize = size;
            }
        }
        return maxSize;
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
