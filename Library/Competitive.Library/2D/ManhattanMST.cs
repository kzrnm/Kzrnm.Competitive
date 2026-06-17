using AtCoder;
using AtCoder.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// マンハッタン距離 |x2-x1| + |y2-y1| での最小全域木を構築する
    /// </summary>
    public static class ManhattanMST
    {
#if !NET10_0_OR_GREATER
        /// <inheritdoc cref="Solve{T}(ReadOnlySpan{ValueTuple{T, T}})"/>
        public static (int Index1, int Index2)[] Solve<T>((T, T)[] ps)
            where T : INumber<T>, IMinMaxValue<T>
            => Impl<T>.Solve(ps);
        /// <inheritdoc cref="Solve{T}(ReadOnlySpan{ValueTuple{T, T}})"/>
        public static (int Index1, int Index2)[] Solve<T>(Span<(T, T)> ps)
            where T : INumber<T>, IMinMaxValue<T>
            => Impl<T>.Solve(ps);
#endif
        /// <summary>
        /// マンハッタン距離 |x2-x1| + |y2-y1| での最小全域木を構築する
        /// </summary>
        /// <param name="ps">座標の一覧</param>
        public static (int Index1, int Index2)[] Solve<T>(ReadOnlySpan<(T, T)> ps)
            where T : INumber<T>, IMinMaxValue<T>
            => Impl<T>.Solve(ps);


        internal static class Impl<T>
            where T : INumber<T>, IMinMaxValue<T>
        {
            readonly record struct MaxFw(T Value, int Index) : IAdditionOperators<MaxFw, MaxFw, MaxFw>, ISubtractionOperators<MaxFw, MaxFw, MaxFw>, IAdditiveIdentity<MaxFw, MaxFw>
            {
                public static MaxFw AdditiveIdentity => new(T.MinValue, 0);

                [凾(256)]
                public static MaxFw operator +(MaxFw x, MaxFw y)
                {
                    if (x.Index == 0) return y;
                    if (y.Index == 0) return x;
                    return x.Value > y.Value ? x : y;
                }

                public static MaxFw operator -(MaxFw left, MaxFw right) => left;
            }

            /// <summary>
            /// マンハッタン距離 |x2-x1| + |y2-y1| での最小全域木を構築する
            /// </summary>
            /// <param name="ps">座標の一覧</param>
            public static (int Index1, int Index2)[] Solve(ReadOnlySpan<(T, T)> ps)
            {
                List<(int Index1, int Index2)> edgesList = new(ps.Length * 4);
                var minf = MaxFw.AdditiveIdentity;
                var p = ps.ToArray();
                for (int ph = 0; ph < 4; ph++)
                {
                    var ids = Enumerable.Range(0, p.Length).ToArray();
                    Array.Sort(p.Select(p => (-(p.Item1 + p.Item2), -p.Item2)).ToArray(), ids);

                    var xv = p.Select(p => p.Item1).Distinct().ToArray();
                    Array.Sort(xv);
                    FenwickTree<MaxFw> fw = new(p.Length);
                    fw.data.AsSpan().Fill(minf);
                    foreach (var ix in ids)
                    {
                        var xi = xv.LowerBound(p[ix].Item1);
                        var max = fw.Sum(xi + 1);
                        if (max.Index > 0)
                            edgesList.Add((ix, max.Index - 1));
                        var x = p[ix].Item1 - p[ix].Item2;
                        fw.Add(xi, new(x, ix + 1));
                    }

                    for (int i = 0; i < p.Length; i++)
                        p[i] = (p[i].Item2, p[i].Item1);
                    if (ph == 1)
                        for (int i = 0; i < p.Length; i++)
                            p[i].Item2 = -p[i].Item2;
                }
                edgesList.Select(new Dist(p).Convert).ToArray().AsSpan(0, edgesList.Count).Sort(edgesList.AsSpan());

                UnionFind uf = new(p.Length);
                var res = new (int Index1, int Index2)[p.Length - 1];
                int ri = 0;
                foreach (var (ix1, ix2) in edgesList.AsSpan())
                {
                    if (uf.Merge(ix1, ix2))
                        res[ri++] = (ix1, ix2);
                }
                Debug.Assert(ri == res.Length);
                return res;
            }
            readonly struct Dist : IComparer<(int Index1, int Index2)>
            {
                readonly (T, T)[] ps;
                public Dist((T, T)[] ps) { this.ps = ps; }

                [凾(256)]
                public int Compare((int Index1, int Index2) x, (int Index1, int Index2) y)
                    => Convert(x).CompareTo(Convert(y));
                [凾(256)]
                public T Convert((int Index1, int Index2) v)
                {
                    var (i, j) = v;
                    return T.Abs(ps[i].Item1 - ps[j].Item1) + T.Abs(ps[i].Item2 - ps[j].Item2);
                }
            }
        }
    }
}