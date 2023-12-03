using AtCoder;
using System;
using System.Diagnostics;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly record struct RollingHashValue(ulong a, ulong b);
    namespace Internal
    {
        /*
         * h[1] = s[0]
         * h[2] = s[1] + s[0] * B
         * h[3] = s[2] + s[1] * B + s[0] * B^2
         * h[4] = s[3] + s[2] * B + s[1] * B^2 + s[0] * B^3
         * ...
         */
        public class RollingHashBase<Hash1, Hash2>
            where Hash1 : RhFast<Rh1>
            where Hash2 : RhFast<Rh2>
        {
            public Hash1 hash1;
            public Hash2 hash2;
            public int Length { get; }

            public RollingHashBase(int length)
            {
                Length = length;
            }
            public RollingHashBase(int length, Hash1 h1, Hash2 h2)
            {
                Length = length;
                hash1 = h1;
                hash2 = h2;
            }

            [凾(256)]
            public static void ResizePow(int size)
            {
                RhFast<Rh1>.ResizePow(size);
                RhFast<Rh2>.ResizePow(size);
            }
            [凾(256)]
            protected static uint Xorshift<T>(T x) where T : IBinaryInteger<T>
            {
                var v = uint.CreateTruncating(x) + 1;
                v ^= v << 13;
                v ^= v >> 17;
                v ^= v << 5;
                return v;
            }
        }

        public readonly struct Rh1 : IRollingHash
        {
            [凾(256)] public uint GetBase() => (uint)Random.Shared.Next(65536, int.MaxValue);
        }
        public readonly struct Rh2 : IRollingHash
        {
            [凾(256)] public uint GetBase() => uint.MaxValue - (uint)Random.Shared.Next();
        }

        [IsOperator]
        public interface IRollingHash
        {
            uint GetBase();
        }

        // https://qiita.com/keymoon/items/11fac5627672a6d6a9f6#%E9%AB%98%E9%80%9F%E3%81%AA%E3%83%AD%E3%83%AA%E3%83%8F%E3%82%92%E6%B1%82%E3%82%81%E3%81%A6%E3%83%A1%E3%83%AB%E3%82%BB%E3%83%B3%E3%83%8C%E7%B4%A0%E6%95%B0mod 
        public class RhFast<T> where T : struct, IRollingHash
        {
            /// <summary>
            /// Base
            /// </summary>
            protected static readonly uint B = new T().GetBase();
            protected const ulong Mod = (1UL << 61) - 1;
            protected static ulong[] pow = new ulong[1] { 1 };
            [凾(256)]
            public static void ResizePow(int size)
            {
                if (size <= pow.Length) return;
                var prevSize = pow.Length;
                Array.Resize(ref pow, size);
                for (int i = prevSize; i < pow.Length; i++)
                    pow[i] = Mul(pow[i - 1], B);
            }
            protected RhFast(int length)
            {
                ResizePow(length);
            }

            [凾(256)] public static ulong Pow(int b) => pow[b];
            [凾(256)]
            public static ulong Add(ulong l, ulong r)
            {
                l += r;
                if (l >= Mod) l -= Mod;
                return l;
            }
            [凾(256)]
            public static ulong Mul(ulong l, ulong r)
            {
                var hi = Math.BigMul(l, r, out var lo);
                var t = (hi << 3) + (lo >> 61) + (lo & Mod);
                Debug.Assert(t == (ulong)(((UInt128)l * (UInt128)r) >> 61) + ((l * r) & Mod));
                if (t >= Mod) t -= Mod;
                return t;
            }
        }
    }
}