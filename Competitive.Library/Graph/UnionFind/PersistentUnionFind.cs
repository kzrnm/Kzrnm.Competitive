﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AtCoder.Internal;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 永続UnionFind。
    /// </summary>
    public readonly struct PersistentUnionFind
    {
        private readonly ImmutableList<int> _parentOrSize;

        /// <summary>
        /// <see cref="PersistentUnionFind"/> を、<paramref name="n"/> 頂点 0 辺のグラフとして初期化します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        public PersistentUnionFind(int n)
        {
            _parentOrSize = ImmutableList<int>.Empty.AddRange(Enumerable.Repeat(-1, n));
        }

        private PersistentUnionFind(ImmutableList<int> parentOrSize)
        {
            _parentOrSize = parentOrSize;
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> と頂点 <paramref name="b"/> を結ぶ辺を追加します。既に追加済みならば false、新たに追加されたならば true を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: O(log^2(n))</para>
        /// </remarks>
        public PersistentUnionFind Merge(int a, int b)
        {
            Contract.Assert(0 <= a && a < _parentOrSize.Count);
            Contract.Assert(0 <= b && b < _parentOrSize.Count);
            int x = Leader(a), y = Leader(b);
            if (x == y) return this;
            var u = _parentOrSize[x];
            var v = _parentOrSize[y];

            if (-u < -v)
            {
                (x, y) = (y, x);
                (u, v) = (v, u);
            }

            return new PersistentUnionFind(_parentOrSize.SetItem(x, u + v).SetItem(y, x));
        }

        /// <summary>
        /// 頂点 <paramref name="a"/>, <paramref name="b"/> が連結かどうかを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: O(log^2(n))</para>
        /// </remarks>
        public bool Same(int a, int b)
        {
            Contract.Assert(0 <= a && a < _parentOrSize.Count);
            Contract.Assert(0 <= b && b < _parentOrSize.Count);
            return Leader(a) == Leader(b);
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> の属する連結成分の代表元を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>&lt;n</para>
        /// <para>計算量: O(log^2(n))</para>
        /// </remarks>
        public int Leader(int a)
        {
            var p = _parentOrSize[a];
            if (p < 0) return a;
            return Leader(p);
        }


        /// <summary>
        /// 頂点 <paramref name="a"/> の属する連結成分のサイズを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>&lt;n</para>
        /// <para>計算量: O(log^2(n))</para>
        /// </remarks>
        public int Size(int a)
        {
            Contract.Assert(0 <= a && a < _parentOrSize.Count);
            return -_parentOrSize[Leader(a)];
        }

        /// <summary>
        /// グラフを連結成分に分け、その情報を返します。
        /// </summary>
        /// <para>計算量: O(n)</para>
        /// <returns>「一つの連結成分の頂点番号のリスト」のリスト。</returns>
        public List<int[]> Groups()
        {
            int _n = _parentOrSize.Count;
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