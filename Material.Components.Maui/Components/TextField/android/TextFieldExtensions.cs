namespace Material.Components.Maui.Core;
internal static partial class TextFieldExtensions
{
    internal static void OpenIME(this TextField view)
    {
        (view.Handler?.PlatformView as ATextField)?.OpenIME();
    }

    internal static void CloseIME(this TextField view)
    {
        (view.Handler?.PlatformView as ATextField)?.CloseIME();
    }
}
