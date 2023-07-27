using System.Text;

#if WINDOWS
using Microsoft.UI;
using WinRT.Interop;
#endif

namespace Material.Components.Maui.Platform.Editable;

public class EditableHandler
{
    public TextField editor;

    TextRange selectionRange = new();
    public TextRange SelectionRange
    {
        get => this.selectionRange;
        set
        {
            if (!this.SelectionRange.Equals(value))
            {
                this.selectionRange = value;
                this.editor.UpdateSelectionRange(value);
                this.SelectionChanged?.Invoke(this, new(value));
            }
        }
    }
    public int TextLength => this.TextBuilder.Length;

    public event EventHandler<SelectionChangedArgs> SelectionChanged;

    public StringBuilder TextBuilder { get; private set; } = new();

    readonly UndoRedoHelper undoRedoHelper = new();

    public EditableHandler(TextField editor)
    {
        this.editor = editor;
    }

    public void UpdateText(string text)
    {
        if (text == null || text == this.ToString())
            return;

        var range = this.SelectionRange.Normalized();
        this.TextBuilder.Clear();
        this.TextBuilder.Append(text);
        this.editor.Text = text;
        this.SelectionRange = new TextRange(
            Math.Min(range.Start, text.Length),
            Math.Min(range.End, text.Length)
        );
    }

    public void ReplaceText(string text)
    {
        var range = this.SelectionRange.Normalized();
        this.ReplaceText(text, range);
    }

    public void ReplaceText(string text, TextRange range)
    {
        if (range.IsRange)
        {
            var replaceText = this.ToString(range.Start, range.Length);
            this.TextBuilder.Replace(replaceText, text, range.Start, range.Length);
        }
        else
            this.TextBuilder.Insert(range.End, text);

        var location = Math.Min(range.Start + text.Length, this.TextLength);
        var newText = this.TextBuilder.ToString();
        this.undoRedoHelper.Add(newText, location);
        this.editor.Text = newText;
        this.SelectionRange = new TextRange(location);
    }

    public void DeleteText()
    {
        var range = this.SelectionRange.Normalized();
        this.DeleteText(range);
    }

    public void DeleteText(TextRange range)
    {
        var location = 0;
        if (range.IsRange)
        {
            var replaceText = this.TextBuilder.ToString(range.Start, range.Length);
            this.TextBuilder.Replace(replaceText, string.Empty, range.Start, range.Length);
            location = Math.Max(0, range.Start);
        }
        else if (this.TextLength > 0 && range.End > 0)
        {
            this.TextBuilder.Remove(range.End - 1, 1);
            location = Math.Max(0, range.End - 1);
        }

        var newText = this.TextBuilder.ToString();
        this.undoRedoHelper.Add(newText, location);
        this.editor.Text = newText;
        this.SelectionRange = new TextRange(location);
    }

    public void CommitText(string text)
    {
        if (!string.IsNullOrEmpty(text))
            this.ReplaceText(text);
    }

    public void Clear()
    {
        this.TextBuilder.Clear();
        this.SelectionRange = new(0);
    }

    public void SelectAll() => this.SelectionRange = new TextRange(0, this.TextBuilder.Length);

    public void CopyRangeTextToClipboard()
    {
        if (this.editor.InputType is InputType.Password) return;

        var range = this.SelectionRange.Normalized();
        if (range.IsRange)
        {
            var text = this.TextBuilder.ToString(range.Start, range.Length);
            Clipboard.SetTextAsync(text);
        }
    }

    public void CopyRangeTextFromClipboard()
    {
        var text = Clipboard.Default.GetTextAsync().Result;
        if (!string.IsNullOrEmpty(text))
            this.ReplaceText(text);
    }

    public void CutRangeTextToClipboard()
    {
        if (this.editor.InputType is InputType.Password) return;

        var range = this.SelectionRange.Normalized();
        if (range.IsRange)
        {
            var text = this.TextBuilder.ToString(range.Start, range.Length);
            Clipboard.Default.SetTextAsync(text);
            this.DeleteText();
        }
    }

    public void Undo()
    {
        var cache = this.undoRedoHelper.GetUndoCache();
        if (cache != null)
        {
            this.TextBuilder.Clear();
            this.TextBuilder.Append(cache.Value.Text);
            var newText = this.TextBuilder.ToString();
            this.editor.Text = newText;
            this.SelectionRange = cache.Value.SelectionRange;
        }
    }

    public void Redo()
    {
        var cache = this.undoRedoHelper.GetRedoCache();
        if (cache != null)
        {
            this.TextBuilder.Clear();
            this.TextBuilder.Append(cache.Value.Text);
            var newText = this.TextBuilder.ToString();
            this.editor.Text = newText;
            this.SelectionRange = cache.Value.SelectionRange;
        }
    }

    public override string ToString()
    {
        return this.TextBuilder.Length > 0 ? this.TextBuilder.ToString() : string.Empty;
    }

    public string ToString(int start, int length)
    {
        start = Math.Max(start, 0);
        length = Math.Clamp(length, 0, this.TextBuilder.Length - start);

        return length > 0 ? this.TextBuilder.ToString(start, length) : string.Empty;
    }

    internal CaretInfo GetCaretInfo()
    {
        var result = this.editor.GetCaretInfo(
            (float)(this.editor.Bounds.Width),
            this.selectionRange.Start
        );
        result.X += (float)this.editor.EditablePadding.Left;
        result.Y += (float)this.editor.EditablePadding.Top;

        return result;
    }

#if WINDOWS
    internal void Navigate(NavigationKind kind)
    {
        switch (kind)
        {
            case NavigationKind.Left:
                this.SelectionRange = new(Math.Max(0, this.SelectionRange.End - 1));
                break;
            case NavigationKind.Right:
                this.SelectionRange = new(Math.Min(this.SelectionRange.End + 1, this.TextLength));
                break;
            case NavigationKind.Up:
                {
                    var caretInfo = this.editor.NavigateUp((float)this.editor.Bounds.Width);
                    this.SelectionRange = new TextRange(caretInfo.Position);
                    break;
                }
            case NavigationKind.Down:
                {
                    var caretInfo = this.editor.NavigateDown((float)this.editor.Bounds.Width);
                    this.SelectionRange = new TextRange(caretInfo.Position);
                    break;
                }
            case NavigationKind.SelectLeft:
                this.SelectionRange = new TextRange(
                    this.SelectionRange.Start,
                    Math.Max(0, this.SelectionRange.End - 1)
                );
                break;
            case NavigationKind.SelectRight:
                this.SelectionRange = new TextRange(
                    this.SelectionRange.Start,
                    Math.Min(this.SelectionRange.End + 1, this.TextLength)
                );
                break;
            case NavigationKind.SelectUp:
                {
                    var caretInfo = this.editor.NavigateUp((float)this.editor.Bounds.Width);
                    this.SelectionRange = new TextRange(this.SelectionRange.Start, caretInfo.Position);
                    break;
                }
            case NavigationKind.SelectDown:
                {
                    var caretInfo = this.editor.NavigateDown((float)this.editor.Bounds.Width);
                    this.SelectionRange = new TextRange(this.SelectionRange.Start, caretInfo.Position);
                    break;
                }
            //TODO
            case NavigationKind.WordLeft:
                this.SelectionRange = new(Math.Max(0, this.SelectionRange.End - 1));
                break;
            //TODO
            case NavigationKind.WordRight:
                this.SelectionRange = new(Math.Min(this.SelectionRange.End + 1, this.TextLength));
                break;
            //TODO
            case NavigationKind.SelectWordleft:
                this.SelectionRange = new TextRange(
                    this.SelectionRange.Start,
                    Math.Max(0, this.SelectionRange.End - 1)
                );
                break;
            //TODO
            case NavigationKind.SelectWordRight:
                this.SelectionRange = new TextRange(
                    this.SelectionRange.Start,
                    Math.Min(this.SelectionRange.End + 1, this.TextLength)
                );
                break;
            default:
                break;
        }
    }

    internal WindowId? GetWindowId()
    {
        var hwnd = WindowNative.GetWindowHandle(this.editor.Window.Handler.PlatformView);

        return hwnd != IntPtr.Zero ? Win32Interop.GetWindowIdFromWindow(hwnd) : null;
    }

    internal Thickness GetEditablePadding() => this.editor.EditablePadding;
#endif
}
