using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AtCoder.GraphAcl
{
    public class DSU
    {
        public int Count { get; }
        private readonly int[] ParentOrSize;

        /// <summary>
        /// <see cref="DSU"/> クラスの新しいインスタンスを、<paramref name="n"/> 頂点 0 辺のグラフとして初期化します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        public DSU(int n)
        {
            Count = n;
            ParentOrSize = new int[n];
            for (int i = 0; i < ParentOrSize.Length; i++) ParentOrSize[i] = -1;
        }

        /// <summary>
        /// 頂点 <paramref name="a"/> と頂点 <paramref name="b"/> を結ぶ辺がなければ追加します。追加されたかどうかを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="a"/>, <paramref name="b"/>&lt;n</para>
        /// <para>計算量: ならしO(a(n))</para>
        /// </remarks>
        public bool Merge(int a, int b)
        {
            Debug.Assert(0 <= a && a < Count);
            Debug.Assert(0 <= b && b < Count);
            int x = Leader(a), y = Leader(b);
            if (x == y) return false;
            if (-ParentOrSize[x] < -ParentOrSize[y]) (x, y) = (y, x);
            ParentOrSize[x] += ParentOrSize[y];
            ParentOrSize[y] = x;
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
            Debug.Assert(0 <= a && a < Count);
            Debug.Assert(0 <= b && b < Count);
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
            if (ParentOrSize[a] < 0) return a;
            while (0 <= ParentOrSize[ParentOrSize[a]])
            {
                (a, ParentOrSize[a]) = (ParentOrSize[a], ParentOrSize[ParentOrSize[a]]);
            }
            return ParentOrSize[a];
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
            Debug.Assert(0 <= a && a < Count);
            return -ParentOrSize[Leader(a)];
        }

        /// <summary>
        /// グラフを連結成分に分け、その情報を返します。
        /// </summary>
        /// <para>計算量: O(n)</para>
        /// <returns>「一つの連結成分の頂点番号のリスト」のリスト。</returns>
        public int[][] Groups()
        {
            int[] leaderBuf = new int[Count];
            int[] id = new int[Count];
            var result = new List<int[]>(Count);
            int groupCount = 0;
            for (int i = 0; i < leaderBuf.Length; i++)
            {
                leaderBuf[i] = Leader(i);
                if (i == leaderBuf[i])
                {
                    id[i] = groupCount++;
                    result.Add(new int[-ParentOrSize[i]]);
                }
            }
            int[] ind = new int[groupCount];
            for (int i = 0; i < leaderBuf.Length; i++)
            {
                var leaderID = id[leaderBuf[i]];
                result[leaderID][ind[leaderID]++] = i;
            }
            return result.ToArray();
        }
    }
}
