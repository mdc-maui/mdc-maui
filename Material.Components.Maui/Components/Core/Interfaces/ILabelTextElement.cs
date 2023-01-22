using Topten.RichTextKit;

namespace Material.Components.Maui.Core.Interfaces;
internal interface ILabelTextElement
{
    public string LabelText { get; set; }
    TextBlock InternalLabelText { get; set; }
    public Color LabelTextColor { get; set; }
    public float LabelTextOpacity { get; set; }
    TextStyle LabelTextStyle { get; set; }
}
