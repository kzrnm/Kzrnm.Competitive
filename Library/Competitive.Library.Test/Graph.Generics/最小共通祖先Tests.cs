namespace Kzrnm.Competitive.Testing.Graph;

public class 最小共通祖先Tests
{
    [Test, MultipleAssertions]
    public async Task 重みなしグラフ()
    {
        var gb = new GraphBuilder(8, false);
        gb.Add(0, 1);
        gb.Add(0, 2);
        gb.Add(1, 3);
        gb.Add(1, 4);
        gb.Add(2, 5);
        gb.Add(2, 6);
        gb.Add(3, 7);

        var tree = gb.ToTree();
        await tree.HlDecomposition.LowestCommonAncestor(0, 0).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(0, 1).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(0, 2).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(0, 3).Should().BeEqualTo(0);
        await tree.HlDecomposition.LowestCommonAncestor(0, 4).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(0, 5).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(0, 6).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(0, 7).Should().BeEqualTo(0);
        await tree.HlDecomposition.LowestCommonAncestor(1, 0).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(1, 1).Should().BeEqualTo(1); await tree.HlDecomposition.LowestCommonAncestor(1, 2).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(1, 3).Should().BeEqualTo(1);
        await tree.HlDecomposition.LowestCommonAncestor(1, 4).Should().BeEqualTo(1); await tree.HlDecomposition.LowestCommonAncestor(1, 5).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(1, 6).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(1, 7).Should().BeEqualTo(1);
        await tree.HlDecomposition.LowestCommonAncestor(2, 0).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(2, 1).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(2, 2).Should().BeEqualTo(2); await tree.HlDecomposition.LowestCommonAncestor(2, 3).Should().BeEqualTo(0);
        await tree.HlDecomposition.LowestCommonAncestor(2, 4).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(2, 5).Should().BeEqualTo(2); await tree.HlDecomposition.LowestCommonAncestor(2, 6).Should().BeEqualTo(2); await tree.HlDecomposition.LowestCommonAncestor(2, 7).Should().BeEqualTo(0);
        await tree.HlDecomposition.LowestCommonAncestor(3, 0).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(3, 1).Should().BeEqualTo(1); await tree.HlDecomposition.LowestCommonAncestor(3, 2).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(3, 3).Should().BeEqualTo(3);
        await tree.HlDecomposition.LowestCommonAncestor(3, 4).Should().BeEqualTo(1); await tree.HlDecomposition.LowestCommonAncestor(3, 5).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(3, 6).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(3, 7).Should().BeEqualTo(3);
        await tree.HlDecomposition.LowestCommonAncestor(4, 0).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(4, 1).Should().BeEqualTo(1); await tree.HlDecomposition.LowestCommonAncestor(4, 2).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(4, 3).Should().BeEqualTo(1);
        await tree.HlDecomposition.LowestCommonAncestor(4, 4).Should().BeEqualTo(4); await tree.HlDecomposition.LowestCommonAncestor(4, 5).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(4, 6).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(4, 7).Should().BeEqualTo(1);
        await tree.HlDecomposition.LowestCommonAncestor(5, 0).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(5, 1).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(5, 2).Should().BeEqualTo(2); await tree.HlDecomposition.LowestCommonAncestor(5, 3).Should().BeEqualTo(0);
        await tree.HlDecomposition.LowestCommonAncestor(5, 4).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(5, 5).Should().BeEqualTo(5); await tree.HlDecomposition.LowestCommonAncestor(5, 6).Should().BeEqualTo(2); await tree.HlDecomposition.LowestCommonAncestor(5, 7).Should().BeEqualTo(0);
        await tree.HlDecomposition.LowestCommonAncestor(6, 0).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(6, 1).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(6, 2).Should().BeEqualTo(2); await tree.HlDecomposition.LowestCommonAncestor(6, 3).Should().BeEqualTo(0);
        await tree.HlDecomposition.LowestCommonAncestor(6, 4).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(6, 5).Should().BeEqualTo(2); await tree.HlDecomposition.LowestCommonAncestor(6, 6).Should().BeEqualTo(6); await tree.HlDecomposition.LowestCommonAncestor(6, 7).Should().BeEqualTo(0);
        await tree.HlDecomposition.LowestCommonAncestor(7, 0).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(7, 1).Should().BeEqualTo(1); await tree.HlDecomposition.LowestCommonAncestor(7, 2).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(7, 3).Should().BeEqualTo(3);
        await tree.HlDecomposition.LowestCommonAncestor(7, 4).Should().BeEqualTo(1); await tree.HlDecomposition.LowestCommonAncestor(7, 5).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(7, 6).Should().BeEqualTo(0); await tree.HlDecomposition.LowestCommonAncestor(7, 7).Should().BeEqualTo(7);

        var lca0 = tree.LowestCommonAncestorDoubling();
        await lca0.Lca(0, 0).Should().BeEqualTo(0); await lca0.Lca(0, 1).Should().BeEqualTo(0); await lca0.Lca(0, 2).Should().BeEqualTo(0); await lca0.Lca(0, 3).Should().BeEqualTo(0);
        await lca0.Lca(0, 4).Should().BeEqualTo(0); await lca0.Lca(0, 5).Should().BeEqualTo(0); await lca0.Lca(0, 6).Should().BeEqualTo(0); await lca0.Lca(0, 7).Should().BeEqualTo(0);
        await lca0.Lca(1, 0).Should().BeEqualTo(0); await lca0.Lca(1, 1).Should().BeEqualTo(1); await lca0.Lca(1, 2).Should().BeEqualTo(0); await lca0.Lca(1, 3).Should().BeEqualTo(1);
        await lca0.Lca(1, 4).Should().BeEqualTo(1); await lca0.Lca(1, 5).Should().BeEqualTo(0); await lca0.Lca(1, 6).Should().BeEqualTo(0); await lca0.Lca(1, 7).Should().BeEqualTo(1);
        await lca0.Lca(2, 0).Should().BeEqualTo(0); await lca0.Lca(2, 1).Should().BeEqualTo(0); await lca0.Lca(2, 2).Should().BeEqualTo(2); await lca0.Lca(2, 3).Should().BeEqualTo(0);
        await lca0.Lca(2, 4).Should().BeEqualTo(0); await lca0.Lca(2, 5).Should().BeEqualTo(2); await lca0.Lca(2, 6).Should().BeEqualTo(2); await lca0.Lca(2, 7).Should().BeEqualTo(0);
        await lca0.Lca(3, 0).Should().BeEqualTo(0); await lca0.Lca(3, 1).Should().BeEqualTo(1); await lca0.Lca(3, 2).Should().BeEqualTo(0); await lca0.Lca(3, 3).Should().BeEqualTo(3);
        await lca0.Lca(3, 4).Should().BeEqualTo(1); await lca0.Lca(3, 5).Should().BeEqualTo(0); await lca0.Lca(3, 6).Should().BeEqualTo(0); await lca0.Lca(3, 7).Should().BeEqualTo(3);
        await lca0.Lca(4, 0).Should().BeEqualTo(0); await lca0.Lca(4, 1).Should().BeEqualTo(1); await lca0.Lca(4, 2).Should().BeEqualTo(0); await lca0.Lca(4, 3).Should().BeEqualTo(1);
        await lca0.Lca(4, 4).Should().BeEqualTo(4); await lca0.Lca(4, 5).Should().BeEqualTo(0); await lca0.Lca(4, 6).Should().BeEqualTo(0); await lca0.Lca(4, 7).Should().BeEqualTo(1);
        await lca0.Lca(5, 0).Should().BeEqualTo(0); await lca0.Lca(5, 1).Should().BeEqualTo(0); await lca0.Lca(5, 2).Should().BeEqualTo(2); await lca0.Lca(5, 3).Should().BeEqualTo(0);
        await lca0.Lca(5, 4).Should().BeEqualTo(0); await lca0.Lca(5, 5).Should().BeEqualTo(5); await lca0.Lca(5, 6).Should().BeEqualTo(2); await lca0.Lca(5, 7).Should().BeEqualTo(0);
        await lca0.Lca(6, 0).Should().BeEqualTo(0); await lca0.Lca(6, 1).Should().BeEqualTo(0); await lca0.Lca(6, 2).Should().BeEqualTo(2); await lca0.Lca(6, 3).Should().BeEqualTo(0);
        await lca0.Lca(6, 4).Should().BeEqualTo(0); await lca0.Lca(6, 5).Should().BeEqualTo(2); await lca0.Lca(6, 6).Should().BeEqualTo(6); await lca0.Lca(6, 7).Should().BeEqualTo(0);
        await lca0.Lca(7, 0).Should().BeEqualTo(0); await lca0.Lca(7, 1).Should().BeEqualTo(1); await lca0.Lca(7, 2).Should().BeEqualTo(0); await lca0.Lca(7, 3).Should().BeEqualTo(3);
        await lca0.Lca(7, 4).Should().BeEqualTo(1); await lca0.Lca(7, 5).Should().BeEqualTo(0); await lca0.Lca(7, 6).Should().BeEqualTo(0); await lca0.Lca(7, 7).Should().BeEqualTo(7);

        await lca0[0, 0].Should().BeEqualTo(0); await lca0[0, 1].Should().BeEqualTo(0); await lca0[0, 2].Should().BeEqualTo(0); await lca0[0, 3].Should().BeEqualTo(0);
        await lca0[0, 4].Should().BeEqualTo(0); await lca0[0, 5].Should().BeEqualTo(0); await lca0[0, 6].Should().BeEqualTo(0); await lca0[0, 7].Should().BeEqualTo(0);
        await lca0[1, 0].Should().BeEqualTo(0); await lca0[1, 1].Should().BeEqualTo(1); await lca0[1, 2].Should().BeEqualTo(0); await lca0[1, 3].Should().BeEqualTo(1);
        await lca0[1, 4].Should().BeEqualTo(1); await lca0[1, 5].Should().BeEqualTo(0); await lca0[1, 6].Should().BeEqualTo(0); await lca0[1, 7].Should().BeEqualTo(1);
        await lca0[2, 0].Should().BeEqualTo(0); await lca0[2, 1].Should().BeEqualTo(0); await lca0[2, 2].Should().BeEqualTo(2); await lca0[2, 3].Should().BeEqualTo(0);
        await lca0[2, 4].Should().BeEqualTo(0); await lca0[2, 5].Should().BeEqualTo(2); await lca0[2, 6].Should().BeEqualTo(2); await lca0[2, 7].Should().BeEqualTo(0);
        await lca0[3, 0].Should().BeEqualTo(0); await lca0[3, 1].Should().BeEqualTo(1); await lca0[3, 2].Should().BeEqualTo(0); await lca0[3, 3].Should().BeEqualTo(3);
        await lca0[3, 4].Should().BeEqualTo(1); await lca0[3, 5].Should().BeEqualTo(0); await lca0[3, 6].Should().BeEqualTo(0); await lca0[3, 7].Should().BeEqualTo(3);
        await lca0[4, 0].Should().BeEqualTo(0); await lca0[4, 1].Should().BeEqualTo(1); await lca0[4, 2].Should().BeEqualTo(0); await lca0[4, 3].Should().BeEqualTo(1);
        await lca0[4, 4].Should().BeEqualTo(4); await lca0[4, 5].Should().BeEqualTo(0); await lca0[4, 6].Should().BeEqualTo(0); await lca0[4, 7].Should().BeEqualTo(1);
        await lca0[5, 0].Should().BeEqualTo(0); await lca0[5, 1].Should().BeEqualTo(0); await lca0[5, 2].Should().BeEqualTo(2); await lca0[5, 3].Should().BeEqualTo(0);
        await lca0[5, 4].Should().BeEqualTo(0); await lca0[5, 5].Should().BeEqualTo(5); await lca0[5, 6].Should().BeEqualTo(2); await lca0[5, 7].Should().BeEqualTo(0);
        await lca0[6, 0].Should().BeEqualTo(0); await lca0[6, 1].Should().BeEqualTo(0); await lca0[6, 2].Should().BeEqualTo(2); await lca0[6, 3].Should().BeEqualTo(0);
        await lca0[6, 4].Should().BeEqualTo(0); await lca0[6, 5].Should().BeEqualTo(2); await lca0[6, 6].Should().BeEqualTo(6); await lca0[6, 7].Should().BeEqualTo(0);
        await lca0[7, 0].Should().BeEqualTo(0); await lca0[7, 1].Should().BeEqualTo(1); await lca0[7, 2].Should().BeEqualTo(0); await lca0[7, 3].Should().BeEqualTo(3);
        await lca0[7, 4].Should().BeEqualTo(1); await lca0[7, 5].Should().BeEqualTo(0); await lca0[7, 6].Should().BeEqualTo(0); await lca0[7, 7].Should().BeEqualTo(7);

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

        await lca0.Ascend(0, 0).Should().BeEqualTo(0); await lca0.Ascend(0, 1).Should().BeEqualTo(-1);
        await lca0.Ascend(1, 0).Should().BeEqualTo(1); await lca0.Ascend(1, 1).Should().BeEqualTo(0); await lca0.Ascend(1, 2).Should().BeEqualTo(-1);
        await lca0.Ascend(2, 0).Should().BeEqualTo(2); await lca0.Ascend(2, 1).Should().BeEqualTo(0); await lca0.Ascend(2, 2).Should().BeEqualTo(-1);
        await lca0.Ascend(3, 0).Should().BeEqualTo(3); await lca0.Ascend(3, 1).Should().BeEqualTo(1); await lca0.Ascend(3, 2).Should().BeEqualTo(0); await lca0.Ascend(3, 3).Should().BeEqualTo(-1);
        await lca0.Ascend(4, 0).Should().BeEqualTo(4); await lca0.Ascend(4, 1).Should().BeEqualTo(1); await lca0.Ascend(4, 2).Should().BeEqualTo(0); await lca0.Ascend(4, 3).Should().BeEqualTo(-1);
        await lca0.Ascend(5, 0).Should().BeEqualTo(5); await lca0.Ascend(5, 1).Should().BeEqualTo(2); await lca0.Ascend(5, 2).Should().BeEqualTo(0); await lca0.Ascend(5, 3).Should().BeEqualTo(-1);
        await lca0.Ascend(6, 0).Should().BeEqualTo(6); await lca0.Ascend(6, 1).Should().BeEqualTo(2); await lca0.Ascend(6, 2).Should().BeEqualTo(0); await lca0.Ascend(6, 3).Should().BeEqualTo(-1);
        await lca0.Ascend(7, 0).Should().BeEqualTo(7); await lca0.Ascend(7, 1).Should().BeEqualTo(3); await lca0.Ascend(7, 2).Should().BeEqualTo(1); await lca0.Ascend(7, 3).Should().BeEqualTo(0); await lca0.Ascend(7, 4).Should().BeEqualTo(-1);
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
        var lca0 = tree.LowestCommonAncestorDoubling();
        await lca0.Lca(0, 0).Should().BeEqualTo(0); await lca0.Lca(0, 1).Should().BeEqualTo(0); await lca0.Lca(0, 2).Should().BeEqualTo(0); await lca0.Lca(0, 3).Should().BeEqualTo(0);
        await lca0.Lca(0, 4).Should().BeEqualTo(0); await lca0.Lca(0, 5).Should().BeEqualTo(0); await lca0.Lca(0, 6).Should().BeEqualTo(0); await lca0.Lca(0, 7).Should().BeEqualTo(0);
        await lca0.Lca(1, 0).Should().BeEqualTo(0); await lca0.Lca(1, 1).Should().BeEqualTo(1); await lca0.Lca(1, 2).Should().BeEqualTo(0); await lca0.Lca(1, 3).Should().BeEqualTo(1);
        await lca0.Lca(1, 4).Should().BeEqualTo(1); await lca0.Lca(1, 5).Should().BeEqualTo(0); await lca0.Lca(1, 6).Should().BeEqualTo(0); await lca0.Lca(1, 7).Should().BeEqualTo(1);
        await lca0.Lca(2, 0).Should().BeEqualTo(0); await lca0.Lca(2, 1).Should().BeEqualTo(0); await lca0.Lca(2, 2).Should().BeEqualTo(2); await lca0.Lca(2, 3).Should().BeEqualTo(0);
        await lca0.Lca(2, 4).Should().BeEqualTo(0); await lca0.Lca(2, 5).Should().BeEqualTo(2); await lca0.Lca(2, 6).Should().BeEqualTo(2); await lca0.Lca(2, 7).Should().BeEqualTo(0);
        await lca0.Lca(3, 0).Should().BeEqualTo(0); await lca0.Lca(3, 1).Should().BeEqualTo(1); await lca0.Lca(3, 2).Should().BeEqualTo(0); await lca0.Lca(3, 3).Should().BeEqualTo(3);
        await lca0.Lca(3, 4).Should().BeEqualTo(1); await lca0.Lca(3, 5).Should().BeEqualTo(0); await lca0.Lca(3, 6).Should().BeEqualTo(0); await lca0.Lca(3, 7).Should().BeEqualTo(3);
        await lca0.Lca(4, 0).Should().BeEqualTo(0); await lca0.Lca(4, 1).Should().BeEqualTo(1); await lca0.Lca(4, 2).Should().BeEqualTo(0); await lca0.Lca(4, 3).Should().BeEqualTo(1);
        await lca0.Lca(4, 4).Should().BeEqualTo(4); await lca0.Lca(4, 5).Should().BeEqualTo(0); await lca0.Lca(4, 6).Should().BeEqualTo(0); await lca0.Lca(4, 7).Should().BeEqualTo(1);
        await lca0.Lca(5, 0).Should().BeEqualTo(0); await lca0.Lca(5, 1).Should().BeEqualTo(0); await lca0.Lca(5, 2).Should().BeEqualTo(2); await lca0.Lca(5, 3).Should().BeEqualTo(0);
        await lca0.Lca(5, 4).Should().BeEqualTo(0); await lca0.Lca(5, 5).Should().BeEqualTo(5); await lca0.Lca(5, 6).Should().BeEqualTo(2); await lca0.Lca(5, 7).Should().BeEqualTo(0);
        await lca0.Lca(6, 0).Should().BeEqualTo(0); await lca0.Lca(6, 1).Should().BeEqualTo(0); await lca0.Lca(6, 2).Should().BeEqualTo(2); await lca0.Lca(6, 3).Should().BeEqualTo(0);
        await lca0.Lca(6, 4).Should().BeEqualTo(0); await lca0.Lca(6, 5).Should().BeEqualTo(2); await lca0.Lca(6, 6).Should().BeEqualTo(6); await lca0.Lca(6, 7).Should().BeEqualTo(0);
        await lca0.Lca(7, 0).Should().BeEqualTo(0); await lca0.Lca(7, 1).Should().BeEqualTo(1); await lca0.Lca(7, 2).Should().BeEqualTo(0); await lca0.Lca(7, 3).Should().BeEqualTo(3);
        await lca0.Lca(7, 4).Should().BeEqualTo(1); await lca0.Lca(7, 5).Should().BeEqualTo(0); await lca0.Lca(7, 6).Should().BeEqualTo(0); await lca0.Lca(7, 7).Should().BeEqualTo(7);

        await lca0[0, 0].Should().BeEqualTo(0); await lca0[0, 1].Should().BeEqualTo(0); await lca0[0, 2].Should().BeEqualTo(0); await lca0[0, 3].Should().BeEqualTo(0);
        await lca0[0, 4].Should().BeEqualTo(0); await lca0[0, 5].Should().BeEqualTo(0); await lca0[0, 6].Should().BeEqualTo(0); await lca0[0, 7].Should().BeEqualTo(0);
        await lca0[1, 0].Should().BeEqualTo(0); await lca0[1, 1].Should().BeEqualTo(1); await lca0[1, 2].Should().BeEqualTo(0); await lca0[1, 3].Should().BeEqualTo(1);
        await lca0[1, 4].Should().BeEqualTo(1); await lca0[1, 5].Should().BeEqualTo(0); await lca0[1, 6].Should().BeEqualTo(0); await lca0[1, 7].Should().BeEqualTo(1);
        await lca0[2, 0].Should().BeEqualTo(0); await lca0[2, 1].Should().BeEqualTo(0); await lca0[2, 2].Should().BeEqualTo(2); await lca0[2, 3].Should().BeEqualTo(0);
        await lca0[2, 4].Should().BeEqualTo(0); await lca0[2, 5].Should().BeEqualTo(2); await lca0[2, 6].Should().BeEqualTo(2); await lca0[2, 7].Should().BeEqualTo(0);
        await lca0[3, 0].Should().BeEqualTo(0); await lca0[3, 1].Should().BeEqualTo(1); await lca0[3, 2].Should().BeEqualTo(0); await lca0[3, 3].Should().BeEqualTo(3);
        await lca0[3, 4].Should().BeEqualTo(1); await lca0[3, 5].Should().BeEqualTo(0); await lca0[3, 6].Should().BeEqualTo(0); await lca0[3, 7].Should().BeEqualTo(3);
        await lca0[4, 0].Should().BeEqualTo(0); await lca0[4, 1].Should().BeEqualTo(1); await lca0[4, 2].Should().BeEqualTo(0); await lca0[4, 3].Should().BeEqualTo(1);
        await lca0[4, 4].Should().BeEqualTo(4); await lca0[4, 5].Should().BeEqualTo(0); await lca0[4, 6].Should().BeEqualTo(0); await lca0[4, 7].Should().BeEqualTo(1);
        await lca0[5, 0].Should().BeEqualTo(0); await lca0[5, 1].Should().BeEqualTo(0); await lca0[5, 2].Should().BeEqualTo(2); await lca0[5, 3].Should().BeEqualTo(0);
        await lca0[5, 4].Should().BeEqualTo(0); await lca0[5, 5].Should().BeEqualTo(5); await lca0[5, 6].Should().BeEqualTo(2); await lca0[5, 7].Should().BeEqualTo(0);
        await lca0[6, 0].Should().BeEqualTo(0); await lca0[6, 1].Should().BeEqualTo(0); await lca0[6, 2].Should().BeEqualTo(2); await lca0[6, 3].Should().BeEqualTo(0);
        await lca0[6, 4].Should().BeEqualTo(0); await lca0[6, 5].Should().BeEqualTo(2); await lca0[6, 6].Should().BeEqualTo(6); await lca0[6, 7].Should().BeEqualTo(0);
        await lca0[7, 0].Should().BeEqualTo(0); await lca0[7, 1].Should().BeEqualTo(1); await lca0[7, 2].Should().BeEqualTo(0); await lca0[7, 3].Should().BeEqualTo(3);
        await lca0[7, 4].Should().BeEqualTo(1); await lca0[7, 5].Should().BeEqualTo(0); await lca0[7, 6].Should().BeEqualTo(0); await lca0[7, 7].Should().BeEqualTo(7);

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

        await lca0.Ascend(0, 0).Should().BeEqualTo(0); await lca0.Ascend(0, 1).Should().BeEqualTo(-1);
        await lca0.Ascend(1, 0).Should().BeEqualTo(1); await lca0.Ascend(1, 1).Should().BeEqualTo(0); await lca0.Ascend(1, 2).Should().BeEqualTo(-1);
        await lca0.Ascend(2, 0).Should().BeEqualTo(2); await lca0.Ascend(2, 1).Should().BeEqualTo(0); await lca0.Ascend(2, 2).Should().BeEqualTo(-1);
        await lca0.Ascend(3, 0).Should().BeEqualTo(3); await lca0.Ascend(3, 1).Should().BeEqualTo(1); await lca0.Ascend(3, 2).Should().BeEqualTo(0); await lca0.Ascend(3, 3).Should().BeEqualTo(-1);
        await lca0.Ascend(4, 0).Should().BeEqualTo(4); await lca0.Ascend(4, 1).Should().BeEqualTo(1); await lca0.Ascend(4, 2).Should().BeEqualTo(0); await lca0.Ascend(4, 3).Should().BeEqualTo(-1);
        await lca0.Ascend(5, 0).Should().BeEqualTo(5); await lca0.Ascend(5, 1).Should().BeEqualTo(2); await lca0.Ascend(5, 2).Should().BeEqualTo(0); await lca0.Ascend(5, 3).Should().BeEqualTo(-1);
        await lca0.Ascend(6, 0).Should().BeEqualTo(6); await lca0.Ascend(6, 1).Should().BeEqualTo(2); await lca0.Ascend(6, 2).Should().BeEqualTo(0); await lca0.Ascend(6, 3).Should().BeEqualTo(-1);
        await lca0.Ascend(7, 0).Should().BeEqualTo(7); await lca0.Ascend(7, 1).Should().BeEqualTo(3); await lca0.Ascend(7, 2).Should().BeEqualTo(1); await lca0.Ascend(7, 3).Should().BeEqualTo(0); await lca0.Ascend(7, 4).Should().BeEqualTo(-1);
    }

    [Test, MultipleAssertions]
    public async Task Random()
    {
        const int N = 20;
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
            var lca = tree.LowestCommonAncestorDoubling();
            var wlca = wtree.LowestCommonAncestorDoubling();
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    var (expectedLca, expectedDist) = LcaDirect(tree.AsArray(), i, j);
                    await tree.HlDecomposition.LowestCommonAncestor(i, j).Should().BeEqualTo(expectedLca);
                    await lca.Lca(i, j).Should().BeEqualTo(expectedLca);
                    await wlca.Lca(i, j).Should().BeEqualTo(expectedLca);
                    await lca.Distance(i, j).Should().BeEqualTo(expectedDist);
                    await wlca.Distance(i, j).Should().BeEqualTo(expectedDist);
                }
        }
    }
    static (int lca, int distance) LcaDirect(TreeNode<GraphEdge>[] tree, int i, int j)
    {
        int distance = 0;
        while (tree[i].Depth > tree[j].Depth)
        {
            i = tree[i].Parent;
            ++distance;
        }
        while (tree[i].Depth < tree[j].Depth)
        {
            j = tree[j].Parent;
            ++distance;
        }
        while (i != j)
        {
            i = tree[i].Parent;
            j = tree[j].Parent;
            distance += 2;
        }
        return (i, distance);
    }
}