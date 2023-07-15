using Material.Components.Maui.Platform.Editable;
using Microsoft.Maui.Platform;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Text.Core;
using Rect = Windows.Foundation.Rect;

namespace Material.Components.Maui.Platform;

public class PlatformTextEditor : PlatformTouchGraphicsView, IDisposable
{
    public CoreTextEditContext EditContext { get; private set; }

    readonly EditableHandler editableHandler;

    public PlatformTextEditor(EditableHandler handler) : base()
    {
        this.editableHandler = handler;
        this.editableHandler.SelectionChanged += this.OnSelectionChanged;

        this.IsTabStop = true;
        // The CoreTextEditContext processes text input, but other keys are
        // the app's responsibility.

        // Create a CoreTextEditContext for our custom edit control.
        var manager = CoreTextServicesManager.GetForCurrentView();
        this.EditContext = manager.CreateEditContext();

        // For demonstration purposes, this sample sets the Input Pane display policy to Manual
        // so that it can manually show the software keyboard when the control gains focus and
        // dismiss it when the control loses focus. If you leave the policy as Automatic, then
        // the system will hide and show the Input Pane for you. Note that on Desktop, you will
        // need to implement the UIA text pattern to get expected automatic behavior.
        this.EditContext.InputPaneDisplayPolicy = CoreTextInputPaneDisplayPolicy.Manual;

        // Set the input scope to Text because this text box is for any text.
        // This also informs software keyboards to show their regular
        // text entry layout.  There are many other input scopes and each will
        // inform a keyboard layout and text behavior.
        this.EditContext.InputScope = CoreTextInputScope.Text;

        // The system raises this event to request a specific range of text.
        this.EditContext.TextRequested += this.OnEditContextTextRequested;

        //this.EditContext.SelectionRequested += this.OnEditContextSelectionRequested;

        // The system raises this event to update text in the edit control.
        this.EditContext.TextUpdating += this.OnEditContextTextUpdating;

        // The system raises this event to change the selection in the edit control.
        //this.EditContext.SelectionUpdating += this.OnEditContextSelectionUpdating;

        // The system raises this event to request layout information.
        // This is used to help choose a position for the IME candidate window.
        this.EditContext.LayoutRequested += this.OnEditContextLayoutRequested;
    }

    protected override void OnCharacterReceived(CharacterReceivedRoutedEventArgs e)
    {
        if (this.FocusState != FocusState.Unfocused && !char.IsControl(e.Character))
        {
            this.editableHandler.CommitText(e.Character.ToString());
        }
    }

    protected override void OnKeyDown(KeyRoutedEventArgs e)
    {
        var isCtrlPressed = Microsoft.UI.Input.InputKeyboardSource
            .GetKeyStateForCurrentThread(VirtualKey.Control)
            .HasFlag(CoreVirtualKeyStates.Down);

        var isShiftPressed = Microsoft.UI.Input.InputKeyboardSource
            .GetKeyStateForCurrentThread(VirtualKey.Shift)
            .HasFlag(CoreVirtualKeyStates.Down);

        if (e.Key == VirtualKey.Back)
        {
            this.editableHandler.DeleteText();
        }
        else if (e.Key == VirtualKey.Enter)
        {
            this.editableHandler.CommitText("\n");
        }
        else if (e.Key == VirtualKey.Left)
        {
            if (isShiftPressed && isCtrlPressed)
                this.editableHandler.Navigate(NavigationKind.SelectWordleft);
            else if (isShiftPressed)
                this.editableHandler.Navigate(NavigationKind.SelectLeft);
            else if (isCtrlPressed)
                this.editableHandler.Navigate(NavigationKind.WordLeft);
            else
                this.editableHandler.Navigate(NavigationKind.Left);
        }
        else if (e.Key == VirtualKey.Right)
        {
            if (isShiftPressed && isCtrlPressed)
                this.editableHandler.Navigate(NavigationKind.SelectWordRight);
            else if (isShiftPressed)
                this.editableHandler.Navigate(NavigationKind.SelectRight);
            else if (isCtrlPressed)
                this.editableHandler.Navigate(NavigationKind.WordRight);
            else
                this.editableHandler.Navigate(NavigationKind.Right);
        }
        else if (e.Key == VirtualKey.Up)
        {
            if (isShiftPressed)
                this.editableHandler.Navigate(NavigationKind.SelectUp);
            else
                this.editableHandler.Navigate(NavigationKind.Up);
        }

        else if (e.Key == VirtualKey.Down)
        {
            if (isShiftPressed)
                this.editableHandler.Navigate(NavigationKind.SelectDown);
            else
                this.editableHandler.Navigate(NavigationKind.Down);
        }
        else if (isCtrlPressed && e.Key == VirtualKey.A)
        {
            this.editableHandler.SelectAll();
        }
        else if (isCtrlPressed && e.Key == VirtualKey.C)
        {
            this.editableHandler.CopyRangeTextToClipboard();
        }
        else if (isCtrlPressed && e.Key == VirtualKey.V)
        {
            this.editableHandler.CopyRangeTextFromClipboard();
        }
        else if (isCtrlPressed && e.Key == VirtualKey.X)
        {
            this.editableHandler.CutRangeTextToClipboard();
        }
        else if (isCtrlPressed && e.Key == VirtualKey.Z)
        {
            this.editableHandler.Undo();
        }
        else if (isCtrlPressed && e.Key == VirtualKey.Y)
        {
            this.editableHandler.Redo();
        }
    }

    private void OnSelectionChanged(object sender, SelectionChangedArgs e)
    {
        var start = e.SelectionRange.Start;
        var end = e.SelectionRange.End;
        this.EditContext.NotifySelectionChanged(new CoreTextRange(start, end));
    }

    protected override void OnTapped(TappedRoutedEventArgs e)
    {
        this.Focus(FocusState.Pointer);
    }

    private void OnEditContextTextRequested(
        CoreTextEditContext sender,
        CoreTextTextRequestedEventArgs args
    )
    {
        var request = args.Request;
        request.Text = this.editableHandler.ToString(
            request.Range.StartCaretPosition,
            request.Range.EndCaretPosition
        );
    }

    private void OnEditContextTextUpdating(
        CoreTextEditContext sender,
        CoreTextTextUpdatingEventArgs args
    )
    {
        if (!string.IsNullOrWhiteSpace(args.Text)) this.editableHandler.CommitText(args.Text);

    }

    Rect lastEditContextLayoutRequestRect = Rect.Empty;
    int lastCaretPositon;

    private void OnEditContextLayoutRequested(
        CoreTextEditContext sender,
        CoreTextLayoutRequestedEventArgs args
    )
    {
        var caretPosition = this.editableHandler.SelectionRange.Start;
        if (
            caretPosition == this.lastCaretPositon
            && this.lastEditContextLayoutRequestRect != Rect.Empty
        )
        {
            args.Request.LayoutBounds.TextBounds = this.lastEditContextLayoutRequestRect;
            args.Request.LayoutBounds.ControlBounds = this.lastEditContextLayoutRequestRect;
            return;
        }

        var windowId = this.editableHandler.GetWindowId();

        if (windowId == null)
            return;
        var window = AppWindow.GetFromWindowId(windowId.Value);

        var windowLocation = window.Position;
        var viewLocation = this.TransformToVisual(null)
            .TransformPoint(new Windows.Foundation.Point());
        var caretInfo = this.editableHandler.GetCaretInfo();

        var requestRect = new Rect
        {
            X = windowLocation.X + viewLocation.X + caretInfo.X + caretInfo.Width + 8,
            Y = windowLocation.Y + viewLocation.Y + caretInfo.Y + caretInfo.Height,
            Width = 200,
            Height = 0
        };

        this.lastCaretPositon = caretPosition;
        this.lastEditContextLayoutRequestRect = requestRect;

        args.Request.LayoutBounds.TextBounds = requestRect;
        args.Request.LayoutBounds.ControlBounds = requestRect;
    }

    bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.EditContext.TextRequested -= this.OnEditContextTextRequested;
                this.EditContext.TextUpdating -= this.OnEditContextTextUpdating;
                this.EditContext.LayoutRequested -= this.OnEditContextLayoutRequested;
            }
            this.disposedValue = true;
        }
    }
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
