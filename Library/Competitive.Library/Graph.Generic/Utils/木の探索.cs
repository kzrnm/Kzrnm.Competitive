using System.Collections.Generic;
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
        /// <summary>
        /// 木を深い順に深さ優先探索するときに訪れる順序に並んだインデックスを返す
        /// </summary>
        [凾(256)]
        public static int[] DfsDescendantLeaf<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
               where TNode : ITreeNode<TEdge>
               where TEdge : IGraphEdge
        {
            var arr = tree.AsArray();
            var st = new Stack<(int Index, bool Start)>(tree.Length * 2);
            var ri = 0;
            var res = new int[tree.Length];
            st.Push((tree.Root, true));
            while (st.TryPop(out var ix, out var start))
            {
                if (start)
                {
                    st.Push((ix, false));
                    foreach (var e in arr[ix].Children)
                        st.Push((e.To, true));
                }
                else
                    res[ri++] = ix;
            }
            return res;
        }
    }
}
