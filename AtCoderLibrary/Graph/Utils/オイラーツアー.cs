using System.Collections.Generic;

namespace AtCoder.Graph
{
    public static class オイラーツアー
    {
        /// <summary>
        /// <para>オイラーツアーを求める。</para>
        /// <para>根から各ノードを深さ優先探索するとき、ノードに入る/出るをイベント化したときのインデックスを返す。</para>
        /// </summary>
        public static (int l, int r)[] EulerianTour<TEdge>(this ITreeNode<TEdge>[] tree)
            where TEdge : IEdge
        {
            var root = 0;
            while (tree[root].Root.To >= 0) root = tree[root].Root.To;

            var cnt = 0;
            var res = new (int l, int r)[tree.Length];

            var idx = new Stack<(int index, int ci)>(tree.Length);
            idx.Push((root, 0));
            while (idx.Count > 0)
            {
                var (index, ci) = idx.Pop();
                var children = tree[index].Children;

                if (ci == 0)
                    res[index].l = cnt++;
                if (ci < children.Length)
                {
                    idx.Push((index, ci + 1));
                    idx.Push((children[ci].To, 0));
                }
                else
                    res[index].r = cnt++;
            }

            /* 再帰版
            void Dfs(int index)
            {
                res[index].l = cnt++;
                foreach (var ch in tree[index].children)
                    Dfs(ch);
                res[index].r = cnt++;
            }
            Dfs(root);
            */
            return res;
        }
    }
}
