using AtCoder;
using AtCoder.Internal;
using AtCoder.Operators;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class IntWeightedUnionFind : WeightedUnionFind<int, IntOperator> { public IntWeightedUnionFind(int n) : base(n) { } }
    public class LongWeightedUnionFind : WeightedUnionFind<long, LongOperator> { public LongWeightedUnionFind(int n) : base(n) { } }
    public class WeightedUnionFind<T, TOp> where TOp : struct, IAdditionOperator<T>, ISubtractOperator<T>, IUnaryNumOperator<T>
    {
        private static TOp op = default;
        internal readonly int _n;
        internal readonly int[] _parentOrSize;
        /// <summary>
        /// 親との重みの差分
        /// </summary>
        internal readonly T[] _weightDiff;

        /// <summary>
        /// <see cref="WeightedUnionFind{T, TOp}"/> クラスの新しいインスタンスを、<paramref name="n"/> 頂点 0 辺のグラフとして初期化します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        public WeightedUnionFind(int n)
        {
            _n = n;
            _parentOrSize = new int[n];
            _weightDiff = new T[n];
            for (int i = 0; i < _parentOrSize.Length; i++) _parentOrSize[i] = -1;
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> から頂点 <paramref name="b"/> を重み <paramref name="w"/> で結ぶ辺を追加します。辺が追加済みでないか既に追加済みで重みが <paramref name="w"/> なら true, 重みに矛盾があるなら false を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        [凾(256)]
        public bool Merge(int a, int b, T w)
        {
            Contract.Assert(0 <= a && a < _n);
            Contract.Assert(0 <= b && b < _n);
            int x = Leader(a), y = Leader(b);
            if (x == y)
                return EqualityComparer<T>.Default.Equals(WeightDiff(a, b), w);

            w = op.Add(w, op.Subtract(Weight(a), Weight(b)));
            if (-_parentOrSize[x] < -_parentOrSize[y])
            {
                (x, y) = (y, x);
                w = op.Minus(w);
            }
            _parentOrSize[x] += _parentOrSize[y];
            _parentOrSize[y] = x;
            _weightDiff[y] = w;
            return true;
        }

        /// <summary>
        /// 頂点 <paramref name="a"/>, <paramref name="b"/> が連結かどうかを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        [凾(256)]
        public bool Same(int a, int b)
        {
            Contract.Assert(0 <= a && a < _n);
            Contract.Assert(0 <= b && b < _n);
            return Leader(a) == Leader(b);
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> の属する連結成分の代表元を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        [凾(256)]
        public int Leader(int a)
        {
            if (_parentOrSize[a] < 0) return a;
            int par = Leader(_parentOrSize[a]);
            _weightDiff[a] = op.Add(_weightDiff[a], _weightDiff[_parentOrSize[a]]);
            return _parentOrSize[a] = par;
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> の重みを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        [凾(256)]
        T Weight(int a)
        {
            Leader(a);
            return _weightDiff[a];
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> と頂点 <paramref name="b"/> の重みの差分を返します。同じグループにない場合の動作は未定義です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        [凾(256)]
        public T WeightDiff(int a, int b) => op.Subtract(Weight(b), Weight(a));

        /// <summary>
        /// 頂点 <paramref name="a"/> の属する連結成分のサイズを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        [凾(256)]
        public int Size(int a)
        {
            Contract.Assert(0 <= a && a < _n);
            return -_parentOrSize[Leader(a)];
        }

        /// <summary>
        /// グラフを連結成分に分け、その情報を返します。
        /// </summary>
        /// <para>計算量: O(n)</para>
        /// <returns>「一つの連結成分の頂点番号のリスト」のリスト。</returns>
        public int[][] Groups()
        {
            int[] leaderBuf = new int[_n];
            int[] id = new int[_n];
            var resultList = new List<int[]>(_n);
            for (int i = 0; i < leaderBuf.Length; i++)
            {
                leaderBuf[i] = Leader(i);
                if (i == leaderBuf[i])
                {
                    id[i] = resultList.Count;
                    resultList.Add(new int[-_parentOrSize[i]]);
                }
            }
            var result = resultList.ToArray();
            int[] ind = new int[result.Length];
            for (int i = 0; i < leaderBuf.Length; i++)
            {
                var leaderID = id[leaderBuf[i]];
                result[leaderID][ind[leaderID]] = i;
                ind[leaderID]++;
            }
            return result;
        }
    }
}
