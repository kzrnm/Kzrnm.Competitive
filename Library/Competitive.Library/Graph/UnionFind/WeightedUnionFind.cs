using AtCoder;
using AtCoder.Internal;
using Kzrnm.Competitive.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 重み付き UnionFind
    /// </summary>
    /// <remarks>頂点間の重みを保持し、整合性チェックを行う。</remarks>
    public class WeightedUnionFind<T> : WeightedUnionFind<T, WeightedUnionFind<T>.Adder>
    where T :
    IAdditionOperators<T, T, T>
    , IAdditiveIdentity<T, T>
    , IUnaryNegationOperators<T, T>
    {
        /// <inheritdoc />
        public WeightedUnionFind(int n) : base(n) { }
        public readonly struct Adder : IWeightedUnionFindOperator<T>
        {
            public T Identity => T.AdditiveIdentity;
            [凾(256)] public T Negate(T v) => -v;
            [凾(256)] public T Operate(T x, T y) => x + y;
        }
    }

    namespace Internal
    {
        [IsOperator]
        public interface IWeightedUnionFindOperator<T> : ISegtreeOperator<T>
        {
            /// <summary>
            /// 符号を逆転させる処理。足し算ならマイナス、掛け算なら逆数。
            /// </summary>
            T Negate(T v);
        }
    }

    /// <summary>
    /// 重み付き UnionFind
    /// </summary>
    /// <remarks>頂点間の重みを保持し、整合性チェックを行う。</remarks>
    public class WeightedUnionFind<T, TOp> where TOp : struct, Internal.IWeightedUnionFindOperator<T>
    {
        static TOp op => new();

        /// <summary>
        /// Parent or size. A negative value indicates size, a positive value indicates parent.
        /// </summary>
        internal readonly int[] _ps;
        /// <summary>
        /// 親との重みの差分
        /// </summary>
        internal readonly T[] _w;

        /// <summary>
        /// <see cref="WeightedUnionFind{T, TOp}"/> クラスの新しいインスタンスを、<paramref name="n"/> 頂点 0 辺のグラフとして初期化します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        public WeightedUnionFind(int n)
        {
            _ps = new int[n];
            _w = new T[n];
            _ps.AsSpan().Fill(-1);
            var id = new TOp().Identity;
            if (!EqualityComparer<T>.Default.Equals(id, default))
                _w.AsSpan().Fill(id);
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
            Contract.Assert(0 <= a && a < _ps.Length);
            Contract.Assert(0 <= b && b < _ps.Length);
            int x = Leader(a), y = Leader(b);
            if (x == y)
                return EqualityComparer<T>.Default.Equals(_w[b], op.Operate(_w[a], w));

            if (_ps[x] > _ps[y])
            {
                (x, y) = (y, x);
                (a, b) = (b, a);
                w = op.Negate(w);
            }

            _ps[x] += _ps[y];
            _ps[y] = x;
            _w[y] = op.Operate(op.Operate(_w[a], w), op.Negate(_w[b]));
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
            Contract.Assert(0 <= a && a < _ps.Length);
            Contract.Assert(0 <= b && b < _ps.Length);
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
            if (_ps[a] < 0) return a;
            int par = Leader(_ps[a]);
            // マージが済んでいないので重みを再計算する
            _w[a] = op.Operate(_w[_ps[a]], _w[a]);
            return _ps[a] = par;
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
            return _w[a];
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> と頂点 <paramref name="b"/> の重みの差分を返します。同じグループにない場合の動作は未定義です。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        [凾(256)]
        public T WeightDiff(int a, int b)
            => op.Operate(op.Negate(Weight(a)), Weight(b));


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
            Contract.Assert(0 <= a && a < _ps.Length);
            return -_ps[Leader(a)];
        }

        /// <summary>
        /// グラフを連結成分に分け、その情報を返します。
        /// </summary>
        /// <para>計算量: O(n)</para>
        /// <returns>「一つの連結成分の頂点番号のリスト」のリスト。</returns>
        [凾(256)]
        public int[][] Groups() => GroupsAndIds().Groups;

        /// <summary>
        /// グラフを連結成分に分け、そのIDを返します。
        /// </summary>
        /// <para>計算量: O(n)</para>
        /// <returns>頂点番号に対応する連結成分のID。</returns>
        [凾(256)]
        public int[] GroupIds() => GroupsAndIds().GroupIds;

        /// <summary>
        /// グラフを連結成分に分け、その情報を返します。
        /// </summary>
        /// <para>計算量: O(n)</para>
        /// <returns>「一つの連結成分の頂点番号のリスト」のリスト, 頂点番号に対応する連結成分のID。</returns>
        public (int[][] Groups, int[] GroupIds) GroupsAndIds()
        {
            var leaderBuf = new int[_ps.Length];
            var id = new int[_ps.Length];
            var gr = new int[_ps.Length];
            var resultList = new List<int[]>(_ps.Length);
            for (int i = 0; i < leaderBuf.Length; i++)
            {
                leaderBuf[i] = Leader(i);
                if (i == leaderBuf[i])
                {
                    id[i] = resultList.Count;
                    resultList.Add(new int[-_ps[i]]);
                }
            }
            var result = resultList.ToArray();
            var ind = new int[result.Length];
            for (int i = 0; i < leaderBuf.Length; i++)
            {
                var leaderID = id[leaderBuf[i]];
                gr[i] = leaderID;
                result[leaderID][ind[leaderID]] = i;
                ind[leaderID]++;
            }
            return (result, gr);
        }
    }

    public interface IWeight<T> : ISegtreeOperator<T>
    {

    }
}
