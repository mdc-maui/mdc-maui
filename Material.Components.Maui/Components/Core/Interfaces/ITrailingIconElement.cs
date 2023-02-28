namespace Material.Components.Maui.Core.Interfaces;

public interface ITrailingIconElement
{
    IconKind TrailingIconKind { get; set; }
    string TrailingIconData { get; set; }
    SKPicture TrailingIconSource { get; set; }
    Color TrailingIconColor { get; set; }
    SKRect TrailingIconBounds { get; set; }

    event EventHandler<SKTouchEventArgs> TrailingIconClicked;
}
