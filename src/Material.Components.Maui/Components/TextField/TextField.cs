namespace Material.Components.Maui;
public class TextField : TemplatedView
{
    public static readonly BindableProperty TextProperty = IEditableElement.TextProperty;
    public static readonly BindableProperty SelectionRangeProperty = IEditableElement.SelectionRangeProperty;



    public string Text
    {
        get => (string)this.GetValue(TextProperty);
        set => this.SetValue(TextProperty, value);
    }



    public TextRange SelectionRange
    {
        get => (TextRange)this.GetValue(SelectionRangeProperty);
        set => this.SetValue(SelectionRangeProperty, value);
    }

    public TextField()
    {
    }

}
