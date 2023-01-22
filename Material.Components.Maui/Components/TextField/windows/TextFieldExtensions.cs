namespace Material.Components.Maui.Core;
internal static partial class TextFieldExtensions
{
    internal static void SetCursor(this TextField view, bool isArrow)
    {
        (view.Handler?.PlatformView as WTextField)?.SetCursor(isArrow);
    }

    internal static void OpenIME(this TextField view)
    {
        IMEInput.OpenIME(view);
    }

    internal static void CloseIME(this TextField view)
    {
        //IMEInput.CloseIME();
    }
}
