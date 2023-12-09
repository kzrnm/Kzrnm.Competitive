using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
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
            var rt = new int[arr.Length];
            rt[0] = tree.Root;

            int tar = 1;
            int cur = 0;
            while ((uint)tar < (uint)rt.Length)
            {
                foreach (var e in arr[rt[cur++]].Children)
                    rt[tar++] = e.To;
            }

            return rt;
        }
        /// <summary>
        /// 木を深さ優先探索するときに訪れる順序に並んだインデックスを返します。
        /// </summary>
        [凾(256)]
        public static int[] DfsDescendant<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
               where TNode : ITreeNode<TEdge>
               where TEdge : IGraphEdge
        {
            var d = tree.HlDecomposition.down;
            var rt = new int[d.Length];
            for (int i = 0; i < d.Length; i++)
                rt[d[i]] = i;
            return rt;
        }
        /// <summary>
        /// 木を深い順に深さ優先探索するときに訪れる順序に並んだインデックスを返します。
        /// </summary>
        [凾(256)]
        public static int[] DfsDescendantLeaf<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
               where TNode : ITreeNode<TEdge>
               where TEdge : IGraphEdge
        {
            var rt = tree.DfsDescendant();
            rt.AsSpan().Reverse();
            return rt;
        }
        /// <summary>
        /// 木を深さ優先探索するときに入る・出る順序に並んだインデックスを返します。非負の値は入るノード、負の値は出るノードのビット反転を表します。
        /// </summary>
        [凾(256)]
        public static int[] DfsDescendantEvents<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
               where TNode : ITreeNode<TEdge>
               where TEdge : IGraphEdge
        {
            var ev = tree.DfsDescendant();
            var up = tree.HlDecomposition.up;
            var rt = new int[up.Length << 1];
            Span<int> rs = rt;
            var uu = new List<int>[up.Length];
            for (int i = 0; i < ev.Length; i++)
            {
                rs[0] = ev[i];
                var p = up[ev[i]] - 1;
                if (p == i)
                {
                    rs[1] = ~ev[i];
                    rs = rs[2..];
                }
                else
                {
                    (uu[p] ??= new()).Add(~ev[i]);
                    rs = rs[1..];
                }
                var us = uu[i].AsSpan();
                us.Reverse();
                us.CopyTo(rs);
                rs = rs[us.Length..];
            }
            return rt;
        }
    }
}
