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
    }
}