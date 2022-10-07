using Material.Components.Maui.Core.ComboBox;
using Material.Components.Maui.Core;
using Material.Components.Maui.Core.Menu;
using Microsoft.Maui.Animations;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Topten.RichTextKit;

#if WINDOWS
using Grid = Microsoft.Maui.Controls.Grid;
#endif

namespace Material.Components.Maui;

[ContentProperty(nameof(Items))]
public partial class ComboBox : ContentView, IView, ITextElement, IBackgroundElement, IForegroundElement, IShapeElement, IOutlineElement, IRippleElement
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
                ControlState.Pressed => this.IsDropDown ? "dropDown" : "normal",
                ControlState.Disabled => "disabled",
                _ => "normal",
            });
            this.controlState = value;
        }
    }
    public void OnPropertyChanged()
    {
        this.PART_Content?.InvalidateSurface();
    }

    #endregion

    #region ITextElement

    public static readonly BindableProperty TextProperty = TextElement.TextProperty;
    public static readonly BindableProperty FontFamilyProperty = TextElement.FontFamilyProperty;
    public static readonly BindableProperty FontSizeProperty = TextElement.FontSizeProperty;
    public static readonly BindableProperty FontWeightProperty = TextElement.FontWeightProperty;
    public static readonly BindableProperty FontItalicProperty = TextElement.FontItalicProperty;
    public TextBlock TextBlock { get; set; } = new();
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
    void ITextElement.OnTextBlockChanged()
    {
        this.UpdateLabelTextBounds();
    }

    #endregion

    #region IForegroundElement

    public static readonly BindableProperty ForegroundColorProperty = ForegroundElement.ForegroundColorProperty;
    public static readonly BindableProperty ForegroundOpacityProperty = ForegroundElement.ForegroundOpacityProperty;
    public Color ForegroundColor
    {
        get => (Color)this.GetValue(ForegroundColorProperty);
        set => this.SetValue(ForegroundColorProperty, value);
    }
    public float ForegroundOpacity
    {
        get => (float)this.GetValue(ForegroundOpacityProperty);
        set => this.SetValue(ForegroundOpacityProperty, value);
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

    private static readonly BindablePropertyKey ItemsPropertyKey =
       BindableProperty.CreateReadOnly(
           nameof(Items),
           typeof(ComboBoxItemCollection),
           typeof(ComboBox),
           null,
           defaultValueCreator: bo =>
               new ComboBoxItemCollection { Inner = ((ComboBox)bo).PART_MenuItemContainer });

    public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;
    public ComboBoxItemCollection Items
    {
        get => (ComboBoxItemCollection)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    [AutoBindable(OnChanged = nameof(OnItemsSourceChanged))]
    private readonly IList itemsSource;

    [AutoBindable(DefaultValue = "-1", OnChanged = nameof(OnSelectedIndexChanged))]
    private readonly int selectedIndex;

    [AutoBindable(DefaultValue = "TextFieldStyle.Filled", OnChanged = nameof(OnPropertyChanged))]
    private readonly TextFieldStyle textFieldStyle;

    [AutoBindable(DefaultValue = "Label text", OnChanged = nameof(UpdateLabelTextBounds))]
    private readonly string labelText;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly Color labelTextColor;

    [AutoBindable(DefaultValue = "1f", OnChanged = nameof(OnPropertyChanged))]
    private readonly float labelTextOpacity;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly int activeIndicatorHeight;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly Color activeIndicatorColor;

    [AutoBindable(DefaultValue = "1f", OnChanged = nameof(OnPropertyChanged))]
    private readonly float activeIndicatorOpacity;

    private void OnSelectedIndexChanged()
    {
        this.Text = this.Items[this.SelectedIndex].Text;
        this.SelectedIndexChanged?.Invoke(this, this.SelectedIndex);
        this.Command?.Execute(this.CommandParameter ?? this.SelectedIndex);
    }

    private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (var item in e.NewItems)
            {
                this.Items.Add(new ComboBoxItem { Text = item.ToString() });
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (var item in e.OldItems)
            {
                for (int i = e.OldStartingIndex; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Text == item.ToString())
                    {
                        this.Items.RemoveAt(i);
                        break;
                    }
                }

            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Replace)
        {
            for (int j = 0; j < e.OldItems.Count; j++)
            {
                for (int i = e.OldStartingIndex; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Text == e.OldItems[j].ToString())
                    {
                        this.Items[i] = new ComboBoxItem { Text = e.NewItems[j].ToString() };
                        break;
                    }
                }
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            this.Items.Clear();
        }
    }

    private void OnItemsSourceChanged()
    {
        if (this.ItemsSource is not null)
        {
            if (this.ItemsSource is INotifyCollectionChanged ncc)
            {
                ncc.CollectionChanged += this.OnItemsSourceCollectionChanged;
            }

            foreach (var item in this.ItemsSource)
            {
                if (item is ComboBoxItem cbItem)
                {
                    this.Items.Add(cbItem);
                }
                else if (item is string text)
                {
                    this.Items.Add(new ComboBoxItem { Text = text });
                }
            }
        }
    }

    private void UpdateLabelTextBounds()
    {
        var style = this.TextStyle.Modify(fontSize: this.TextStyle.FontSize - 4);
        this.LabelTextBlock.Clear();
        this.LabelTextBlock.AddText(this.LabelText, style);
        this.PART_Content.InvalidateSurface();
    }


    [AutoBindable]
    private readonly ICommand command;

    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<int> SelectedIndexChanged;

    internal bool IsDropDown { get; private set; } = false;
    internal TextBlock LabelTextBlock { get; private set; } = new();
    internal float PlaceholderAnimationPercent { get; private set; } = 1f;

    private PopupMenu popup;
    private readonly SKCanvasView PART_Content;
    private Card PART_Menu;
    private readonly VerticalStackLayout PART_MenuItemContainer;

    private readonly ComboBoxDrawable drawable;
    private IAnimationManager animationManager;


    public ComboBox()
    {
        this.PART_MenuItemContainer = new VerticalStackLayout();
        this.PART_MenuItemContainer.ChildAdded += this.OnItemsAdded;
        this.PART_MenuItemContainer.ChildRemoved += this.OnItemRemoved;

        this.PART_Content = new SKCanvasView
        {
            EnableTouchEvents = true,
            IgnorePixelScaling = true,
        };
        this.PART_Content.Touch += this.OnTouch;
        this.PART_Content.PaintSurface += this.OnPaintSurface;

        this.HeightRequest = 64d;
        this.Content = new Grid { this.PART_Content };

        this.drawable = new ComboBoxDrawable(this);
    }

    private void OnTouch(object sender, SKTouchEventArgs e)
    {
        if (e.ActionType == SKTouchAction.Pressed)
        {
            if (!this.IsDropDown)
            {
                this.IsDropDown = true;
                this.ControlState = ControlState.Pressed;
                this.PART_Content.InvalidateSurface();
                this.StartLabelTextAnimation();
                this.DropDown();
            }
            else
            {
                this.popup?.Close();
            }
            e.Handled = true;
        }
    }

    private void StartLabelTextAnimation()
    {
        if (this.SelectedIndex != -1) return;
        this.animationManager ??= this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        var start = 0f;
        var end = 1f;

        this.animationManager?.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
        {
            this.PlaceholderAnimationPercent = start.Lerp(end, progress);
            this.PART_Content.InvalidateSurface();
        },
        duration: 0.25f,
        easing: Easing.SinInOut));
    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var bounds = new SKRect(e.Info.Rect.Left, e.Info.Rect.Top + 8, e.Info.Rect.Right, e.Info.Rect.Bottom);
        this.drawable.Draw(e.Surface.Canvas, bounds);
    }

    private void ConnectPopup()
    {
        this.PART_Menu = new Card
        {
            WidthRequest = this.Width,
            Style = Application.Current.Resources.FindStyle("MenuCardStyle"),
            Content = new ScrollView
            {
                MaximumHeightRequest = 240d,
                Content = PART_MenuItemContainer,
            }
        };

        this.popup = new PopupMenu
        {
            Content = PART_Menu,
            Anchor = this.Content
        };

        this.popup.Connect(this.Handler.MauiContext);
        this.popup.Closed += (sender, e) =>
        {
            if (this.popup.Result is int result)
            {
                if (result != this.SelectedIndex)
                {
                    this.SelectedIndex = result;
                    this.SelectedIndexChanged?.Invoke(this, result);
                }
            }

            this.IsDropDown = false;
            this.ControlState = ControlState.Normal;

            if (this.SelectedIndex != -1)
            {
                this.PART_Content.InvalidateSurface();
            }
            else
            {
                this.StartLabelTextAnimation();
            }
        };
    }

    private void DropDown()
    {
        if (this.popup is null) this.ConnectPopup();
        this.popup.Show(this.Items.Count);
    }

    private void OnItemsAdded(object sender, ElementEventArgs e)
    {
        if (e.Element is ComboBoxItem item)
        {
            item.Clicked += (sender, e) =>
            {
                this.popup.Close(this.Items.IndexOf((ComboBoxItem)sender));
            };
        }
    }

    private void OnItemRemoved(object sender, ElementEventArgs e)
    {

    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == "IsEnabled")
        {
            this.ControlState = this.IsEnabled ? ControlState.Normal : ControlState.Disabled;
            this.PART_Content.InvalidateSurface();
        }
    }
}