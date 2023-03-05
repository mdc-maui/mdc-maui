namespace SampleApp;

public partial class MainPage : ContentPage, IVisualTreeElement
{
    public MainPage()
    {
        try
        {
            this.InitializeComponent();
        }
        catch (Exception ex) { }
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() =>
        this.Content != null
            ? new List<IVisualTreeElement> { this.Content }
            : Array.Empty<IVisualTreeElement>().ToList();

    public IVisualTreeElement GetVisualParent() => null;
}
