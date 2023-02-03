using System;
using System.Numerics;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://prng.di.unimi.it/xoshiro256starstar.c
    // competitive-verifier: TITLE Xoshiro256**
    public class Xoshiro256
    {
        ulong s0, s1, s2, s3;
        public Xoshiro256() : this(new Random()) { }
        public Xoshiro256(int seed) : this(new Random(seed)) { }
        public Xoshiro256(ulong t0, ulong t1, ulong t2, ulong t3)
        {
            s0 = t0; s1 = t1; s2 = t2; s3 = t3;
        }
        private Xoshiro256(Random rnd)
        {
            Span<ulong> vs = stackalloc ulong[4];
            rnd.NextBytes(MemoryMarshal.AsBytes(vs));
            s0 = vs[0];
            s1 = vs[1];
            s2 = vs[2];
            s3 = vs[3];
            if ((s0 | s1 | s2 | s3) == 0)
                s0 = 1;
        }

        [凾(256)]
        public ulong NextUInt64()
        {
            var result = BitOperations.RotateLeft(s1 + s1 + s1 + s1 + s1, 7) * 9;

            var t = s1 << 17;

            s2 ^= s0;
            s3 ^= s1;
            s1 ^= s2;
            s0 ^= s3;

            s2 ^= t;

            s3 = BitOperations.RotateLeft(s3, 45);

            return result;
        }

        /// <summary>
        /// [0, <paramref name="max"/>) の乱数を返します。
        /// </summary>
        [凾(256)]
        public ulong NextUInt64(ulong max)
        {
            if (max <= 1) return 0;
            var lzc = 63 ^ BitOperations.Log2(max - 1);
            ulong r;
            while ((r = NextUInt64() >> lzc) >= max) { }
            return r;
        }

        /// <summary>
        /// [<paramref name="min"/>, <paramref name="max"/>) の乱数を返します。
        /// </summary>
        [凾(256)]
        public ulong NextUInt64(ulong min, ulong max)
            => NextUInt64(max - min) + min;        
    }
}