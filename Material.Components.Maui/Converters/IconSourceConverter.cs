using Svg.Model;
using Svg.Skia;
using System.ComponentModel;
using System.Globalization;

namespace Material.Components.Maui.Converters;

public class IconSourceConverter : TypeConverter
{
    private static readonly SkiaAssetLoader assetLoader = new();

    public override object ConvertFrom(
        ITypeDescriptorContext context,
        CultureInfo culture,
        object value
    )
    {
        if (value is string text)
        {
            if (!FileSystem.AppPackageFileExistsAsync(text).Result)
            {
                throw new FileNotFoundException(text);
            }
            using var stream = FileSystem.OpenAppPackageFileAsync(text).Result;
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;
            var document = SvgExtensions.Open(ms);
            return SvgExtensions.ToModel(document, assetLoader, out _, out _).ToSKPicture();
        }
        return null;
    }
}
