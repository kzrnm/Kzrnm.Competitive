using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AtCoderProject;
using Xunit;

namespace AtCoderLib.範囲演算
{
    public class SegmentTreeTests
    {
        const int N = 100000;
        static readonly long[] arr = Util.MakeLongArray(N);
        readonly SegmentTree seg = new SegmentTree(arr);

        public static TheoryData Query_Data = new TheoryData<int, int, long>
        {
            {0, N, 9223193755055837681 },
            {0, 60420, 9222700237768686924 },
            {60421, N, 9222938855238630072 },
        };


        [Theory]
        [MemberData(nameof(Query_Data))]
        public void Query(int from, int toExclusive, long expected)
        {
            seg.Query(from, toExclusive).Should().Be(expected);
        }
        [Theory]
        [MemberData(nameof(Query_Data))]
        public void Range(int from, int toExclusive, long expected)
        {
            seg[from..toExclusive].Should().Be(expected);
        }
    }
}
