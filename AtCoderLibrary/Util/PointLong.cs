using System;

namespace AtCoder
{
    public readonly struct PointLong : IEquatable<PointLong>, IComparable<PointLong>
    {
        public readonly long x;
        public readonly long y;
        public PointLong(long x, long y)
        {
            this.x = x;
            this.y = y;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public void Deconstruct(out long v1, out long v2) { v1 = x; v2 = y; }
        public static implicit operator PointLong((long x, long y) tuple) => new PointLong(tuple.x, tuple.y);
        public double Distance(PointLong other) => Math.Sqrt(Distance2(other));
        public long Distance2(PointLong other)
        {
            var p = other - this;
            return p.x * p.x + p.y * p.y;
        }
        /// <summary>
        /// 内積
        /// </summary>
        public long Inner(PointLong other) => x * other.x + y * other.y;
        /// <summary>
        /// 外積
        /// </summary>
        public long Cross(PointLong other) => x * other.y - y * other.x;
        public static PointLong operator +(PointLong a, PointLong b) => new PointLong(a.x + b.x, a.y + b.y);
        public static PointLong operator -(PointLong a, PointLong b) => new PointLong(a.x - b.x, a.y - b.y);
        public int CompareTo(PointLong other)
        {
            var xd = this.x.CompareTo(other.x);
            if (xd != 0) return xd;
            return this.y.CompareTo(other.y);
        }

        public bool Equals(PointLong other) => this.x == other.x && this.y == other.y;
        public override bool Equals(object obj) => obj is PointLong p && this.Equals(p);
        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => $"{x} {y}";

        public static bool operator ==(PointLong left, PointLong right) => left.Equals(right);
        public static bool operator !=(PointLong left, PointLong right) => !left.Equals(right);
    }

}
