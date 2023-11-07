using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Material.Components.Maui.Tokens;

[TypeConverter(typeof(ShapeConverter))]
public readonly struct Shape(float topLeft, float topRight, float bottomLeft, float bottomRight)
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

    public readonly float TopLeft { get; } = topLeft;
    public readonly float TopRight { get; } = topRight;
    public readonly float BottomLeft { get; } = bottomLeft;
    public readonly float BottomRight { get; } = bottomRight;

    public Shape(float uniformRadius)
        : this(uniformRadius, uniformRadius, uniformRadius, uniformRadius) { }

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
            return [full, full, full, full];
        }

        return [this.TopLeft, this.TopRight, this.BottomLeft, this.BottomRight,];
    }

    public static implicit operator Shape(float value)
    {
        return new Shape(value);
    }

    public static bool operator ==(Shape left, Shape right)
    {
        return left.TopLeft == right.TopLeft
            && left.TopRight == right.TopRight
            && left.BottomLeft == right.BottomLeft
            && left.BottomRight == right.BottomRight;
    }

    public static bool operator !=(Shape left, Shape right)
    {
        return left.TopLeft != right.TopLeft
            || left.TopRight != right.TopRight
            || left.BottomLeft != right.BottomLeft
            || left.BottomRight != right.BottomRight;
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
