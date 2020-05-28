using AtCoderProject;
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

    // A*x+B*y=C
    public static (double A, double B, double C) 垂直二等分線(PointD a, PointD b)
        => (a.x - b.x, a.y - b.y, (a.x * a.x - b.x * b.x + a.y * a.y - b.y * b.y) * .5);


    // A*x+B*y+C=0, U*x+V*y+W=0の交点
    public static PointD 直線と直線の交点(double a, double b, double c, double u, double v, double w)
    {
        var dd = a * v - b * u;
        return new PointD((b * w - c * v) / dd, (c * u - a * w) / dd);
    }


    // A*x+B*y+C=0, Pを中心とする半径rの円の交点
    public static PointD[] 直線と円の交点(double a, double b, double c, PointD p, double r)
    {
        var l = a * a + b * b;
        var k = a * p.x + b * p.y + c;
        var d = l * r * r - k * k;

        if (d < 0)
            return Array.Empty<PointD>();

        var apl = a / l;
        var bpl = b / l;
        var xc = p.x - apl * k;
        var yc = p.y - bpl * k;
        if (d == 0)
            return new[] { new PointD(xc, yc), };

        var ds = Math.Sqrt(d);
        var xd = bpl * ds;
        var yd = apl * ds;
        return new[]
        {
            new PointD(xc - xd, yc + yd),
            new PointD(xc + xd, yc - yd),
        };
    }


    public static PointD[] 円の交点(PointD p1, double r1, PointD p2, double r2)
    {
        var xx = p1.x - p2.x;
        var yy = p1.y - p2.y;
        return 直線と円の交点(
            xx,
            yy,
            0.5 * ((r1 - r2) * (r1 + r2) - xx * (p1.x + p2.x) - yy * (p1.y + p2.y)), p1, r1);
    }

    public static bool 線分が交差しているか(PointD a1, PointD b1, PointD a2, PointD b2)
    {
        var ta = (a2.x - b2.x) * (a1.y - a2.y) + (a2.y - b2.y) * (a2.x - a1.x);
        var tb = (a2.x - b2.x) * (b1.y - a2.y) + (a2.y - b2.y) * (a2.x - b1.x);
        var tc = (a1.x - b1.x) * (a2.y - a1.y) + (a1.y - b1.y) * (a1.x - a2.x);
        var td = (a1.x - b1.x) * (b2.y - a1.y) + (a1.y - b1.y) * (a1.x - b2.x);

        return tc * td < 0 && ta * tb < 0;
    }
}


