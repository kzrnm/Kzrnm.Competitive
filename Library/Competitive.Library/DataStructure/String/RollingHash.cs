using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class RollingHash : Internal.RollingHashBase<RollingHash.RhUInt64, RollingHash.RhFast>
    {
        [凾(256)]
        public static RollingHash Create(string s)
            => new(s.Length, RhUInt64.Create<char>(s), RhFast.Create<char>(s));
        [凾(256)]
        public static RollingHash Create<T>(T[] s) where T : IBinaryInteger<T>
            => new(s.Length, RhUInt64.Create<T>(s), RhFast.Create<T>(s));
        [凾(256)]
        public static RollingHash Create<T>(Span<T> s) where T : IBinaryInteger<T>
            => new(s.Length, RhUInt64.Create<T>(s), RhFast.Create<T>(s));
        [凾(256)]
        public static RollingHash Create<T>(ReadOnlySpan<T> s) where T : IBinaryInteger<T>
            => new(s.Length, RhUInt64.Create(s), RhFast.Create(s));

        public RollingHash(int length, RhUInt64 h1, RhFast h2) : base(length, h1, h2) { }

        ///<summary>
        ///[<paramref name="from"/>, <paramref name="len"/>) のハッシュ
        ///</summary>
        [凾(256)]
        public RollingHashValue Slice(int from, int len) => new RollingHashValue(hash1.Slice(from, len), hash2.Slice(from, len));

        public class RhUInt64 : Internal.RhUInt64
        {
            public readonly ulong[] h;
            private RhUInt64(ulong[] hs) : base(hs.Length)
            {
                h = hs;
            }

            [凾(256)]
            public static RhUInt64 Create<T>(ReadOnlySpan<T> s) where T : IBinaryInteger<T>
            {
                var hash = new ulong[s.Length + 1];
                for (int i = 0; i < s.Length; i++)
                    hash[i + 1] = hash[i] * B + Xorshift(s[i]);
                return new(hash);
            }

            ///<summary>
            ///[<paramref name="from"/>, <paramref name="len"/>) のハッシュ
            ///</summary>
            [凾(256)]
            public ulong Slice(int from, int len) => h[from + len] - (h[from] * pow[len]);
        }

        public class RhFast : Internal.RhFast
        {
            readonly ulong[] h;
            private RhFast(ulong[] hs) : base(hs.Length)
            {
                ResizePow(hs.Length);
                h = hs;
            }

            [凾(256)]
            public static RhFast Create<T>(ReadOnlySpan<T> s) where T : IBinaryInteger<T>
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