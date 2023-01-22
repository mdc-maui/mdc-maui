using Topten.RichTextKit;
using Topten.RichTextKit.Editor;
using Topten.RichTextKit.Utils;

namespace Material.Components.Maui.Core;

internal class EditTextManager
{
    private readonly TextFieldHandler Handler;
    private TextField VirtualView => this.Handler.VirtualView;
    public TextDocument Document
    {
        get => this.Handler.VirtualView.TextDocument;
        set => this.Handler.VirtualView.TextDocument = value;
    }

    internal string Text
    {
        get => this.Document.Text;
        set => this.Document.Text = value;
    }

    private bool focus = false;
    internal bool Focus
    {
        get => this.focus;
        set
        {
            this.focus = value;
            this.VirtualView.InternalFocus = value;
        }
    }

    public int CaretPosition
    {
        get => this.VirtualView.CaretPosition;
        set
        {
            var position = Math.Max(0, Math.Min(value, this.Document.Length - 1));
            if (this.VirtualView.CaretPosition != position)
            {
                this.VirtualView.CaretPosition = position;
            }
            this.Range = new TextRange(position);
        }
    }

    public TextRange Range
    {
        get => this.VirtualView.SelectionTextRange;
        set => this.VirtualView.SelectionTextRange = value;
    }

    public EditTextManager(TextFieldHandler handler)
    {
        this.Handler = handler;
    }

    internal void CommitText(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            this.Document.ReplaceText(null, this.Range, text, EditSemantics.None);
            var textLength = Utf32Utils.ToUtf32(text).Length;
            this.CaretPosition += textLength;
            this.VirtualView.Text = this.Document.Text;
        }
    }

    internal void DeleteRangeText()
    {
        if (this.Range.IsRange)
        {
            this.Document.ReplaceText(null, this.Range, string.Empty, EditSemantics.None);
            this.CaretPosition = this.Range.Start;
        }
        else
        {
            this.Document.ReplaceText(
                null,
                new TextRange(Math.Max(0, this.Range.Start - 1), this.Range.End),
                string.Empty,
                EditSemantics.None
            );
            this.CaretPosition -= 1;
        }
        this.VirtualView.Text = this.Document.Text;
    }

    internal void SelectAll()
    {
        this.Range = new TextRange(0, this.Document.Length - 1);
    }

    internal void CopyRangeTextToClipboard()
    {
        if (this.Range.IsRange)
        {
            Clipboard.SetTextAsync(this.Document.GetText(this.Range).ToString());
        }
    }

    internal void CopyRangeTextFromClipboard()
    {
        var text = Clipboard.Default.GetTextAsync().Result;
        if (!string.IsNullOrEmpty(text))
        {
            this.Document.ReplaceText(null, this.Range, text, EditSemantics.None);
            var textLength = Utf32Utils.ToUtf32(text).Length;
            this.CaretPosition += textLength;
        }
        this.VirtualView.Text = this.Document.Text;
    }

    internal void CutRangeTextToClipboard()
    {
        if (this.Range.IsRange)
        {
            Clipboard.Default.SetTextAsync(this.Document.GetText(this.Range).ToString());
            this.Document.ReplaceText(null, this.Range, string.Empty, EditSemantics.None);
            this.CaretPosition = this.Range.Start;
        }
        this.VirtualView.Text = this.Document.Text;
    }

    internal void Navigate(NavigationKind kind)
    {
        float? xCoord = 0f;
        this.CaretPosition = this.Document
            .Navigate(new CaretPosition(this.CaretPosition), kind, 0, ref xCoord)
            .CodePointIndex;
    }

    internal void Undo()
    {
        this.Document.Undo(null);
    }

    internal void Redo()
    {
        this.Document.Redo(null);
    }

    internal SKRect GetCaretBounds()
    {
        return this.Document.GetCaretInfo(new CaretPosition(this.CaretPosition)).CaretRectangle;
    }
}
