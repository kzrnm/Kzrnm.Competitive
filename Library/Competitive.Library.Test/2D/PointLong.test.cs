using FluentAssertions;
using System;
using Xunit;

namespace Kzrnm.Competitive.Testing.TwoDimensional
{
    // verification-helper: SAMEAS Library/run.test.py
    public class PointLongTests
    {
        public static TheoryData Distance_Data = new TheoryData<PointLong, PointLong, long, double>
        {
            { new PointLong(0,0), new PointLong(0,0), 0, 0 },
            { new PointLong(1,1), new PointLong(1,1), 0, 0 },
            { new PointLong(0,0), new PointLong(1,1), 2, Math.Sqrt(2) },
            { new PointLong(0,0), new PointLong(-1,-1), 2, Math.Sqrt(2) },
            { new PointLong(-1,-2), new PointLong(3,5), 65, Math.Sqrt(65) },
        };
        [Theory]
        [MemberData(nameof(Distance_Data))]
        public void Distance(PointLong p1, PointLong p2, long d2, double distance)
        {
            p1.Distance2(p2).Should().Be(d2);
            p2.Distance2(p1).Should().Be(d2);
            p1.Distance(p2).Should().Be(distance);
            p2.Distance(p1).Should().Be(distance);
        }

        public static PointLong[] SortedPoints = new PointLong[]
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

        public static TheoryData Inner_Data = new TheoryData<PointLong, PointLong, long>
        {
            { new PointLong(0,0), new PointLong(0,0), 0 },
            { new PointLong(0,1), new PointLong(1,0), 0 },
            { new PointLong(5,1), new PointLong(-2,10), 0 },
            { new PointLong(10,0), new PointLong(-2,10), -20 },
            { new PointLong(5,3), new PointLong(-2,-7), -31 },
        };
        [Theory]
        [MemberData(nameof(Inner_Data))]
        public void Inner(PointLong p1, PointLong p2, long expected)
        {
            p1.Inner(p2).Should().Be(expected);
        }

        public static TheoryData Cross_Data = new TheoryData<PointLong, PointLong, long>
        {
            { new PointLong(0,0), new PointLong(0,0), 0 },
            { new PointLong(0,1), new PointLong(1,0), -1 },
            { new PointLong(5,1), new PointLong(-2,10), 52 },
            { new PointLong(10,0), new PointLong(-2,10), 100 },
            { new PointLong(5,3), new PointLong(-2,-7), -29 },
        };
        [Theory]
        [MemberData(nameof(Cross_Data))]
        public void Cross(PointLong p1, PointLong p2, long expected)
        {
            p1.Cross(p2).Should().Be(expected);
        }

        public static TheoryData Area_Data = new TheoryData<PointLong[], long>
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
            PointLong.Area2(points).Should().Be(expected);
            PointLong.Area(points).Should().Be(expected / 2.0);
        }
    }
}
