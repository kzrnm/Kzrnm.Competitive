using System.Collections.Generic;

namespace AtCoder
{
    public static class 木の探索
    {
        /// <summary>
        /// 木を幅優先探索するときに訪れる順序に並んだインデックスを返す
        /// </summary>
        public static int[] BfsDescendant<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree, bool skipFirst = false)
            where TNode : ITreeNode<TEdge>
            where TEdge : IEdge
        {
            var arr = tree.AsArray();
            var res = new int[arr.Length - (skipFirst ? 1 : 0)];
            int tar = 0;
            if (skipFirst)
            {
                foreach (var e in arr[tree.Root].Children)
                    res[tar++] = e.To;
            }
            else res[tar++] = tree.Root;


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
        public static int[] DfsDescendant<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree, bool skipFirst = false)
            where TNode : ITreeNode<TEdge>
            where TEdge : IEdge
        {
            var arr = tree.AsArray();
            var res = new int[arr.Length - (skipFirst ? 1 : 0)];
            int cur = 0;
            var stack = new Stack<int>(arr.Length);
            if (skipFirst)
            {
                var children = arr[tree.Root].Children;
                for (int i = children.Length - 1; i >= 0; i--)
                    stack.Push(children[i].To);
            }
            else
                stack.Push(tree.Root);
            while (stack.TryPop(out var ix))
            {
                res[cur++] = ix;
                var children = arr[ix].Children;
                for (int i = children.Length - 1; i >= 0; i--)
                    stack.Push(children[i].To);
            }

            return res;
        }
    }
}
