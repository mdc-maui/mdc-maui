using CommunityToolkit.Maui.Markup;
using Material.Components.Maui.Extensions;
using Material.Components.Maui.Tokens;
using Microsoft.Extensions.Logging;
#if WINDOWS
using Microsoft.Maui.LifecycleEvents;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

namespace SampleApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMaterialComponents(
                new List<string>
                {
                    "Roboto-Regular.ttf",
                    "Roboto-Italic.ttf",
                    "Roboto-Medium.ttf",
                    "Roboto-MediumItalic.ttf",
                    "Roboto-Bold.ttf",
                    "Roboto-BoldItalic.ttf",
                }
            )
            .UseMauiCommunityToolkitMarkup();

        FontMapper.AddFont("OpenSans-Regular.ttf", "OpenSans");

#if WINDOWS
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddWindows(wndLifeCycleBuilder =>
            {
                wndLifeCycleBuilder.OnWindowCreated(window =>
                {
                    var nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    var win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                    var winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);

                    var width = 1150;
                    var height = 750;
                    winuiAppWindow.MoveAndResize(new RectInt32((1920 / 2) - (width / 2), (1080 / 2) - (height / 2), width, height));
                });
            });
        });
#endif


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
