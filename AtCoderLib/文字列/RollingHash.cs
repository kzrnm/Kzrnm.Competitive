using System;
using System.Security.Cryptography;

class RollingHash
{
    private static Random rnd = new Random();
    public struct Hash : IEquatable<Hash>
    {
        public ulong a;
        public ulong b;

        public override bool Equals(object obj) => obj is Hash && Equals((Hash)obj);
        public bool Equals(Hash other) => this.a == other.a && this.b == other.b;
        public static bool operator ==(Hash hash1, Hash hash2) => hash1.Equals(hash2);
        public static bool operator !=(Hash hash1, Hash hash2) => !hash1.Equals(hash2);
        public override int GetHashCode() => HashCode.Combine(a, b);
    }


    public RollingHashUInt64 hash1;
    public RollingHashFast hash2;
    public int length;
    public RollingHash(ReadOnlySpan<char> s)
    {
        this.length = s.Length;
        hash1 = new RollingHashUInt64(s);
        hash2 = new RollingHashFast(s);
    }


    /** <summary>[<paramref name="l"/>, <paramref name="r"/>) のハッシュ</summary> */
    public Hash Slice(int l, int r) => new Hash { a = hash1.Slice(l, r), b = hash2.Slice(l, r) };
    public Hash this[Range range] => Slice(range.Start.GetOffset(length), range.End.GetOffset(length));


    public class RollingHashUInt64 /* https://webbibouroku.com/Blog/Article/cs-rollinghash */
    {
        static uint B = (uint)rnd.Next(129, int.MaxValue);
        public ulong[] pow;
        public ulong[] hash;
        public int length;
        public RollingHashUInt64(ReadOnlySpan<char> s)
        {
            this.length = s.Length;
            pow = new ulong[s.Length + 1];
            pow[0] = 1;
            for (int i = 0; i < s.Length; i++)
                pow[i + 1] = pow[i] * B;
            hash = new ulong[s.Length + 1];
            for (int i = 0; i < s.Length; i++)
                hash[i + 1] = hash[i] * B + s[i];
        }

        /** <summary>[<paramref name="l"/>, <paramref name="r"/>) のハッシュ</summary> */
        public ulong Slice(int l, int r) => hash[r] - (hash[l] * pow[r - l]);
        public ulong this[Range range] => Slice(range.Start.GetOffset(length), range.End.GetOffset(length));
    }

    public class RollingHashFast /* https://qiita.com/keymoon/items/11fac5627672a6d6a9f6#%E9%AB%98%E9%80%9F%E3%81%AA%E3%83%AD%E3%83%AA%E3%83%8F%E3%82%92%E6%B1%82%E3%82%81%E3%81%A6%E3%83%A1%E3%83%AB%E3%82%BB%E3%83%B3%E3%83%8C%E7%B4%A0%E6%95%B0mod */
    {
        const int MAX_LENGTH = 500000;
        const ulong MASK30 = (1UL << 30) - 1;
        const ulong MASK31 = (1UL << 31) - 1;
        const ulong MOD = (1UL << 61) - 1;
        const ulong POSITIVIZER = MOD * ((1UL << 3) - 1);
        static uint Base = (uint)rnd.Next(129, int.MaxValue);
        static ulong[] powMemo = new ulong[MAX_LENGTH + 1];
        static RollingHashFast()
        {
            powMemo[0] = 1;
            for (int i = 1; i < powMemo.Length; i++)
                powMemo[i] = CalcMod(Mul(powMemo[i - 1], Base));
        }

        ulong[] hash;
        public int length;
        public RollingHashFast(ReadOnlySpan<char> s)
        {
            this.length = s.Length;
            hash = new ulong[s.Length + 1];
            for (int i = 0; i < s.Length; i++)
                hash[i + 1] = CalcMod(Mul(hash[i], Base) + s[i]);
        }

        public ulong Slice(int l, int r) => CalcMod(hash[r] + POSITIVIZER - Mul(hash[l], powMemo[r - l]));
        public ulong this[Range range] => Slice(range.Start.GetOffset(length), range.End.GetOffset(length));

        private static ulong Mul(ulong l, ulong r)
        {
            var lu = l >> 31;
            var ld = l & MASK31;
            var ru = r >> 31;
            var rd = r & MASK31;
            var middleBit = ld * ru + lu * rd;
            return ((lu * ru) << 1) + ld * rd + ((middleBit & MASK30) << 31) + (middleBit >> 30);
        }

        private static ulong Mul(ulong l, uint r)
        {
            var lu = l >> 31;
            var rd = r & MASK31;
            var middleBit = lu * rd;
            return (l & MASK31) * rd + ((middleBit & MASK30) << 31) + (middleBit >> 30);
        }

        private static ulong CalcMod(ulong val)
        {
            val = (val & MOD) + (val >> 61);
            if (val >= MOD) val -= MOD;
            return val;
        }
    }
}