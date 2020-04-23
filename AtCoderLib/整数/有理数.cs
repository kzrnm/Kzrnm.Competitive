using System;

// 有理数
// 有理数
struct Fraction : IEquatable<Fraction>, IComparable<Fraction>
{
    static long Gcd(long a, long b) => b > a ? Gcd(b, a) : (b == 0 ? a : Gcd(b, a % b));
    public readonly long numerator; // 分子
    public readonly long denominator; // 分母
    public Fraction(long 分子, long 分母)
    {
        var sign = Math.Sign(分子) * Math.Sign(分母);
        分子 = Math.Abs(分子); 分母 = Math.Abs(分母);
        var gcd = Gcd(分母, 分子);
        numerator = sign * 分子 / gcd;
        denominator = 分母 / gcd;
    }
    public override string ToString() => $"{numerator}/{denominator}";
    public override bool Equals(object obj) => obj is Fraction && Equals((Fraction)obj);
    public bool Equals(Fraction other) => this.numerator == other.numerator && this.denominator == other.denominator;
    public override int GetHashCode() => ((numerator.GetHashCode() << 5) + numerator.GetHashCode()) ^ denominator.GetHashCode();

    public static implicit operator Fraction(long x) => new Fraction(x, 1);
    public int CompareTo(Fraction other) => (this.numerator * other.denominator).CompareTo(other.numerator * this.denominator);

    public static Fraction operator +(Fraction x, Fraction y)
    {
        var gcd = Gcd(x.denominator, y.denominator);
        var lcm = x.denominator / gcd * y.denominator;
        return new Fraction((x.numerator * y.denominator + y.numerator * x.denominator) / gcd, lcm);
    }
    public static Fraction operator -(Fraction x, Fraction y)
    {
        var gcd = Gcd(x.denominator, y.denominator);
        var lcm = x.denominator / gcd * y.denominator;
        return new Fraction((x.numerator * y.denominator - y.numerator * x.denominator) / gcd, lcm);
    }
    public static Fraction operator *(Fraction x, Fraction y) => new Fraction(x.numerator * y.numerator, x.denominator * y.denominator);
    public static Fraction operator /(Fraction x, Fraction y) => new Fraction(x.numerator * y.denominator, x.denominator * y.numerator);
    public static bool operator ==(Fraction x, Fraction y) => x.Equals(y);
    public static bool operator !=(Fraction x, Fraction y) => !x.Equals(y);
    public static bool operator >=(Fraction x, Fraction y) => x.CompareTo(y) >= 0;
    public static bool operator <=(Fraction x, Fraction y) => x.CompareTo(y) <= 0;
    public static bool operator >(Fraction x, Fraction y) => x.CompareTo(y) > 0;
    public static bool operator <(Fraction x, Fraction y) => x.CompareTo(y) < 0;

    public Fraction Inverse() => new Fraction(denominator, numerator);
    public double ToDouble() => (double)numerator / denominator;
}
