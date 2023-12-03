using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class RollingHashEditable : RollingHashBase<RollingHashEditable.RhUInt64, RollingHashEditable.RhFast>
    {
        [凾(256)]
        public static RollingHashEditable Create(string s)
            => Create(SegVals<char>(s));
        [凾(256)]
        public static RollingHashEditable Create<T>(T[] s) where T : IBinaryInteger<T>
            => Create(SegVals<T>(s));
        [凾(256)]
        public static RollingHashEditable Create<T>(Span<T> s) where T : IBinaryInteger<T>
            => Create(SegVals<T>(s));
        [凾(256)]
        public static RollingHashEditable Create<T>(ReadOnlySpan<T> s) where T : IBinaryInteger<T>
            => Create(SegVals(s));

        [凾(256)]
        internal static RollingHashEditable Create(RhSegVal[] s)
            => new(s.Length, new(s), new(s));

        [凾(256)]
        internal static RhSegVal[] SegVals<T>(ReadOnlySpan<T> s) where T : IBinaryInteger<T>
        {
            var sv = new RhSegVal[s.Length];
            for (int i = 0; i < s.Length; i++)
                sv[i] = new(Xorshift(s[i]), 1);
            return sv;
        }

        public RollingHashEditable(int length, RhUInt64 h1, RhFast h2) : base(length, h1, h2) { }

        ///<summary>
        ///[<paramref name="from"/>, <paramref name="len"/>) のハッシュ
        ///</summary>
        [凾(256)]
        public RollingHashValue Slice(int from, int len) => new RollingHashValue(hash1.Slice(from, len), hash2.Slice(from, len));

        [凾(256)]
        public void Set<T>(int index, T x) where T : IBinaryInteger<T>
        {
            var y = Xorshift(x);
            hash1.Set(index, (ulong)y);
            hash2.Set(index, (ulong)y);
        }
        internal readonly record struct RhSegVal(ulong Hash, int Size);
        internal readonly struct RhOp<T> : ISegtreeOperator<RhSegVal> where T : IRollingHashLogic
        {
            public RhSegVal Identity => default;

            [凾(256)]
            public RhSegVal Operate(RhSegVal x, RhSegVal y) => new(T.Add(T.Mul(x.Hash, T.Pow(y.Size)), y.Hash), x.Size + y.Size);
        }
        public class RhUInt64 : Internal.RhUInt64
        {
            internal readonly Segtree<RhSegVal, RhOp<RhUInt64>> s;
            internal RhUInt64(RhSegVal[] hs) : base(hs.Length + 1)
            {
                s = new(hs);
            }

            ///<summary>
            ///[<paramref name="from"/>, <paramref name="len"/>) のハッシュ
            ///</summary>
            [凾(256)]
            public ulong Slice(int from, int len) => s.Slice(from, len).Hash;
            [凾(256)] public void Set(int index, ulong x) => s[index] = new(x, 1);
            [凾(256)]
            public void Set<T>(int index, T x) where T : IBinaryInteger<T> => Set(index, (ulong)Xorshift(x));
        }

        public class RhFast : Internal.RhFast
        {
            internal readonly Segtree<RhSegVal, RhOp<RhFast>> s;
            internal RhFast(RhSegVal[] hs) : base(hs.Length + 1)
            {
                s = new(hs);
            }

            ///<summary>
            ///[<paramref name="from"/>, <paramref name="len"/>) のハッシュ
            ///</summary>
            [凾(256)]
            public ulong Slice(int from, int len) => s.Slice(from, len).Hash;
            [凾(256)] public void Set(int index, ulong x) => s[index] = new(x, 1);
            [凾(256)]
            public void Set<T>(int index, T x) where T : IBinaryInteger<T> => Set(index, (ulong)Xorshift(x));
        }
    }
}