namespace Material.Components.Maui.Core;

public class ValueChangedEventArgs : EventArgs
{
    public object Value { get; init; }

    public ValueChangedEventArgs(object value)
    {
        this.Value = value;
    }
}
