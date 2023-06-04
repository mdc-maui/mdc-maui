namespace Material.Components.Maui.Tokens;

public enum Elevation
{
    Level0,
    Level1,
    Level2,
    Level3,
    Level4,
    Level5,
}

internal static class ElevationExtension
{
    internal static float GetOpacity(this Elevation elevation)
    {
        return elevation switch
        {
            //https://github.com/flutter/flutter/blob/master/packages/flutter/lib/src/material/elevation_overlay.dart#L165
            Elevation.Level0
                => 0f,
            Elevation.Level1 => 0.05f,
            Elevation.Level2 => 0.08f,
            Elevation.Level3 => 0.11f,
            Elevation.Level4 => 0.12f,
            Elevation.Level5 => 0.14f,
            _ => 0f,
        };
    }
}
