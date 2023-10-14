using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 木の子孫数
    public static class 木の子孫数
    {
        /// <summary>
        /// 自身を含む子孫ノードの数を返す
        /// </summary>
        [凾(256)]
        public static int[] DescendantsCounts<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
              where TNode : ITreeNode<TEdge>
              where TEdge : IGraphEdge
        {
            var treeArr = tree.AsArray();
            var res = new int[treeArr.Length];
            var stack = new Stack<(int v, int ci)>(treeArr.Length);
            stack.Push((tree.Root, 0));

            while (stack.TryPop(out var tup))
            {
                var (v, ci) = tup;
                var children = treeArr[v].Children;

                if (ci == 0)
                    res[v] = 1;
                else
                    res[v] += res[children[ci - 1].To];

                if (ci < children.Length)
                {
                    stack.Push((v, ci + 1));
                    stack.Push((children[ci].To, 0));
                }
            }
            return res;
        }
    }
}
