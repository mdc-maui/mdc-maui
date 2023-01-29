using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Java.Lang;
using System.Diagnostics;
using Topten.RichTextKit;
using Topten.RichTextKit.Editor;
using Math = System.Math;

namespace Material.Components.Maui.Core;

internal class InputConnection : BaseInputConnection
{
    private readonly EditTextManager editTextManager;
    private bool isShiftPressed;

    internal InputConnection(ATextField view, EditTextManager editTextManager) : base(view, true)
    {
        this.editTextManager = editTextManager;
    }

    public override void CloseConnection()
    {
        base.CloseConnection();
    }

    public override bool BeginBatchEdit()
    {
        return true;
    }

    public override bool CommitText(ICharSequence charSequence, int newCursorPosition)
    {
        Debug.WriteLine(charSequence.ToString());
        this.editTextManager.CommitText(charSequence.ToString());
        return true;
    }

    public override bool SendKeyEvent(KeyEvent e)
    {
        var position = this.editTextManager.CaretPosition;
        var start = this.editTextManager.Range.Start;
        var end = this.editTextManager.Range.End;
        var textLength = this.editTextManager.Document.Length;

        if (e.Action == KeyEventActions.Down)
        {
            switch (e.KeyCode)
            {
                case Keycode.ShiftLeft:
                case Keycode.ShiftRight:
                    this.isShiftPressed = true;
                    return true;
                case Keycode.Del:
                    this.editTextManager.DeleteRangeText();
                    return true;
                case Keycode.Enter:
                    this.editTextManager.CommitText("\n");
                    return true;
                case Keycode.DpadLeft:
                    if (this.isShiftPressed)
                    {
                        if (end > position)
                            this.editTextManager.Range = new TextRange(position, end - 1);
                        else
                            this.editTextManager.Range = new TextRange(
                                Math.Max(0, start - 1),
                                position
                            );
                    }
                    else if (start != end)
                        this.editTextManager.CaretPosition = start;
                    else
                        this.editTextManager.CaretPosition--;
                    return true;
                case Keycode.DpadRight:
                    if (this.isShiftPressed)
                    {
                        if (start < position)
                            this.editTextManager.Range = new TextRange(start + 1, position);
                        else
                            this.editTextManager.Range = new TextRange(
                                position,
                                Math.Min(end + 1, textLength - 1)
                            );
                    }
                    else if (start != end)
                        this.editTextManager.CaretPosition = end;
                    else
                        this.editTextManager.CaretPosition++;
                    return true;
                case Keycode.DpadUp:
                    if (this.isShiftPressed)
                    {
                        var currPosition = end > position ? end : start;
                        float? xCoord = 0f;
                        var newPosition = this.editTextManager.Document
                            .Navigate(
                                new CaretPosition(currPosition),
                                NavigationKind.LineUp,
                                0,
                                ref xCoord
                            )
                            .CodePointIndex;
                        this.editTextManager.Range = new TextRange(newPosition, this.editTextManager.CaretPosition).Normalized;
                    }
                    else
                        this.editTextManager.Navigate(NavigationKind.LineUp);
                    return true;
                case Keycode.DpadDown:
                    if (this.isShiftPressed)
                    {
                        var currPosition = start < position ? start : end;
                        float? xCoord = 0f;
                        var newPosition = this.editTextManager.Document
                            .Navigate(
                                new CaretPosition(currPosition),
                                NavigationKind.LineDown,
                                0,
                                ref xCoord
                            )
                            .CodePointIndex;
                        this.editTextManager.Range = new TextRange(this.editTextManager.CaretPosition, newPosition).Normalized;
                    }
                    else
                        this.editTextManager.Navigate(NavigationKind.LineDown);
                    return true;
            }
        }
        else if (e.Action == KeyEventActions.Up)
        {
            if (e.KeyCode is Keycode.ShiftLeft or Keycode.ShiftRight)
            {
                this.isShiftPressed = false;
                return true;
            }
        }
        return false;
    }

    public override bool SetSelection(int start, int end)
    {
        this.editTextManager.Range = new TextRange(start, end);
        return true;
    }

    public override bool PerformContextMenuAction(int id)
    {
        switch (id)
        {
            case Android.Resource.Id.SelectAll:
                this.SetSelection(0, 3);
                this.editTextManager.SelectAll();
                return true;
            case Android.Resource.Id.Cut:
                this.editTextManager.CutRangeTextToClipboard();
                return true;
            case Android.Resource.Id.Paste:
            case Android.Resource.Id.PasteAsPlainText:
                this.editTextManager.CopyRangeTextFromClipboard();
                return true;
            case Android.Resource.Id.Copy:
                this.editTextManager.CopyRangeTextToClipboard();
                return true;
            case Android.Resource.Id.Undo:
                this.editTextManager.Undo();
                return true;
            case Android.Resource.Id.Redo:
                this.editTextManager.Redo();
                return true;
        }
        return false;
    }

    public override ICharSequence GetSelectedTextFormatted([GeneratedEnum] GetTextFlags flags)
    {
        return new Java.Lang.String(
            this.editTextManager.Document.GetText(this.editTextManager.Range).ToString()
        );
    }

    public override ExtractedText GetExtractedText(
        ExtractedTextRequest request,
        [GeneratedEnum] GetTextFlags flags
    )
    {
        var result = new ExtractedText
        {
            Text = new Java.Lang.String(this.editTextManager.Text),
            SelectionStart = this.editTextManager.Range.Start,
            SelectionEnd = this.editTextManager.Range.End
        };
        return result;
    }

    public override ICharSequence GetTextAfterCursorFormatted(
        int length,
        [GeneratedEnum] GetTextFlags flags
    )
    {
        var text = this.editTextManager.Document.GetText(
            new TextRange(
                this.editTextManager.Range.Start,
                Math.Min(
                    this.editTextManager.Range.End + length,
                    this.editTextManager.Document.Length - 1
                )
            )
        );
        return new Java.Lang.String(text.ToString());
    }

    public override ICharSequence GetTextBeforeCursorFormatted(
        int length,
        [GeneratedEnum] GetTextFlags flags
    )
    {
        var text = this.editTextManager.Document.GetText(
            new TextRange(
                Math.Max(0, this.editTextManager.Range.Start - length),
                this.editTextManager.Range.Start
            )
        );
        return new Java.Lang.String(text.ToString());
    }
}
