namespace Material.Components.Maui.Core.Interfaces;

public interface IIconElement
{
    IconKind IconKind { get; set; }
    string IconData { get; set; }
    SKPicture IconSource { get; set; }
}
