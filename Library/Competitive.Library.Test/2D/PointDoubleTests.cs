using System;
using System.Linq;
using Xunit.Sdk;

namespace Kzrnm.Competitive.Testing.TwoDimensional;

using Point = PointDouble;

public class PointDoubleTests
{
    public static TheoryData<Point, Point, double, double> Distance_Data => new()
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
    public void Distance(Point p1, Point p2, double d2, double distance)
    {
        p1.Distance2(p2).ShouldBe(d2);
        p2.Distance2(p1).ShouldBe(d2);
        p1.Distance(p2).ShouldBe(distance);
        p2.Distance(p1).ShouldBe(distance);
    }

    public static Point[] SortedPoints =>
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
                SortedPoints[i].CompareTo(SortedPoints[j])
                    .ShouldBe(i.CompareTo(j), $"({SortedPoints[i]}).CompareTo(({SortedPoints[j]})) == {i}.CompareTo({j})");
    }

    public static TheoryData<Point, Point, double> Inner_Data => new()
    {
        { new (0,0), new (0,0), 0 },
        { new (0,1), new (1,0), 0 },
        { new (5,1), new (-2,10), 0 },
        { new (10,0), new (-2,10), -20 },
        { new (5,3), new (-2,-7), -31 },
    };
    [Theory]
    [MemberData(nameof(Inner_Data))]
    public void Inner(Point p1, Point p2, double expected)
    {
        p1.Inner(p2).ShouldBe(expected);
    }

    public static TheoryData<Point, Point, double> Cross_Data => new()
    {
        { new (0,0), new (0,0), 0 },
        { new (0,1), new (1,0), -1 },
        { new (5,1), new (-2,10), 52 },
        { new (10,0), new (-2,10), 100 },
        { new (5,3), new (-2,-7), -29 },
    };
    [Theory]
    [MemberData(nameof(Cross_Data))]
    public void Cross(Point p1, Point p2, double expected)
    {
        p1.Cross(p2).ShouldBe(expected);
    }

    public static TheoryData<Point[], double> Area_Data => new()
    {
        {
            new Point[]
            {
                new(1,1),
                new(2,2),
                new(1,3),
                new(-1,1),
            },
            6
        },
        {
            new Point[]
            {
                new(-1,1),
                new(1,3),
                new(2,2),
                new(1,1),
            },
            6
        },
        {
            new Point[]
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
    public void Area(Point[] points, double expected)
    {
        Point.Area2(points).ShouldBe(expected);
        Point.Area(points).ShouldBe(expected / 2.0);
    }
    public static TheoryData<Point[], int[], int[]> ConvexHull_Data => new()
    {
        {
            [
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
            ],
            [2, 4, 5, 3, 8, 0, 6, 11],
            [2, 3, 8, 0, 6, 11]
        },
        {
            [
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
                (-1,  1),
            ],
            [4, 5, 3, 9, 0, 10, 11, 12, 13],
            [4, 5, 3, 9, 0, 10, 11]
        },
        {
            [
                (0, 0),
                (0, 1),
                (0, 2),
                (0, 2),
                (1, 0),
                (1, 1),
                (1, 1),
                (1, 2),
                (2, 0),
                (2, 1),
                (2, 2),
            ],
            [0, 4, 8, 9, 10, 7, 2, 1],
            [0, 8, 10, 3]
        }
    };

    [Theory]
    [MemberData(nameof(ConvexHull_Data))]
    public void ConvexHull(Point[] points, int[] expectedNotStrict, int[] expectedStrict)
    {
        Point.ConvexHull(points).ShouldBe(expectedNotStrict);
        Point.ConvexHull(points, true).ShouldBe(expectedStrict);
    }
    public static TheoryData<Point, Point, Point, Point> 外心_Data => new()
    {
        { new (0,0), new (1,0), new (0,1), new (0.5000000000000001,0.5000000000000001) },
        { new (-11,4), new (-0.2,60), new (62,-10), new (30.86367239101717, 24.967720324589546) },
    };
    [Theory]
    [MemberData(nameof(外心_Data))]
    public void 外心(Point p1, Point p2, Point p3, Point expected)
    {
        Point.外心(p1, p2, p3).ShouldBe(expected);
    }

    public static TheoryData<Point, double, double, double, double> 直線との距離_Data => new()
    {
        { new (0,0), 1, 1, -1, 0.7071067811865475 },
        { new (0,0), -1, -1, 2, 1.414213562373095 },
        { new (1,0), -1, -1, 2, 0.7071067811865475 },
        { new (-1,0), -1, -1, 2, 2.1213203435596424 },
        { new (1,1), -1, -1, 2, 0 },
    };
    [Theory]
    [MemberData(nameof(直線との距離_Data))]
    public void 直線との距離(Point p, double a, double b, double c, double expected)
    {
        p.直線との距離(a, b, c).ShouldBe(expected);
    }

    public static TheoryData<Point, Point, SerializableTuple<double, double, double>> 直線_Data => new()
    {
        { new (0,0), new (1,1), (1, -1, 0) },
        { new (1,0), new (1,1), (1, 0, -1) },
        { new (0,1), new (1,1), (0, -1, 1) },
        { new (-1,10), new (10,2), (-8, -11, 102) },
    };
    [Theory]
    [MemberData(nameof(直線_Data))]
    public void 直線(Point p1, Point p2, SerializableTuple<double, double, double> expected)
    {
        p1.直線(p2).ShouldBe(expected.ToTuple());
    }

    public static TheoryData<Point, Point, SerializableTuple<double, double, double>> 垂直二等分線_Data => new()
    {
        { new (0,0), new (1,1), (-1, -1, 1) },
        { new (1,0), new (1,1), (0, -1, 0.5) },
        { new (0,1), new (1,1), (-1, 0, 0.5) },
        { new (-1,10), new (10,2), (-11, 8, 1.5) },
    };
    [Theory]
    [MemberData(nameof(垂直二等分線_Data))]
    public void 垂直二等分線(Point p1, Point p2, SerializableTuple<double, double, double> expected)
    {
        p1.垂直二等分線(p2).ShouldBe(expected.ToTuple());
    }

    public static TheoryData<double, double, double, double, double, double, Point> 直線と直線の交点_Data => new()
    {
        { 1, 1, 1, -1, 1, 2, new (0.5, -1.5) },
        { -1, 5, .5, -7, 7, 5.8, new (0.9107142857142857, 0.08214285714285714) },
    };
    [Theory]
    [MemberData(nameof(直線と直線の交点_Data))]
    public void 直線と直線の交点(double a, double b, double c, double u, double v, double w, Point expected)
    {
        Point.直線と直線の交点(a, b, c, u, v, w).ShouldBe(expected);
    }

    public static TheoryData<double, double, Point, SerializableTuple<double, double, double>> 直線の垂線_Data => new()
    {
        { 1, 1, new (0.5, -1.5), (1, -1, -2) },
        { 4, 7, new (-10, 2), (7, -4, 78) },
        { 0, 2, new (7, 5), (2, 0, -14) },
        { 2, 0, new (7, 5), (0, -2, 10) },
    };
    [Theory]
    [MemberData(nameof(直線の垂線_Data))]
    public void 直線の垂線(double a, double b, Point p, SerializableTuple<double, double, double> expected)
    {
        Point.直線の垂線(a, b, p).ShouldBe(expected.ToTuple());
    }

    public static TheoryData<double, double, double, Point, double, Point[]> 直線と円の交点_Data => new()
    {
        { 1, -1, 1, new (0, 0), 0.1, Array.Empty<Point>() },
        { 0, -1, 1, new (0, 0), 1, new Point[]{ new (0, 1) } },
        { 1, -1, 1, new (0, 0), 1, new Point[]{ new (-1, 0), new (0, 1) } },
    };
    [Theory]
    [MemberData(nameof(直線と円の交点_Data))]
    public void 直線と円の交点(double a, double b, double c, Point p, double r, Point[] expected)
    {
        Point.直線と円の交点(a, b, c, p, r).Order().ShouldBe(expected.Order());
    }

    public static TheoryData<Point, double, Point, double, Point[]> 円の交点_Data => new()
    {
        { new (-1, -1), 10, new (1, 2), 1, Array.Empty<Point>() },
        { new (5, 0), 5, new (1, 0), 1, new Point[]{ new (0, 0) } },
        { new (-1, 0), 1.2, new (1, 0), 1.2, new Point[]{ new (0, 0.6633249580710799), new (0, -0.6633249580710799) } },
        { new (0, 0), 1, new (1, 1), 1, new Point[]{ new (0, 1), new (1, 0) } },
        { new (-1, 0), 1, new (1, 0), 1, new Point[]{ new (0, 0) } },
        { new (-1, 0), 0.8, new (1, 0), 1, Array.Empty<Point>() },
    };
    [Theory]
    [MemberData(nameof(円の交点_Data))]
    public void 円の交点(Point p1, double r1, Point p2, double r2, Point[] expected)
    {
        Point.円の交点(p1, r1, p2, r2).Order().ShouldBe(expected.Order());
    }

    public static TheoryData<Point, double, Point, double, CirclePosition> 円の位置関係_Data => new()
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
    public void 円の位置関係(Point p1, double r1, Point p2, double r2, CirclePosition expected)
    {
        Point.円の位置関係(p1, r1, p2, r2).ShouldBe(expected);
    }

    public static TheoryData<Point, double, Point, double, double> 円の距離_Data => new()
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
    public void 円の距離(Point p1, double r1, Point p2, double r2, double expected)
    {
        Point.円の距離(p1, r1, p2, r2).ShouldBe(expected, 1e-9);
    }

    public static TheoryData<Point, Point, Point, Point, int> 線分が交差しているか_Data => new()
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
    public void 線分が交差しているか(Point a1, Point b1, Point a2, Point b2, int expected)
    {
        Point.線分が交差しているか(a1, b1, a2, b2).ShouldBe(expected);
        Point.線分が交差しているか(b1, a1, a2, b2).ShouldBe(expected);
        Point.線分が交差しているか(a1, b1, b2, a2).ShouldBe(expected);
        Point.線分が交差しているか(b1, a1, b2, a2).ShouldBe(expected);

        Point.線分が交差しているか(a2, b2, a1, b1).ShouldBe(expected);
        Point.線分が交差しているか(b2, a2, a1, b1).ShouldBe(expected);
        Point.線分が交差しているか(a2, b2, b1, a1).ShouldBe(expected);
        Point.線分が交差しているか(b2, a2, b1, a1).ShouldBe(expected);
    }

    public class 三角形に分割Data : IXunitSerializable
    {
        public Point[] Input { get; set; }
        public (Point, Point, Point)[] Expected { get; set; }

        void IXunitSerializable.Deserialize(IXunitSerializationInfo info)
        {
            Input = info.GetValue<Point[]>(nameof(Input));
            var ex = info.GetValue<Point[]>(nameof(Expected));
            Expected = ex.Chunk(3).Select(t => (t[0], t[1], t[2])).ToArray();
        }

        void IXunitSerializable.Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Input), Input);
            info.AddValue(nameof(Expected), Expected.SelectMany(t => new[] { t.Item1, t.Item2, t.Item3 }).ToArray());
        }
    }
    public static TheoryData<三角形に分割Data> 三角形に分割_Data => new()
    {
        new 三角形に分割Data {
            Input = [],
            Expected = [],
        },
        new 三角形に分割Data {
            Input = new Point[1],
            Expected = [],
        },
        new 三角形に分割Data {
            Input = new Point[2],
            Expected = [],
        },
        new 三角形に分割Data {
            Input =
            [
                new (0, 1),
                new (0, 0),
                new (1, 1),
            ],
            Expected =
            [
                (new (0, 0), new (0, 1), new (1, 1)),
            ],
        },
        new 三角形に分割Data {
            Input =
            [
                new (0, 0),
                new (10, 0),
                new (5, 5),
                new (10, 10),
                new (0, 10),
            ],
            Expected =
            [
                (new (10, 0), new (0, 0), new (0, 10)),
                (new (10, 0), new (0, 10), new (10, 10)),
                (new (5, 5), new (10, 0), new (10, 10)),
            ],
        },
    };
    [Theory]
    [MemberData(nameof(三角形に分割_Data))]
    public void 三角形に分割(三角形に分割Data d)
    {
        Point.三角形に分割(d.Input).ShouldBe(d.Expected);
    }

    [Fact]
    public void ConsoleWriter()
    {
        var utf8Wrapper = new Utf8ConsoleWriterWrapper();
        using (var cw = utf8Wrapper.GetWriter())
        {
            var arr = new Point[]
            {
                new(1.0/1, 1e-10),
                new(-1.0/3, 1e-5),
                new(-1.0/5, 1e+5),
                new(1.0/7, 1e+10),
            };
            cw.WriteLines(arr);
        }
        utf8Wrapper.Read().ShouldBe("""
        1.00000000000000000000 0.00000000010000000000
        -0.33333333333333331483 0.00001000000000000000
        -0.20000000000000001110 100000.00000000000000000000
        0.14285714285714284921 10000000000.00000000000000000000

        """.Replace("\r\n", "\n"));
    }
}
