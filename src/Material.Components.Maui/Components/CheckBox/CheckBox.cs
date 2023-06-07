namespace Material.Components.Maui;

public class CheckBox : TouchGraphicView, IOutlineElement, IIconElement
{
    protected override void ChangeVisualState()
    {
        this.IsVisualStateChanging = true;
        var state = this.ViewState switch
        {
            ViewState.Normal => "normal",
            ViewState.Hovered => "hovered",
            ViewState.Pressed => "pressed",
            ViewState.Disabled => "disabled",
            _ => "normal",
        };
        VisualStateManager.GoToState(this, state);
        this.IsVisualStateChanging = false;

        this.Invalidate();
    }

    public static readonly BindableProperty IconDataProperty = IIconElement.IconDataProperty;
    public static readonly BindableProperty IconColorProperty = IIconElement.IconColorProperty;
    public static readonly BindableProperty OutlineWidthProperty =
        IOutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineColorProperty =
        IOutlineElement.OutlineColorProperty;
    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
        nameof(IsChecked),
        typeof(bool),
        typeof(CheckBox),
        false,
        propertyChanged: (bo, ov, nv) =>
        {
            var cb = (CheckBox)bo;
            cb.CheckedChanged?.Invoke(cb, new CheckedChangedEventArgs(cb.IsChecked));
            cb.Command?.Execute(cb.CommandParameter ?? cb.IsChecked);
        }
    );

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

    public bool IsChecked
    {
        get => (bool)this.GetValue(IsCheckedProperty);
        set => this.SetValue(IsCheckedProperty, value);
    }

    event EventHandler<CheckedChangedEventArgs> CheckedChanged;

    public CheckBox()
    {
        this.Drawable = new CheckBoxDrawable(this);
        this.EndInteraction += this.OnCheckChanged;
    }

    private void OnCheckChanged(object sender, TouchEventArgs e)
    {
        this.IsChecked = !this.IsChecked;
    }

    protected override void Dispose(bool disposing)
    {
        if (!this.disposedValue && disposing)
        {
            this.EndInteraction -= this.OnCheckChanged;
        }
        base.Dispose(disposing);
    }
}
