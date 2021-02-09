using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Kzrnm.Competitive.Graph
{
    public class 最小共通祖先Tests
    {
        [Fact]
        public void 重みなしグラフ()
        {
            var gb = new GraphBuilder(8, false);
            gb.Add(0, 1);
            gb.Add(0, 2);
            gb.Add(1, 3);
            gb.Add(1, 4);
            gb.Add(2, 5);
            gb.Add(2, 6);
            gb.Add(3, 7);
            var lca0 = gb.ToTree().LowestCommonAncestor();
            lca0.GetLca(0, 1).Should().Be(0);
            lca0.GetLca(3, 6).Should().Be(0);
            lca0.GetLca(4, 7).Should().Be(1);
        }
        [Fact]
        public void 重み付きグラフ()
        {
            var gb = new WIntGraphBuilder(8, false);
            gb.Add(0, 1, 1);
            gb.Add(0, 2, 2);
            gb.Add(1, 3, 3);
            gb.Add(1, 4, 4);
            gb.Add(2, 5, 5);
            gb.Add(2, 6, 6);
            gb.Add(3, 7, 7);
            var lca0 = gb.ToTree().LowestCommonAncestor();
            lca0.GetLca(0, 1).Should().Be(0);
            lca0.GetLca(3, 6).Should().Be(0);
            lca0.GetLca(4, 7).Should().Be(1);
        }

        [Fact]
        public void Random()
        {
            const int N = 60;
            var wgb = new WIntGraphBuilder(N, false);
            var gb = new GraphBuilder(N, false);
            var arr = Enumerable.Range(0, N).OrderBy(_ => Guid.NewGuid()).ToArray();
            for (int i = 1; i < arr.Length; i++)
            {
                gb.Add(arr[i - 1], arr[i]);
                wgb.Add(arr[i - 1], arr[i], i);
            }
            for (int r = 0; r < N; r++)
            {
                var tree = gb.ToTree(r);
                var wtree = wgb.ToTree(r);
                var lca = tree.LowestCommonAncestor();
                var wlca = wtree.LowestCommonAncestor();
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < N; j++)
                    {
                        var expected = LcaDirect(tree.AsArray(), i, j);
                        lca.GetLca(i, j).Should().Be(expected);
                        wlca.GetLca(i, j).Should().Be(expected);
                    }
            }
        }
        static int LcaDirect(TreeNode[] tree, int i, int j)
        {
            while (tree[i].Depth > tree[j].Depth) i = tree[i].Root;
            while (tree[i].Depth < tree[j].Depth) j = tree[j].Root;
            while (i != j)
            {
                i = tree[i].Root;
                j = tree[j].Root;
            }
            return i;
        }
    }
}
