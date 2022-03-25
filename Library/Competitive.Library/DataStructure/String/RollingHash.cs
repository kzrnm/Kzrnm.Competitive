using System;

using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class RollingHash
    {
        static readonly Random rnd = new Random();
        public struct Hash : IEquatable<Hash>
        {
            public ulong a;
            public ulong b;

            public override bool Equals(object obj) => obj is Hash h && Equals(h);
            public bool Equals(Hash other) => a == other.a && b == other.b;
            public static bool operator ==(Hash hash1, Hash hash2) => hash1.Equals(hash2);
            public static bool operator !=(Hash hash1, Hash hash2) => !hash1.Equals(hash2);
            public override int GetHashCode() => HashCode.Combine(a, b);
        }


        public RollingHashUInt64 hash1;
        public RollingHashFast hash2;
        public int Length { get; }
        public RollingHash(ReadOnlySpan<char> s)
        {
            Length = s.Length;
            hash1 = new RollingHashUInt64(s);
            hash2 = new RollingHashFast(s);
        }


        /** <summary>[<paramref name="from"/>, <paramref name="len"/>) のハッシュ</summary> */
        [凾(256)]
        public Hash Slice(int from, int len) => new Hash { a = hash1.Slice(from, len), b = hash2.Slice(from, len) };


        public class RollingHashUInt64 /* https://webbibouroku.com/Blog/Article/cs-rollinghash */
        {
            static readonly uint B = (uint)rnd.Next(129, int.MaxValue);
            public readonly ulong[] pow;
            public readonly ulong[] hash;
            public int Length { get; }
            public RollingHashUInt64(ReadOnlySpan<char> s)
            {
                Length = s.Length;
                pow = new ulong[s.Length + 1];
                pow[0] = 1;
                for (int i = 0; i < s.Length; i++)
                    pow[i + 1] = pow[i] * B;
                hash = new ulong[s.Length + 1];
                for (int i = 0; i < s.Length; i++)
                    hash[i + 1] = hash[i] * B + s[i];
            }

            /** <summary>[<paramref name="from"/>, <paramref name="len"/>) のハッシュ</summary> */
            [凾(256)]
            public ulong Slice(int from, int len) => hash[from + len] - (hash[from] * pow[len]);
        }

        public class RollingHashFast /* https://qiita.com/keymoon/items/11fac5627672a6d6a9f6#%E9%AB%98%E9%80%9F%E3%81%AA%E3%83%AD%E3%83%AA%E3%83%8F%E3%82%92%E6%B1%82%E3%82%81%E3%81%A6%E3%83%A1%E3%83%AB%E3%82%BB%E3%83%B3%E3%83%8C%E7%B4%A0%E6%95%B0mod */
        {
            const int MAX_LENGTH = 500000;
            const ulong MASK30 = (1UL << 30) - 1;
            const ulong MASK31 = (1UL << 31) - 1;
            const ulong MOD = (1UL << 61) - 1;
            const ulong POSITIVIZER = MOD * ((1UL << 3) - 1);
            static readonly uint Base = (uint)rnd.Next(129, int.MaxValue);
            static readonly ulong[] powMemo = new ulong[MAX_LENGTH + 1];
            static RollingHashFast()
            {
                powMemo[0] = 1;
                for (int i = 1; i < powMemo.Length; i++)
                    powMemo[i] = CalcMod(Mul(powMemo[i - 1], Base));
            }

            readonly ulong[] hash;
            public int Length { get; }
            public RollingHashFast(ReadOnlySpan<char> s)
            {
                Length = s.Length;
                hash = new ulong[s.Length + 1];
                for (int i = 0; i < s.Length; i++)
                    hash[i + 1] = CalcMod(Mul(hash[i], Base) + s[i]);
            }

            [凾(256)]
            public ulong Slice(int from, int len) => CalcMod(hash[from + len] + POSITIVIZER - Mul(hash[from], powMemo[len]));

            [凾(256)]
            static ulong Mul(ulong l, ulong r)
            {
                var lu = l >> 31;
                var ld = l & MASK31;
                var ru = r >> 31;
                var rd = r & MASK31;
                var middleBit = ld * ru + lu * rd;
                return ((lu * ru) << 1) + ld * rd + ((middleBit & MASK30) << 31) + (middleBit >> 30);
            }

            [凾(256)]
            static ulong Mul(ulong l, uint r)
            {
                var lu = l >> 31;
                var rd = r & MASK31;
                var middleBit = lu * rd;
                return (l & MASK31) * rd + ((middleBit & MASK30) << 31) + (middleBit >> 30);
            }

            [凾(256)]
            static ulong CalcMod(ulong val)
            {
                val = (val & MOD) + (val >> 61);
                if (val >= MOD) val -= MOD;
                return val;
            }
        }
    }
}