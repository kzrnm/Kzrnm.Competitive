using System;


struct Point : IEquatable<Point>, IComparable<Point>
{
    public int x;
    public int y;
    public Point(int[] arr) : this(arr[0], arr[1]) { }
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Deconstruct(out int v1, out int v2) { v1 = x; v2 = y; }
    public static implicit operator Point(Tuple<int, int> tuple) => new Point(tuple.Item1, tuple.Item2);
    public double Distance(Point other) => Math.Sqrt(Distance2(other));
    private long Distance2(Point other)
    {
        var p = other - this;
        return (long)p.x * p.x + (long)p.y * p.y;
    }
    public long Inner(Point other) => (long)x * other.x + (long)y * other.y;
    public long Cross(Point other) => (long)x * other.y - (long)y * other.x;
    public static Point operator +(Point a, Point b) => new Point(a.x + b.x, a.y + b.y);
    public static Point operator -(Point a, Point b) => new Point(a.x - b.x, a.y - b.y);
    public int CompareTo(Point other)
    {
        var xd = this.x.CompareTo(other.x);
        if (xd != 0) return xd;
        return this.y.CompareTo(other.y);
    }

    public bool Equals(Point other) => this.x == other.x && this.y == other.y;
    public override bool Equals(object obj) => obj is Point && Equals((Point)obj);
    public override int GetHashCode() => ((x << 5) + x) ^ y;
    public override string ToString() => $"({x}, {y})";
}
