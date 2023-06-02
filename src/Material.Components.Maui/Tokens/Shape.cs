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

    public readonly float TopLeft { get; }
    public readonly float TopRight { get; }
    public readonly float BottomLeft { get; }
    public readonly float BottomRight { get; }

    public Shape(float uniformRadius)
        : this(uniformRadius, uniformRadius, uniformRadius, uniformRadius) { }

    public Shape(float topLeft, float topRight, float bottomLeft, float bottomRight)
    {
        this.TopLeft = topLeft;
        this.TopRight = topRight;
        this.BottomLeft = bottomLeft;
        this.BottomRight = bottomRight;
    }

    public double[] GetRadii(float width, float height)
    {
        if (
            this.TopLeft is -1
            && this.TopRight is -1
            && this.BottomLeft is -1
            && this.BottomRight is -1
        )
        {
            var full = Math.Min(width, height) / 2;
            return new double[] { full, full, full, full };
        }

        return new double[] { this.TopLeft, this.TopRight, this.BottomLeft, this.BottomRight, };
    }

    public static implicit operator Shape(float value)
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

    public override string ToString()
    {
        return $"{this.TopLeft},{this.TopRight},{this.BottomLeft},{this.BottomRight}";
    }
}
