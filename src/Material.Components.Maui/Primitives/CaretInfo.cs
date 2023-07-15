namespace Material.Components.Maui.Primitives;
internal struct CaretInfo
{
    public int Position;
    public float X;
    public float Y;
    public float Width;
    public float Height;
    public readonly bool IsZero => this.Position == 0 && this.X == 0 && this.Y == 0 && this.Width == 0 && this.Height == 0;
    public static CaretInfo Zero => new(0, 0, 0, 0, 0);
    public CaretInfo(int location, float x, float y, float width, float height)
    {
        this.Position = location;
        this.X = x;
        this.Y = y;
        this.Width = width;
        this.Height = height;
    }

}
