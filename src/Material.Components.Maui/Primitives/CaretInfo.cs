namespace Material.Components.Maui.Primitives;
internal struct CaretInfo(int location, float x, float y, float width, float height)
{
    public int Position = location;
    public float X = x;
    public float Y = y;
    public float Width = width;
    public float Height = height;
    public readonly bool IsZero => this.Position == 0 && this.X == 0 && this.Y == 0 && this.Width == 0 && this.Height == 0;
    public static CaretInfo Zero => new(0, 0, 0, 0, 0);
}
