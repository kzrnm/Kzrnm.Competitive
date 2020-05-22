using AtCoderProject.Reader;
using System;




readonly struct PointD : IEquatable<PointD>, IComparable<PointD>
{
    public readonly double x;
    public readonly double y;
    public PointD(ConsoleReader cr) : this(cr, cr) { }
    public PointD(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Deconstruct(out double v1, out double v2) { v1 = x; v2 = y; }
    public static implicit operator PointD((double x, double y) tuple) => new PointD(tuple.x, tuple.y);
    public double Distance(PointD other) => Math.Sqrt(Distance2(other));
    private double Distance2(PointD other)
    {
        var p = other - this;
        return p.x * p.x + p.y * p.y;
    }
    public double Inner(PointD other) => x * other.x + y * other.y;
    public double Cross(PointD other) => x * other.y - y * other.x;
    public static PointD operator +(PointD a, PointD b) => new PointD(a.x + b.x, a.y + b.y);
    public static PointD operator -(PointD a, PointD b) => new PointD(a.x - b.x, a.y - b.y);
    public int CompareTo(PointD other)
    {
        var xd = this.x.CompareTo(other.x);
        if (xd != 0) return xd;
        return this.y.CompareTo(other.y);
    }

    public bool Equals(PointD other) => this.x == other.x && this.y == other.y;
    public override bool Equals(object obj) => obj is PointD && Equals((PointD)obj);
    public override int GetHashCode() => HashCode.Combine(x, y);
    public override string ToString() => $"({x}, {y})";

    public static PointD 外心(PointD a, PointD b, PointD c)
    {
        var ab = a.Distance(b);
        var cb = c.Distance(b);
        var ac = a.Distance(c);

        var cosA = (ab * ab + ac * ac - cb * cb) / 2 / ab / ac;
        var cosB = (ab * ab + cb * cb - ac * ac) / 2 / ab / cb;
        var cosC = (cb * cb + ac * ac - ab * ab) / 2 / cb / ac;

        var d = cb * cosA + ac * cosB + ab * cosC;
        return new PointD(cb * cosA * a.x / d, cb * cosA * a.y / d)
            + new PointD(ac * cosB * b.x / d, ac * cosB * b.y / d)
            + new PointD(ab * cosC * c.x / d, ab * cosC * c.y / d);
    }
}


