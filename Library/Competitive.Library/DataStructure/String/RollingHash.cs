using Kzrnm.Competitive.Internal;
using Kzrnm.Competitive.IO;
using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using Rhf1 = RollingHash.Rhf<Rh1>;
    using Rhf2 = RollingHash.Rhf<Rh2>;
    public class RollingHash : RollingHashBase<Rhf1, Rhf2>
    {
        [凾(256)]
        public static RollingHash Create(string s)
            => new(s.Length, Rhf1.Create<char>(s), Rhf2.Create<char>(s));
        [凾(256)]
        public static RollingHash Create(Asciis s)
            => new(s.Length, Rhf1.Create(s.AsSpan()), Rhf2.Create(s.AsSpan()));
        [凾(256)]
        public static RollingHash Create<T>(T[] s) where T : IBinaryInteger<T>
            => new(s.Length, Rhf1.Create<T>(s), Rhf2.Create<T>(s));
        [凾(256)]
        public static RollingHash Create<T>(Span<T> s) where T : IBinaryInteger<T>
            => new(s.Length, Rhf1.Create<T>(s), Rhf2.Create<T>(s));
        [凾(256)]
        public static RollingHash Create<T>(ReadOnlySpan<T> s) where T : IBinaryInteger<T>
            => new(s.Length, Rhf1.Create(s), Rhf2.Create(s));

        public RollingHash(int length, Rhf1 h1, Rhf2 h2) : base(length, h1, h2) { }

        ///<summary>
        ///[<paramref name="from"/>, <paramref name="len"/>) のハッシュ
        ///</summary>
        [凾(256)]
        public RollingHashValue Slice(int from, int len) => new RollingHashValue(hash1.Slice(from, len), hash2.Slice(from, len));

        public class Rhf<H> : RhFast<H> where H : struct, IRollingHash
        {
            readonly ulong[] h;
            private Rhf(ulong[] hs) : base(hs.Length)
            {
                ResizePow(hs.Length);
                h = hs;
            }

            [凾(256)]
            public static Rhf<H> Create<T>(ReadOnlySpan<T> s) where T : IBinaryInteger<T>
            {
                var hash = new ulong[s.Length + 1];
                for (int i = 0; i < s.Length; i++)
                    hash[i + 1] = Add(Mul(hash[i], B), Xorshift(s[i]));
                return new(hash);
            }
            ///<summary>
            ///[<paramref name="from"/>, <paramref name="len"/>) のハッシュ
            ///</summary>
            [凾(256)]
            public ulong Slice(int from, int len)
            {
                var ret = Mod + h[from + len] - Mul(h[from], pow[len]);
                return ret < Mod ? ret : ret - Mod;
            }
        }
    }
}