using Topten.RichTextKit;

namespace Material.Components.Maui.Core.Interfaces;
internal interface ITextElement
{
    string Text { get; set; }
    TextBlock TextBlock { get; set; }
    TextStyle TextStyle { get; set; }
    string FontFamily { get; set; }
    float FontSize { get; set; }
    int FontWeight { get; set; }
    bool FontItalic { get; set; }

    void OnTextBlockChanged();
}
