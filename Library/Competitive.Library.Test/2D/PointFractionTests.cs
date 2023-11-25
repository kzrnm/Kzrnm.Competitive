using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.TwoDimensional
{
    public class PointFractionTests
    {
        public static TheoryData Distance_Data => new TheoryData<PointFraction, PointFraction, Fraction, double>
        {
            { new (0,0), new (0,0), 0, 0 },
            { new (1,1), new (1,1), 0, 0 },
            { new (0,0), new (1,1), 2, Math.Sqrt(2) },
            { new (0,0), new (-1,-1), 2, Math.Sqrt(2) },
            { new (-1,-2), new (3,5), 65, Math.Sqrt(65) },
        };
        [Theory]
        [MemberData(nameof(Distance_Data))]
        public void Distance(PointFraction p1, PointFraction p2, Fraction d2, double distance)
        {
            p1.Distance2(p2).Should().Be(d2);
            p2.Distance2(p1).Should().Be(d2);
            p1.Distance(p2).Should().Be(distance);
            p2.Distance(p1).Should().Be(distance);
        }

        public static PointFraction[] SortedPoints => new PointFraction[]
        {
            new (0, 0),
            new (1, 0),
            new (5, 0),
            new (100000, 1),
            new (1, 1),
            new (1, 10000),
            new (0, 1),
            new (0, 55),
            new (-1, 10000),
            new (-1, 1),
            new (-1500, 1),
            new (-10000000, 1),
            new (-1, 0),
            new (-2, 0),
            new (-500, 0),
            new (-500, -1),
            new (-1, -1),
            new (-2, -2),
            new (-1, -50000),
            new (0, -1),
            new (0, -2000),
            new (1, -10),
            new (100000, -1),
        };
        [Fact]
        public void CompareTo()
        {
            for (int i = 0; i < SortedPoints.Length; i++)
                for (int j = 0; j < SortedPoints.Length; j++)
                    SortedPoints[i].CompareTo(SortedPoints[j]).Should()
                        .Be(i.CompareTo(j), "({0}).CompareTo(({1})) == {2}.CompareTo({3})",
                        SortedPoints[i], SortedPoints[j], i, j);
        }

        public static TheoryData Inner_Data => new TheoryData<PointFraction, PointFraction, Fraction>
        {
            { new (0,0), new (0,0), 0 },
            { new (0,1), new (1,0), 0 },
            { new (5,1), new (-2,10), 0 },
            { new (10,0), new (-2,10), -20 },
            { new (5,3), new (-2,-7), -31 },
        };
        [Theory]
        [MemberData(nameof(Inner_Data))]
        public void Inner(PointFraction p1, PointFraction p2, Fraction expected)
        {
            p1.Inner(p2).Should().Be(expected);
        }

        public static TheoryData Cross_Data => new TheoryData<PointFraction, PointFraction, Fraction>
        {
            { new (0,0), new (0,0), 0 },
            { new (0,1), new (1,0), -1 },
            { new (5,1), new (-2,10), 52 },
            { new (10,0), new (-2,10), 100 },
            { new (5,3), new (-2,-7), -29 },
        };
        [Theory]
        [MemberData(nameof(Cross_Data))]
        public void Cross(PointFraction p1, PointFraction p2, Fraction expected)
        {
            p1.Cross(p2).Should().Be(expected);
        }

        public static TheoryData Area_Data => new TheoryData<PointFraction[], Fraction>
        {
            {
                new PointFraction[]
                {
                    new(1,1),
                    new(2,2),
                    new(1,3),
                    new(-1,1),
                },
                6
            },
            {
                new PointFraction[]
                {
                    new(-1,1),
                    new(1,3),
                    new(2,2),
                    new(1,1),
                },
                6
            },
            {
                new PointFraction[]
                {
                    new(new(11,10),new(51,100)),
                    new(new(195,100),new(64423,10000)),
                    new(new(4341,1000),new(12265,10000)),
                    new(new(9,2),4),
                },
                new(97425693,10000000)
            },
        };

        [Theory]
        [MemberData(nameof(Area_Data))]
        public void Area(PointFraction[] points, Fraction expected)
        {
            PointFraction.Area2(points).Should().Be(expected);
            PointFraction.Area(points).Should().Be(expected / 2);
        }

        [Fact]
        public void ConvexHull1()
        {
            var points = new PointFraction[]
            {
                (100000000, 100000000),
                ( 80000000,  90000000),
                ( 10000000, -10000000),
                (100000000,  80000000),
                ( 40000000,  20000000),
                ( 80000000,  60000000),
                ( 40000000,  80000000),
                ( 10000000,  50000000),
                (110000000, 100000000),
                ( 60000000,  80000000),
                ( 80000000,  80000000),
                (-10000000,  50000000),
            };
            PointFraction.ConvexHull(points)
                .Select(i => points[i])
                .Should().Equal(
                    (10000000, -10000000),
                    (40000000, 20000000),
                    (80000000, 60000000),
                    (100000000, 80000000),
                    (110000000, 100000000),
                    (100000000, 100000000),
                    (40000000, 80000000),
                    (-10000000, 50000000)
                );
        }

        [Fact]
        public void ConvexHull2()
        {
            var points = new PointFraction[]
            {
                (10, 10),
                ( 8,  9),
                ( 8,  8),
                (10,  8),
                ( 0,  0),
                ( 4,  0),
                ( 8,  6),
                ( 1,  5),
                ( 6,  8),
                (11, 10),
                ( 1,  8),
                (-6,  6),
                (-4,  4),
                (-1,  1)
            };
            PointFraction.ConvexHull(points)
                .Select(i => points[i])
                .Should().Equal(
                    (0, 0),
                    (4, 0),
                    (10, 8),
                    (11, 10),
                    (10, 10),
                    (1, 8),
                    (-6, 6),
                    (-4, 4),
                    (-1, 1)
                );
        }

        public static TheoryData 直線との距離_Data => new TheoryData<PointFraction, Fraction, Fraction, Fraction, double>
        {
            { new (0,0), 1, 1, -1, 0.7071067811865475 },
            { new (0,0), -1, -1, 2, 1.414213562373095 },
            { new (1,0), -1, -1, 2, 0.7071067811865475 },
            { new (-1,0), -1, -1, 2, 2.1213203435596424 },
            { new (1,1), -1, -1, 2, 0 },
        };
        [Theory]
        [MemberData(nameof(直線との距離_Data))]
        public void 直線との距離(PointFraction p, Fraction a, Fraction b, Fraction c, double expected)
        {
            p.直線との距離(a, b, c).Should().Be(expected);
        }

        public static TheoryData 直線_Data => new TheoryData<PointFraction, PointFraction, (Fraction, Fraction, Fraction)>
        {
            { new (0,0), new (1,1), (1, -1, 0) },
            { new (1,0), new (1,1), (1, 0, -1) },
            { new (0,1), new (1,1), (0, -1, 1) },
            { new (-1,10), new (10,2), (-8, -11, 102) },
        };
        [Theory]
        [MemberData(nameof(直線_Data))]
        public void 直線(PointFraction p1, PointFraction p2, (Fraction, Fraction, Fraction) expected)
        {
            p1.直線(p2).Should().Be(expected);
        }

        public static TheoryData 垂直二等分線_Data => new TheoryData<PointFraction, PointFraction, (Fraction, Fraction, Fraction)>
        {
            { new (0,0), new (1,1), (-1, -1, 1) },
            { new (1,0), new (1,1), (0, -1, new(1,2)) },
            { new (0,1), new (1,1), (-1, 0, new(1,2)) },
            { new (-1,10), new (10,2), (-11, 8, new(3,2)) },
        };
        [Theory]
        [MemberData(nameof(垂直二等分線_Data))]
        public void 垂直二等分線(PointFraction p1, PointFraction p2, (Fraction, Fraction, Fraction) expected)
        {
            p1.垂直二等分線(p2).Should().Be(expected);
        }

        public static TheoryData 直線と直線の交点_Data => new TheoryData<Fraction, Fraction, Fraction, Fraction, Fraction, Fraction, PointFraction>
        {
            { 1, 1, 1, -1, 1, 2, new(new(1,2), new(-3,2)) },
            { -1, 5, new(1,2), -7, 7, new(58,10), new(new(51,56), new(23,280)) },
        };
        [Theory]
        [MemberData(nameof(直線と直線の交点_Data))]
        public void 直線と直線の交点(Fraction a, Fraction b, Fraction c, Fraction u, Fraction v, Fraction w, PointFraction expected)
        {
            PointFraction.直線と直線の交点(a, b, c, u, v, w).Should().Be(expected);
        }

        public static TheoryData 直線の垂線_Data => new TheoryData<Fraction, Fraction, PointFraction, (Fraction, Fraction, Fraction)>
        {
            { 1, 1, new (new(1,2), new(-3,2)), (1, -1, -2) },
            { 4, 7, new (-10, 2), (7, -4, 78) },
            { 0, 2, new (7, 5), (2, 0, -14) },
            { 2, 0, new (7, 5), (0, -2, 10) },
        };
        [Theory]
        [MemberData(nameof(直線の垂線_Data))]
        public void 直線の垂線(Fraction a, Fraction b, PointFraction p, (Fraction, Fraction, Fraction) expected)
        {
            PointFraction.直線の垂線(a, b, p).Should().Be(expected);
        }

        public static TheoryData 円の位置関係_Data => new TheoryData<PointFraction, Fraction, PointFraction, Fraction, CirclePosition>
        {
            { new (-1, -1), 10, new (1, 2), 1, CirclePosition.Inner },
            { new (5, 0), 5, new (1, 0), 1, CirclePosition.Inscribed },
            { new (-1, 0), new(12,10), new (1, 0), new(12,10), CirclePosition.Intersected },
            { new (0, 0), 1, new (1, 1), 1, CirclePosition.Intersected },
            { new (-1, 0), 1, new (1, 0), 1, CirclePosition.Circumscribed },
            { new (-1, 0), new(8,10), new (1, 0), 1, CirclePosition.Separated },
        };
        [Theory]
        [MemberData(nameof(円の位置関係_Data))]
        public void 円の位置関係(PointFraction p1, Fraction r1, PointFraction p2, Fraction r2, CirclePosition expected)
        {
            PointFraction.円の位置関係(p1, r1, p2, r2).Should().Be(expected);
        }

        public static TheoryData 線分が交差しているか_Data => new TheoryData<PointFraction, PointFraction, PointFraction, PointFraction, int>
        {
            { new (-1, -1), new (1, 1), new (-1, 0), new (0, new(1,1000)), -1 },
            { new (-1, -1), new (1, 1), new (-1, 0), new (0, new(-1,1000)), 1 },
            { new (-1, -1), new (1, 1), new (-1, 0), new (0, 0), 0 },

            { new (-1, -1), new (1, 1), new (2, 2), new (3, 3), -1 },
            { new (-1, -1), new (1, 1), new (0, 0), new (3, 3), 1 },
            { new (-1, -1), new (1, 1), new (1, 1), new (3, 3), 0 },

            { new (-1, -1), new (-1, 1), new (-1, 2), new (-1, 3), -1 },
            { new (-1, -1), new (-1, 1), new (-1, 0), new (-1, 3), 1 },
            { new (-1, -1), new (-1, 1), new (-1, 1), new (-1, 3), 0 },

            { new (-1, 1), new (1, 1), new (2, 1), new (3, 1), -1 },
            { new (-1, 1), new (1, 1), new (0, 1), new (3, 1), 1 },
            { new (-1, 1), new (1, 1), new (1, 1), new (3, 1), 0 },
        };
        [Theory]
        [MemberData(nameof(線分が交差しているか_Data))]
        public void 線分が交差しているか(PointFraction a1, PointFraction b1, PointFraction a2, PointFraction b2, int expected)
        {
            PointFraction.線分が交差しているか(a1, b1, a2, b2).Should().Be(expected);
            PointFraction.線分が交差しているか(b1, a1, a2, b2).Should().Be(expected);
            PointFraction.線分が交差しているか(a1, b1, b2, a2).Should().Be(expected);
            PointFraction.線分が交差しているか(b1, a1, b2, a2).Should().Be(expected);

            PointFraction.線分が交差しているか(a2, b2, a1, b1).Should().Be(expected);
            PointFraction.線分が交差しているか(b2, a2, a1, b1).Should().Be(expected);
            PointFraction.線分が交差しているか(a2, b2, b1, a1).Should().Be(expected);
            PointFraction.線分が交差しているか(b2, a2, b1, a1).Should().Be(expected);
        }

        public static TheoryData 三角形に分割_Data => new TheoryData<PointFraction[], (PointFraction, PointFraction, PointFraction)[]>
        {
            {
                new PointFraction[0],
                new (PointFraction, PointFraction, PointFraction)[0]
            },
            {
                new PointFraction[1],
                new (PointFraction, PointFraction, PointFraction)[0]
            },
            {
                new PointFraction[2],
                new (PointFraction, PointFraction, PointFraction)[0]
            },
            {
                new PointFraction[3]
                {
                    new (0, 1),
                    new (0, 0),
                    new (1, 1),
                },
                new (PointFraction, PointFraction, PointFraction)[]
                {
                    (new (0, 0), new (0, 1), new (1, 1)),
                }
            },
            {
                new PointFraction[5]
                {
                    new (0, 0),
                    new (10, 0),
                    new (5, 5),
                    new (10, 10),
                    new (0, 10),
                },
                new (PointFraction, PointFraction, PointFraction)[]
                {
                    (new (10, 0), new (0, 0), new (0, 10)),
                    (new (10, 0), new (0, 10), new (10, 10)),
                    (new (5, 5), new (10, 0), new (10, 10)),
                }
            },
        };
        [Theory]
        [MemberData(nameof(三角形に分割_Data))]
        public void 三角形に分割(PointFraction[] s, (PointFraction, PointFraction, PointFraction)[] expected)
        {
            PointFraction.三角形に分割(s).Should().Equal(expected);
        }

        [Fact]
        public void ConsoleWriter()
        {
            var utf8Wrapper = new Utf8ConsoleWriterWrapper();
            using (var cw = utf8Wrapper.GetWriter())
            {
                var arr = new PointFraction[]
                {
                    new(1,2),
                    new(new(1,2),new(4,3)),
                };
                cw.WriteLines(arr);
            }
            utf8Wrapper.Read().Should().Be("""
            1/1 2/1
            1/2 4/3

            """.Replace("\r\n", "\n"));
        }
    }
}
