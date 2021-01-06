using System;

namespace AtCoder
{

    public readonly struct PointInt : IEquatable<PointInt>, IComparable<PointInt>
    {
        public readonly int x;
        public readonly int y;
        public PointInt(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public void Deconstruct(out int v1, out int v2) { v1 = x; v2 = y; }
        public static implicit operator PointInt((int x, int y) tuple) => new PointInt(tuple.x, tuple.y);
        public double Distance(PointInt other) => Math.Sqrt(Distance2(other));
        public long Distance2(PointInt other)
        {
            var p = other - this;
            return (long)p.x * p.x + (long)p.y * p.y;
        }
        /// <summary>
        /// 内積
        /// </summary>
        public long Inner(PointInt other) => (long)x * other.x + (long)y * other.y;
        /// <summary>
        /// 外積
        /// </summary>
        public long Cross(PointInt other) => (long)x * other.y - (long)y * other.x;
        public static PointInt operator +(PointInt a, PointInt b) => new PointInt(a.x + b.x, a.y + b.y);
        public static PointInt operator -(PointInt a, PointInt b) => new PointInt(a.x - b.x, a.y - b.y);
        public int CompareTo(PointInt other)
        {
            var xd = this.x.CompareTo(other.x);
            if (xd != 0) return xd;
            return this.y.CompareTo(other.y);
        }

        public bool Equals(PointInt other) => this.x == other.x && this.y == other.y;
        public override bool Equals(object obj) => obj is PointInt p && this.Equals(p);
        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => $"({x}, {y})";

        public static bool operator ==(PointInt left, PointInt right) => left.Equals(right);
        public static bool operator !=(PointInt left, PointInt right) => !left.Equals(right);
    }

}
