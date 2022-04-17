using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public static class CartesianTree
    {
        /// <summary>
        /// <para>Cartesian tree を構築します。</para>
        /// <para>Cartesian tree: 最も小さい値を根として、左右それぞれで根の次に大きいものを子とする二分木</para>
        /// <para>https://en.wikipedia.org/wiki/Cartesian_tree</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        /// <returns>親のインデックス(根は -1 )を保持する配列</returns>
        public static int[] Create(int[] a, IComparer<int> comparer = null)
        {
            var parents = new int[a.Length];
            parents.AsSpan().Fill(-1);

            for (int i = 1; i < a.Length; i++)
            {
                int p = i - 1;
                int l = -1;

                while (p != -1 && (
                    comparer == null
                    ? a[i].CompareTo(a[p])
                    : comparer.Compare(a[i], a[p])) < 0)
                {
                    int pp = parents[p];
                    if (l != -1)
                    {
                        parents[l] = p;
                    }
                    parents[p] = i;
                    l = p;
                    p = pp;
                }
                parents[i] = p;
            }
            return parents;
        }
    }
}
