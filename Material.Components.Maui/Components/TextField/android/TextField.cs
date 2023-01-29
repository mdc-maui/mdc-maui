using Android.Content;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using SKCanvasView = SkiaSharp.Views.Android.SKCanvasView;

namespace Material.Components.Maui.Core;

public class ATextField : SKCanvasView
{
    private readonly EditTextManager editTextManager;
    private readonly InputConnection inputConnection;
    private readonly InputMethodManager inputMethodManager;

    internal ATextField(Context context, EditTextManager editTextManager) : base(context)
    {
        this.editTextManager = editTextManager;
        this.inputConnection = new InputConnection(this, editTextManager);
        this.inputMethodManager = (InputMethodManager)
            context.GetSystemService(Context.InputMethodService);
    }

    public void OpenIME()
    {
        if (!this.Focusable || !this.FocusableInTouchMode)
        {
            this.Focusable = true;
            this.FocusableInTouchMode = true;
        }
        this.RequestFocus();
        this.RequestFocusFromTouch();
        this.inputMethodManager.ShowSoftInput(this, 0);
    }

    public void CloseIME()
    {
        this.inputMethodManager.HideSoftInputFromWindow(this.WindowToken, 0);
    }

    public override bool OnCheckIsTextEditor()
    {
        return true;
    }

    public override IInputConnection OnCreateInputConnection(EditorInfo outAttrs)
    {
        outAttrs.InputType = InputTypes.ClassText | InputTypes.TextFlagMultiLine;
        outAttrs.InitialSelStart = 0;
        outAttrs.InitialSelEnd = 0;
        outAttrs.ImeOptions = ImeFlags.NoFullscreen;
        outAttrs.InitialCapsMode = this.inputConnection.GetCursorCapsMode(0);
        return this.inputConnection;
    }

    public override bool DispatchKeyEvent(KeyEvent e)
    {
        return this.inputConnection.SendKeyEvent(e);
    }

    public override bool DispatchTouchEvent(MotionEvent e)
    {
        this.Parent.RequestDisallowInterceptTouchEvent(
            e.Action is MotionEventActions.Move
        );
        return base.DispatchTouchEvent(e);
    }
}
