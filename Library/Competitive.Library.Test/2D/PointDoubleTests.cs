using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.TwoDimensional
{
    public class PointDoubleTests
    {
        public static TheoryData Distance_Data => new TheoryData<PointDouble, PointDouble, double, double>
        {
            { new (0,0), new (0,0), 0, 0 },
            { new (1,1), new (1,1), 0, 0 },
            { new (0,0), new (1,1), 2, Math.Sqrt(2) },
            { new (0,0), new (-1,-1), 2, Math.Sqrt(2) },
            { new (-1,-2), new (3,5), 65, Math.Sqrt(65) },
            { new (1.2,-3.33), new (-.5,1.7), 28.190900000000003, Math.Sqrt(28.190900000000003) },
        };
        [Theory]
        [MemberData(nameof(Distance_Data))]
        public void Distance(PointDouble p1, PointDouble p2, double d2, double distance)
        {
            p1.Distance2(p2).Should().Be(d2);
            p2.Distance2(p1).Should().Be(d2);
            p1.Distance(p2).Should().Be(distance);
            p2.Distance(p1).Should().Be(distance);
        }

        public static PointDouble[] SortedPoints =>
        [
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
        ];
        [Fact]
        public void CompareTo()
        {
            for (int i = 0; i < SortedPoints.Length; i++)
                for (int j = 0; j < SortedPoints.Length; j++)
                    SortedPoints[i].CompareTo(SortedPoints[j]).Should()
                        .Be(i.CompareTo(j), "({0}).CompareTo(({1})) == {2}.CompareTo({3})",
                        SortedPoints[i], SortedPoints[j], i, j);
        }

        public static TheoryData Inner_Data => new TheoryData<PointDouble, PointDouble, long>
        {
            { new (0,0), new (0,0), 0 },
            { new (0,1), new (1,0), 0 },
            { new (5,1), new (-2,10), 0 },
            { new (10,0), new (-2,10), -20 },
            { new (5,3), new (-2,-7), -31 },
        };
        [Theory]
        [MemberData(nameof(Inner_Data))]
        public void Inner(PointDouble p1, PointDouble p2, double expected)
        {
            p1.Inner(p2).Should().Be(expected);
        }

        public static TheoryData Cross_Data => new TheoryData<PointDouble, PointDouble, long>
        {
            { new (0,0), new (0,0), 0 },
            { new (0,1), new (1,0), -1 },
            { new (5,1), new (-2,10), 52 },
            { new (10,0), new (-2,10), 100 },
            { new (5,3), new (-2,-7), -29 },
        };
        [Theory]
        [MemberData(nameof(Cross_Data))]
        public void Cross(PointDouble p1, PointDouble p2, double expected)
        {
            p1.Cross(p2).Should().Be(expected);
        }

        public static TheoryData Area_Data => new TheoryData<PointDouble[], double>
        {
            {
                new PointDouble[]
                {
                    new(1,1),
                    new(2,2),
                    new(1,3),
                    new(-1,1),
                },
                6
            },
            {
                new PointDouble[]
                {
                    new(-1,1),
                    new(1,3),
                    new(2,2),
                    new(1,1),
                },
                6
            },
            {
                new PointDouble[]
                {
                    new(1.1,0.51),
                    new(1.95,6.4423),
                    new(4.341,1.2265),
                    new(4.5,4),
                },
                9.7425693
            },
        };

        [Theory]
        [MemberData(nameof(Area_Data))]
        public void Area(PointDouble[] points, double expected)
        {
            PointDouble.Area2(points).Should().Be(expected);
            PointDouble.Area(points).Should().Be(expected / 2.0);
        }

        [Fact]
        public void ConvexHull1()
        {
            var points = new PointDouble[]
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
            PointDouble.ConvexHull(points)
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
            var points = new PointDouble[]
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
            PointDouble.ConvexHull(points)
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

        public static TheoryData 外心_Data => new TheoryData<PointDouble, PointDouble, PointDouble, PointDouble>
        {
            { new (0,0), new (1,0), new (0,1), new (0.5000000000000001,0.5000000000000001) },
            { new (-11,4), new (-0.2,60), new (62,-10), new (30.86367239101717, 24.967720324589546) },
        };
        [Theory]
        [MemberData(nameof(外心_Data))]
        public void 外心(PointDouble p1, PointDouble p2, PointDouble p3, PointDouble expected)
        {
            PointDouble.外心(p1, p2, p3).Should().Be(expected);
        }

        public static TheoryData 直線との距離_Data => new TheoryData<PointDouble, double, double, double, double>
        {
            { new (0,0), 1, 1, -1, 0.7071067811865475 },
            { new (0,0), -1, -1, 2, 1.414213562373095 },
            { new (1,0), -1, -1, 2, 0.7071067811865475 },
            { new (-1,0), -1, -1, 2, 2.1213203435596424 },
            { new (1,1), -1, -1, 2, 0 },
        };
        [Theory]
        [MemberData(nameof(直線との距離_Data))]
        public void 直線との距離(PointDouble p, double a, double b, double c, double expected)
        {
            p.直線との距離(a, b, c).Should().Be(expected);
        }

        public static TheoryData 直線_Data => new TheoryData<PointDouble, PointDouble, (double, double, double)>
        {
            { new (0,0), new (1,1), (1, -1, 0) },
            { new (1,0), new (1,1), (1, 0, -1) },
            { new (0,1), new (1,1), (0, -1, 1) },
            { new (-1,10), new (10,2), (-8, -11, 102) },
        };
        [Theory]
        [MemberData(nameof(直線_Data))]
        public void 直線(PointDouble p1, PointDouble p2, (double, double, double) expected)
        {
            p1.直線(p2).Should().Be(expected);
        }

        public static TheoryData 垂直二等分線_Data => new TheoryData<PointDouble, PointDouble, (double, double, double)>
        {
            { new (0,0), new (1,1), (-1, -1, 1) },
            { new (1,0), new (1,1), (0, -1, 0.5) },
            { new (0,1), new (1,1), (-1, 0, 0.5) },
            { new (-1,10), new (10,2), (-11, 8, 1.5) },
        };
        [Theory]
        [MemberData(nameof(垂直二等分線_Data))]
        public void 垂直二等分線(PointDouble p1, PointDouble p2, (double, double, double) expected)
        {
            p1.垂直二等分線(p2).Should().Be(expected);
        }

        public static TheoryData 直線と直線の交点_Data => new TheoryData<double, double, double, double, double, double, PointDouble>
        {
            { 1, 1, 1, -1, 1, 2, new (0.5, -1.5) },
            { -1, 5, .5, -7, 7, 5.8, new (0.9107142857142857, 0.08214285714285714) },
        };
        [Theory]
        [MemberData(nameof(直線と直線の交点_Data))]
        public void 直線と直線の交点(double a, double b, double c, double u, double v, double w, PointDouble expected)
        {
            PointDouble.直線と直線の交点(a, b, c, u, v, w).Should().Be(expected);
        }

        public static TheoryData 直線の垂線_Data => new TheoryData<double, double, PointDouble, (double, double, double)>
        {
            { 1, 1, new (0.5, -1.5), (1, -1, -2) },
            { 4, 7, new (-10, 2), (7, -4, 78) },
            { 0, 2, new (7, 5), (2, 0, -14) },
            { 2, 0, new (7, 5), (0, -2, 10) },
        };
        [Theory]
        [MemberData(nameof(直線の垂線_Data))]
        public void 直線の垂線(double a, double b, PointDouble p, (double, double, double) expected)
        {
            PointDouble.直線の垂線(a, b, p).Should().Be(expected);
        }

        public static TheoryData 直線と円の交点_Data => new TheoryData<double, double, double, PointDouble, double, PointDouble[]>
        {
            { 1, -1, 1, new (0, 0), 0.1, Array.Empty<PointDouble>() },
            { 0, -1, 1, new (0, 0), 1, new PointDouble[]{ new (0, 1) } },
            { 1, -1, 1, new (0, 0), 1, new PointDouble[]{ new (-1, 0), new (0, 1) } },
        };
        [Theory]
        [MemberData(nameof(直線と円の交点_Data))]
        public void 直線と円の交点(double a, double b, double c, PointDouble p, double r, PointDouble[] expected)
        {
            PointDouble.直線と円の交点(a, b, c, p, r).Should().BeEquivalentTo(expected);
        }

        public static TheoryData 円の交点_Data => new TheoryData<PointDouble, double, PointDouble, double, PointDouble[]>
        {
            { new (-1, -1), 10, new (1, 2), 1, Array.Empty<PointDouble>() },
            { new (5, 0), 5, new (1, 0), 1, new PointDouble[]{ new (0, 0) } },
            { new (-1, 0), 1.2, new (1, 0), 1.2, new PointDouble[]{ new (0, 0.6633249580710799), new (0, -0.6633249580710799) } },
            { new (0, 0), 1, new (1, 1), 1, new PointDouble[]{ new (0, 1), new (1, 0) } },
            { new (-1, 0), 1, new (1, 0), 1, new PointDouble[]{ new (0, 0) } },
            { new (-1, 0), 0.8, new (1, 0), 1, Array.Empty<PointDouble>() },
        };
        [Theory]
        [MemberData(nameof(円の交点_Data))]
        public void 円の交点(PointDouble p1, double r1, PointDouble p2, double r2, PointDouble[] expected)
        {
            PointDouble.円の交点(p1, r1, p2, r2).Should().BeEquivalentTo(expected);
        }

        public static TheoryData 円の位置関係_Data => new TheoryData<PointDouble, double, PointDouble, double, CirclePosition>
        {
            { new (-1, -1), 10, new (1, 2), 1, CirclePosition.Inner },
            { new (5, 0), 5, new (1, 0), 1, CirclePosition.Inscribed },
            { new (-1, 0), 1.2, new (1, 0), 1.2, CirclePosition.Intersected },
            { new (0, 0), 1, new (1, 1), 1, CirclePosition.Intersected },
            { new (-1, 0), 1, new (1, 0), 1, CirclePosition.Circumscribed },
            { new (-1, 0), 0.8, new (1, 0), 1, CirclePosition.Separated },
        };
        [Theory]
        [MemberData(nameof(円の位置関係_Data))]
        public void 円の位置関係(PointDouble p1, double r1, PointDouble p2, double r2, CirclePosition expected)
        {
            PointDouble.円の位置関係(p1, r1, p2, r2).Should().Be(expected);
        }

        public static TheoryData 円の距離_Data => new TheoryData<PointDouble, double, PointDouble, double, double>
        {
            { new (-1, -1), 10, new (1, 2), 1, 5.39444872453601 },
            { new (5, 0), 5, new (1, 0), 1, 0 },
            { new (-1, 0), 1.2, new (1, 0), 1.2, 0 },
            { new (0, 0), 1, new (1, 1), 1, 0 },
            { new (-1, 0), 1, new (1, 0), 1, 0 },
            { new (-1, 0), 0.8, new (1, 0), 1, 0.2 },
        };
        [Theory]
        [MemberData(nameof(円の距離_Data))]
        public void 円の距離(PointDouble p1, double r1, PointDouble p2, double r2, double expected)
        {
            PointDouble.円の距離(p1, r1, p2, r2).Should().BeApproximately(expected, 1e-9);
        }

        public static TheoryData 線分が交差しているか_Data => new TheoryData<PointDouble, PointDouble, PointDouble, PointDouble, int>
        {
            { new (-1, -1), new (1, 1), new (-1, 0), new (0, 0.001), -1 },
            { new (-1, -1), new (1, 1), new (-1, 0), new (0, -0.001), 1 },
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
        public void 線分が交差しているか(PointDouble a1, PointDouble b1, PointDouble a2, PointDouble b2, int expected)
        {
            PointDouble.線分が交差しているか(a1, b1, a2, b2).Should().Be(expected);
            PointDouble.線分が交差しているか(b1, a1, a2, b2).Should().Be(expected);
            PointDouble.線分が交差しているか(a1, b1, b2, a2).Should().Be(expected);
            PointDouble.線分が交差しているか(b1, a1, b2, a2).Should().Be(expected);

            PointDouble.線分が交差しているか(a2, b2, a1, b1).Should().Be(expected);
            PointDouble.線分が交差しているか(b2, a2, a1, b1).Should().Be(expected);
            PointDouble.線分が交差しているか(a2, b2, b1, a1).Should().Be(expected);
            PointDouble.線分が交差しているか(b2, a2, b1, a1).Should().Be(expected);
        }

        public static TheoryData 三角形に分割_Data => new TheoryData<PointDouble[], (PointDouble, PointDouble, PointDouble)[]>
        {
            {
                new PointDouble[0],
                []
            },
            {
                new PointDouble[1],
                []
            },
            {
                new PointDouble[2],
                []
            },
            {
                new PointDouble[3]
                {
                    new (0, 1),
                    new (0, 0),
                    new (1, 1),
                },
                new (PointDouble, PointDouble, PointDouble)[]
                {
                    (new (0, 0), new (0, 1), new (1, 1)),
                }
            },
            {
                new PointDouble[5]
                {
                    new (0, 0),
                    new (10, 0),
                    new (5, 5),
                    new (10, 10),
                    new (0, 10),
                },
                new (PointDouble, PointDouble, PointDouble)[]
                {
                    (new (10, 0), new (0, 0), new (0, 10)),
                    (new (10, 0), new (0, 10), new (10, 10)),
                    (new (5, 5), new (10, 0), new (10, 10)),
                }
            },
        };
        [Theory]
        [MemberData(nameof(三角形に分割_Data))]
        public void 三角形に分割(PointDouble[] s, (PointDouble, PointDouble, PointDouble)[] expected)
        {
            PointDouble.三角形に分割(s).Should().Equal(expected);
        }

        [Fact]
        public void ConsoleWriter()
        {
            var utf8Wrapper = new Utf8ConsoleWriterWrapper();
            using (var cw = utf8Wrapper.GetWriter())
            {
                var arr = new PointDouble[]
                {
                    new(1.0/1, 1e-10),
                    new(-1.0/3, 1e-5),
                    new(-1.0/5, 1e+5),
                    new(1.0/7, 1e+10),
                };
                cw.WriteLines(arr);
            }
            utf8Wrapper.Read().Should().Be("""
            1.00000000000000000000 0.00000000010000000000
            -0.33333333333333331483 0.00001000000000000000
            -0.20000000000000001110 100000.00000000000000000000
            0.14285714285714284921 10000000000.00000000000000000000

            """.Replace("\r\n", "\n"));
        }
    }
}
