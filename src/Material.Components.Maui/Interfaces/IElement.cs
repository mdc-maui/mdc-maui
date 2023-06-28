using Microsoft.Maui.Controls.Internals;

namespace Material.Components.Maui.Interfaces;

public interface IElement
{
    ViewState ViewState { get; }

    bool IsEnabled { get; set; }

    void OnPropertyChanged();

    void InvalidateMeasure()
    {
        if (this is View view)
            (view.Parent as VisualElement)?.InvalidateMeasureNonVirtual(
                InvalidationTrigger.MeasureChanged
            );
        this.OnPropertyChanged();
    }

    public static readonly BindableProperty IsEnabledProperty = BindableProperty.Create(
        nameof(IsEnabled),
        typeof(bool),
        typeof(IElement),
        true,
        propertyChanged: (bo, ov, nv) =>
        {
            ((TouchGraphicsView)bo).ViewState = ViewState.Disabled;
            ((IElement)bo).OnPropertyChanged();
        }
    );
}
