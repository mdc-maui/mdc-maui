using MaterialColorUtilities.Schemes;

namespace Material.Components.Maui.Extensions;

internal static class ResourceExtension
{
    public static ResourceDictionary ColorRes { get; set; }

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
        ColorRes["PrimaryColor"] = scheme.Primary;
        ColorRes["PrimaryContainerColor"] = scheme.PrimaryContainer;
        ColorRes["OnPrimaryColor"] = scheme.OnPrimary;
        ColorRes["OnPrimaryContainerColor"] = scheme.OnPrimaryContainer;
        ColorRes["InversePrimaryColor"] = scheme.InversePrimary;

        ColorRes["SecondaryColor"] = scheme.Secondary;
        ColorRes["SecondaryContainerColor"] = scheme.SecondaryContainer;
        ColorRes["OnSecondaryColor"] = scheme.OnSecondary;
        ColorRes["OnSecondaryContainerColor"] = scheme.OnSecondaryContainer;

        ColorRes["TertiaryColor"] = scheme.Tertiary;
        ColorRes["TertiaryContainerColor"] = scheme.TertiaryContainer;
        ColorRes["OnTertiaryColor"] = scheme.OnTertiary;
        ColorRes["OnTertiaryContainerColor"] = scheme.OnTertiaryContainer;

        ColorRes["SurfaceColor"] = scheme.Surface;
        ColorRes["SurfaceDimColor"] = scheme.SurfaceDim;
        ColorRes["SurfaceBrightColor"] = scheme.SurfaceBright;
        ColorRes["SurfaceContainerLowestColor"] = scheme.SurfaceContainerLowest;
        ColorRes["SurfaceContainerLowColor"] = scheme.SurfaceContainerLow;
        ColorRes["SurfaceContainerColor"] = scheme.SurfaceContainer;
        ColorRes["SurfaceContainerHighColor"] = scheme.SurfaceContainerHigh;
        ColorRes["SurfaceContainerHighestColor"] = scheme.SurfaceContainerHighest;
        ColorRes["SurfaceVariantColor"] = scheme.SurfaceVariant;
        ColorRes["OnSurfaceColor"] = scheme.OnSurface;
        ColorRes["OnSurfaceVariantColor"] = scheme.OnSurfaceVariant;
        ColorRes["InverseSurfaceColor"] = scheme.InverseSurface;
        ColorRes["InverseOnSurfaceColor"] = scheme.InverseOnSurface;

        ColorRes["BackgroundColor"] = scheme.Background;
        ColorRes["OnBackgroundColor"] = scheme.OnBackground;

        ColorRes["ErrorColor"] = scheme.Error;
        ColorRes["ErrorContainerColor"] = scheme.ErrorContainer;
        ColorRes["OnErrorColor"] = scheme.OnError;
        ColorRes["OnErrorContainerColor"] = scheme.OnErrorContainer;

        ColorRes["OutlineColor"] = scheme.Outline;
        ColorRes["OutlineVariantColor"] = scheme.OutlineVariant;

        ColorRes["ShadowColor"] = scheme.Shadow;
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
