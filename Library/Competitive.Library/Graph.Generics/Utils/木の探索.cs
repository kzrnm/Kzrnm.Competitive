using System;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class 木の探索
    {
        /// <summary>
        /// 木を幅優先探索するときに訪れる順序に並んだインデックスを返す
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
        /// 木を深さ優先探索するときに訪れる順序に並んだインデックスを返す
        /// </summary>
        [凾(256)]
        public static int[] DfsDescendant<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
               where TNode : ITreeNode<TEdge>
               where TEdge : IGraphEdge
        {
            var down = tree.HlDecomposition.down;
            var res = new int[down.Length];
            for (int i = 0; i < res.Length; i++)
                res[down[i]] = i;
            return res;
        }
    }
}
