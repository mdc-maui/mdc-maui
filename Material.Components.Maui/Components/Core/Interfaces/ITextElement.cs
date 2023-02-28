using Topten.RichTextKit;

namespace Material.Components.Maui.Core.Interfaces;

public interface ITextElement : IForegroundElement
{
    string Text { get; set; }
    TextBlock InternalText { get; set; }
    TextStyle TextStyle { get; set; }
    string FontFamily { get; set; }
    float FontSize { get; set; }
    int FontWeight { get; set; }
    bool FontItalic { get; set; }

    void OnChanged();
}
