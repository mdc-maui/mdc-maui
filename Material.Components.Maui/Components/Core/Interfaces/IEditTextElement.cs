using Topten.RichTextKit.Editor;

namespace Material.Components.Maui.Core.Interfaces;
internal interface IEditTextElement
{
    string Text { get; set; }
    TextDocument TextDocument { get; set; }
    TextStyle TextStyle { get; set; }
    string FontFamily { get; set; }
    float FontSize { get; set; }
    int FontWeight { get; set; }
    bool FontItalic { get; set; }

    event EventHandler<TextChangedEventArgs> TextChanged;
    void OnTextChanged(string oldValue, string newValue);
    void OnChanged();
}
