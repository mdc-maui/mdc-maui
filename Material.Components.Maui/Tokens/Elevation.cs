using Material.Components.Maui.Converters;
using System.ComponentModel;

namespace Material.Components.Maui.Tokens;

[TypeConverter(typeof(ElevationConverter))]
public readonly struct Elevation
{
    public static readonly Elevation Level0 = 0;
    public static readonly Elevation Level1 = 1;
    public static readonly Elevation Level2 = 2;
    public static readonly Elevation Level3 = 3;
    public static readonly Elevation Level4 = 4;
    public static readonly Elevation Level5 = 5;

    public int Value { get; init; }

    public Elevation(int value)
    {
        this.Value = value;
    }

    public static implicit operator Elevation(int value)
    {
        return new Elevation(value);
    }

    public int ToInt()
    {
        return this.Value;
    }

    public override string ToString()
    {
        return this.Value.ToString();
    }

    public override bool Equals(object obj)
    {
        if (obj is int i32)
        {
            return this.Value == i32;
        }
        else if (obj is Elevation elevation)
        {
            return this.Value == elevation.Value;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return RuntimeHelpers.GetHashCode(this);
    }
}
