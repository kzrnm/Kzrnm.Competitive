using AtCoder;
using Kzrnm.Competitive.Internal;
using System;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using Rhf1 = RhFast<Rh1>;
    using Rhf2 = RhFast<Rh2>;
    /// <summary>
    /// 値を書き換えできる Rolling Hash
    /// </summary>
    public class RollingHashWritable : RollingHashBase<Rhf1, Rhf2>
    {
        [凾(256)]
        public static RollingHashWritable Create(string s)
            => Create(SegVals<char>(s));
        [凾(256)]
        public static RollingHashWritable Create<T>(T[] s) where T : IBinaryInteger<T>
            => Create(SegVals<T>(s));
        [凾(256)]
        public static RollingHashWritable Create<T>(Span<T> s) where T : IBinaryInteger<T>
            => Create(SegVals<T>(s));
        [凾(256)]
        public static RollingHashWritable Create<T>(ReadOnlySpan<T> s) where T : IBinaryInteger<T>
            => Create(SegVals(s));

        [凾(256)]
        internal static RollingHashWritable Create(RhSegVal[] s)
            => new(s);
        [凾(256)]
        internal static RhSegVal[] SegVals<T>(ReadOnlySpan<T> s) where T : IBinaryInteger<T>
        {
            var sv = new RhSegVal[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                var v = Xorshift(s[i]);
                sv[i] = new(new(v, v), 1);
            }
            return sv;
        }


        internal readonly Segtree<RhSegVal, RhOp> s;
        internal RollingHashWritable(RhSegVal[] vs) : base(vs.Length)
        {
            ResizePow(vs.Length + 1);
            s = new(vs);
        }

        ///<summary>
        ///[<paramref name="from"/>, <paramref name="len"/>) のハッシュ
        ///</summary>
        [凾(256)]
        public RollingHashValue Slice(int from, int len) => s.Slice(from, len).Hash;

        [凾(256)]
        public void Set<T>(int index, T x) where T : IBinaryInteger<T>
        {
            var v = Xorshift(x);
            s[index] = new(new(v, v), 1);
        }
        internal readonly record struct RhSegVal(RollingHashValue Hash, int Size);
        internal readonly struct RhOp : ISegtreeOperator<RhSegVal>
        {
            public RhSegVal Identity => default;

            [凾(256)]
            public RhSegVal Operate(RhSegVal x, RhSegVal y) => new(
                new(Rhf1.Add(Rhf1.Mul(x.Hash.a, Rhf1.Pow(y.Size)), y.Hash.a), Rhf2.Add(Rhf2.Mul(x.Hash.b, Rhf2.Pow(y.Size)), y.Hash.b)),
                x.Size + y.Size);
        }
    }
}