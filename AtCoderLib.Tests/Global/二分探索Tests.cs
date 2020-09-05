using System;
using Xunit;
using FluentAssertions;
using System.Linq;
using AtCoderProject;

namespace AtCoderLib.Global
{
    public class “ñ•ª’TõTests
    {
        const int N = 40000;
        readonly int[] arr;
        readonly int[] rev;
        public “ñ•ª’TõTests()
        {
            arr = Util.MakeIntArray(N).Select(i => i % 17659).ToArray();
            Array.Sort(arr);
            rev = (int[])arr.Clone();
            Array.Reverse(rev);

            arr[19997].Should().Be(-12);
            arr[19998].Should().Be(-11);
            arr[19999].Should().Be(-7);
            arr[20000].Should().Be(-7);
            arr[20001].Should().Be(-6);
            arr[20002].Should().Be(-6);

            rev[20002].Should().Be(-12);
            rev[20001].Should().Be(-11);
            rev[20000].Should().Be(-7);
            rev[19999].Should().Be(-7);
            rev[19998].Should().Be(-6);
            rev[19997].Should().Be(-6);
        }
        [Fact]
        public void BinarySearch()
        {
            “ñ•ª’Tõ.BinarySearch(0, N, i => arr[i] < -7).Should().Be(19998);
            “ñ•ª’Tõ.BinarySearch(0, N, i => arr[i] <= -7).Should().Be(20000);
            “ñ•ª’Tõ.BinarySearch(N, 0, i => arr[i] >= -7).Should().Be(19999);
            “ñ•ª’Tõ.BinarySearch(N, 0, i => arr[i] > -7).Should().Be(20001);
        }
        [Fact]
        public void LowerBound()
        {
            arr.LowerBound(-7).Should().Be(19999);
            ((Span<int>)arr).LowerBound(-7).Should().Be(19999);
            ((ReadOnlySpan<int>)arr).LowerBound(-7).Should().Be(19999);

            rev.LowerBound(-7, ExComparer<int>.DefaultReverse).Should().Be(19999);
            ((Span<int>)rev).LowerBound(-7, ExComparer<int>.DefaultReverse).Should().Be(19999);
            ((ReadOnlySpan<int>)rev).LowerBound(-7, ExComparer<int>.DefaultReverse).Should().Be(19999);

            rev.LowerBound(-11, ExComparer<int>.DefaultReverse).Should().Be(20001);
            ((Span<int>)rev).LowerBound(-11, ExComparer<int>.DefaultReverse).Should().Be(20001);
            ((ReadOnlySpan<int>)rev).LowerBound(-11, ExComparer<int>.DefaultReverse).Should().Be(20001);
        }
        [Fact]
        public void UpperBound()
        {
            arr.UpperBound(-7).Should().Be(20001);
            ((Span<int>)arr).UpperBound(-7).Should().Be(20001);
            ((ReadOnlySpan<int>)arr).UpperBound(-7).Should().Be(20001);

            rev.UpperBound(-7, ExComparer<int>.DefaultReverse).Should().Be(20001);
            ((Span<int>)rev).UpperBound(-7, ExComparer<int>.DefaultReverse).Should().Be(20001);
            ((ReadOnlySpan<int>)rev).UpperBound(-7, ExComparer<int>.DefaultReverse).Should().Be(20001);

            rev.UpperBound(-11, ExComparer<int>.DefaultReverse).Should().Be(20002);
            ((Span<int>)rev).UpperBound(-11, ExComparer<int>.DefaultReverse).Should().Be(20002);
            ((ReadOnlySpan<int>)rev).UpperBound(-11, ExComparer<int>.DefaultReverse).Should().Be(20002);
        }
    }
}
