using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    [DebuggerTypeProxy(typeof(UnionFind<>.DebugView))]
    [DebuggerDisplay("Count = {" + nameof(_parentOrSize) + "." + nameof(Array.Length) + "}")]
    public class UnionFind<T>
    {
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
            _datas = (T[])datas.Clone();
            _mergeDataFunc = mergeFunc;
            _parentOrSize = new int[_datas.Length];
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
            Contract.Assert(0 <= a && a < _parentOrSize.Length);
            Contract.Assert(0 <= b && b < _parentOrSize.Length);
            int x = Leader(a), y = Leader(b);
            if (x == y) return false;
            if (_parentOrSize[x] > _parentOrSize[y]) (x, y) = (y, x);
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
            Contract.Assert(0 <= a && a < _parentOrSize.Length);
            Contract.Assert(0 <= b && b < _parentOrSize.Length);
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
            Contract.Assert(0 <= a && a < _parentOrSize.Length);
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
            Contract.Assert(0 <= a && a < _parentOrSize.Length);
            return _datas[Leader(a)];
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
            var leaderBuf = new int[_parentOrSize.Length];
            var id = new int[_parentOrSize.Length];
            var gr = new int[_parentOrSize.Length];
            var resultList = new List<int[]>(_parentOrSize.Length);
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
