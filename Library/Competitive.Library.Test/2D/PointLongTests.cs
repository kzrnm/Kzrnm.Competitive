using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.TwoDimensional
{
    public class PointLongTests
    {
        public static TheoryData Distance_Data => new TheoryData<PointLong, PointLong, long, double>
        {
            { new (0,0), new (0,0), 0, 0 },
            { new (1,1), new (1,1), 0, 0 },
            { new (0,0), new (1,1), 2, Math.Sqrt(2) },
            { new (0,0), new (-1,-1), 2, Math.Sqrt(2) },
            { new (-1,-2), new (3,5), 65, Math.Sqrt(65) },
        };
        [Theory]
        [MemberData(nameof(Distance_Data))]
        public void Distance(PointLong p1, PointLong p2, long d2, double distance)
        {
            p1.Distance2(p2).ShouldBe(d2);
            p2.Distance2(p1).ShouldBe(d2);
            p1.Distance(p2).ShouldBe(distance);
            p2.Distance(p1).ShouldBe(distance);
        }

        public static PointLong[] SortedPoints =>
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
        [Fact]
        public void CompareTo()
        {
            for (int i = 0; i < SortedPoints.Length; i++)
                for (int j = 0; j < SortedPoints.Length; j++)
                    SortedPoints[i].CompareTo(SortedPoints[j]).ShouldBe(i.CompareTo(j),
                        $"({SortedPoints[i]}).CompareTo(({SortedPoints[j]})) == {i}.CompareTo({j})");
        }

        public static TheoryData Inner_Data => new TheoryData<PointLong, PointLong, long>
        {
            { new (0,0), new (0,0), 0 },
            { new (0,1), new (1,0), 0 },
            { new (5,1), new (-2,10), 0 },
            { new (10,0), new (-2,10), -20 },
            { new (5,3), new (-2,-7), -31 },
        };
        [Theory]
        [MemberData(nameof(Inner_Data))]
        public void Inner(PointLong p1, PointLong p2, long expected)
        {
            p1.Inner(p2).ShouldBe(expected);
        }

        public static TheoryData Cross_Data => new TheoryData<PointLong, PointLong, long>
        {
            { new (0,0), new (0,0), 0 },
            { new (0,1), new (1,0), -1 },
            { new (5,1), new (-2,10), 52 },
            { new (10,0), new (-2,10), 100 },
            { new (5,3), new (-2,-7), -29 },
        };
        [Theory]
        [MemberData(nameof(Cross_Data))]
        public void Cross(PointLong p1, PointLong p2, long expected)
        {
            p1.Cross(p2).ShouldBe(expected);
        }

        public static TheoryData Area_Data => new TheoryData<PointLong[], long>
        {
            {
                new PointLong[]
                {
                    new(1,1),
                    new(2,2),
                    new(1,3),
                    new(-1,1),
                },
                6
            },
            {
                new PointLong[]
                {
                    new(-1,1),
                    new(1,3),
                    new(2,2),
                    new(1,1),
                },
                6
            },
            {
                new PointLong[]
                {
                    new(1000000000-1,1000000000+1),
                    new(1000000000+1,1000000000+3),
                    new(1000000000+2,1000000000+2),
                    new(1000000000+1,1000000000+1),
                },
                6
            },
        };

        [Theory]
        [MemberData(nameof(Area_Data))]
        public void Area(PointLong[] points, long expected)
        {
            PointLong.Area2(points).ShouldBe(expected);
            PointLong.Area(points).ShouldBe(expected / 2.0);
        }

        [Fact]
        public void ConvexHull1()
        {
            var points = new PointLong[]
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
            PointLong.ConvexHull(points)
                .Select(i => points[i])
                .ShouldBe([
                    (10000000, -10000000),
                    (40000000, 20000000),
                    (80000000, 60000000),
                    (100000000, 80000000),
                    (110000000, 100000000),
                    (100000000, 100000000),
                    (40000000, 80000000),
                    (-10000000, 50000000),
               ]);
        }

        [Fact]
        public void ConvexHull2()
        {
            var points = new PointLong[]
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
            PointLong.ConvexHull(points)
                .Select(i => points[i])
                .ShouldBe([
                    (0, 0),
                    (4, 0),
                    (10, 8),
                    (11, 10),
                    (10, 10),
                    (1, 8),
                    (-6, 6),
                    (-4, 4),
                    (-1, 1),
                ]);
        }

        [Fact]
        public void ConsoleWriter()
        {
            var utf8Wrapper = new Utf8ConsoleWriterWrapper();
            using (var cw = utf8Wrapper.GetWriter())
            {
                var arr = new PointLong[]
                {
                    new(1, long.MinValue+0),
                    new(3, long.MinValue+1),
                    new(5, long.MinValue+2),
                    new(7, long.MinValue+3),
                };
                cw.WriteLines(arr);
            }
            utf8Wrapper.Read().ShouldBe("""
            1 -9223372036854775808
            3 -9223372036854775807
            5 -9223372036854775806
            7 -9223372036854775805

            """.Replace("\r\n", "\n"));
        }
    }
}
