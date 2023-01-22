namespace Material.Components.Maui.Core.Interfaces;

internal interface ITrailingIconElement
{
    IconKind TrailingIcon { get; set; }
    SKPicture TrailingIconSource { get; set; }
    Color TrailingIconColor { get; set; }

    SKRect TrailingIconBounds { get; set; }

    event EventHandler<SKTouchEventArgs> TrailingIconClicked;
}
