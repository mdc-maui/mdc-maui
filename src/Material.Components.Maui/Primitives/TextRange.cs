namespace Material.Components.Maui.Primitives;

public struct TextRange
{
    public int Start { get; set; }
    public int End { get; set; }

    public readonly int Length => this.End - this.Start;

    public readonly bool IsRange => this.Start != this.End;

    public TextRange() : this(0, 0) { }

    public TextRange(int positon) : this(positon, positon) { }

    public TextRange(int start, int end)
    {
        this.Start = start;
        this.End = end;
    }

    public readonly TextRange Normalized()
    {
        return this.Start > this.End
            ? new TextRange(this.End, this.Start)
            : new TextRange(this.Start, this.End);
    }

    public static TextRange CopyOf(TextRange range)
    {
        return new TextRange(range.Start, range.End);
    }

    public readonly bool Equals(TextRange value)
    {
        return this.Start == value.Start && this.End == value.End;
    }

    public readonly override string ToString()
    {
        return $"TextRange: Start = {{{this.Start}, End = {this.End}, Length = {this.Length}}}";
    }
}
