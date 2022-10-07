namespace Material.Components.Maui.Core.Menu;
public partial class PopupMenu : Element
{
    public View Content { get; set; }
    public View Anchor { get; set; }
    public object Result { get; private set; }

    public EventHandler<object> Closed;

    private void OnClosed(object sender, object e)
    {
        this.Closed?.Invoke(sender, this.Result);
    }

#if !WINDOWS && !__ANDROID__
    public void Connect(IMauiContext context)
    {
        throw new NotImplementedException();
    }

    public void Show(int count)
    {
        throw new NotImplementedException();
    }

    public void Close(object result = null)
    {
        throw new NotImplementedException();
    }
#endif
}
