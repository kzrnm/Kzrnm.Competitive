using System;
using FluentAssertions;
using Xunit;

namespace AtCoder.Util
{
    public class PointIntTests
    {
        public static TheoryData Distance_Data = new TheoryData<PointInt, PointInt, long, double>
        {
            { new PointInt(0,0), new PointInt(0,0), 0, 0 },
            { new PointInt(1,1), new PointInt(1,1), 0, 0 },
            { new PointInt(0,0), new PointInt(1,1), 2, Math.Sqrt(2) },
            { new PointInt(0,0), new PointInt(-1,-1), 2, Math.Sqrt(2) },
            { new PointInt(-1,-2), new PointInt(3,5), 65, Math.Sqrt(65) },
        };
        [Theory]
        [MemberData(nameof(Distance_Data))]
        public void Distance(PointInt p1, PointInt p2, long d2, double distance)
        {
            p1.Distance2(p2).Should().Be(d2);
            p2.Distance2(p1).Should().Be(d2);
            p1.Distance(p2).Should().Be(distance);
            p2.Distance(p1).Should().Be(distance);
        }

        public static TheoryData CompareTo_Data = new TheoryData<PointInt, PointInt, int>
        {
            { new PointInt(0,0), new PointInt(0,0), 0 },
            { new PointInt(1,1), new PointInt(1,1), 0 },
            { new PointInt(0,1), new PointInt(1,1), -1 },
            { new PointInt(1,-1), new PointInt(1,1), -1 },
            { new PointInt(2,1), new PointInt(1,1), 1 },
            { new PointInt(1,2), new PointInt(1,1), 1 },
        };
        [Theory]
        [MemberData(nameof(CompareTo_Data))]
        public void CompareTo(PointInt p1, PointInt p2, int expected)
        {
            p1.CompareTo(p2).Should().Be(expected);
        }

        public static TheoryData Inner_Data = new TheoryData<PointInt, PointInt, long>
        {
            { new PointInt(0,0), new PointInt(0,0), 0 },
            { new PointInt(0,1), new PointInt(1,0), 0 },
            { new PointInt(5,1), new PointInt(-2,10), 0 },
            { new PointInt(10,0), new PointInt(-2,10), -20 },
            { new PointInt(5,3), new PointInt(-2,-7), -31 },
        };
        [Theory]
        [MemberData(nameof(Inner_Data))]
        public void Inner(PointInt p1, PointInt p2, int expected)
        {
            p1.Inner(p2).Should().Be(expected);
        }

        public static TheoryData Cross_Data = new TheoryData<PointInt, PointInt, long>
        {
            { new PointInt(0,0), new PointInt(0,0), 0 },
            { new PointInt(0,1), new PointInt(1,0), -1 },
            { new PointInt(5,1), new PointInt(-2,10), 52 },
            { new PointInt(10,0), new PointInt(-2,10), 100 },
            { new PointInt(5,3), new PointInt(-2,-7), -29 },
        };
        [Theory]
        [MemberData(nameof(Cross_Data))]
        public void Cross(PointInt p1, PointInt p2, int expected)
        {
            p1.Cross(p2).Should().Be(expected);
        }
    }
}
