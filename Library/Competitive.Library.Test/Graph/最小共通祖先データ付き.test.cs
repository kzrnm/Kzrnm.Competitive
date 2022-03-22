using AtCoder;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Kzrnm.Competitive.Testing.Graph
{
    // verification-helper: SAMEAS Library/run.test.py
    public class 最小共通祖先データ付きTests
    {
        struct TOp : ISegtreeOperator<int>
        {
            public int Identity => 0;
            public int Operate(int x, int y) => x + y;
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
            var tree = gb.ToTree();
            var lca0 = tree.LowestCommonAncestorWithDataBuilder().Build<int, TOp>(tree.AsArray().Select(n => n.Root.Value).ToArray());
            lca0.Lca(0, 0).Should().Be((0, 0)); lca0.Lca(0, 1).Should().Be((0, 1)); lca0.Lca(0, 2).Should().Be((0, 2)); lca0.Lca(0, 3).Should().Be((0, 4));
            lca0.Lca(0, 4).Should().Be((0, 5)); lca0.Lca(0, 5).Should().Be((0, 7)); lca0.Lca(0, 6).Should().Be((0, 8)); lca0.Lca(0, 7).Should().Be((0, 11));
            lca0.Lca(1, 0).Should().Be((0, 1)); lca0.Lca(1, 1).Should().Be((1, 0)); lca0.Lca(1, 2).Should().Be((0, 3)); lca0.Lca(1, 3).Should().Be((1, 3));
            lca0.Lca(1, 4).Should().Be((1, 4)); lca0.Lca(1, 5).Should().Be((0, 8)); lca0.Lca(1, 6).Should().Be((0, 9)); lca0.Lca(1, 7).Should().Be((1, 10));
            lca0.Lca(2, 0).Should().Be((0, 2)); lca0.Lca(2, 1).Should().Be((0, 3)); lca0.Lca(2, 2).Should().Be((2, 0)); lca0.Lca(2, 3).Should().Be((0, 6));
            lca0.Lca(2, 4).Should().Be((0, 7)); lca0.Lca(2, 5).Should().Be((2, 5)); lca0.Lca(2, 6).Should().Be((2, 6)); lca0.Lca(2, 7).Should().Be((0, 13));
            lca0.Lca(3, 0).Should().Be((0, 4)); lca0.Lca(3, 1).Should().Be((1, 3)); lca0.Lca(3, 2).Should().Be((0, 6)); lca0.Lca(3, 3).Should().Be((3, 0));
            lca0.Lca(3, 4).Should().Be((1, 7)); lca0.Lca(3, 5).Should().Be((0, 11)); lca0.Lca(3, 6).Should().Be((0, 12)); lca0.Lca(3, 7).Should().Be((3, 7));
            lca0.Lca(4, 0).Should().Be((0, 5)); lca0.Lca(4, 1).Should().Be((1, 4)); lca0.Lca(4, 2).Should().Be((0, 7)); lca0.Lca(4, 3).Should().Be((1, 7));
            lca0.Lca(4, 4).Should().Be((4, 0)); lca0.Lca(4, 5).Should().Be((0, 12)); lca0.Lca(4, 6).Should().Be((0, 13)); lca0.Lca(4, 7).Should().Be((1, 14));
            lca0.Lca(5, 0).Should().Be((0, 7)); lca0.Lca(5, 1).Should().Be((0, 8)); lca0.Lca(5, 2).Should().Be((2, 5)); lca0.Lca(5, 3).Should().Be((0, 11));
            lca0.Lca(5, 4).Should().Be((0, 12)); lca0.Lca(5, 5).Should().Be((5, 0)); lca0.Lca(5, 6).Should().Be((2, 11)); lca0.Lca(5, 7).Should().Be((0, 18));
            lca0.Lca(6, 0).Should().Be((0, 8)); lca0.Lca(6, 1).Should().Be((0, 9)); lca0.Lca(6, 2).Should().Be((2, 6)); lca0.Lca(6, 3).Should().Be((0, 12));
            lca0.Lca(6, 4).Should().Be((0, 13)); lca0.Lca(6, 5).Should().Be((2, 11)); lca0.Lca(6, 6).Should().Be((6, 0)); lca0.Lca(6, 7).Should().Be((0, 19));
            lca0.Lca(7, 0).Should().Be((0, 11)); lca0.Lca(7, 1).Should().Be((1, 10)); lca0.Lca(7, 2).Should().Be((0, 13)); lca0.Lca(7, 3).Should().Be((3, 7));
            lca0.Lca(7, 4).Should().Be((1, 14)); lca0.Lca(7, 5).Should().Be((0, 18)); lca0.Lca(7, 6).Should().Be((0, 19)); lca0.Lca(7, 7).Should().Be((7, 0));

            lca0[0, 0].Should().Be((0, 0)); lca0[0, 1].Should().Be((0, 1)); lca0[0, 2].Should().Be((0, 2)); lca0[0, 3].Should().Be((0, 4));
            lca0[0, 4].Should().Be((0, 5)); lca0[0, 5].Should().Be((0, 7)); lca0[0, 6].Should().Be((0, 8)); lca0[0, 7].Should().Be((0, 11));
            lca0[1, 0].Should().Be((0, 1)); lca0[1, 1].Should().Be((1, 0)); lca0[1, 2].Should().Be((0, 3)); lca0[1, 3].Should().Be((1, 3));
            lca0[1, 4].Should().Be((1, 4)); lca0[1, 5].Should().Be((0, 8)); lca0[1, 6].Should().Be((0, 9)); lca0[1, 7].Should().Be((1, 10));
            lca0[2, 0].Should().Be((0, 2)); lca0[2, 1].Should().Be((0, 3)); lca0[2, 2].Should().Be((2, 0)); lca0[2, 3].Should().Be((0, 6));
            lca0[2, 4].Should().Be((0, 7)); lca0[2, 5].Should().Be((2, 5)); lca0[2, 6].Should().Be((2, 6)); lca0[2, 7].Should().Be((0, 13));
            lca0[3, 0].Should().Be((0, 4)); lca0[3, 1].Should().Be((1, 3)); lca0[3, 2].Should().Be((0, 6)); lca0[3, 3].Should().Be((3, 0));
            lca0[3, 4].Should().Be((1, 7)); lca0[3, 5].Should().Be((0, 11)); lca0[3, 6].Should().Be((0, 12)); lca0[3, 7].Should().Be((3, 7));
            lca0[4, 0].Should().Be((0, 5)); lca0[4, 1].Should().Be((1, 4)); lca0[4, 2].Should().Be((0, 7)); lca0[4, 3].Should().Be((1, 7));
            lca0[4, 4].Should().Be((4, 0)); lca0[4, 5].Should().Be((0, 12)); lca0[4, 6].Should().Be((0, 13)); lca0[4, 7].Should().Be((1, 14));
            lca0[5, 0].Should().Be((0, 7)); lca0[5, 1].Should().Be((0, 8)); lca0[5, 2].Should().Be((2, 5)); lca0[5, 3].Should().Be((0, 11));
            lca0[5, 4].Should().Be((0, 12)); lca0[5, 5].Should().Be((5, 0)); lca0[5, 6].Should().Be((2, 11)); lca0[5, 7].Should().Be((0, 18));
            lca0[6, 0].Should().Be((0, 8)); lca0[6, 1].Should().Be((0, 9)); lca0[6, 2].Should().Be((2, 6)); lca0[6, 3].Should().Be((0, 12));
            lca0[6, 4].Should().Be((0, 13)); lca0[6, 5].Should().Be((2, 11)); lca0[6, 6].Should().Be((6, 0)); lca0[6, 7].Should().Be((0, 19));
            lca0[7, 0].Should().Be((0, 11)); lca0[7, 1].Should().Be((1, 10)); lca0[7, 2].Should().Be((0, 13)); lca0[7, 3].Should().Be((3, 7));
            lca0[7, 4].Should().Be((1, 14)); lca0[7, 5].Should().Be((0, 18)); lca0[7, 6].Should().Be((0, 19)); lca0[7, 7].Should().Be((7, 0));

            lca0.ChildOfLca(0, 0).Should().Be((0, 0)); lca0.ChildOfLca(0, 1).Should().Be((0, 1)); lca0.ChildOfLca(0, 2).Should().Be((0, 2)); lca0.ChildOfLca(0, 3).Should().Be((0, 1));
            lca0.ChildOfLca(0, 4).Should().Be((0, 1)); lca0.ChildOfLca(0, 5).Should().Be((0, 2)); lca0.ChildOfLca(0, 6).Should().Be((0, 2)); lca0.ChildOfLca(0, 7).Should().Be((0, 1));
            lca0.ChildOfLca(1, 0).Should().Be((1, 0)); lca0.ChildOfLca(1, 1).Should().Be((1, 1)); lca0.ChildOfLca(1, 2).Should().Be((1, 2)); lca0.ChildOfLca(1, 3).Should().Be((1, 3));
            lca0.ChildOfLca(1, 4).Should().Be((1, 4)); lca0.ChildOfLca(1, 5).Should().Be((1, 2)); lca0.ChildOfLca(1, 6).Should().Be((1, 2)); lca0.ChildOfLca(1, 7).Should().Be((1, 3));
            lca0.ChildOfLca(2, 0).Should().Be((2, 0)); lca0.ChildOfLca(2, 1).Should().Be((2, 1)); lca0.ChildOfLca(2, 2).Should().Be((2, 2)); lca0.ChildOfLca(2, 3).Should().Be((2, 1));
            lca0.ChildOfLca(2, 4).Should().Be((2, 1)); lca0.ChildOfLca(2, 5).Should().Be((2, 5)); lca0.ChildOfLca(2, 6).Should().Be((2, 6)); lca0.ChildOfLca(2, 7).Should().Be((2, 1));
            lca0.ChildOfLca(3, 0).Should().Be((1, 0)); lca0.ChildOfLca(3, 1).Should().Be((3, 1)); lca0.ChildOfLca(1, 2).Should().Be((1, 2)); lca0.ChildOfLca(3, 3).Should().Be((3, 3));
            lca0.ChildOfLca(3, 4).Should().Be((3, 4)); lca0.ChildOfLca(3, 5).Should().Be((1, 2)); lca0.ChildOfLca(3, 6).Should().Be((1, 2)); lca0.ChildOfLca(3, 7).Should().Be((3, 7));
            lca0.ChildOfLca(4, 0).Should().Be((1, 0)); lca0.ChildOfLca(4, 1).Should().Be((4, 1)); lca0.ChildOfLca(4, 2).Should().Be((1, 2)); lca0.ChildOfLca(4, 3).Should().Be((4, 3));
            lca0.ChildOfLca(4, 4).Should().Be((4, 4)); lca0.ChildOfLca(4, 5).Should().Be((1, 2)); lca0.ChildOfLca(4, 6).Should().Be((1, 2)); lca0.ChildOfLca(4, 7).Should().Be((4, 3));
            lca0.ChildOfLca(5, 0).Should().Be((2, 0)); lca0.ChildOfLca(5, 1).Should().Be((2, 1)); lca0.ChildOfLca(5, 2).Should().Be((5, 2)); lca0.ChildOfLca(5, 3).Should().Be((2, 1));
            lca0.ChildOfLca(5, 4).Should().Be((2, 1)); lca0.ChildOfLca(5, 5).Should().Be((5, 5)); lca0.ChildOfLca(5, 6).Should().Be((5, 6)); lca0.ChildOfLca(5, 7).Should().Be((2, 1));
            lca0.ChildOfLca(6, 0).Should().Be((2, 0)); lca0.ChildOfLca(6, 1).Should().Be((2, 1)); lca0.ChildOfLca(6, 2).Should().Be((6, 2)); lca0.ChildOfLca(6, 3).Should().Be((2, 1));
            lca0.ChildOfLca(6, 4).Should().Be((2, 1)); lca0.ChildOfLca(6, 5).Should().Be((6, 5)); lca0.ChildOfLca(6, 6).Should().Be((6, 6)); lca0.ChildOfLca(6, 7).Should().Be((2, 1));
            lca0.ChildOfLca(7, 0).Should().Be((1, 0)); lca0.ChildOfLca(7, 1).Should().Be((3, 1)); lca0.ChildOfLca(7, 2).Should().Be((1, 2)); lca0.ChildOfLca(7, 3).Should().Be((7, 3));
            lca0.ChildOfLca(7, 4).Should().Be((3, 4)); lca0.ChildOfLca(7, 5).Should().Be((1, 2)); lca0.ChildOfLca(7, 6).Should().Be((1, 2)); lca0.ChildOfLca(7, 7).Should().Be((7, 7));
        }
    }
}
