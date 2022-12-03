//https://github.com/toptensoftware/RichTextKit/issues/7

using Topten.RichTextKit;

namespace Material.Components.Maui.Tokens;

public class FontMapper : Topten.RichTextKit.FontMapper
{
    internal static TextStyle DefaultStyle { get; private set; }
    private static readonly Dictionary<string, List<SKTypeface>> fonts = new();

    public static void AddFont(string filename, string alias = null)
    {
        if (!FileSystem.AppPackageFileExistsAsync(filename).Result)
            throw new FileNotFoundException(filename);

        using var stream = FileSystem.OpenAppPackageFileAsync(filename).Result;
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        ms.Position = 0;
        var tf = SKTypeface.FromStream(ms);
        if (tf == null)
            return;

        var qualifiedName = alias ?? tf.FamilyName;
        if (tf.FontSlant != SKFontStyleSlant.Upright)
            qualifiedName += "-Italic";

        if (!fonts.TryGetValue(qualifiedName, out var listFonts))
        {
            listFonts = new List<SKTypeface>();
            fonts[qualifiedName] = listFonts;
        }
        listFonts.Add(tf);
        return;
    }

    static FontMapper()
    {
        Default = new FontMapper();
        DefaultStyle = new TextStyle
        {
            FontFamily = "default",
            FontSize = 14,
            FontItalic = false,
            FontWeight = 400
        };
    }

    private FontMapper() { }

    public override SKTypeface TypefaceFromStyle(IStyle style, bool ignoreFontVariants)
    {
        var qualifiedName = style.FontFamily;
        if (style.FontItalic)
            qualifiedName += "-Italic";

        if (fonts.TryGetValue(qualifiedName, out var listFonts))
        {
            return listFonts.MinBy(x => Math.Abs(x.FontWeight - style.FontWeight));
        }
        var s = base.TypefaceFromStyle(style, ignoreFontVariants);
        return s;
    }
}
