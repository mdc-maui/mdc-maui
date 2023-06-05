using MaterialColorUtilities.Schemes;

namespace Material.Components.Maui.Extensions;

internal static class ResourceExtension
{
    public static ICollection<ResourceDictionary> MaterialDictionaries { get; set; }

    private static Scheme<Color> lightScheme = SchemeExtensions.GetDefaultScheme(AppTheme.Light);
    public static Scheme<Color> LightScheme
    {
        get => lightScheme;
        set
        {
            lightScheme = value;
            if (Application.Current.RequestedTheme is AppTheme.Light)
            {
                UpdateMaterialColors();
            }
        }
    }

    private static Scheme<Color> darkScheme = SchemeExtensions.GetDefaultScheme(AppTheme.Dark);
    public static Scheme<Color> DarkScheme
    {
        get => darkScheme;
        set
        {
            darkScheme = value;
            if (Application.Current.RequestedTheme is AppTheme.Dark)
            {
                UpdateMaterialColors();
            }
        }
    }

    internal static void UpdateMaterialColors()
    {
        var scheme =
            Application.Current.RequestedTheme is AppTheme.Light ? LightScheme : DarkScheme;

        var colorRes = MaterialDictionaries.First(
            x => x.GetType() == typeof(Styles.MaterialColors)
        );

        colorRes["PrimaryColor"] = scheme.Primary;
        colorRes["PrimaryContainerColor"] = scheme.PrimaryContainer;
        colorRes["OnPrimaryColor"] = scheme.OnPrimary;
        colorRes["OnPrimaryContainerColor"] = scheme.OnPrimaryContainer;
        colorRes["InversePrimaryColor"] = scheme.InversePrimary;

        colorRes["SecondaryColor"] = scheme.Secondary;
        colorRes["SecondaryContainerColor"] = scheme.SecondaryContainer;
        colorRes["OnSecondaryColor"] = scheme.OnSecondary;
        colorRes["OnSecondaryContainerColor"] = scheme.OnSecondaryContainer;

        colorRes["TertiaryColor"] = scheme.Tertiary;
        colorRes["TertiaryContainerColor"] = scheme.TertiaryContainer;
        colorRes["OnTertiaryColor"] = scheme.OnTertiary;
        colorRes["OnTertiaryContainerColor"] = scheme.OnTertiaryContainer;

        colorRes["SurfaceColor"] = scheme.Surface;
        colorRes["SurfaceDimColor"] = scheme.SurfaceDim;
        colorRes["SurfaceBrightColor"] = scheme.SurfaceBright;
        colorRes["SurfaceContainerLowestColor"] = scheme.SurfaceContainerLowest;
        colorRes["SurfaceContainerLowColor"] = scheme.SurfaceContainerLow;
        colorRes["SurfaceContainerColor"] = scheme.SurfaceContainer;
        colorRes["SurfaceContainerHighColor"] = scheme.SurfaceContainerHigh;
        colorRes["SurfaceContainerHighestColor"] = scheme.SurfaceContainerHighest;
        colorRes["SurfaceVariantColor"] = scheme.SurfaceVariant;
        colorRes["OnSurfaceColor"] = scheme.OnSurface;
        colorRes["OnSurfaceVariantColor"] = scheme.OnSurfaceVariant;
        colorRes["InverseSurfaceColor"] = scheme.InverseSurface;
        colorRes["InverseOnSurfaceColor"] = scheme.InverseOnSurface;

        colorRes["BackgroundColor"] = scheme.Background;
        colorRes["OnBackgroundColor"] = scheme.OnBackground;

        colorRes["ErrorColor"] = scheme.Error;
        colorRes["ErrorContainerColor"] = scheme.ErrorContainer;
        colorRes["OnErrorColor"] = scheme.OnError;
        colorRes["OnErrorContainerColor"] = scheme.OnErrorContainer;

        colorRes["OutlineColor"] = scheme.Outline;
        colorRes["OutlineVariantColor"] = scheme.OutlineVariant;

        colorRes["ShadowColor"] = scheme.Shadow;
    }

    internal static Style FindStyle(this ResourceDictionary resources, string key)
    {
        if (resources.ContainsKey(key))
        {
            return resources[key] as Style;
        }
        if (resources.MergedDictionaries.Any())
        {
            foreach (var r in resources.MergedDictionaries)
            {
                var result = r.FindStyle(key);
                if (result != null)
                {
                    return result;
                }
            }
        }
        return null;
    }
}
