namespace Material.Components.Maui.Extensions;

public static class SkCanvasViewExtensions
{
    public static void OnTouchEvents(this ITouchElement view, SKTouchEventArgs e)
    {
        if (e.ActionType == SKTouchAction.Pressed)
        {
            view.OnPressed(e);
            view.PressedTimer = new Timer(new TimerCallback((e) =>
            {
                view.PressedTimer?.Dispose();
                var v = view as View;
                v.Dispatcher.Dispatch(() =>
                {
                    view.OnLongPressed((SKTouchEventArgs)e);
                });
            }), e, 500, Timeout.Infinite);
            e.Handled = true;
        }
        else if (e.ActionType == SKTouchAction.Released)
        {
            if (view.PressedTimer is not null)
            {
                view.OnClicked(e);
            }
            view.PressedTimer?.Dispose();
            e.Handled = true;
        }
    }
}