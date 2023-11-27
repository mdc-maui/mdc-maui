namespace Material.Components.Maui;

public partial class Popup
{
    private void PlatformShow(Page anchor)
    {
        throw new NotImplementedException();
    }

    public void Close(object result = null)
    {
        throw new NotImplementedException();
    }

    protected virtual void Dispose(bool disposing)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
