using Kzrnm.Competitive.IO;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Kzrnm.Competitive.Testing.Graph
{
    public class WGraphBuilderCreateTests
    {
        static readonly UTF8Encoding enc = new UTF8Encoding(false);
        PropertyConsoleReader GetReader(string text)
        {
            return new PropertyConsoleReader(new MemoryStream(enc.GetBytes(text)), enc);
        }

        #region int
        [Fact]
        public void CreateNonDirectedInt()
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
            var graph = WIntGraphBuilder.Create(5, cr, 6, false).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<int>[5][]
            {
                new WEdge<int>[]
                {
                    new (1, -1),
                },
                new WEdge<int>[]
                {
                    new (0, -1),
                    new (2, 11),
                    new (4, 55),
                },
                new WEdge<int>[]
                {
                    new (1, 11),
                    new (3, 22),
                    new (4, 44),
                },
                new WEdge<int>[]
                {
                    new (2, 22),
                    new (4, 33),
                },
                new WEdge<int>[]
                {
                    new (2, 44),
                    new (3, 33),
                    new (1, 55),
                },
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
        public void CreateDirectedInt()
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
            var graph = WIntGraphBuilder.Create(5, cr, 6, true).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<int>[5][]
            {
                new WEdge<int>[]
                {
                    new(1, -1),
                },
                new WEdge<int>[]
                {
                    new(2, 11),
                },
                new WEdge<int>[]
                {
                    new(3, 22),
                    new(4, 44),
                },
                new WEdge<int>[]
                {
                    new(4, 33),
                },
                new WEdge<int>[]
                {
                    new(1, 55),
                },
            };
            var expectedParents = new WEdge<int>[5][]
            {
                new WEdge<int>[]
                {
                },
                new WEdge<int>[]
                {
                    new(0, -1),
                    new(4, 55),
                },
                new WEdge<int>[]
                {
                    new(1, 11),
                },
                new WEdge<int>[]
                {
                    new(2, 22),
                },
                new WEdge<int>[]
                {
                    new(2, 44),
                    new(3, 33),
                },
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }

        [Fact]
        public void CreateTreeInt()
        {
            var cr = GetReader(
                """
                1 2 -1
                2 3 11
                3 4 22
                5 2 55
                """);
            var graph = WIntGraphBuilder.CreateTree(5, cr).ToTree(0);
            graph.Length.Should().Be(5);
            graph.Root.Should().Be(0);

            var expectedChildren = new WEdge<int>[5][]
            {
                new WEdge<int>[]
                {
                    new(1, -1),
                },
                new WEdge<int>[]
                {
                    new(2, 11),
                    new(4, 55),
                },
                new WEdge<int>[]
                {
                    new(3, 22),
                },
                new WEdge<int>[]
                {
                },
                new WEdge<int>[]
                {
                },
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
        public void CreateWithEdgeIndexNonDirectedInt()
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
            var graph = WIntGraphBuilder.CreateWithEdgeIndex(5, cr, 6, false).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<int, int>[5][]
            {
                new WEdge<int, int>[]
                {
                    new (1, -1, 0),
                },
                new WEdge<int, int>[]
                {
                    new (0, -1, 0),
                    new (2, 11, 1),
                    new (4, 55, 5),
                },
                new WEdge<int, int>[]
                {
                    new (1, 11, 1),
                    new (3, 22, 2),
                    new (4, 44, 4),
                },
                new WEdge<int, int>[]
                {
                    new (2, 22, 2),
                    new (4, 33, 3),
                },
                new WEdge<int, int>[]
                {
                    new (2, 44, 4),
                    new (3, 33, 3),
                    new (1, 55, 5),
                },
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
        public void CreateWithEdgeIndexDirectedInt()
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
            var graph = WIntGraphBuilder.CreateWithEdgeIndex(5, cr, 6, true).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<int, int>[5][]
            {
                new WEdge<int, int>[]
                {
                    new(1, -1, 0),
                },
                new WEdge<int, int>[]
                {
                    new(2, 11, 1),
                },
                new WEdge<int, int>[]
                {
                    new(3, 22, 2),
                    new(4, 44, 4),
                },
                new WEdge<int, int>[]
                {
                    new(4, 33, 3),
                },
                new WEdge<int, int>[]
                {
                    new(1, 55, 5),
                },
            };
            var expectedParents = new WEdge<int, int>[5][]
            {
                new WEdge<int, int>[]
                {
                },
                new WEdge<int, int>[]
                {
                    new(0, -1, 0),
                    new(4, 55, 5),
                },
                new WEdge<int, int>[]
                {
                    new(1, 11, 1),
                },
                new WEdge<int, int>[]
                {
                    new(2, 22, 2),
                },
                new WEdge<int, int>[]
                {
                    new(2, 44, 4),
                    new(3, 33, 3),
                },
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }
        #endregion int

        #region long
        [Fact]
        public void CreateNonDirectedLong()
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
            var graph = WLongGraphBuilder.Create(5, cr, 6, false).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<long>[5][]
            {
                new WEdge<long>[]
                {
                    new (1, -1),
                },
                new WEdge<long>[]
                {
                    new (0, -1),
                    new (2, 11),
                    new (4, 55),
                },
                new WEdge<long>[]
                {
                    new (1, 11),
                    new (3, 22),
                    new (4, 44),
                },
                new WEdge<long>[]
                {
                    new (2, 22),
                    new (4, 33),
                },
                new WEdge<long>[]
                {
                    new (2, 44),
                    new (3, 33),
                    new (1, 55),
                },
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
        public void CreateDirectedLong()
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
            var graph = WLongGraphBuilder.Create(5, cr, 6, true).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<long>[5][]
            {
                new WEdge<long>[]
                {
                    new(1, -1),
                },
                new WEdge<long>[]
                {
                    new(2, 11),
                },
                new WEdge<long>[]
                {
                    new(3, 22),
                    new(4, 44),
                },
                new WEdge<long>[]
                {
                    new(4, 33),
                },
                new WEdge<long>[]
                {
                    new(1, 55),
                },
            };
            var expectedParents = new WEdge<long>[5][]
            {
                new WEdge<long>[]
                {
                },
                new WEdge<long>[]
                {
                    new(0, -1),
                    new(4, 55),
                },
                new WEdge<long>[]
                {
                    new(1, 11),
                },
                new WEdge<long>[]
                {
                    new(2, 22),
                },
                new WEdge<long>[]
                {
                    new(2, 44),
                    new(3, 33),
                },
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }

        [Fact]
        public void CreateTreeLong()
        {
            var cr = GetReader(
                """
                1 2 -1
                2 3 11
                3 4 22
                5 2 55
                """);
            var graph = WLongGraphBuilder.CreateTree(5, cr).ToTree(0);
            graph.Length.Should().Be(5);
            graph.Root.Should().Be(0);

            var expectedChildren = new WEdge<long>[5][]
            {
                new WEdge<long>[]
                {
                    new(1, -1),
                },
                new WEdge<long>[]
                {
                    new(2, 11),
                    new(4, 55),
                },
                new WEdge<long>[]
                {
                    new(3, 22),
                },
                new WEdge<long>[]
                {
                },
                new WEdge<long>[]
                {
                },
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
        public void CreateWithEdgeIndexNonDirectedLong()
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
            var graph = WLongGraphBuilder.CreateWithEdgeIndex(5, cr, 6, false).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<long, int>[5][]
            {
                new WEdge<long, int>[]
                {
                    new (1, -1, 0),
                },
                new WEdge<long, int>[]
                {
                    new (0, -1, 0),
                    new (2, 11, 1),
                    new (4, 55, 5),
                },
                new WEdge<long, int>[]
                {
                    new (1, 11, 1),
                    new (3, 22, 2),
                    new (4, 44, 4),
                },
                new WEdge<long, int>[]
                {
                    new (2, 22, 2),
                    new (4, 33, 3),
                },
                new WEdge<long, int>[]
                {
                    new (2, 44, 4),
                    new (3, 33, 3),
                    new (1, 55, 5),
                },
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
        public void CreateWithEdgeIndexDirectedLong()
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
            var graph = WLongGraphBuilder.CreateWithEdgeIndex(5, cr, 6, true).ToGraph();
            graph.Length.Should().Be(5);

            var expectedChildren = new WEdge<long, int>[5][]
            {
                new WEdge<long, int>[]
                {
                    new(1, -1, 0),
                },
                new WEdge<long, int>[]
                {
                    new(2, 11, 1),
                },
                new WEdge<long, int>[]
                {
                    new(3, 22, 2),
                    new(4, 44, 4),
                },
                new WEdge<long, int>[]
                {
                    new(4, 33, 3),
                },
                new WEdge<long, int>[]
                {
                    new(1, 55, 5),
                },
            };
            var expectedParents = new WEdge<long, int>[5][]
            {
                new WEdge<long, int>[]
                {
                },
                new WEdge<long, int>[]
                {
                    new(0, -1, 0),
                    new(4, 55, 5),
                },
                new WEdge<long, int>[]
                {
                    new(1, 11, 1),
                },
                new WEdge<long, int>[]
                {
                    new(2, 22, 2),
                },
                new WEdge<long, int>[]
                {
                    new(2, 44, 4),
                    new(3, 33, 3),
                },
            };

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i].IsDirected.Should().BeTrue("index: {0}", i);
                graph[i].Children.Should().Equal(expectedChildren[i], "index: {0}", i);
                graph[i].Parents.Should().Equal(expectedParents[i], "index: {0}", i);
            }
        }
        #endregion long
    }
}
