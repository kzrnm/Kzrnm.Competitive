﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AtCoder
{
    public class UnionFind
    {
        internal readonly int _n;
        internal readonly int[] _parentOrSize;

        /// <summary>
        /// <see cref="UnionFind"/> クラスの新しいインスタンスを、<paramref name="n"/> 頂点 0 辺のグラフとして初期化します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        public UnionFind(int n)
        {
            _n = n;
            _parentOrSize = new int[n];
            for (int i = 0; i < _parentOrSize.Length; i++) _parentOrSize[i] = -1;
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> と頂点 <paramref name="b"/> を結ぶ辺を追加します。既に追加済みならば false、新たに追加されたならば true を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        public bool Merge(int a, int b)
        {
            Debug.Assert(0 <= a && a < _n);
            Debug.Assert(0 <= b && b < _n);
            int x = Leader(a), y = Leader(b);
            if (x == y) return false;
            if (-_parentOrSize[x] < -_parentOrSize[y]) (x, y) = (y, x);
            _parentOrSize[x] += _parentOrSize[y];
            _parentOrSize[y] = x;
            return true;
        }

        /// <summary>
        /// 頂点 <paramref name="a"/>, <paramref name="b"/> が連結かどうかを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        public bool Same(int a, int b)
        {
            Debug.Assert(0 <= a && a < _n);
            Debug.Assert(0 <= b && b < _n);
            return Leader(a) == Leader(b);
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> の属する連結成分の代表元を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
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
        public int Size(int a)
        {
            Debug.Assert(0 <= a && a < _n);
            return -_parentOrSize[Leader(a)];
        }

        /// <summary>
        /// グラフを連結成分に分け、その情報を返します。
        /// </summary>
        /// <para>計算量: O(n)</para>
        /// <returns>「一つの連結成分の頂点番号のリスト」のリスト。</returns>
        public List<int[]> Groups()
        {
            int[] leaderBuf = new int[_n];
            int[] id = new int[_n];
            var result = new List<int[]>(_n);
            for (int i = 0; i < leaderBuf.Length; i++)
            {
                leaderBuf[i] = Leader(i);
                if (i == leaderBuf[i])
                {
                    id[i] = result.Count;
                    result.Add(new int[-_parentOrSize[i]]);
                }
            }
            int[] ind = new int[result.Count];
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
