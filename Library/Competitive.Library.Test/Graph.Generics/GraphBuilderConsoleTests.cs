using Kzrnm.Competitive.IO;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kzrnm.Competitive.Testing.Graph
{
    public class GraphBuilderConsoleTestsBase
    {
        static readonly UTF8Encoding enc = new UTF8Encoding(false);
        protected static PropertyConsoleReader GetReader(string text)
            => new(new MemoryStream(enc.GetBytes(text)), enc);
        protected static PropertyConsoleReader GetReader(IEnumerable<int> nums)
            => GetReader(string.Join(" ", nums));
    }
    public class GraphBuilderConsoleTests : GraphBuilderConsoleTestsBase
    {
        [Fact]
        public void GraphUndirected()
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
            var graph = cr.Graph(5, 6, false).ToGraph();
            graph.Length.Should().Be(5);
            var expectedChildren = new GraphEdge[5][]
            {
                [
                    new(1),
                ],
                [
                    new(0),
                    new(2),
                    new(4),
                ],
                [
                    new(1),
                    new(3),
                    new(4),
                ],
                [
                    new(2),
                    new(4),
                ],
                [
                    new(2),
                    new(3),
                    new(1),
                ],
            };
            var expectedParents = expectedChildren;

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeFalse("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }

        [Fact]
        public void GraphUndirectedZeroBased()
        {
            var cr = GetReader(
                """
                0 1
                1 2
                2 3
                3 4
                2 4
                4 1
                """);
            var graph = cr.Graph(5, 6, false, based: 0).ToGraph();
            graph.Length.Should().Be(5);
            var expectedChildren = new GraphEdge[5][]
            {
                [
                    new(1),
                ],
                [
                    new(0),
                    new(2),
                    new(4),
                ],
                [
                    new(1),
                    new(3),
                    new(4),
                ],
                [
                    new(2),
                    new(4),
                ],
                [
                    new(2),
                    new(3),
                    new(1),
                ],
            };
            var expectedParents = expectedChildren;

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeFalse("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }

        [Fact]
        public void GraphDirected()
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
            var graph = cr.Graph(5, 6, true).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new GraphEdge[5][]
            {
                [
                    new(1),
                ],
                [
                    new(2),
                ],
                [
                    new(3),
                    new(4),
                ],
                [
                    new(4),
                ],
                [
                    new(1),
                ],
            };
            var expectedParents = new GraphEdge[5][]
            {
                [],
                [
                    new(0),
                    new(4),
                ],
                [
                    new(1),
                ],
                [
                    new(2),
                ],
                [
                    new(2),
                    new(3),
                ],
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }

        [Fact]
        public void Tree()
        {
            var cr = GetReader(
                """
                1 2
                2 3
                3 4
                5 2
                """);
            var graph = cr.Tree(5).ToTree(0);
            graph.Length.Should().Be(5);
            graph.Root.Should().Be(0);

            var expectedChildren = new GraphEdge[5][]
            {
                [
                    new(1),
                ],
                [
                    new(2),
                    new(4),
                ],
                [
                    new(3),
                ],
                [],
                [],
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
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parent.Should().BeEquivalentTo(expectedParent[i], "index: {0}", i);
            }
        }

        [Fact]
        public void TreeZeroBased()
        {
            var cr = GetReader(
                """
                0 1
                1 2
                2 3
                4 1
                """);
            var graph = cr.Tree(5, based: 0).ToTree(0);
            graph.Length.Should().Be(5);
            graph.Root.Should().Be(0);

            var expectedChildren = new GraphEdge[5][]
            {
                [
                    new(1),
                ],
                [
                    new(2),
                    new(4),
                ],
                [
                    new(3),
                ],
                [],
                [],
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
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parent.Should().BeEquivalentTo(expectedParent[i], "index: {0}", i);
            }
        }

        [Fact]
        public void GraphWithEdgeIndexUndirected()
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
            var graph = cr.GraphWithEdgeIndex(5, 6, false).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new GraphEdge<int>[5][]
            {
                [
                    new (1, 0),
                ],
                [
                    new (0, 0),
                    new (2, 1),
                    new (4, 5),
                ],
                [
                    new (1, 1),
                    new (3, 2),
                    new (4, 4),
                ],
                [
                    new (2, 2),
                    new (4, 3),
                ],
                [
                    new (2, 4),
                    new (3, 3),
                    new (1, 5),
                ],
            };
            var expectedParents = expectedChildren;

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeFalse("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }

        [Fact]
        public void GraphWithEdgeIndexDirected()
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
            var graph = cr.GraphWithEdgeIndex(5, 6, true).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new GraphEdge<int>[5][]
            {
                [
                    new(1, 0),
                ],
                [
                    new(2, 1),
                ],
                [
                    new(3, 2),
                    new(4, 4),
                ],
                [
                    new(4, 3),
                ],
                [
                    new(1, 5),
                ],
            };
            var expectedParents = new GraphEdge<int>[5][]
            {
                [],
                [
                    new(0, 0),
                    new(4, 5),
                ],
                [
                    new(1, 1),
                ],
                [
                    new(2, 2),
                ],
                [
                    new(2, 4),
                    new(3, 3),
                ],
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }


        [Fact]
        public void GraphWithEdgeIndexDirectedZeroBased()
        {
            var cr = GetReader(
                """
                0 1
                1 2
                2 3
                3 4
                2 4
                4 1
                """);
            var graph = cr.GraphWithEdgeIndex(5, 6, true, based: 0).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new GraphEdge<int>[5][]
            {
                [
                    new(1, 0),
                ],
                [
                    new(2, 1),
                ],
                [
                    new(3, 2),
                    new(4, 4),
                ],
                [
                    new(4, 3),
                ],
                [
                    new(1, 5),
                ],
            };
            var expectedParents = new GraphEdge<int>[5][]
            {
                [],
                [
                    new(0, 0),
                    new(4, 5),
                ],
                [
                    new(1, 1),
                ],
                [
                    new(2, 2),
                ],
                [
                    new(2, 4),
                    new(3, 3),
                ],
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }
    }


    public class WGraphBuilderInt32CreateTests : GraphBuilderConsoleTestsBase
    {
        [Fact]
        public void GraphUndirected()
        {
            var cr = GetReader(
                """
                1 2 -1
                2 3 11
                3 4 22
                4 5 33
                3 5 44
                5 2 55
                """);
            var graph = cr.Graph<int>(5, 6, false).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<int>[5][]
            {
                [
                    new (1, -1),
                ],
                [
                    new (0, -1),
                    new (2, 11),
                    new (4, 55),
                ],
                [
                    new (1, 11),
                    new (3, 22),
                    new (4, 44),
                ],
                [
                    new (2, 22),
                    new (4, 33),
                ],
                [
                    new (2, 44),
                    new (3, 33),
                    new (1, 55),
                ],
            };
            var expectedParents = expectedChildren;

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeFalse("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }

        [Fact]
        public void GraphDirected()
        {
            var cr = GetReader(
                """
                1 2 -1
                2 3 11
                3 4 22
                4 5 33
                3 5 44
                5 2 55
                """);
            var graph = cr.Graph<int>(5, 6, true).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<int>[5][]
            {
                [
                    new(1, -1),
                ],
                [
                    new(2, 11),
                ],
                [
                    new(3, 22),
                    new(4, 44),
                ],
                [
                    new(4, 33),
                ],
                [
                    new(1, 55),
                ],
            };
            var expectedParents = new WEdge<int>[5][]
            {
                [],
                [
                    new(0, -1),
                    new(4, 55),
                ],
                [
                    new(1, 11),
                ],
                [
                    new(2, 22),
                ],
                [
                    new(2, 44),
                    new(3, 33),
                ],
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }

        [Fact]
        public void GraphDirectedZeroBased()
        {
            var cr = GetReader(
                """
                0 1 -1
                1 2 11
                2 3 22
                3 4 33
                2 4 44
                4 1 55
                """);
            var graph = cr.Graph<int>(5, 6, true, based: 0).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<int>[5][]
            {
                [
                    new(1, -1),
                ],
                [
                    new(2, 11),
                ],
                [
                    new(3, 22),
                    new(4, 44),
                ],
                [
                    new(4, 33),
                ],
                [
                    new(1, 55),
                ],
            };
            var expectedParents = new WEdge<int>[5][]
            {
                [],
                [
                    new(0, -1),
                    new(4, 55),
                ],
                [
                    new(1, 11),
                ],
                [
                    new(2, 22),
                ],
                [
                    new(2, 44),
                    new(3, 33),
                ],
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }

        [Fact]
        public void Tree()
        {
            var cr = GetReader(
                """
                1 2 -1
                2 3 11
                3 4 22
                5 2 55
                """);
            var graph = cr.Tree<int>(5).ToTree(0);
            graph.Length.Should().Be(5);
            graph.Root.Should().Be(0);

            var expectedChildren = new WEdge<int>[5][]
            {
                [
                    new(1, -1),
                ],
                [
                    new(2, 11),
                    new(4, 55),
                ],
                [
                    new(3, 22),
                ],
                [],
                [],
            };
            var expectedParent = new WEdge<int>[5]
            {
                WEdge<int>.None,
                new(0, -1),
                new(1, 11),
                new(2, 22),
                new(1, 55),
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parent.Should().BeEquivalentTo(expectedParent[i], "index: {0}", i);
            }
        }

        [Fact]
        public void TreeZeroBased()
        {
            var cr = GetReader(
                """
                0 1 -1
                1 2 11
                2 3 22
                4 1 55
                """);
            var graph = cr.Tree<int>(5, based: 0).ToTree(0);
            graph.Length.Should().Be(5);
            graph.Root.Should().Be(0);

            var expectedChildren = new WEdge<int>[5][]
            {
                [
                    new(1, -1),
                ],
                [
                    new(2, 11),
                    new(4, 55),
                ],
                [
                    new(3, 22),
                ],
                [],
                [],
            };
            var expectedParent = new WEdge<int>[5]
            {
                WEdge<int>.None,
                new(0, -1),
                new(1, 11),
                new(2, 22),
                new(1, 55),
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parent.Should().BeEquivalentTo(expectedParent[i], "index: {0}", i);
            }
        }

        [Fact]
        public void GraphWithEdgeIndexUndirected()
        {
            var cr = GetReader(
                """
                1 2 -1
                2 3 11
                3 4 22
                4 5 33
                3 5 44
                5 2 55
                """);
            var graph = cr.GraphWithEdgeIndex<int>(5, 6, false).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<int, int>[5][]
            {
                [
                    new (1, -1, 0),
                ],
                [
                    new (0, -1, 0),
                    new (2, 11, 1),
                    new (4, 55, 5),
                ],
                [
                    new (1, 11, 1),
                    new (3, 22, 2),
                    new (4, 44, 4),
                ],
                [
                    new (2, 22, 2),
                    new (4, 33, 3),
                ],
                [
                    new (2, 44, 4),
                    new (3, 33, 3),
                    new (1, 55, 5),
                ],
            };
            var expectedParents = expectedChildren;

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeFalse("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }

        [Fact]
        public void GraphWithEdgeIndexDirected()
        {
            var cr = GetReader(
                """
                1 2 -1
                2 3 11
                3 4 22
                4 5 33
                3 5 44
                5 2 55
                """);
            var graph = cr.GraphWithEdgeIndex<int>(5, 6, true).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<int, int>[5][]
            {
                [
                    new(1, -1, 0),
                ],
                [
                    new(2, 11, 1),
                ],
                [
                    new(3, 22, 2),
                    new(4, 44, 4),
                ],
                [
                    new(4, 33, 3),
                ],
                [
                    new(1, 55, 5),
                ],
            };
            var expectedParents = new WEdge<int, int>[5][]
            {
                [],
                [
                    new(0, -1, 0),
                    new(4, 55, 5),
                ],
                [
                    new(1, 11, 1),
                ],
                [
                    new(2, 22, 2),
                ],
                [
                    new(2, 44, 4),
                    new(3, 33, 3),
                ],
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }

        [Fact]
        public void GraphWithEdgeIndexDirectedZeroBased()
        {
            var cr = GetReader(
                """
                0 1 -1
                1 2 11
                2 3 22
                3 4 33
                2 4 44
                4 1 55
                """);
            var graph = cr.GraphWithEdgeIndex<int>(5, 6, true, based: 0).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<int, int>[5][]
            {
                [
                    new(1, -1, 0),
                ],
                [
                    new(2, 11, 1),
                ],
                [
                    new(3, 22, 2),
                    new(4, 44, 4),
                ],
                [
                    new(4, 33, 3),
                ],
                [
                    new(1, 55, 5),
                ],
            };
            var expectedParents = new WEdge<int, int>[5][]
            {
                [],
                [
                    new(0, -1, 0),
                    new(4, 55, 5),
                ],
                [
                    new(1, 11, 1),
                ],
                [
                    new(2, 22, 2),
                ],
                [
                    new(2, 44, 4),
                    new(3, 33, 3),
                ],
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }
    }

    public class WGraphBuilderInt64CreateTests : GraphBuilderConsoleTestsBase
    {
        [Fact]
        public void GraphUndirected()
        {
            var cr = GetReader(
                """
                1 2 -1
                2 3 11
                3 4 22
                4 5 33
                3 5 44
                5 2 55
                """);
            var graph = cr.Graph<long>(5, 6, false).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<long>[5][]
            {
                [
                    new (1, -1),
                ],
                [
                    new (0, -1),
                    new (2, 11),
                    new (4, 55),
                ],
                [
                    new (1, 11),
                    new (3, 22),
                    new (4, 44),
                ],
                [
                    new (2, 22),
                    new (4, 33),
                ],
                [
                    new (2, 44),
                    new (3, 33),
                    new (1, 55),
                ],
            };
            var expectedParents = expectedChildren;

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeFalse("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }

        [Fact]
        public void GraphDirected()
        {
            var cr = GetReader(
                """
                1 2 -1
                2 3 11
                3 4 22
                4 5 33
                3 5 44
                5 2 55
                """);
            var graph = cr.Graph<long>(5, 6, true).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<long>[5][]
            {
                [
                    new(1, -1),
                ],
                [
                    new(2, 11),
                ],
                [
                    new(3, 22),
                    new(4, 44),
                ],
                [
                    new(4, 33),
                ],
                [
                    new(1, 55),
                ],
            };
            var expectedParents = new WEdge<long>[5][]
            {
                [],
                [
                    new(0, -1),
                    new(4, 55),
                ],
                [
                    new(1, 11),
                ],
                [
                    new(2, 22),
                ],
                [
                    new(2, 44),
                    new(3, 33),
                ],
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }

        [Fact]
        public void Tree()
        {
            var cr = GetReader(
                """
                1 2 -1
                2 3 11
                3 4 22
                5 2 55
                """);
            var graph = cr.Tree<long>(5).ToTree(0);
            graph.Length.Should().Be(5);
            graph.Root.Should().Be(0);

            var expectedChildren = new WEdge<long>[5][]
            {
                [
                    new(1, -1),
                ],
                [
                    new(2, 11),
                    new(4, 55),
                ],
                [
                    new(3, 22),
                ],
                [],
                [],
            };
            var expectedParent = new WEdge<long>[5]
            {
                WEdge<long>.None,
                new(0, -1),
                new(1, 11),
                new(2, 22),
                new(1, 55),
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parent.Should().BeEquivalentTo(expectedParent[i], "index: {0}", i);
            }
        }


        [Fact]
        public void GraphWithEdgeIndexUndirected()
        {
            var cr = GetReader(
                """
                1 2 -1
                2 3 11
                3 4 22
                4 5 33
                3 5 44
                5 2 55
                """);
            var graph = cr.GraphWithEdgeIndex<long>(5, 6, false).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<long, int>[5][]
            {
                [
                    new (1, -1, 0),
                ],
                [
                    new (0, -1, 0),
                    new (2, 11, 1),
                    new (4, 55, 5),
                ],
                [
                    new (1, 11, 1),
                    new (3, 22, 2),
                    new (4, 44, 4),
                ],
                [
                    new (2, 22, 2),
                    new (4, 33, 3),
                ],
                [
                    new (2, 44, 4),
                    new (3, 33, 3),
                    new (1, 55, 5),
                ],
            };
            var expectedParents = expectedChildren;

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeFalse("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }

        [Fact]
        public void GraphWithEdgeIndexDirected()
        {
            var cr = GetReader(
                """
                1 2 -1
                2 3 11
                3 4 22
                4 5 33
                3 5 44
                5 2 55
                """);
            var graph = cr.GraphWithEdgeIndex<long>(5, 6, true).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<long, int>[5][]
            {
                [
                    new(1, -1, 0),
                ],
                [
                    new(2, 11, 1),
                ],
                [
                    new(3, 22, 2),
                    new(4, 44, 4),
                ],
                [
                    new(4, 33, 3),
                ],
                [
                    new(1, 55, 5),
                ],
            };
            var expectedParents = new WEdge<long, int>[5][]
            {
                [],
                [
                    new(0, -1, 0),
                    new(4, 55, 5),
                ],
                [
                    new(1, 11, 1),
                ],
                [
                    new(2, 22, 2),
                ],
                [
                    new(2, 44, 4),
                    new(3, 33, 3),
                ],
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }
    }
}
