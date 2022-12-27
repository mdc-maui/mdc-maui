namespace Material.Components.Maui;

// Copy from XamarinCommunityToolkit
// https://github.com/xamarin/XamarinCommunityToolkit/blob/main/src/CommunityToolkit/Xamarin.CommunityToolkit/Views/LazyView

/// <summary>
/// This a basic implementation of the LazyView based on <see cref="BaseLazyView"/> use this an exemple to create yours
/// </summary>
/// <typeparam name="TView">Any <see cref="View"/></typeparam>
public class LazyView<TView> : BaseLazyView where TView : View, new()
{
    /// <summary>
    /// This method initializes your <see cref="LazyView{TView}"/>.
    /// </summary>
    /// <returns><see cref="ValueTask"/></returns>
    public override ValueTask LoadViewAsync()
    {
        View view = new TView { BindingContext = BindingContext };

        Content = view;

        SetIsLoaded(true);
        return new ValueTask(Task.FromResult(true));
    }
}

[Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
public abstract class BaseLazyView : ContentView, IDisposable
{
    internal static readonly BindablePropertyKey IsLoadedPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(IsLoaded),
            typeof(bool),
            typeof(BaseLazyView),
            default
        );

    /// <summary>
    /// This is a read-only <see cref="BindableProperty"/> that indicates when the view is loaded.
    /// </summary>
    public static readonly BindableProperty IsLoadedProperty = IsLoadedPropertyKey.BindableProperty;

    /// <summary>
    /// This is a read-only property that indicates when the view is loaded.
    /// </summary>
    public new bool IsLoaded => (bool)GetValue(IsLoadedProperty);

    /// <summary>
    /// This method change the value of the <see cref="IsLoaded"/> property.
    /// </summary>
    /// <param name="isLoaded"></param>
    protected void SetIsLoaded(bool isLoaded) => SetValue(IsLoadedPropertyKey, isLoaded);

    /// <summary>
    /// Use this method to do the initialization of the <see cref="View"/> and change the status IsLoaded value here.
    /// </summary>
    /// <returns><see cref="ValueTask"/></returns>
    public abstract ValueTask LoadViewAsync();

    /// <summary>
    /// This method dispose the <see cref="ContentView.Content"/> if it's <see cref="IDisposable"/>.
    /// </summary>
    public void Dispose()
    {
        if (Content is IDisposable disposable)
            disposable.Dispose();
    }

    protected override void OnBindingContextChanged()
    {
        if (Content != null)
            Content.BindingContext = BindingContext;
    }
}
