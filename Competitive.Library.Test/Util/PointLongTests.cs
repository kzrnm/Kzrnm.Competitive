using System;
using FluentAssertions;
using Xunit;

namespace AtCoder.Util
{
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

        public static TheoryData CompareTo_Data = new TheoryData<PointLong, PointLong, int>
        {
            { new PointLong(0,0), new PointLong(0,0), 0 },
            { new PointLong(1,1), new PointLong(1,1), 0 },
            { new PointLong(0,1), new PointLong(1,1), 1 },
            { new PointLong(1,-1), new PointLong(1,1), -1 },
            { new PointLong(2,1), new PointLong(1,1), -1 },
            { new PointLong(1,2), new PointLong(1,1), 1 },
        };
        [Theory]
        [MemberData(nameof(CompareTo_Data))]
        public void CompareTo(PointLong p1, PointLong p2, int expected)
        {
            p1.CompareTo(p2).Should().Be(expected);
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
    }
}
