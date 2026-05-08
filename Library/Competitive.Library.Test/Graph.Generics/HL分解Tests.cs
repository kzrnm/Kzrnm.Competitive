namespace Kzrnm.Competitive.Testing.Graph;

public class HL分解Tests
{
    GraphBuilder gb;
    WIntGraphBuilder wgb;

    public HL分解Tests()
    {
        gb = new GraphBuilder(8, false);
        gb.Add(0, 1);
        gb.Add(0, 2);
        gb.Add(1, 3);
        gb.Add(1, 4);
        gb.Add(2, 5);
        gb.Add(2, 6);
        gb.Add(3, 7);

        wgb = new WIntGraphBuilder(8, false);
        wgb.Add(0, 1, 1);
        wgb.Add(0, 2, 2);
        wgb.Add(1, 3, 3);
        wgb.Add(1, 4, 4);
        wgb.Add(2, 5, 5);
        wgb.Add(2, 6, 6);
        wgb.Add(3, 7, 7);
    }

    [Test, MultipleAssertions]
    public async Task InternalArray()
    {
        await Inner(gb.ToTree(0));
        await Inner(wgb.ToTree(0));
        static async Task Inner<TNode, TEdge>(ITreeGraph<TNode, TEdge> tree)
            where TNode : ITreeNode<TEdge>
            where TEdge : IGraphEdge
        {
            await tree.HlDecomposition.down.Should().BeStrictlyEquivalentTo([0, 1, 5, 2, 4, 7, 6, 3]);
            await tree.HlDecomposition.up.Should().BeStrictlyEquivalentTo([8, 5, 8, 4, 5, 8, 7, 4]);
            await tree.HlDecomposition.nxt.Should().BeStrictlyEquivalentTo([0, 0, 2, 0, 4, 5, 2, 0]);
        }
    }

    [Test, MultipleAssertions]
    public async Task PathQuery()
    {
        await Inner(gb.ToTree(0));
        await Inner(wgb.ToTree(0));
        static async Task Inner<TNode, TEdge>(ITreeGraph<TNode, TEdge> tree)
            where TNode : ITreeNode<TEdge>
            where TEdge : IGraphEdge
        {
            var list = new List<(int u, int v)>();
            list.Clear();
            tree.HlDecomposition.PathQuery(7, 5, false, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (4, 1),
                (5, 6),
                (7, 8),
            ]);

            list.Clear();
            tree.HlDecomposition.PathQuery(7, 5, true, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (4, 1),
                (0, 1),
                (5, 6),
                (7, 8),
            ]);

            list.Clear();
            tree.HlDecomposition.PathQuery(7, 6, false, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (4, 1),
                (5, 7),
            ]);

            list.Clear();
            tree.HlDecomposition.PathQuery(7, 6, true, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (4, 1),
                (0, 1),
                (5, 7),
            ]);


            list.Clear();
            tree.HlDecomposition.PathQuery(6, 7, false, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (7, 5),
                (1, 4),
            ]);

            list.Clear();
            tree.HlDecomposition.PathQuery(6, 7, true, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (7, 5),
                (0, 1),
                (1, 4),
            ]);
        }
    }

    [Test, MultipleAssertions]
    public async Task SubtreeQuery()
    {
        await Inner(gb.ToTree(0));
        await Inner(wgb.ToTree(0));
        static async Task Inner<TNode, TEdge>(ITreeGraph<TNode, TEdge> tree)
            where TNode : ITreeNode<TEdge>
            where TEdge : IGraphEdge
        {
            var list = new List<(int u, int v)>();
            list.Clear();
            tree.HlDecomposition.SubtreeQuery(0, false, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (1, 8),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(0, true, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (0, 8),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(1, false, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (2, 5),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(1, true, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (1, 5),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(2, false, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (6, 8),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(2, true, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (5, 8),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(3, false, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (3, 4),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(3, true, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (2, 4),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(4, false, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (5, 5),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(4, true, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (4, 5),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(5, false, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (8, 8),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(5, true, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (7, 8),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(6, false, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (7, 7),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(6, true, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (6, 7),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(7, false, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (4, 4),
            ]);

            list.Clear();
            tree.HlDecomposition.SubtreeQuery(7, true, (u, v) => list.Add((u, v)));
            await list.Should().BeStrictlyEquivalentTo([
                (3, 4),
            ]);
        }
    }
}