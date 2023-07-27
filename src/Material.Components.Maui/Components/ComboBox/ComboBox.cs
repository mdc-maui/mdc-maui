using Microsoft.Maui.Animations;
using System.Collections;
using System.Collections.Specialized;

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public class ComboBox
    : TouchGraphicsView,
        IItemsElement<MenuItem>,
        IOutlineElement,
        IFontElement,
        ILabelTextElement,
        IActiveIndicatorElement,
        IElement,
        IBackgroundElement,
        IICommandElement,
        IVisualTreeElement
{
    protected override void ChangeVisualState()
    {
        this.IsVisualStateChanging = true;
        var state = this.ViewState switch
        {
            ViewState.Disabled => "disabled",
            _ => this.IsDropDown ? "drop_down" : "normal",
        };

        VisualStateManager.GoToState(this, state);
        this.IsVisualStateChanging = false;
    }

    public static readonly BindableProperty ItemsProperty = IItemsElement<MenuItem>.ItemsProperty;

    public static readonly BindableProperty ItemsSourceProperty =
        IItemsElement<MenuItem>.ItemsSourceProperty;

    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
        nameof(SelectedIndex),
        typeof(int),
        typeof(ComboBox),
        -1,
        propertyChanged: (bo, ov, nv) =>
        {
            var comboBox = bo as ComboBox;
            var index = (int)nv;
            if (index >= 0 && index < comboBox.Items.Count)
                comboBox.SelectedItem = comboBox.Items[index];

            comboBox.Command?.Execute(comboBox.CommandParameter ?? index);
            comboBox.SelectedChanged?.Invoke(comboBox, new(comboBox.Items[index], index));
            ((IElement)comboBox).OnPropertyChanged();
        }
    );

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
        nameof(SelectedItem),
        typeof(MenuItem),
        typeof(ComboBox),
        propertyChanged: (bo, ov, nv) =>
        {
            var comboBox = bo as ComboBox;
            var item = (MenuItem)nv;
            if (comboBox.Items.Contains(item))
                comboBox.SelectedIndex = comboBox.Items.IndexOf(item);
        }
    );

    public static readonly BindableProperty FontColorProperty = IFontElement.FontColorProperty;
    public static readonly BindableProperty FontSizeProperty = IFontElement.FontSizeProperty;
    public static readonly BindableProperty FontFamilyProperty = IFontElement.FontFamilyProperty;
    public static readonly BindableProperty FontWeightProperty = IFontElement.FontWeightProperty;
    public static readonly BindableProperty FontIsItalicProperty =
        IFontElement.FontIsItalicProperty;

    public static readonly BindableProperty LabelTextProperty = ILabelTextElement.LabelTextProperty;
    public static readonly BindableProperty LabelFontColorProperty =
        ILabelTextElement.LabelFontColorProperty;

    public static readonly BindableProperty ActiveIndicatorHeightProperty =
        IActiveIndicatorElement.ActiveIndicatorHeightProperty;
    public static readonly BindableProperty ActiveIndicatorColorProperty =
        IActiveIndicatorElement.ActiveIndicatorColorProperty;

    public static readonly BindableProperty OutlineWidthProperty =
        IOutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineColorProperty =
        IOutlineElement.OutlineColorProperty;

    public ItemCollection<MenuItem> Items
    {
        get => (ItemCollection<MenuItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    public IList ItemsSource
    {
        get => (IList)this.GetValue(ItemsSourceProperty);
        set => this.SetValue(ItemsSourceProperty, value);
    }

    void IItemsElement<MenuItem>.OnItemsCollectionChanged(
        object sender,
        NotifyCollectionChangedEventArgs e
    )
    {
        if (e.OldItems != null)
        {
            foreach (MenuItem item in e.OldItems)
            {
                this.menu.Items.Remove(item);
            }
        }

        if (e.NewItems != null)
        {
            var index = e.NewStartingIndex;
            foreach (MenuItem item in e.NewItems)
            {
                this.menu.Items.Insert(index, item);
                index++;
            }
        }
        ((IElement)this).InvalidateMeasure();
    }

    void IItemsElement<MenuItem>.OnItemsSourceCollectionChanged(
        object sender,
        NotifyCollectionChangedEventArgs e
    )
    {
        if (e.OldItems != null)
        {
            var index = e.OldStartingIndex;
            foreach (string item in e.OldItems)
            {
                this.Items.RemoveAt(index);
                index++;
            }
        }

        if (e.NewItems != null)
        {
            var index = e.NewStartingIndex;
            foreach (string item in e.NewItems)
            {
                this.Items.Insert(index, new MenuItem { Text = item });
                index++;
            }
        }
    }

    public int SelectedIndex
    {
        get => (int)this.GetValue(SelectedIndexProperty);
        set => this.SetValue(SelectedIndexProperty, value);
    }

    public MenuItem SelectedItem
    {
        get => (MenuItem)this.GetValue(SelectedItemProperty);
        set => this.SetValue(SelectedItemProperty, value);
    }

    public new bool IsEnabled
    {
        get => (bool)this.GetValue(IsEnabledProperty);
        set => this.SetValue(IsEnabledProperty, value);
    }

    public new Color BackgroundColor
    {
        get => (Color)this.GetValue(BackgroundColorProperty);
        set => this.SetValue(BackgroundColorProperty, value);
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

    public string LabelText
    {
        get => (string)this.GetValue(LabelTextProperty);
        set => this.SetValue(LabelTextProperty, value);
    }
    public Color LabelFontColor
    {
        get => (Color)this.GetValue(LabelFontColorProperty);
        set => this.SetValue(LabelFontColorProperty, value);
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

    public event EventHandler<SelectedItemChangedEventArgs> SelectedChanged;

    bool isDropDown = false;
    internal bool IsDropDown
    {
        get => this.isDropDown;
        private set
        {
            this.isDropDown = value;
            this.ChangeVisualState();
        }
    }

    internal float LabelAnimationPercent { get; private set; } = 1f;

    readonly ContextMenu menu = new();

    public ComboBox()
    {
        this.Drawable = new ComboBoxDrawable(this);

        this.StartInteraction += this.OnStartInteraction;
        this.EndInteraction += this.OnEndInteraction;
        this.CancelInteraction += this.OnCancelInteraction;

        this.menu.Closed += this.OnMenuClosed;
    }

    protected override void OnEndInteraction(object sender, TouchEventArgs e)
    {
        if (this.isTouching)
        {
            this.IsDropDown = !this.IsDropDown;
        }

        if (this.IsDropDown)
        {
            if (this.menu.DesiredSize == Size.Zero)
                this.menu.WidthRequest = this.DesiredSize.Width;

            this.StartLabelTextAnimation();
            this.menu.Show(this);
        }
        else
        {
            this.menu.Close();
        }

        base.OnEndInteraction(sender, e);
    }

    private void OnMenuClosed(object sender, object e)
    {
        this.IsDropDown = false;
        this.StartLabelTextAnimation();
        if (e is int index)
            this.SelectedIndex = index;
    }

    public void StartLabelTextAnimation()
    {
        if (this.SelectedIndex != -1)
        {
            this.Invalidate();
            return;
        }

        this.animationManager ??=
            this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        var start = 0f;
        var end = 1f;

        this.animationManager?.Add(
            new Microsoft.Maui.Animations.Animation(
                callback: (progress) =>
                {
                    this.LabelAnimationPercent = start.Lerp(end, progress);
                    this.Invalidate();
                },
                duration: 0.5f,
                easing: Easing.Default
            )
        );
    }

    protected override float GetRippleSize()
    {
        return 0f;
    }

    protected override void StartRippleEffect()
    {
        return;
    }

    protected override void OnParentChanged()
    {
        base.OnParentChanged();

        this.OnChildAdded(this.menu);
        VisualDiagnostics.OnChildAdded(this, this.menu);
        SetInheritedBindingContext(this.menu, this.BindingContext);
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        SetInheritedBindingContext(this.menu, this.BindingContext);
    }

    Size GetMaxItemTextSize()
    {
        var result = Size.Zero;
        foreach (var item in this.Items)
        {
            var size = this.GetStringSize(item.Text);
            if (size.Width > result.Width)
                result.Width = size.Width;
        }
        return result;
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        var maxWidth = Math.Min(
            Math.Min(widthConstraint, this.MaximumWidthRequest),
            this.WidthRequest != -1 ? this.WidthRequest : double.PositiveInfinity
        );
        var maxHeight = Math.Min(
            Math.Min(heightConstraint, this.MaximumHeightRequest),
            this.HeightRequest != -1 ? this.HeightRequest : double.PositiveInfinity
        );

        var scale =
            (this.HeightRequest != -1 ? this.HeightRequest : Math.Min(64f, maxHeight)) / 64f;

        var itemTextSize = this.GetMaxItemTextSize();
        //16 + itemSize + 16 + arrowIconSize(24) + 16
        var needWidth = 72f * scale + itemTextSize.Width;
        var needHeight = 64f * scale;

        var width =
            this.HorizontalOptions.Alignment == LayoutAlignment.Fill
                ? maxWidth
                : this.Margin.HorizontalThickness
                    + Math.Max(
                        this.MinimumWidthRequest,
                        this.WidthRequest == -1 ? Math.Min(maxWidth, needWidth) : this.WidthRequest
                    );
        var height =
            this.VerticalOptions.Alignment == LayoutAlignment.Fill
                ? maxHeight
                : this.Margin.VerticalThickness
                    + Math.Max(
                        this.MinimumHeightRequest,
                        this.HeightRequest == -1
                            ? Math.Min(maxHeight, needHeight)
                            : this.HeightRequest
                    );

        this.DesiredSize = new Size(Math.Ceiling(width), Math.Ceiling(height));
        return this.DesiredSize;
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        new List<IVisualTreeElement> { this.menu };

    public IVisualTreeElement GetVisualParent() => null;
}
