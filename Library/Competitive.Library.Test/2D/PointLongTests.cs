namespace Kzrnm.Competitive.Testing.TwoDimensional;

using Point = PointLong;

public class PointLongTests
{
    public static IEnumerable<(Point, Point, long, double)> Distance_Data =>
    [
        (new (0,0), new (0,0), 0, 0),
        (new (1,1), new (1,1), 0, 0),
        (new (0,0), new (1,1), 2, Math.Sqrt(2)),
        (new (0,0), new (-1,-1), 2, Math.Sqrt(2)),
        (new (-1,-2), new (3,5), 65, Math.Sqrt(65)),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Distance_Data))]
    public async Task Distance(Point p1, Point p2, long d2, double distance)
    {
        await p1.Distance2(p2).Should().BeEqualTo(d2);
        await p2.Distance2(p1).Should().BeEqualTo(d2);
        await p1.Distance(p2).Should().BeEqualTo(distance);
        await p2.Distance(p1).Should().BeEqualTo(distance);
    }

    public static Point[] SortedPoints =>
    [
        new(0, 0),
        new(1, 0),
        new(5, 0),
        new(100000, 1),
        new(1, 1),
        new(1, 10000),
        new(0, 1),
        new(0, 55),
        new(-1, 10000),
        new(-1, 1),
        new(-1500, 1),
        new(-10000000, 1),
        new(-1, 0),
        new(-2, 0),
        new(-500, 0),
        new(-500, -1),
        new(-1, -1),
        new(-2, -2),
        new(-1, -50000),
        new(0, -1),
        new(0, -2000),
        new(1, -10),
        new(100000, -1),
    ];
    [Test, MultipleAssertions]
    public async Task CompareTo()
    {
        for (int i = 0; i < SortedPoints.Length; i++)
            for (int j = 0; j < SortedPoints.Length; j++)
                await SortedPoints[i].CompareTo(SortedPoints[j]).Should().BeEqualTo(i.CompareTo(j));
    }

    public static IEnumerable<(Point, Point, long)> Inner_Data =>
    [
        (new (0,0), new (0,0), 0),
        (new (0,1), new (1,0), 0),
        (new (5,1), new (-2,10), 0),
        (new (10,0), new (-2,10), -20),
        (new (5,3), new (-2,-7), -31),
    ];
    [Test]
    [MethodDataSource(nameof(Inner_Data))]
    public async Task Inner(Point p1, Point p2, long expected)
    {
        await p1.Inner(p2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(Point, Point, long)> Cross_Data =>
    [
        (new (0,0), new (0,0), 0),
        (new (0,1), new (1,0), -1),
        (new (5,1), new (-2,10), 52),
        (new (10,0), new (-2,10), 100),
        (new (5,3), new (-2,-7), -29),
    ];
    [Test]
    [MethodDataSource(nameof(Cross_Data))]
    public async Task Cross(Point p1, Point p2, long expected)
    {
        await p1.Cross(p2).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(Point[], long)> Area_Data =>
    [
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
                new(1000000000-1,1000000000+1),
                new(1000000000+1,1000000000+3),
                new(1000000000+2,1000000000+2),
                new(1000000000+1,1000000000+1),
            },
            6
        ),
    ];

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Area_Data))]
    public async Task Area(Point[] points, long expected)
    {
        await Point.Area2(points).Should().BeEqualTo(expected);
        await Point.Area(points).Should().BeEqualTo(expected / 2.0);
    }

    public static IEnumerable<(Point[], int[], int[])> ConvexHull_Data =>
    [
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
        await Point.ConvexHull(points).Should().BeEquivalentOrderTo(expectedNotStrict);
        await Point.ConvexHull(points, true).Should().BeEquivalentOrderTo(expectedStrict);
    }

    [Test]
    public async Task ConsoleWriter()
    {
        var utf8Wrapper = new Utf8ConsoleWriterWrapper();
        using (var cw = utf8Wrapper.GetWriter())
        {
            var arr = new Point[]
            {
                new(1, long.MinValue+0),
                new(3, long.MinValue+1),
                new(5, long.MinValue+2),
                new(7, long.MinValue+3),
            };
            cw.WriteLines(arr);
        }
        await utf8Wrapper.Read().Should().BeEqualTo("""
        1 -9223372036854775808
        3 -9223372036854775807
        5 -9223372036854775806
        7 -9223372036854775805

        """.Replace("\r\n", "\n"));
    }
}