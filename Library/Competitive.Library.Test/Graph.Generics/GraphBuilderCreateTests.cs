using Kzrnm.Competitive.IO;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Kzrnm.Competitive.Testing.Graph
{
    public class GraphBuilderCreateTests
    {
        static readonly UTF8Encoding enc = new UTF8Encoding(false);
        PropertyConsoleReader GetReader(string text)
        {
            return new PropertyConsoleReader(new MemoryStream(enc.GetBytes(text)), enc);
        }

        static bool StrictEqualEdge<T>(T e1, T e2) where T : unmanaged
        {
            return MemoryMarshal.AsBytes(stackalloc[] { e1 }).SequenceEqual(MemoryMarshal.AsBytes(stackalloc[] { e2 }));
        }
        static bool StrictEqualEdge(GraphEdge e1, GraphEdge e2)
        {
            return StrictEqualEdge<GraphEdge>(e1, e2);
        }
        static bool StrictEqualEdge(GraphEdge<int> e1, GraphEdge<int> e2)
        {
            return StrictEqualEdge<GraphEdge<int>>(e1, e2);
        }

        [Fact]
        public void CreateNonDirected()
        {
            var cr = GetReader(
                """
                1 2
                2 3
                3 4
                4 5
                3 5
                5 2
                """);
            var graph = GraphBuilder.Create(5, cr, 6, false).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new GraphEdge[5][]
            {
                new GraphEdge[]
                {
                    new(1),
                },
                new GraphEdge[]
                {
                    new(0),
                    new(2),
                    new(4),
                },
                new GraphEdge[]
                {
                    new(1),
                    new(3),
                    new(4),
                },
                new GraphEdge[]
                {
                    new(2),
                    new(4),
                },
                new GraphEdge[]
                {
                    new(2),
                    new(3),
                    new(1),
                },
            };
            var expectedParents = expectedChildren;

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeFalse("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], StrictEqualEdge, "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], StrictEqualEdge, "index: {0}", i);
            }
        }

        [Fact]
        public void CreateDirected()
        {
            var cr = GetReader(
                """
                1 2
                2 3
                3 4
                4 5
                3 5
                5 2
                """);
            var graph = GraphBuilder.Create(5, cr, 6, true).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new GraphEdge[5][]
            {
                new GraphEdge[]
                {
                    new(1),
                },
                new GraphEdge[]
                {
                    new(2),
                },
                new GraphEdge[]
                {
                    new(3),
                    new(4),
                },
                new GraphEdge[]
                {
                    new(4),
                },
                new GraphEdge[]
                {
                    new(1),
                },
            };
            var expectedParents = new GraphEdge[5][]
            {
                new GraphEdge[]
                {
                },
                new GraphEdge[]
                {
                    new(0),
                    new(4),
                },
                new GraphEdge[]
                {
                    new(1),
                },
                new GraphEdge[]
                {
                    new(2),
                },
                new GraphEdge[]
                {
                    new(2),
                    new(3),
                },
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], StrictEqualEdge, "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], StrictEqualEdge, "index: {0}", i);
            }
        }

        [Fact]
        public void CreateTree()
        {
            var cr = GetReader(
                """
                1 2
                2 3
                3 4
                5 2
                """);
            var graph = GraphBuilder.CreateTree(5, cr).ToTree(0);
            graph.Length.Should().Be(5);
            graph.Root.Should().Be(0);

            var expectedChildren = new GraphEdge[5][]
            {
                new GraphEdge[]
                {
                    new(1),
                },
                new GraphEdge[]
                {
                    new(2),
                    new(4),
                },
                new GraphEdge[]
                {
                    new(3),
                },
                new GraphEdge[]
                {
                },
                new GraphEdge[]
                {
                },
            };
            var expectedParent = new GraphEdge[5]
            {
                GraphEdge.None,
                new(0),
                new(1),
                new(2),
                new(1),
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].Children.Should().Equal(expectedChildren[i], StrictEqualEdge, "index: {0}", i);
                graph[i].Parent.Should().BeEquivalentTo(expectedParent[i], "index: {0}", i);
            }
        }


        [Fact]
        public void CreateWithEdgeIndexNonDirected()
        {
            var cr = GetReader(
                """
                1 2
                2 3
                3 4
                4 5
                3 5
                5 2
                """);
            var graph = GraphBuilder.CreateWithEdgeIndex(5, cr, 6, false).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new GraphEdge<int>[5][]
            {
                new GraphEdge<int>[]
                {
                    new (1, 0),
                },
                new GraphEdge<int>[]
                {
                    new (0, 0),
                    new (2, 1),
                    new (4, 5),
                },
                new GraphEdge<int>[]
                {
                    new (1, 1),
                    new (3, 2),
                    new (4, 4),
                },
                new GraphEdge<int>[]
                {
                    new (2, 2),
                    new (4, 3),
                },
                new GraphEdge<int>[]
                {
                    new (2, 4),
                    new (3, 3),
                    new (1, 5),
                },
            };
            var expectedParents = expectedChildren;

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeFalse("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], StrictEqualEdge, "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], StrictEqualEdge, "index: {0}", i);
            }
        }

        [Fact]
        public void CreateWithEdgeIndexDirected()
        {
            var cr = GetReader(
                """
                1 2
                2 3
                3 4
                4 5
                3 5
                5 2
                """);
            var graph = GraphBuilder.CreateWithEdgeIndex(5, cr, 6, true).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new GraphEdge<int>[5][]
            {
                new GraphEdge<int>[]
                {
                    new(1, 0),
                },
                new GraphEdge<int>[]
                {
                    new(2, 1),
                },
                new GraphEdge<int>[]
                {
                    new(3, 2),
                    new(4, 4),
                },
                new GraphEdge<int>[]
                {
                    new(4, 3),
                },
                new GraphEdge<int>[]
                {
                    new(1, 5),
                },
            };
            var expectedParents = new GraphEdge<int>[5][]
            {
                new GraphEdge<int>[]
                {
                },
                new GraphEdge<int>[]
                {
                    new(0, 0),
                    new(4, 5),
                },
                new GraphEdge<int>[]
                {
                    new(1, 1),
                },
                new GraphEdge<int>[]
                {
                    new(2, 2),
                },
                new GraphEdge<int>[]
                {
                    new(2, 4),
                    new(3, 3),
                },
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], StrictEqualEdge, "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], StrictEqualEdge, "index: {0}", i);
            }
        }
    }
}
