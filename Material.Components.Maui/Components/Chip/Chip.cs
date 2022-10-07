using Material.Components.Maui.Core.Chip;
using Material.Components.Maui.Core;
using Material.Components.Maui.Converters;
using Microsoft.Maui.Animations;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Topten.RichTextKit;

namespace Material.Components.Maui;
public partial class Chip : SKCanvasView, IView, IForegroundElement, IOutlineElement, IBackgroundElement, IImageElement, IElevationElement, IShapeElement, IStateLayerElement, IRippleElement, ITextElement
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
                ControlState.Normal => this.IsSelected ? "normal:actived" : "normal",
                ControlState.Hovered => this.IsSelected ? "hovered:actived" : "hovered",
                ControlState.Pressed => this.IsSelected ? "pressed:actived" : "pressed",
                ControlState.Disabled => this.IsSelected ? "disabled:actived" : "disabled",
                _ => "Normal",
            });
            this.controlState = value;
        }
    }
    public void OnPropertyChanged()
    {
        this.InvalidateSurface();
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
        var width = 32d + (this.HasIcon ? 18d : 0d) + this.TextBlock.MeasuredWidth + (this.HasCloseIcon ? 18d : 0d) + this.Margin.HorizontalThickness;
        var height = 32d + this.Margin.VerticalThickness;
        this.WidthRequest = width;
        this.HeightRequest = height;
        this.DesiredSize = new Size(this.WidthRequest, this.HeightRequest);
        this.InvalidateSurface();
    }

    #endregion

    #region IImageElement

    public static readonly BindableProperty IconProperty = ImageElement.IconProperty;
    public static readonly BindableProperty ImageProperty = ImageElement.ImageProperty;
    public IconKind Icon
    {
        get => (IconKind)this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    [System.ComponentModel.TypeConverter(typeof(ImageConverter))]
    public SKPicture Image
    {
        get => (SKPicture)this.GetValue(ImageProperty);
        set => this.SetValue(ImageProperty, value);
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


    [AutoBindable(OnChanged = nameof(OnIsSelectedChanged))]
    private readonly bool isSelected;

    [AutoBindable(DefaultValue = "true", OnChanged = nameof(OnHasIconChanged))]
    private readonly bool hasIcon;

    [AutoBindable(OnChanged = nameof(OnHasCloseIconChanged))]
    private readonly bool hasCloseIcon;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly Color iconColor;


    [AutoBindable]
    private readonly ICommand command;

    [AutoBindable]
    private readonly object commandParameter;

    private void OnTextChanged()
    {
        var width = 32d + (this.HasIcon ? 18d : 0d) + this.TextBlock.MeasuredWidth + (this.HasCloseIcon ? 18d : 0d) + this.Margin.HorizontalThickness;
        var height = 32d + this.Margin.VerticalThickness;
        this.WidthRequest = width;
        this.HeightRequest = height;
        this.DesiredSize = new Size(this.WidthRequest, this.HeightRequest);
        this.InvalidateSurface();
    }

    private void OnIsSelectedChanged()
    {
        SelectedChanged?.Invoke(this, this.IsSelected);
        VisualStateManager.GoToState(this, this.ControlState switch
        {
            ControlState.Normal => this.IsSelected ? "normal:actived" : "normal",
            ControlState.Hovered => this.IsSelected ? "hovered:actived" : "hovered",
            ControlState.Pressed => this.IsSelected ? "pressed:actived" : "pressed",
            ControlState.Disabled => this.IsSelected ? "disabled:actived" : "disabled",
            _ => "normal",
        });

        var minWidth = 32d + this.TextBlock.MeasuredWidth + (this.HasCloseIcon ? 18d : 0d) + this.Margin.HorizontalThickness;
        var height = 32d + this.Margin.VerticalThickness;
        if (this.Handler is null)
        {
            var width = minWidth + (this.HasIcon ? 18d : 0d);
            this.WidthRequest = width;
            this.HeightRequest = height;
            this.DesiredSize = new Size(this.WidthRequest, this.HeightRequest);
            this.InvalidateSurface();
            return;
        }

        this.animationManager ??= this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        this.ChangingPercent = 0f;
        var start = 0f;
        var end = 1f;
        var currWidth = 0d;
        this.animationManager?.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
        {
            this.ChangingPercent = start.Lerp(end, progress);
            if (this.HasIcon)
                currWidth = minWidth + (18d * this.ChangingPercent);
            else
                currWidth = minWidth + (18d * (1 - this.ChangingPercent));

            this.WidthRequest = currWidth;
            this.HeightRequest = height;
            this.DesiredSize = new Size(this.WidthRequest, this.HeightRequest);
            this.InvalidateSurface();
        },
        duration: 0.25f,
        easing: Easing.SinInOut));
    }

    private void OnHasIconChanged()
    {
        var width = 32d + (this.HasIcon ? 18d : 0d) + this.TextBlock.MeasuredWidth + (this.HasCloseIcon ? 18d : 0d) + this.Margin.HorizontalThickness;
        var height = 32d + this.Margin.VerticalThickness;
        this.WidthRequest = width;
        this.HeightRequest = height;
        this.DesiredSize = new Size(this.WidthRequest, this.HeightRequest);
        this.InvalidateSurface();
    }

    private void OnHasCloseIconChanged()
    {
        var width = 32d + (this.HasIcon ? 18d : 0d) + this.TextBlock.MeasuredWidth + (this.HasCloseIcon ? 18d : 0d) + this.Margin.HorizontalThickness;
        var height = 32d + this.Margin.VerticalThickness;
        this.WidthRequest = width;
        this.HeightRequest = height;
        this.DesiredSize = new Size(this.WidthRequest, this.HeightRequest);
        this.InvalidateSurface();
    }

    public event EventHandler<SKTouchEventArgs> Clicked;
    public event EventHandler<bool> SelectedChanged;
    public event EventHandler CloseClicked;

    internal float ChangingPercent { get; private set; } = 1f;
    private readonly ChipDrawable drawable;
    private IAnimationManager animationManager;

    public Chip()
    {
        this.drawable = new ChipDrawable(this);
        this.CloseClicked += (sender, e) =>
        {
            var chip = sender as Chip;
            if (chip.Parent is Layout parent)
            {
                parent.Remove(chip);
            }
        };
    }

    protected override void OnTouch(SKTouchEventArgs e)
    {
        if (this.ControlState == ControlState.Disabled) return;

        if (e.ActionType == SKTouchAction.Pressed)
        {
            this.ControlState = ControlState.Pressed;
            this.TouchPoint = e.Location;
            this.StartRippleEffect();
            e.Handled = true;
        }
        else if (e.ActionType == SKTouchAction.Released)
        {
            if (this.HasCloseIcon && this.TouchPoint.X >= this.Width - 34)
            {
                this.CloseClicked?.Invoke(this, e);
            }
            else
            {
                this.Clicked?.Invoke(this, e);
            }

#if WINDOWS || MACCATALYST
            this.ControlState = ControlState.Hovered;
#else
            this.ControlState = ControlState.Normal;
#endif
            if (this.RipplePercent == 1f)
            {
                this.RipplePercent = 0f;
            }
            this.InvalidateSurface();
            e.Handled = true;
        }
        else if (e.ActionType == SKTouchAction.Entered)
        {
            this.ControlState = ControlState.Hovered;
            this.InvalidateSurface();
            e.Handled = true;
        }
        else if (e.ActionType == SKTouchAction.Cancelled || e.ActionType == SKTouchAction.Exited)
        {
            this.ControlState = ControlState.Normal;
            this.InvalidateSurface();
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
            this.InvalidateSurface();
        },
        duration: 0.35f,
        easing: Easing.SinInOut,
        finished: () =>
        {
            if (this.ControlState != ControlState.Pressed)
            {
                this.RipplePercent = 0;
                this.InvalidateSurface();
            }
        }));
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
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
            this.InvalidateSurface();
        }
    }

    protected override void OnParentChanged()
    {
        base.OnParentChanged();
    }
}
