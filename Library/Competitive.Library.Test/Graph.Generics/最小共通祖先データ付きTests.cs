using AtCoder;

namespace Kzrnm.Competitive.Testing.Graph;

public class 最小共通祖先データ付きTests
{
    readonly struct TOp : ISegtreeOperator<int>
    {
        public int Identity => 0;
        public int Operate(int x, int y) => x + y;
    }
    [Test, MultipleAssertions]
    public async Task 重み付きグラフ()
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
        await lca0.Lca(0, 0).Should().BeEqualTo((0, 0)); await lca0.Lca(0, 1).Should().BeEqualTo((0, 1)); await lca0.Lca(0, 2).Should().BeEqualTo((0, 2)); await lca0.Lca(0, 3).Should().BeEqualTo((0, 4));
        await lca0.Lca(0, 4).Should().BeEqualTo((0, 5)); await lca0.Lca(0, 5).Should().BeEqualTo((0, 7)); await lca0.Lca(0, 6).Should().BeEqualTo((0, 8)); await lca0.Lca(0, 7).Should().BeEqualTo((0, 11));
        await lca0.Lca(1, 0).Should().BeEqualTo((0, 1)); await lca0.Lca(1, 1).Should().BeEqualTo((1, 0)); await lca0.Lca(1, 2).Should().BeEqualTo((0, 3)); await lca0.Lca(1, 3).Should().BeEqualTo((1, 3));
        await lca0.Lca(1, 4).Should().BeEqualTo((1, 4)); await lca0.Lca(1, 5).Should().BeEqualTo((0, 8)); await lca0.Lca(1, 6).Should().BeEqualTo((0, 9)); await lca0.Lca(1, 7).Should().BeEqualTo((1, 10));
        await lca0.Lca(2, 0).Should().BeEqualTo((0, 2)); await lca0.Lca(2, 1).Should().BeEqualTo((0, 3)); await lca0.Lca(2, 2).Should().BeEqualTo((2, 0)); await lca0.Lca(2, 3).Should().BeEqualTo((0, 6));
        await lca0.Lca(2, 4).Should().BeEqualTo((0, 7)); await lca0.Lca(2, 5).Should().BeEqualTo((2, 5)); await lca0.Lca(2, 6).Should().BeEqualTo((2, 6)); await lca0.Lca(2, 7).Should().BeEqualTo((0, 13));
        await lca0.Lca(3, 0).Should().BeEqualTo((0, 4)); await lca0.Lca(3, 1).Should().BeEqualTo((1, 3)); await lca0.Lca(3, 2).Should().BeEqualTo((0, 6)); await lca0.Lca(3, 3).Should().BeEqualTo((3, 0));
        await lca0.Lca(3, 4).Should().BeEqualTo((1, 7)); await lca0.Lca(3, 5).Should().BeEqualTo((0, 11)); await lca0.Lca(3, 6).Should().BeEqualTo((0, 12)); await lca0.Lca(3, 7).Should().BeEqualTo((3, 7));
        await lca0.Lca(4, 0).Should().BeEqualTo((0, 5)); await lca0.Lca(4, 1).Should().BeEqualTo((1, 4)); await lca0.Lca(4, 2).Should().BeEqualTo((0, 7)); await lca0.Lca(4, 3).Should().BeEqualTo((1, 7));
        await lca0.Lca(4, 4).Should().BeEqualTo((4, 0)); await lca0.Lca(4, 5).Should().BeEqualTo((0, 12)); await lca0.Lca(4, 6).Should().BeEqualTo((0, 13)); await lca0.Lca(4, 7).Should().BeEqualTo((1, 14));
        await lca0.Lca(5, 0).Should().BeEqualTo((0, 7)); await lca0.Lca(5, 1).Should().BeEqualTo((0, 8)); await lca0.Lca(5, 2).Should().BeEqualTo((2, 5)); await lca0.Lca(5, 3).Should().BeEqualTo((0, 11));
        await lca0.Lca(5, 4).Should().BeEqualTo((0, 12)); await lca0.Lca(5, 5).Should().BeEqualTo((5, 0)); await lca0.Lca(5, 6).Should().BeEqualTo((2, 11)); await lca0.Lca(5, 7).Should().BeEqualTo((0, 18));
        await lca0.Lca(6, 0).Should().BeEqualTo((0, 8)); await lca0.Lca(6, 1).Should().BeEqualTo((0, 9)); await lca0.Lca(6, 2).Should().BeEqualTo((2, 6)); await lca0.Lca(6, 3).Should().BeEqualTo((0, 12));
        await lca0.Lca(6, 4).Should().BeEqualTo((0, 13)); await lca0.Lca(6, 5).Should().BeEqualTo((2, 11)); await lca0.Lca(6, 6).Should().BeEqualTo((6, 0)); await lca0.Lca(6, 7).Should().BeEqualTo((0, 19));
        await lca0.Lca(7, 0).Should().BeEqualTo((0, 11)); await lca0.Lca(7, 1).Should().BeEqualTo((1, 10)); await lca0.Lca(7, 2).Should().BeEqualTo((0, 13)); await lca0.Lca(7, 3).Should().BeEqualTo((3, 7));
        await lca0.Lca(7, 4).Should().BeEqualTo((1, 14)); await lca0.Lca(7, 5).Should().BeEqualTo((0, 18)); await lca0.Lca(7, 6).Should().BeEqualTo((0, 19)); await lca0.Lca(7, 7).Should().BeEqualTo((7, 0));

        await lca0[0, 0].Should().BeEqualTo((0, 0)); await lca0[0, 1].Should().BeEqualTo((0, 1)); await lca0[0, 2].Should().BeEqualTo((0, 2)); await lca0[0, 3].Should().BeEqualTo((0, 4));
        await lca0[0, 4].Should().BeEqualTo((0, 5)); await lca0[0, 5].Should().BeEqualTo((0, 7)); await lca0[0, 6].Should().BeEqualTo((0, 8)); await lca0[0, 7].Should().BeEqualTo((0, 11));
        await lca0[1, 0].Should().BeEqualTo((0, 1)); await lca0[1, 1].Should().BeEqualTo((1, 0)); await lca0[1, 2].Should().BeEqualTo((0, 3)); await lca0[1, 3].Should().BeEqualTo((1, 3));
        await lca0[1, 4].Should().BeEqualTo((1, 4)); await lca0[1, 5].Should().BeEqualTo((0, 8)); await lca0[1, 6].Should().BeEqualTo((0, 9)); await lca0[1, 7].Should().BeEqualTo((1, 10));
        await lca0[2, 0].Should().BeEqualTo((0, 2)); await lca0[2, 1].Should().BeEqualTo((0, 3)); await lca0[2, 2].Should().BeEqualTo((2, 0)); await lca0[2, 3].Should().BeEqualTo((0, 6));
        await lca0[2, 4].Should().BeEqualTo((0, 7)); await lca0[2, 5].Should().BeEqualTo((2, 5)); await lca0[2, 6].Should().BeEqualTo((2, 6)); await lca0[2, 7].Should().BeEqualTo((0, 13));
        await lca0[3, 0].Should().BeEqualTo((0, 4)); await lca0[3, 1].Should().BeEqualTo((1, 3)); await lca0[3, 2].Should().BeEqualTo((0, 6)); await lca0[3, 3].Should().BeEqualTo((3, 0));
        await lca0[3, 4].Should().BeEqualTo((1, 7)); await lca0[3, 5].Should().BeEqualTo((0, 11)); await lca0[3, 6].Should().BeEqualTo((0, 12)); await lca0[3, 7].Should().BeEqualTo((3, 7));
        await lca0[4, 0].Should().BeEqualTo((0, 5)); await lca0[4, 1].Should().BeEqualTo((1, 4)); await lca0[4, 2].Should().BeEqualTo((0, 7)); await lca0[4, 3].Should().BeEqualTo((1, 7));
        await lca0[4, 4].Should().BeEqualTo((4, 0)); await lca0[4, 5].Should().BeEqualTo((0, 12)); await lca0[4, 6].Should().BeEqualTo((0, 13)); await lca0[4, 7].Should().BeEqualTo((1, 14));
        await lca0[5, 0].Should().BeEqualTo((0, 7)); await lca0[5, 1].Should().BeEqualTo((0, 8)); await lca0[5, 2].Should().BeEqualTo((2, 5)); await lca0[5, 3].Should().BeEqualTo((0, 11));
        await lca0[5, 4].Should().BeEqualTo((0, 12)); await lca0[5, 5].Should().BeEqualTo((5, 0)); await lca0[5, 6].Should().BeEqualTo((2, 11)); await lca0[5, 7].Should().BeEqualTo((0, 18));
        await lca0[6, 0].Should().BeEqualTo((0, 8)); await lca0[6, 1].Should().BeEqualTo((0, 9)); await lca0[6, 2].Should().BeEqualTo((2, 6)); await lca0[6, 3].Should().BeEqualTo((0, 12));
        await lca0[6, 4].Should().BeEqualTo((0, 13)); await lca0[6, 5].Should().BeEqualTo((2, 11)); await lca0[6, 6].Should().BeEqualTo((6, 0)); await lca0[6, 7].Should().BeEqualTo((0, 19));
        await lca0[7, 0].Should().BeEqualTo((0, 11)); await lca0[7, 1].Should().BeEqualTo((1, 10)); await lca0[7, 2].Should().BeEqualTo((0, 13)); await lca0[7, 3].Should().BeEqualTo((3, 7));
        await lca0[7, 4].Should().BeEqualTo((1, 14)); await lca0[7, 5].Should().BeEqualTo((0, 18)); await lca0[7, 6].Should().BeEqualTo((0, 19)); await lca0[7, 7].Should().BeEqualTo((7, 0));

        await lca0.ChildOfLca(0, 0).Should().BeEqualTo((0, 0)); await lca0.ChildOfLca(0, 1).Should().BeEqualTo((0, 1)); await lca0.ChildOfLca(0, 2).Should().BeEqualTo((0, 2)); await lca0.ChildOfLca(0, 3).Should().BeEqualTo((0, 1));
        await lca0.ChildOfLca(0, 4).Should().BeEqualTo((0, 1)); await lca0.ChildOfLca(0, 5).Should().BeEqualTo((0, 2)); await lca0.ChildOfLca(0, 6).Should().BeEqualTo((0, 2)); await lca0.ChildOfLca(0, 7).Should().BeEqualTo((0, 1));
        await lca0.ChildOfLca(1, 0).Should().BeEqualTo((1, 0)); await lca0.ChildOfLca(1, 1).Should().BeEqualTo((1, 1)); await lca0.ChildOfLca(1, 2).Should().BeEqualTo((1, 2)); await lca0.ChildOfLca(1, 3).Should().BeEqualTo((1, 3));
        await lca0.ChildOfLca(1, 4).Should().BeEqualTo((1, 4)); await lca0.ChildOfLca(1, 5).Should().BeEqualTo((1, 2)); await lca0.ChildOfLca(1, 6).Should().BeEqualTo((1, 2)); await lca0.ChildOfLca(1, 7).Should().BeEqualTo((1, 3));
        await lca0.ChildOfLca(2, 0).Should().BeEqualTo((2, 0)); await lca0.ChildOfLca(2, 1).Should().BeEqualTo((2, 1)); await lca0.ChildOfLca(2, 2).Should().BeEqualTo((2, 2)); await lca0.ChildOfLca(2, 3).Should().BeEqualTo((2, 1));
        await lca0.ChildOfLca(2, 4).Should().BeEqualTo((2, 1)); await lca0.ChildOfLca(2, 5).Should().BeEqualTo((2, 5)); await lca0.ChildOfLca(2, 6).Should().BeEqualTo((2, 6)); await lca0.ChildOfLca(2, 7).Should().BeEqualTo((2, 1));
        await lca0.ChildOfLca(3, 0).Should().BeEqualTo((1, 0)); await lca0.ChildOfLca(3, 1).Should().BeEqualTo((3, 1)); await lca0.ChildOfLca(1, 2).Should().BeEqualTo((1, 2)); await lca0.ChildOfLca(3, 3).Should().BeEqualTo((3, 3));
        await lca0.ChildOfLca(3, 4).Should().BeEqualTo((3, 4)); await lca0.ChildOfLca(3, 5).Should().BeEqualTo((1, 2)); await lca0.ChildOfLca(3, 6).Should().BeEqualTo((1, 2)); await lca0.ChildOfLca(3, 7).Should().BeEqualTo((3, 7));
        await lca0.ChildOfLca(4, 0).Should().BeEqualTo((1, 0)); await lca0.ChildOfLca(4, 1).Should().BeEqualTo((4, 1)); await lca0.ChildOfLca(4, 2).Should().BeEqualTo((1, 2)); await lca0.ChildOfLca(4, 3).Should().BeEqualTo((4, 3));
        await lca0.ChildOfLca(4, 4).Should().BeEqualTo((4, 4)); await lca0.ChildOfLca(4, 5).Should().BeEqualTo((1, 2)); await lca0.ChildOfLca(4, 6).Should().BeEqualTo((1, 2)); await lca0.ChildOfLca(4, 7).Should().BeEqualTo((4, 3));
        await lca0.ChildOfLca(5, 0).Should().BeEqualTo((2, 0)); await lca0.ChildOfLca(5, 1).Should().BeEqualTo((2, 1)); await lca0.ChildOfLca(5, 2).Should().BeEqualTo((5, 2)); await lca0.ChildOfLca(5, 3).Should().BeEqualTo((2, 1));
        await lca0.ChildOfLca(5, 4).Should().BeEqualTo((2, 1)); await lca0.ChildOfLca(5, 5).Should().BeEqualTo((5, 5)); await lca0.ChildOfLca(5, 6).Should().BeEqualTo((5, 6)); await lca0.ChildOfLca(5, 7).Should().BeEqualTo((2, 1));
        await lca0.ChildOfLca(6, 0).Should().BeEqualTo((2, 0)); await lca0.ChildOfLca(6, 1).Should().BeEqualTo((2, 1)); await lca0.ChildOfLca(6, 2).Should().BeEqualTo((6, 2)); await lca0.ChildOfLca(6, 3).Should().BeEqualTo((2, 1));
        await lca0.ChildOfLca(6, 4).Should().BeEqualTo((2, 1)); await lca0.ChildOfLca(6, 5).Should().BeEqualTo((6, 5)); await lca0.ChildOfLca(6, 6).Should().BeEqualTo((6, 6)); await lca0.ChildOfLca(6, 7).Should().BeEqualTo((2, 1));
        await lca0.ChildOfLca(7, 0).Should().BeEqualTo((1, 0)); await lca0.ChildOfLca(7, 1).Should().BeEqualTo((3, 1)); await lca0.ChildOfLca(7, 2).Should().BeEqualTo((1, 2)); await lca0.ChildOfLca(7, 3).Should().BeEqualTo((7, 3));
        await lca0.ChildOfLca(7, 4).Should().BeEqualTo((3, 4)); await lca0.ChildOfLca(7, 5).Should().BeEqualTo((1, 2)); await lca0.ChildOfLca(7, 6).Should().BeEqualTo((1, 2)); await lca0.ChildOfLca(7, 7).Should().BeEqualTo((7, 7));

        await lca0.Ascend(0, 0).Should().BeEqualTo((0, 0)); await lca0.Ascend(0, 1).Should().BeEqualTo((-1, 0));
        await lca0.Ascend(1, 0).Should().BeEqualTo((1, 0)); await lca0.Ascend(1, 1).Should().BeEqualTo((0, 1)); await lca0.Ascend(1, 2).Should().BeEqualTo((-1, 0));
        await lca0.Ascend(2, 0).Should().BeEqualTo((2, 0)); await lca0.Ascend(2, 1).Should().BeEqualTo((0, 2)); await lca0.Ascend(2, 2).Should().BeEqualTo((-1, 0));
        await lca0.Ascend(3, 0).Should().BeEqualTo((3, 0)); await lca0.Ascend(3, 1).Should().BeEqualTo((1, 3)); await lca0.Ascend(3, 2).Should().BeEqualTo((0, 4)); await lca0.Ascend(3, 3).Should().BeEqualTo((-1, 0));
        await lca0.Ascend(4, 0).Should().BeEqualTo((4, 0)); await lca0.Ascend(4, 1).Should().BeEqualTo((1, 4)); await lca0.Ascend(4, 2).Should().BeEqualTo((0, 5)); await lca0.Ascend(4, 3).Should().BeEqualTo((-1, 0));
        await lca0.Ascend(5, 0).Should().BeEqualTo((5, 0)); await lca0.Ascend(5, 1).Should().BeEqualTo((2, 5)); await lca0.Ascend(5, 2).Should().BeEqualTo((0, 7)); await lca0.Ascend(5, 3).Should().BeEqualTo((-1, 0));
        await lca0.Ascend(6, 0).Should().BeEqualTo((6, 0)); await lca0.Ascend(6, 1).Should().BeEqualTo((2, 6)); await lca0.Ascend(6, 2).Should().BeEqualTo((0, 8)); await lca0.Ascend(6, 3).Should().BeEqualTo((-1, 0));
        await lca0.Ascend(7, 0).Should().BeEqualTo((7, 0)); await lca0.Ascend(7, 1).Should().BeEqualTo((3, 7)); await lca0.Ascend(7, 2).Should().BeEqualTo((1, 10)); await lca0.Ascend(7, 3).Should().BeEqualTo((0, 11)); await lca0.Ascend(7, 4).Should().BeEqualTo((-1, 0));
    }
}