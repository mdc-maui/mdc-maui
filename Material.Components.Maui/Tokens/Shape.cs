using Material.Components.Maui.Converters;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Material.Components.Maui.Tokens;

[TypeConverter(typeof(ShapeConverter))]
public readonly struct Shape
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

    public Shape(double uniformRadius)
        : this(uniformRadius, uniformRadius, uniformRadius, uniformRadius) { }

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

    public static bool operator ==(Shape left, Shape right)
    {
        if (
            left.TopLeft == right.TopLeft
            && left.TopRight == right.TopRight
            && left.BottomLeft == right.BottomLeft
            && left.BottomRight == right.BottomRight
        )
        {
            return true;
        }
        return false;
    }

    public static bool operator !=(Shape left, Shape right)
    {
        if (
            left.TopLeft != right.TopLeft
            || left.TopRight != right.TopRight
            || left.BottomLeft != right.BottomLeft
            || left.BottomRight != right.BottomRight
        )
        {
            return true;
        }
        return false;
    }

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public SKPoint[] GetRadii()
    {
        return new SKPoint[]
        {
            new SKPoint((float)this.TopLeft, (float)this.TopLeft),
            new SKPoint((float)this.TopRight, (float)this.TopRight),
            new SKPoint((float)this.BottomRight, (float)this.BottomRight),
            new SKPoint((float)this.BottomLeft, (float)this.BottomLeft),
        };
    }

    public override string ToString()
    {
        return $"{this.TopLeft},{this.TopRight},{this.BottomLeft},{this.BottomRight}";
    }
}
