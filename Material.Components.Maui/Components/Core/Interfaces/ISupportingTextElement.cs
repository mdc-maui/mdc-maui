using Topten.RichTextKit;

namespace Material.Components.Maui.Core.Interfaces;
public interface ISupportingTextElement
{
    public string SupportingText { get; set; }
    TextBlock InternalSupportingText { get; set; }
    public Color SupportingTextColor { get; set; }
    public float SupportingTextOpacity { get; set; }
    TextStyle SupportingTextStyle { get; set; }
}
