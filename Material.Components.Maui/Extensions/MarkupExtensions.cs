using CommunityToolkit.Maui.Markup;

namespace Material.Components.Maui.Extensions;

/// <summary>
/// TODO
/// </summary>
public static class MarkupExtensions
{
    public static Button BindText(this Button view, string path)
    {
        view.Bind(Button.TextProperty, path);
        return view;
    }

}
