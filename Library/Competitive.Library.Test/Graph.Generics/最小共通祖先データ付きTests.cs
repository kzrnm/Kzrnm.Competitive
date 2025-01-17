using AtCoder;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Graph
{
    public class 最小共通祖先データ付きTests
    {
        readonly struct TOp : ISegtreeOperator<int>
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
            var lca0 = tree.LowestCommonAncestorWithDataBuilder().Build<int, TOp>(tree.AsArray().Select(n => n.Parent.Value).ToArray());
            lca0.Lca(0, 0).ShouldBe((0, 0)); lca0.Lca(0, 1).ShouldBe((0, 1)); lca0.Lca(0, 2).ShouldBe((0, 2)); lca0.Lca(0, 3).ShouldBe((0, 4));
            lca0.Lca(0, 4).ShouldBe((0, 5)); lca0.Lca(0, 5).ShouldBe((0, 7)); lca0.Lca(0, 6).ShouldBe((0, 8)); lca0.Lca(0, 7).ShouldBe((0, 11));
            lca0.Lca(1, 0).ShouldBe((0, 1)); lca0.Lca(1, 1).ShouldBe((1, 0)); lca0.Lca(1, 2).ShouldBe((0, 3)); lca0.Lca(1, 3).ShouldBe((1, 3));
            lca0.Lca(1, 4).ShouldBe((1, 4)); lca0.Lca(1, 5).ShouldBe((0, 8)); lca0.Lca(1, 6).ShouldBe((0, 9)); lca0.Lca(1, 7).ShouldBe((1, 10));
            lca0.Lca(2, 0).ShouldBe((0, 2)); lca0.Lca(2, 1).ShouldBe((0, 3)); lca0.Lca(2, 2).ShouldBe((2, 0)); lca0.Lca(2, 3).ShouldBe((0, 6));
            lca0.Lca(2, 4).ShouldBe((0, 7)); lca0.Lca(2, 5).ShouldBe((2, 5)); lca0.Lca(2, 6).ShouldBe((2, 6)); lca0.Lca(2, 7).ShouldBe((0, 13));
            lca0.Lca(3, 0).ShouldBe((0, 4)); lca0.Lca(3, 1).ShouldBe((1, 3)); lca0.Lca(3, 2).ShouldBe((0, 6)); lca0.Lca(3, 3).ShouldBe((3, 0));
            lca0.Lca(3, 4).ShouldBe((1, 7)); lca0.Lca(3, 5).ShouldBe((0, 11)); lca0.Lca(3, 6).ShouldBe((0, 12)); lca0.Lca(3, 7).ShouldBe((3, 7));
            lca0.Lca(4, 0).ShouldBe((0, 5)); lca0.Lca(4, 1).ShouldBe((1, 4)); lca0.Lca(4, 2).ShouldBe((0, 7)); lca0.Lca(4, 3).ShouldBe((1, 7));
            lca0.Lca(4, 4).ShouldBe((4, 0)); lca0.Lca(4, 5).ShouldBe((0, 12)); lca0.Lca(4, 6).ShouldBe((0, 13)); lca0.Lca(4, 7).ShouldBe((1, 14));
            lca0.Lca(5, 0).ShouldBe((0, 7)); lca0.Lca(5, 1).ShouldBe((0, 8)); lca0.Lca(5, 2).ShouldBe((2, 5)); lca0.Lca(5, 3).ShouldBe((0, 11));
            lca0.Lca(5, 4).ShouldBe((0, 12)); lca0.Lca(5, 5).ShouldBe((5, 0)); lca0.Lca(5, 6).ShouldBe((2, 11)); lca0.Lca(5, 7).ShouldBe((0, 18));
            lca0.Lca(6, 0).ShouldBe((0, 8)); lca0.Lca(6, 1).ShouldBe((0, 9)); lca0.Lca(6, 2).ShouldBe((2, 6)); lca0.Lca(6, 3).ShouldBe((0, 12));
            lca0.Lca(6, 4).ShouldBe((0, 13)); lca0.Lca(6, 5).ShouldBe((2, 11)); lca0.Lca(6, 6).ShouldBe((6, 0)); lca0.Lca(6, 7).ShouldBe((0, 19));
            lca0.Lca(7, 0).ShouldBe((0, 11)); lca0.Lca(7, 1).ShouldBe((1, 10)); lca0.Lca(7, 2).ShouldBe((0, 13)); lca0.Lca(7, 3).ShouldBe((3, 7));
            lca0.Lca(7, 4).ShouldBe((1, 14)); lca0.Lca(7, 5).ShouldBe((0, 18)); lca0.Lca(7, 6).ShouldBe((0, 19)); lca0.Lca(7, 7).ShouldBe((7, 0));

            lca0[0, 0].ShouldBe((0, 0)); lca0[0, 1].ShouldBe((0, 1)); lca0[0, 2].ShouldBe((0, 2)); lca0[0, 3].ShouldBe((0, 4));
            lca0[0, 4].ShouldBe((0, 5)); lca0[0, 5].ShouldBe((0, 7)); lca0[0, 6].ShouldBe((0, 8)); lca0[0, 7].ShouldBe((0, 11));
            lca0[1, 0].ShouldBe((0, 1)); lca0[1, 1].ShouldBe((1, 0)); lca0[1, 2].ShouldBe((0, 3)); lca0[1, 3].ShouldBe((1, 3));
            lca0[1, 4].ShouldBe((1, 4)); lca0[1, 5].ShouldBe((0, 8)); lca0[1, 6].ShouldBe((0, 9)); lca0[1, 7].ShouldBe((1, 10));
            lca0[2, 0].ShouldBe((0, 2)); lca0[2, 1].ShouldBe((0, 3)); lca0[2, 2].ShouldBe((2, 0)); lca0[2, 3].ShouldBe((0, 6));
            lca0[2, 4].ShouldBe((0, 7)); lca0[2, 5].ShouldBe((2, 5)); lca0[2, 6].ShouldBe((2, 6)); lca0[2, 7].ShouldBe((0, 13));
            lca0[3, 0].ShouldBe((0, 4)); lca0[3, 1].ShouldBe((1, 3)); lca0[3, 2].ShouldBe((0, 6)); lca0[3, 3].ShouldBe((3, 0));
            lca0[3, 4].ShouldBe((1, 7)); lca0[3, 5].ShouldBe((0, 11)); lca0[3, 6].ShouldBe((0, 12)); lca0[3, 7].ShouldBe((3, 7));
            lca0[4, 0].ShouldBe((0, 5)); lca0[4, 1].ShouldBe((1, 4)); lca0[4, 2].ShouldBe((0, 7)); lca0[4, 3].ShouldBe((1, 7));
            lca0[4, 4].ShouldBe((4, 0)); lca0[4, 5].ShouldBe((0, 12)); lca0[4, 6].ShouldBe((0, 13)); lca0[4, 7].ShouldBe((1, 14));
            lca0[5, 0].ShouldBe((0, 7)); lca0[5, 1].ShouldBe((0, 8)); lca0[5, 2].ShouldBe((2, 5)); lca0[5, 3].ShouldBe((0, 11));
            lca0[5, 4].ShouldBe((0, 12)); lca0[5, 5].ShouldBe((5, 0)); lca0[5, 6].ShouldBe((2, 11)); lca0[5, 7].ShouldBe((0, 18));
            lca0[6, 0].ShouldBe((0, 8)); lca0[6, 1].ShouldBe((0, 9)); lca0[6, 2].ShouldBe((2, 6)); lca0[6, 3].ShouldBe((0, 12));
            lca0[6, 4].ShouldBe((0, 13)); lca0[6, 5].ShouldBe((2, 11)); lca0[6, 6].ShouldBe((6, 0)); lca0[6, 7].ShouldBe((0, 19));
            lca0[7, 0].ShouldBe((0, 11)); lca0[7, 1].ShouldBe((1, 10)); lca0[7, 2].ShouldBe((0, 13)); lca0[7, 3].ShouldBe((3, 7));
            lca0[7, 4].ShouldBe((1, 14)); lca0[7, 5].ShouldBe((0, 18)); lca0[7, 6].ShouldBe((0, 19)); lca0[7, 7].ShouldBe((7, 0));

            lca0.ChildOfLca(0, 0).ShouldBe((0, 0)); lca0.ChildOfLca(0, 1).ShouldBe((0, 1)); lca0.ChildOfLca(0, 2).ShouldBe((0, 2)); lca0.ChildOfLca(0, 3).ShouldBe((0, 1));
            lca0.ChildOfLca(0, 4).ShouldBe((0, 1)); lca0.ChildOfLca(0, 5).ShouldBe((0, 2)); lca0.ChildOfLca(0, 6).ShouldBe((0, 2)); lca0.ChildOfLca(0, 7).ShouldBe((0, 1));
            lca0.ChildOfLca(1, 0).ShouldBe((1, 0)); lca0.ChildOfLca(1, 1).ShouldBe((1, 1)); lca0.ChildOfLca(1, 2).ShouldBe((1, 2)); lca0.ChildOfLca(1, 3).ShouldBe((1, 3));
            lca0.ChildOfLca(1, 4).ShouldBe((1, 4)); lca0.ChildOfLca(1, 5).ShouldBe((1, 2)); lca0.ChildOfLca(1, 6).ShouldBe((1, 2)); lca0.ChildOfLca(1, 7).ShouldBe((1, 3));
            lca0.ChildOfLca(2, 0).ShouldBe((2, 0)); lca0.ChildOfLca(2, 1).ShouldBe((2, 1)); lca0.ChildOfLca(2, 2).ShouldBe((2, 2)); lca0.ChildOfLca(2, 3).ShouldBe((2, 1));
            lca0.ChildOfLca(2, 4).ShouldBe((2, 1)); lca0.ChildOfLca(2, 5).ShouldBe((2, 5)); lca0.ChildOfLca(2, 6).ShouldBe((2, 6)); lca0.ChildOfLca(2, 7).ShouldBe((2, 1));
            lca0.ChildOfLca(3, 0).ShouldBe((1, 0)); lca0.ChildOfLca(3, 1).ShouldBe((3, 1)); lca0.ChildOfLca(1, 2).ShouldBe((1, 2)); lca0.ChildOfLca(3, 3).ShouldBe((3, 3));
            lca0.ChildOfLca(3, 4).ShouldBe((3, 4)); lca0.ChildOfLca(3, 5).ShouldBe((1, 2)); lca0.ChildOfLca(3, 6).ShouldBe((1, 2)); lca0.ChildOfLca(3, 7).ShouldBe((3, 7));
            lca0.ChildOfLca(4, 0).ShouldBe((1, 0)); lca0.ChildOfLca(4, 1).ShouldBe((4, 1)); lca0.ChildOfLca(4, 2).ShouldBe((1, 2)); lca0.ChildOfLca(4, 3).ShouldBe((4, 3));
            lca0.ChildOfLca(4, 4).ShouldBe((4, 4)); lca0.ChildOfLca(4, 5).ShouldBe((1, 2)); lca0.ChildOfLca(4, 6).ShouldBe((1, 2)); lca0.ChildOfLca(4, 7).ShouldBe((4, 3));
            lca0.ChildOfLca(5, 0).ShouldBe((2, 0)); lca0.ChildOfLca(5, 1).ShouldBe((2, 1)); lca0.ChildOfLca(5, 2).ShouldBe((5, 2)); lca0.ChildOfLca(5, 3).ShouldBe((2, 1));
            lca0.ChildOfLca(5, 4).ShouldBe((2, 1)); lca0.ChildOfLca(5, 5).ShouldBe((5, 5)); lca0.ChildOfLca(5, 6).ShouldBe((5, 6)); lca0.ChildOfLca(5, 7).ShouldBe((2, 1));
            lca0.ChildOfLca(6, 0).ShouldBe((2, 0)); lca0.ChildOfLca(6, 1).ShouldBe((2, 1)); lca0.ChildOfLca(6, 2).ShouldBe((6, 2)); lca0.ChildOfLca(6, 3).ShouldBe((2, 1));
            lca0.ChildOfLca(6, 4).ShouldBe((2, 1)); lca0.ChildOfLca(6, 5).ShouldBe((6, 5)); lca0.ChildOfLca(6, 6).ShouldBe((6, 6)); lca0.ChildOfLca(6, 7).ShouldBe((2, 1));
            lca0.ChildOfLca(7, 0).ShouldBe((1, 0)); lca0.ChildOfLca(7, 1).ShouldBe((3, 1)); lca0.ChildOfLca(7, 2).ShouldBe((1, 2)); lca0.ChildOfLca(7, 3).ShouldBe((7, 3));
            lca0.ChildOfLca(7, 4).ShouldBe((3, 4)); lca0.ChildOfLca(7, 5).ShouldBe((1, 2)); lca0.ChildOfLca(7, 6).ShouldBe((1, 2)); lca0.ChildOfLca(7, 7).ShouldBe((7, 7));

            lca0.Ascend(0, 0).ShouldBe((0, 0)); lca0.Ascend(0, 1).ShouldBe((-1, 0));
            lca0.Ascend(1, 0).ShouldBe((1, 0)); lca0.Ascend(1, 1).ShouldBe((0, 1)); lca0.Ascend(1, 2).ShouldBe((-1, 0));
            lca0.Ascend(2, 0).ShouldBe((2, 0)); lca0.Ascend(2, 1).ShouldBe((0, 2)); lca0.Ascend(2, 2).ShouldBe((-1, 0));
            lca0.Ascend(3, 0).ShouldBe((3, 0)); lca0.Ascend(3, 1).ShouldBe((1, 3)); lca0.Ascend(3, 2).ShouldBe((0, 4)); lca0.Ascend(3, 3).ShouldBe((-1, 0));
            lca0.Ascend(4, 0).ShouldBe((4, 0)); lca0.Ascend(4, 1).ShouldBe((1, 4)); lca0.Ascend(4, 2).ShouldBe((0, 5)); lca0.Ascend(4, 3).ShouldBe((-1, 0));
            lca0.Ascend(5, 0).ShouldBe((5, 0)); lca0.Ascend(5, 1).ShouldBe((2, 5)); lca0.Ascend(5, 2).ShouldBe((0, 7)); lca0.Ascend(5, 3).ShouldBe((-1, 0));
            lca0.Ascend(6, 0).ShouldBe((6, 0)); lca0.Ascend(6, 1).ShouldBe((2, 6)); lca0.Ascend(6, 2).ShouldBe((0, 8)); lca0.Ascend(6, 3).ShouldBe((-1, 0));
            lca0.Ascend(7, 0).ShouldBe((7, 0)); lca0.Ascend(7, 1).ShouldBe((3, 7)); lca0.Ascend(7, 2).ShouldBe((1, 10)); lca0.Ascend(7, 3).ShouldBe((0, 11)); lca0.Ascend(7, 4).ShouldBe((-1, 0));
        }
    }
}
