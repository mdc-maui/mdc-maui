using Material.Components.Maui.Converters;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Material.Components.Maui.Tokens;

[TypeConverter(typeof(StateLayerOpacityConverter))]
public struct StateLayerOpacity
{
    public static readonly StateLayerOpacity Normal = 1f;
    public static readonly StateLayerOpacity Hovered = 0.08f;
    public static readonly StateLayerOpacity Focused = 0.12f;
    public static readonly StateLayerOpacity Pressed = 0.12f;

    public float Value { get; init; }

    public StateLayerOpacity(float value)
    {
        this.Value = value;
    }

    public static implicit operator StateLayerOpacity(float value)
    {
        return new StateLayerOpacity(value);
    }

    public float ToFloat()
    {
        return this.Value;
    }

    public static bool operator ==(StateLayerOpacity left, StateLayerOpacity right)
    {
        if (left.Value == right.Value)
        {
            return true;
        }
        return false;
    }

    public static bool operator !=(StateLayerOpacity left, StateLayerOpacity right)
    {
        if (left.Value != right.Value)
        {
            return true;
        }
        return false;
    }

    public static bool operator <=(StateLayerOpacity left, StateLayerOpacity right)
    {
        if (left.Value <= right.Value)
        {
            return true;
        }
        return false;
    }

    public static bool operator >=(StateLayerOpacity left, StateLayerOpacity right)
    {
        if (left.Value >= right.Value)
        {
            return true;
        }
        return false;
    }

    public static StateLayerOpacity operator -(StateLayerOpacity left, StateLayerOpacity right)
    {
        return left.Value - right.Value;
    }

    public static StateLayerOpacity operator +(StateLayerOpacity left, StateLayerOpacity right)
    {
        return left.Value + right.Value;
    }

    public static implicit operator StateLayerOpacity(int value)
    {
        return new StateLayerOpacity(value);
    }

    public override string ToString()
    {
        return this.Value.ToString();
    }

    public override bool Equals(object obj)
    {
        if (obj is float f)
        {
            return this.Value == f;
        }
        else if (obj is StateLayerOpacity opacity)
        {
            return this.Value == opacity.Value;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return RuntimeHelpers.GetHashCode(this);
    }
}
