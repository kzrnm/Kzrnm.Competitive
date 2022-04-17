using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    [DebuggerTypeProxy(typeof(UnionFind<>.DebugView))]
    [DebuggerDisplay("Count = {" + nameof(_n) + "}")]
    public class UnionFind<T>
    {
        internal readonly int _n;
        internal readonly int[] _parentOrSize;
        internal readonly T[] _datas;
        internal readonly Func<T, T, T> _mergeDataFunc;

        /// <summary>
        /// <see cref="UnionFind{T}"/> クラスの新しいインスタンスを、|<paramref name="datas"/>| 頂点 0 辺のグラフとして初期化します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤|<paramref name="datas"/>|≤10^8</para>
        /// <para>計算量: O(|<paramref name="datas"/>|)</para>
        /// </remarks>
        /// <param name="datas">各頂点の初期値</param>
        /// <param name="mergeFunc">頂点の持つ値のマージを行う関数</param>
        public UnionFind(T[] datas, Func<T, T, T> mergeFunc)
        {
            _n = datas.Length;
            _datas = (T[])datas.Clone();
            _mergeDataFunc = mergeFunc;
            _parentOrSize = new int[_n];
            _parentOrSize.AsSpan().Fill(-1);
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> と頂点 <paramref name="b"/> を結ぶ辺を追加します。既に追加済みならば false、新たに追加されたならば true を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        [凾(256)]
        public bool Merge(int a, int b)
        {
            Contract.Assert(0 <= a && a < _n);
            Contract.Assert(0 <= b && b < _n);
            int x = Leader(a), y = Leader(b);
            if (x == y) return false;
            if (-_parentOrSize[x] < -_parentOrSize[y]) (x, y) = (y, x);
            _parentOrSize[x] += _parentOrSize[y];
            _parentOrSize[y] = x;
            _datas[x] = _mergeDataFunc(_datas[x], _datas[y]);
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
            while (0 <= _parentOrSize[_parentOrSize[a]])
            {
                (a, _parentOrSize[a]) = (_parentOrSize[a], _parentOrSize[_parentOrSize[a]]);
            }
            return _parentOrSize[a];
        }


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
        /// 頂点 <paramref name="a"/> の属する連結成分の持つデータを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        [凾(256)]
        public T Data(int a)
        {
            Contract.Assert(0 <= a && a < _n);
            return _datas[Leader(a)];
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
        private class DebugView
        {
            private readonly UnionFind<T> uf;
            public DebugView(UnionFind<T> uf)
            {
                this.uf = uf ?? throw new ArgumentNullException(nameof(uf));
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public int[][] Groups => uf.Groups();
        }
    }
}
