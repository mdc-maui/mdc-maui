using Material.Components.Maui.Converters;
using System.ComponentModel;

namespace Material.Components.Maui.Tokens;

[TypeConverter(typeof(ShapeConverter))]
public struct Shape
{
    public static readonly Shape None = 0;
    public static readonly Shape ExtraSmall = 4;
    public static readonly Shape ExtraSmallTop = new(4, 4, 0, 0);
    public static readonly Shape Small = 8;
    public static readonly Shape Medium = 12;
    public static readonly Shape Large = 16;
    public static readonly Shape LargeTop = new(16, 16, 0, 0);
    public static readonly Shape LargeEnd = new(0, 0, 16, 16);
    public static readonly Shape ExtraLarge = 28;
    public static readonly Shape ExtraLargeTop = new(28, 28, 0, 0);
    public static readonly Shape Full = -1;

    public readonly double TopLeft { get; }
    public readonly double TopRight { get; }
    public readonly double BottomLeft { get; }
    public readonly double BottomRight { get; }

    public Shape(double uniformRadius) : this(uniformRadius, uniformRadius, uniformRadius, uniformRadius)
    {

    }

    public Shape(double topLeft, double topRight, double bottomLeft, double bottomRight)
    {
        this.TopLeft = topLeft;
        this.TopRight = topRight;
        this.BottomLeft = bottomLeft;
        this.BottomRight = bottomRight;
    }

    public static implicit operator Shape(double value)
    {
        return new Shape(value);
    }

    public override string ToString()
    {
        return $"{this.TopLeft},{this.TopRight},{this.BottomLeft},{this.BottomRight}";
    }
}