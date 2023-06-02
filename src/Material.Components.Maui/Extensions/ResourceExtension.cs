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
        ColorRes["SecondaryColor"] = scheme.Secondary;
        ColorRes["SecondaryContainerColor"] = scheme.SecondaryContainer;
        ColorRes["TertiaryColor"] = scheme.Tertiary;
        ColorRes["TertiaryContainerColor"] = scheme.TertiaryContainer;
        ColorRes["SurfaceColor"] = scheme.Surface;
        ColorRes["SurfaceVariantColor"] = scheme.SurfaceVariant;
        ColorRes["BackgroundColor"] = scheme.Background;
        ColorRes["ErrorColor"] = scheme.Error;
        ColorRes["ErrorContainerColor"] = scheme.ErrorContainer;
        ColorRes["OnPrimaryColor"] = scheme.OnPrimary;
        ColorRes["OnPrimaryContainerColor"] = scheme.OnPrimaryContainer;
        ColorRes["OnSecondaryColor"] = scheme.OnSecondary;
        ColorRes["OnSecondaryContainerColor"] = scheme.OnSecondaryContainer;
        ColorRes["OnTertiaryColor"] = scheme.OnTertiary;
        ColorRes["OnTertiaryContainerColor"] = scheme.OnTertiaryContainer;
        ColorRes["OnSurfaceColor"] = scheme.OnSurface;
        ColorRes["OnSurfaceVariantColor"] = scheme.OnSurfaceVariant;
        ColorRes["OnErrorColor"] = scheme.OnError;
        ColorRes["OnErrorContainerColor"] = scheme.OnErrorContainer;
        ColorRes["OnBackgroundColor"] = scheme.OnBackground;
        ColorRes["OutlineColor"] = scheme.Outline;
        ColorRes["ShadowColor"] = scheme.Shadow;
        ColorRes["SurfaceTintColor"] = scheme.Primary;
        ColorRes["InversePrimaryColor"] = scheme.InversePrimary;
        ColorRes["InverseSurfaceColor"] = scheme.InverseSurface;
        ColorRes["InverseOnSurfaceColor"] = scheme.InverseOnSurface;
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
