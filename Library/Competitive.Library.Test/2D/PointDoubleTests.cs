
namespace Kzrnm.Competitive.Testing.TwoDimensional;

using Point = PointDouble;

public class PointDoubleTests
{
    public readonly record struct LineEquation(double A, double B, double C)
    {
        public static implicit operator LineEquation((double A, double B, double C) e) => new(e.A, e.B, e.C);
        public (double A, double B, double C) ToTuple() => (A, B, C);
    }

    public static IEnumerable<(Point, Point, double, double)> Distance_Data => [
        (new (0,0), new (0,0), 0, 0),
        (new (1,1), new (1,1), 0, 0),
        (new (0,0), new (1,1), 2, Math.Sqrt(2)),
        (new (0,0), new (-1,-1), 2, Math.Sqrt(2)),
        (new (-1,-2), new (3,5), 65, Math.Sqrt(65)),
        (new (1.2,-3.33), new (-.5,1.7), 28.190900000000003, Math.Sqrt(28.190900000000003)) ,
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Distance_Data))]
    public async Task Distance(Point p1, Point p2, double d2, double distance)
    {
        await p1.Distance2(p2).Should().BeEqualTo(d2);
        await p2.Distance2(p1).Should().BeEqualTo(d2);
        await p1.Distance(p2).Should().BeEqualTo(distance);
        await p2.Distance(p1).Should().BeEqualTo(distance);
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
    [Test, MultipleAssertions]
    public async Task CompareTo()
    {
        for (int i = 0; i < SortedPoints.Length; i++)
            for (int j = 0; j < SortedPoints.Length; j++)
                await SortedPoints[i].CompareTo(SortedPoints[j]).Should().BeEqualTo(i.CompareTo(j));
    }

    public static IEnumerable<(Point, Point, double)> Inner_Data => [
        (new (0,0), new (0,0), 0),
        (new (0,1), new (1,0), 0),
        (new (5,1), new (-2,10), 0),
        (new (10,0), new (-2,10), -20),
        (new (5,3), new (-2,-7), -31),
    ];
    [Test]
    [MethodDataSource(nameof(Inner_Data))]
    public async Task Inner(Point p1, Point p2, double expected)
    {
        await p1.Inner(p2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(Point, Point, double)> Cross_Data => [
        (new (0,0), new (0,0), 0),
        (new (0,1), new (1,0), -1),
        (new (5,1), new (-2,10), 52),
        (new (10,0), new (-2,10), 100),
        (new (5,3), new (-2,-7), -29),
    ];
    [Test]
    [MethodDataSource(nameof(Cross_Data))]
    public async Task Cross(Point p1, Point p2, double expected)
    {
        await p1.Cross(p2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(Point[], double)> Area_Data => [
        (
            new Point[]
            {
                new(1,1),
                new(2,2),
                new(1,3),
                new(-1,1),
            },
            6
        ),
        (
            new Point[]
            {
                new(-1,1),
                new(1,3),
                new(2,2),
                new(1,1),
            },
            6
        ),
        (
            new Point[]
            {
                new(1.1,0.51),
                new(1.95,6.4423),
                new(4.341,1.2265),
                new(4.5,4),
            },
            9.7425693
        ),
    ];

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Area_Data))]
    public async Task Area(Point[] points, double expected)
    {
        await Point.Area2(points).Should().BeEqualTo(expected);
        await Point.Area(points).Should().BeEqualTo(expected / 2.0);
    }
    public static IEnumerable<(Point[], int[], int[])> ConvexHull_Data => [
        (
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
        ),
        (
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
        ),
        (
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
        )
    ];

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(ConvexHull_Data))]
    public async Task ConvexHull(Point[] points, int[] expectedNotStrict, int[] expectedStrict)
    {
        await Point.ConvexHull(points).Should().BeStrictlyEquivalentTo(expectedNotStrict);
        await Point.ConvexHull(points, true).Should().BeStrictlyEquivalentTo(expectedStrict);
    }
    public static IEnumerable<(Point, Point, Point, Point)> 外心_Data => [
        (new (0,0), new (1,0), new (0,1), new (0.5000000000000001,0.5000000000000001)),
        (new (-11,4), new (-0.2,60), new (62,-10), new (30.86367239101717, 24.967720324589546)),
    ];
    [Test]
    [MethodDataSource(nameof(外心_Data))]
    public async Task 外心(Point p1, Point p2, Point p3, Point expected)
    {
        await Point.外心(p1, p2, p3).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(Point, double, double, double, double)> 直線との距離_Data => [
        (new (0,0), 1, 1, -1, 0.7071067811865475),
        (new (0,0), -1, -1, 2, 1.414213562373095),
        (new (1,0), -1, -1, 2, 0.7071067811865475),
        (new (-1,0), -1, -1, 2, 2.1213203435596424),
        (new (1,1), -1, -1, 2, 0),
    ];
    [Test]
    [MethodDataSource(nameof(直線との距離_Data))]
    public async Task 直線との距離(Point p, double a, double b, double c, double expected)
    {
        await p.直線との距離(a, b, c).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(Point, Point, LineEquation)> 直線_Data => [
        (new (0,0), new (1,1), (1, -1, 0)),
        (new (1,0), new (1,1), (1, 0, -1)),
        (new (0,1), new (1,1), (0, -1, 1)),
        (new (-1,10), new (10,2), (-8, -11, 102)),
    ];
    [Test]
    [MethodDataSource(nameof(直線_Data))]
    public async Task 直線(Point p1, Point p2, LineEquation expected)
    {
        await p1.直線(p2).Should().BeEqualTo(expected.ToTuple());
    }

    public static IEnumerable<(Point, Point, LineEquation)> 垂直二等分線_Data => [
        (new (0,0), new (1,1), (-1, -1, 1)),
        (new (1,0), new (1,1), (0, -1, 0.5)),
        (new (0,1), new (1,1), (-1, 0, 0.5)),
        (new (-1,10), new (10,2), (-11, 8, 1.5)),
    ];
    [Test]
    [MethodDataSource(nameof(垂直二等分線_Data))]
    public async Task 垂直二等分線(Point p1, Point p2, LineEquation expected)
    {
        await p1.垂直二等分線(p2).Should().BeEqualTo(expected.ToTuple());
    }

    public static IEnumerable<(double, double, double, double, double, double, Point)> 直線と直線の交点_Data => [
        (1, 1, 1, -1, 1, 2, new (0.5, -1.5)),
        (-1, 5, .5, -7, 7, 5.8, new (0.9107142857142857, 0.08214285714285714)),
    ];
    [Test]
    [MethodDataSource(nameof(直線と直線の交点_Data))]
    public async Task 直線と直線の交点(double a, double b, double c, double u, double v, double w, Point expected)
    {
        await Point.直線と直線の交点(a, b, c, u, v, w).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(double, double, Point, LineEquation)> 直線の垂線_Data => [
        (1, 1, new (0.5, -1.5), (1, -1, -2)),
        (4, 7, new (-10, 2), (7, -4, 78)),
        (0, 2, new (7, 5), (2, 0, -14)),
        (2, 0, new (7, 5), (0, -2, 10)),
    ];
    [Test]
    [MethodDataSource(nameof(直線の垂線_Data))]
    public async Task 直線の垂線(double a, double b, Point p, LineEquation expected)
    {
        await Point.直線の垂線(a, b, p).Should().BeEqualTo(expected.ToTuple());
    }

    public static IEnumerable<(double, double, double, Point, double, Point[])> 直線と円の交点_Data => [
        (1, -1, 1, new (0, 0), 0.1, Array.Empty<Point>()),
        (0, -1, 1, new (0, 0), 1, new Point[]{ new (0, 1) }),
        (1, -1, 1, new (0, 0), 1, new Point[]{ new (-1, 0), new (0, 1) }),
    ];
    [Test]
    [MethodDataSource(nameof(直線と円の交点_Data))]
    public async Task 直線と円の交点(double a, double b, double c, Point p, double r, Point[] expected)
    {
        await Point.直線と円の交点(a, b, c, p, r).Order().Should().BeStrictlyEquivalentTo(expected.Order());
    }

    public static IEnumerable<(Point, double, Point, double, Point[])> 円の交点_Data => [
        (new (-1, -1), 10, new (1, 2), 1, Array.Empty<Point>()),
        (new (5, 0), 5, new (1, 0), 1, new Point[]{ new (0, 0) }),
        (new (-1, 0), 1.2, new (1, 0), 1.2, new Point[]{ new (0, 0.6633249580710799), new (0, -0.6633249580710799) }),
        (new (0, 0), 1, new (1, 1), 1, new Point[]{ new (0, 1), new (1, 0) }),
        (new (-1, 0), 1, new (1, 0), 1, new Point[]{ new (0, 0) }),
        (new (-1, 0), 0.8, new (1, 0), 1, Array.Empty<Point>()),
    ];
    [Test]
    [MethodDataSource(nameof(円の交点_Data))]
    public async Task 円の交点(Point p1, double r1, Point p2, double r2, Point[] expected)
    {
        await Point.円の交点(p1, r1, p2, r2).Order().Should().BeStrictlyEquivalentTo(expected.Order());
    }

    public static IEnumerable<(Point, double, Point, double, CirclePosition)> 円の位置関係_Data => [
        (new (-1, -1), 10, new (1, 2), 1, CirclePosition.Inner),
        (new (5, 0), 5, new (1, 0), 1, CirclePosition.Inscribed),
        (new (-1, 0), 1.2, new (1, 0), 1.2, CirclePosition.Intersected),
        (new (0, 0), 1, new (1, 1), 1, CirclePosition.Intersected),
        (new (-1, 0), 1, new (1, 0), 1, CirclePosition.Circumscribed),
        (new (-1, 0), 0.8, new (1, 0), 1, CirclePosition.Separated),
    ];
    [Test]
    [MethodDataSource(nameof(円の位置関係_Data))]
    public async Task 円の位置関係(Point p1, double r1, Point p2, double r2, CirclePosition expected)
    {
        await Point.円の位置関係(p1, r1, p2, r2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(Point, double, Point, double, double)> 円の距離_Data => [
        (new (-1, -1), 10, new (1, 2), 1, 5.39444872453601),
        (new (5, 0), 5, new (1, 0), 1, 0),
        (new (-1, 0), 1.2, new (1, 0), 1.2, 0),
        (new (0, 0), 1, new (1, 1), 1, 0),
        (new (-1, 0), 1, new (1, 0), 1, 0),
        (new (-1, 0), 0.8, new (1, 0), 1, 0.2),
    ];
    [Test]
    [MethodDataSource(nameof(円の距離_Data))]
    public async Task 円の距離(Point p1, double r1, Point p2, double r2, double expected)
    {
        await Point.円の距離(p1, r1, p2, r2).Should().BeCloseTo(expected, 1e-9);
    }

    public static IEnumerable<(Point, Point, Point, Point, int)> 線分が交差しているか_Data => [
        (new (-1, -1), new (1, 1), new (-1, 0), new (0, 0.001), -1),
        (new (-1, -1), new (1, 1), new (-1, 0), new (0, -0.001), 1),
        (new (-1, -1), new (1, 1), new (-1, 0), new (0, 0), 0),

        (new (-1, -1), new (1, 1), new (2, 2), new (3, 3), -1),
        (new (-1, -1), new (1, 1), new (0, 0), new (3, 3), 1),
        (new (-1, -1), new (1, 1), new (1, 1), new (3, 3), 0),

        (new (-1, -1), new (-1, 1), new (-1, 2), new (-1, 3), -1),
        (new (-1, -1), new (-1, 1), new (-1, 0), new (-1, 3), 1),
        (new (-1, -1), new (-1, 1), new (-1, 1), new (-1, 3), 0),

        (new (-1, 1), new (1, 1), new (2, 1), new (3, 1), -1),
        (new (-1, 1), new (1, 1), new (0, 1), new (3, 1), 1),
        (new (-1, 1), new (1, 1), new (1, 1), new (3, 1), 0),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(線分が交差しているか_Data))]
    public async Task 線分が交差しているか(Point a1, Point b1, Point a2, Point b2, int expected)
    {
        await Point.線分が交差しているか(a1, b1, a2, b2).Should().BeEqualTo(expected);
        await Point.線分が交差しているか(b1, a1, a2, b2).Should().BeEqualTo(expected);
        await Point.線分が交差しているか(a1, b1, b2, a2).Should().BeEqualTo(expected);
        await Point.線分が交差しているか(b1, a1, b2, a2).Should().BeEqualTo(expected);

        await Point.線分が交差しているか(a2, b2, a1, b1).Should().BeEqualTo(expected);
        await Point.線分が交差しているか(b2, a2, a1, b1).Should().BeEqualTo(expected);
        await Point.線分が交差しているか(a2, b2, b1, a1).Should().BeEqualTo(expected);
        await Point.線分が交差しているか(b2, a2, b1, a1).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(Point[], (Point, Point, Point)[])> 三角形に分割_Data => [
        ([],[]),
        (new Point[1],[]),
        (new Point[2],[]),
        (

            [
                new (0, 1),
                new (0, 0),
                new (1, 1),
            ],
            [
                (new (0, 0), new (0, 1), new (1, 1)),
            ]
        ),
        (

            [
                new (0, 0),
                new (10, 0),
                new (5, 5),
                new (10, 10),
                new (0, 10),
            ],
            [
                (new (10, 0), new (0, 0), new (0, 10)),
                (new (10, 0), new (0, 10), new (10, 10)),
                (new (5, 5), new (10, 0), new (10, 10)),
            ]
        ),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(三角形に分割_Data))]
    public async Task 三角形に分割(Point[] input, (Point, Point, Point)[] expected)
    {
        await Point.三角形に分割(input).Should().BeStrictlyEquivalentTo(expected);
    }

    [Test]
    public async Task ConsoleWriter()
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
        await utf8Wrapper.Read().Should().BeEqualTo("""
        1.00000000000000000000 0.00000000010000000000
        -0.33333333333333331483 0.00001000000000000000
        -0.20000000000000001110 100000.00000000000000000000
        0.14285714285714284921 10000000000.00000000000000000000

        """.Replace("\r\n", "\n"));
    }
}