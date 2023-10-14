using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 木の探索
    public static class 木の探索
    {
        /// <summary>
        /// 木を幅優先探索するときに訪れる順序に並んだインデックスを返します。
        /// </summary>
        [凾(256)]
        public static int[] BfsDescendant<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
              where TNode : ITreeNode<TEdge>
              where TEdge : IGraphEdge
        {
            var arr = tree.AsArray();
            var res = new int[arr.Length];
            res[0] = tree.Root;

            int tar = 1;
            int cur = 0;
            while ((uint)tar < (uint)res.Length)
            {
                foreach (var e in arr[res[cur++]].Children)
                    res[tar++] = e.To;
            }

            return res;
        }
        /// <summary>
        /// 木を深さ優先探索するときに訪れる順序に並んだインデックスを返します。
        /// </summary>
        [凾(256)]
        public static int[] DfsDescendant<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
               where TNode : ITreeNode<TEdge>
               where TEdge : IGraphEdge
        {
            var down = tree.HlDecomposition.down;
            var res = new int[down.Length];
            for (int i = 0; i < down.Length; i++)
                res[down[i]] = i;
            return res;
        }
        /// <summary>
        /// 木を深い順に深さ優先探索するときに訪れる順序に並んだインデックスを返します。
        /// </summary>
        [凾(256)]
        public static int[] DfsDescendantLeaf<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
               where TNode : ITreeNode<TEdge>
               where TEdge : IGraphEdge
        {
            var res = tree.DfsDescendant();
            res.AsSpan().Reverse();
            return res;
        }
    }
}
