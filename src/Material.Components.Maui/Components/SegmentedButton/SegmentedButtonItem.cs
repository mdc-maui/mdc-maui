using System.ComponentModel;

namespace Material.Components.Maui;

public class SegmentedItem
    : VisualElement,
        IIconElement,
        ITextElement,
        IBackgroundElement,
        IStateLayerElement,
        IVisualTreeElement,
        IDisposable
{
    protected bool IsVisualStateChanging { get; set; }
    ViewState viewState = ViewState.Normal;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ViewState ViewState
    {
        get => this.viewState;
        set
        {
            this.viewState = value;
            this.ChangeVisualState();
        }
    }

    void IElement.OnPropertyChanged()
    {
        var parent = this.GetParentElement<SegmentedButton>();
        (parent as IElement)?.OnPropertyChanged();
    }

    void IElement.InvalidateMeasure()
    {
    }

    protected override void ChangeVisualState()
    {
        this.IsVisualStateChanging = true;
        var state = this.ViewState switch
        {
            ViewState.Normal => this.IsSelected ? "selected_normal" : "unselected_normal",
            ViewState.Hovered => this.IsSelected ? "selected_hovered" : "unselected_hovered",
            ViewState.Pressed => this.IsSelected ? "selected_pressed" : "unselected_pressed",
            ViewState.Disabled => this.IsSelected ? "selected_disabled" : "unselected_disabled",
            _ => "unselected_normal",
        };

        VisualStateManager.GoToState(this, state);
        this.IsVisualStateChanging = false;
        (this as IElement)?.OnPropertyChanged();
    }

    public static readonly new BindableProperty IsEnabledProperty = IElement.IsEnabledProperty;
    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected),
        typeof(bool),
        typeof(SegmentedItem),
        false,
        propertyChanged: (bo, ov, nv) =>
        {
            var item = (SegmentedItem)bo;
            item.ChangeVisualState();
            var parent = item.GetParentElement<SegmentedButton>();
            parent?.OnSelectedItemChanged(item);
        }
    );

    public static new readonly BindableProperty BackgroundColorProperty =
        IBackgroundElement.BackgroundColorProperty;
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(ITextElement),
        default,
        propertyChanged: (bo, ov, nv) =>
            ((IElement)((SegmentedItem)bo)?.Parent)?.InvalidateMeasure()
    );

    public static readonly BindableProperty IconDataProperty = IIconElement.IconDataProperty;
    public static readonly BindableProperty IconColorProperty = IIconElement.IconColorProperty;

    public static readonly BindableProperty StateLayerColorProperty =
        IStateLayerElement.StateLayerColorProperty;

    public new Color BackgroundColor
    {
        get => (Color)this.GetValue(BackgroundColorProperty);
        set => this.SetValue(BackgroundColorProperty, value);
    }

    public new bool IsEnabled
    {
        get => (bool)this.GetValue(IsEnabledProperty);
        set => this.SetValue(IsEnabledProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)this.GetValue(IsSelectedProperty);
        set => this.SetValue(IsSelectedProperty, value);
    }

    public string Text
    {
        get => (string)this.GetValue(TextProperty);
        set => this.SetValue(TextProperty, value);
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

    Shape IShapeElement.Shape
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    public Color StateLayerColor
    {
        get => (Color)this.GetValue(StateLayerColorProperty);
        set => this.SetValue(StateLayerColorProperty, value);
    }

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                ((IIconElement)this).IconPath?.Dispose();
            }
            this.disposedValue = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
