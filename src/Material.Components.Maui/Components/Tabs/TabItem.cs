using System.ComponentModel;

namespace Material.Components.Maui;

[EditorBrowsable(EditorBrowsableState.Never)]
public enum ItemStyle
{
    Primary,
    ScrollingPrimary,
    Secondary,
    ScrollingSecondary
}

[ContentProperty(nameof(Content))]
public class TabItem
    : TouchGraphicsView,
        IContentElement,
        ITextElement,
        IFontElement,
        IIconElement,
        IActiveIndicatorElement,
        IElement,
        IBackgroundElement,
        IStateLayerElement,
        IRippleElement,
        IVisualTreeElement,
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

    public static readonly BindableProperty ContentProperty = IContentElement.ContentProperty;

    public static readonly BindableProperty ContentTypeProperty = BindableProperty.Create(
        nameof(ContentType),
        typeof(Type),
        typeof(TabItem)
    );

    public static readonly BindableProperty IsActivedProperty = BindableProperty.Create(
        nameof(IsActived),
        typeof(bool),
        typeof(TabItem),
        propertyChanged: (bo, ov, nv) =>
        {
            var nbi = (TabItem)bo;
            nbi.ChangeVisualState();
            if (nv is true)
            {
                var tabs = nbi.GetParentElement<Tabs>();
                if (tabs is not null)
                    tabs.SelectedItem = nbi;
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
    public static readonly BindableProperty ActiveIndicatorShapeProperty = BindableProperty.Create(
        nameof(ActiveIndicatorShape),
        typeof(Shape),
        typeof(TabItem),
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
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

    public Type ContentType
    {
        get => (Type)this.GetValue(ContentTypeProperty);
        set => this.SetValue(ContentTypeProperty, value);
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

    public Shape ActiveIndicatorShape
    {
        get => (Shape)this.GetValue(ActiveIndicatorShapeProperty);
        set => this.SetValue(ActiveIndicatorShapeProperty, value);
    }

    internal PointF rippleStartPoint = new();

    public TabItem()
    {
        this.Drawable = new TabItemDrawable(this);
        this.Clicked += this.OnClicked;
    }

    private void OnClicked(object sender, TouchEventArgs e)
    {
        var tabs = this.GetParentElement<Tabs>();
        tabs.SelectedItem = this;
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        var height = 48f;
        var tabs = this.GetParentElement<Tabs>();

        if (tabs.ItemStyle is ItemStyle.ScrollingPrimary)
            widthConstraint = this.GetStringSize().Width + 64f;
        else if (tabs.ItemStyle is ItemStyle.ScrollingSecondary)
            widthConstraint =
                (((IIconElement)this).IconPath is not null ? 24f : 0f)
                + this.GetStringSize().Width
                + 80f;

        if (
            tabs.ItemStyle is ItemStyle.Primary or ItemStyle.ScrollingPrimary
            && ((IIconElement)this).IconPath is not null
        )
            height = 64f;

        this.DesiredSize = new Size(widthConstraint, height);
        return this.DesiredSize;
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.Content != null ? [this.Content] : Array.Empty<IVisualTreeElement>().ToList();

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
