using AtCoder;
using AtCoder.Extension;
using AtCoder.Operators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// マンハッタン距離 |x2-x1| + |y2-y1| での最小全域木を構築する
    /// </summary>
    public static class ManhattanMST
    {
        /// <summary>
        /// マンハッタン距離 |x2-x1| + |y2-y1| での最小全域木を構築する
        /// </summary>
        /// <param name="ps">座標の一覧</param>
        public static (int Index1, int Index2)[] Solve((int, int)[] ps) => Impl<int, IntOperator>.Solve(ps);
        /// <summary>
        /// マンハッタン距離 |x2-x1| + |y2-y1| での最小全域木を構築する
        /// </summary>
        /// <param name="ps">座標の一覧</param>
        public static (int Index1, int Index2)[] Solve((long, long)[] ps) => Impl<long, LongOperator>.Solve(ps);

        internal static class Impl<T, TOp>
            where T : IComparable<T>
            where TOp : struct
            , IAdditionOperator<T>
            , ISubtractOperator<T>
            , IMinMaxValueOperator<T>
            , IUnaryNumOperator<T>
            , ICompareOperator<T>
        {
            readonly struct MaxFenwickTreeOperator : IAdditionOperator<(T, int)>, ISubtractOperator<(T, int)>
            {
                [凾(256)]
                public (T, int) Add((T, int) x, (T, int) y)
                {
                    if (x.Item2 == 0) return y;
                    if (y.Item2 == 0) return x;
                    return x.Item1.CompareTo(y.Item1) > 0 ? x : y;
                }
                [凾(256)]
                public (T, int) Subtract((T, int) x, (T, int) y) => x;
            }

            /// <summary>
            /// マンハッタン距離 |x2-x1| + |y2-y1| での最小全域木を構築する
            /// </summary>
            /// <param name="ps">座標の一覧</param>
            public static (int Index1, int Index2)[] Solve((T, T)[] ps)
            {
                var op = new TOp();
                using var edgesList = new PoolList<(int Index1, int Index2)>(ps.Length * 4);
                var minf = (op.MinValue, 0);
                ps = ((T, T)[])ps.Clone();
                for (int ph = 0; ph < 4; ph++)
                {
                    var ids = Enumerable.Range(0, ps.Length).ToArray();
                    Array.Sort(ps.Select(p =>
                    {
                        var op = new TOp();
                        return (op.Minus(op.Add(p.Item1, p.Item2)), op.Minus(p.Item2));
                    }).ToArray(), ids);

                    var xv = ps.Select(p => p.Item1).Distinct().ToArray();
                    Array.Sort(xv);
                    var fw = new FenwickTree<(T, int), MaxFenwickTreeOperator>(ps.Length);
                    fw.data.AsSpan().Fill(minf);
                    foreach (var ix in ids)
                    {
                        var xi = xv.LowerBound(ps[ix].Item1);
                        var max = fw.Sum(xi + 1);
                        if (max.Item2 > 0)
                            edgesList.Add((ix, max.Item2 - 1));
                        var x = op.Subtract(ps[ix].Item1, ps[ix].Item2);
                        fw.Add(xi, (x, ix + 1));
                    }

                    for (int i = 0; i < ps.Length; i++)
                        ps[i] = (ps[i].Item2, ps[i].Item1);
                    if (ph == 1)
                        for (int i = 0; i < ps.Length; i++)
                            ps[i].Item2 = op.Minus(ps[i].Item2);
                }
                Array.Sort(edgesList.data.Select(new Dist(ps).Convert).ToArray(), edgesList.data, 0, edgesList.Count);

                var uf = new UnionFind(ps.Length);
                var res = new (int Index1, int Index2)[ps.Length - 1];
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
                    => new TOp().Compare(Convert(x), Convert(y));
                [凾(256)]
                public T Convert((int Index1, int Index2) v)
                {
                    var op = new TOp();
                    var (i, j) = v;
                    return op.Add(Abs(op.Subtract(ps[i].Item1, ps[j].Item1)), Abs(op.Subtract(ps[i].Item2, ps[j].Item2)));
                }
                [凾(256)]
                private T Abs(T v)
                {
                    var op = new TOp();
                    if (op.LessThan(v, default))
                        v = op.Minus(v);
                    return v;
                }
            }
            /*
    template <class T> V<pair<int, int>> manhattan_mst(V<pair<T, T>> ps, T inf = numeric_limits<T>::max()) {
        V<pair<int, int>> edges;
        int n = int(ps.size());
        for (int ph = 0; ph < 4; ph++) {        V<T> xv;        for (int i = 0; i < n; i++) xv.push_back(ps[i].first);        stable_sort(xv.begin(), xv.end());xv.erase(unique(xv.begin(), xv.end()), xv.end());        using P = pair<T, int>;        V<P> fen(n, P(-inf, -1));        for (int id : ids) {            auto xi = int(lower_bound(xv.begin(), xv.end(), ps[id].first) -    xv.begin());            P ma = P(-inf, -1);            {                int i = xi + 1;                while (i > 0) {                    if (ma.first <= fen[i - 1].first) ma = fen[i - 1];                    i -= i & -i;                }            }            if (ma.second != -1) edges.push_back({id, ma.second});            {                T x = ps[id].first - ps[id].second;                int i = xi + 1;                while (i <= n) {                    if (fen[i - 1].first <= x) fen[i - 1] = P(x, id);                    i += i & -i;                }            }        }        for (auto& p : ps) {            swap(p.first, p.second);        }        if (ph == 1) {            for (auto& p : ps) {                p.second *= -1;            }        }    }
        auto dist = [&](int i, int j) {
            return abs(ps[i].first - ps[j].first) +
                   abs(ps[i].second - ps[j].second);
        };
        stable_sort(edges.begin(), edges.end(), [&](auto x, auto y) {
            return dist(x.first, x.second) < dist(y.first, y.second);
        });
        auto uf = UnionFind(n);
        V<pair<int, int>> res;
        for (auto p : edges) {
            if (uf.same(p.first, p.second)) continue;
            res.push_back(p);
            uf.merge(p.first, p.second);
        }
        return res;
    }
    */
        }
    }
}