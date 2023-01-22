using ImeSharp;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace Material.Components.Maui.Core;

internal static class IMEInput
{
    [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
    private static extern IntPtr FindWindowEx(
        IntPtr hwndParent,
        IntPtr hwndChildAfter,
        string lpszClass,
        string lpszWindow
    );

    private static bool initialized;

    private static void Initialize(IntPtr hwnd)
    {
        if (!initialized)
        {
            InputMethod.Initialize(hwnd, true);
            initialized = true;
        }
    }

    internal static void OpenIME(TextField view)
    {
        if (!initialized)
        {
            var topHwnd = WindowNative.GetWindowHandle(view.Window.Handler.PlatformView);
            var currenthwnd = FindWindowEx(
                topHwnd,
                IntPtr.Zero,
                "Microsoft.UI.Content.DesktopChildSiteBridge",
                "DesktopChildSiteBridge"
            );
            Initialize(currenthwnd);
        }
        if (!InputMethod.Enabled)
            InputMethod.Enabled = true;
    }

    internal static void CloseIME()
    {
        InputMethod.Enabled = false;
    }
}
