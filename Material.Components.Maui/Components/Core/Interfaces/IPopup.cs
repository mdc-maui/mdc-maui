namespace Material.Components.Maui.Core.Interfaces;
public interface IPopup
{
    View Content { get; set; }
    View Anchor { get; set; }
    LayoutAlignment HorizontalOptions { get; set; }
    LayoutAlignment VerticalOptions { get; set; }

    void Show(IMauiContext context);
}
