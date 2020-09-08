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
        readonly SegmentTree segMax = new SegmentTree(Util.MakeLongArray(N));

        public static TheoryData QueryMax_Data = new TheoryData<int, int, long>
        {
            {0, N, 9223193755055837681 },
            {0, 60420, 9222700237768686924 },
            {60421, N, 9222938855238630072 },
        };

        [Theory]
        [MemberData(nameof(QueryMax_Data))]
        public void MaxQuery(int from, int toExclusive, long expected)
        {
            segMax.Query(from, toExclusive).Should().Be(expected);
            segMax[from..toExclusive].Should().Be(expected);
        }
        [Fact]
        public void MaxSeg()
        {
            var seg = new SegmentTree(10);
            for (int i = 0; i < 10; i++) seg[i].Should().Be(long.MinValue);
            seg[0..10].Should().Be(long.MinValue);
            seg.Update(1, 10);
            seg[1].Should().Be(10);
            seg[0..1].Should().Be(long.MinValue);
            seg[2..10].Should().Be(long.MinValue);
            seg[0..10].Should().Be(10);
            seg.Update(2, 7);
            seg[2].Should().Be(7);
            seg[0..1].Should().Be(long.MinValue);
            seg[2..10].Should().Be(7);
            seg[0..10].Should().Be(10);
            seg.Update(1, 1);
            seg[1].Should().Be(1);
            seg[0..1].Should().Be(long.MinValue);
            seg[2..10].Should().Be(7);
            seg[0..10].Should().Be(7);

            for (int i = 0; i < 10; i++)
                seg.Update(i, i);

            for (int l = 0; l < 10; l++)
                for (int r = l + 1; r <= 10; r++)
                    seg[l..r].Should().Be(r - 1);
        }


        class SegmentTreeSum : SegmentTreeAbstract<long>
        {
            protected override long DefaultValue => 0;
            protected override long Operate(long v1, long v2) => v1 + v2;
            public SegmentTreeSum(long[] initArray) : base(initArray) { }
            public SegmentTreeSum(int size) : base(size) { }
        }
        readonly SegmentTreeSum segSum = new SegmentTreeSum(Util.MakeIntArray(N).Select(i => (long)i).ToArray());

        public static TheoryData QuerySum_Data = new TheoryData<int, int, long>
        {
            {0, N, 421259876887 },
            {0, 1, -638953894 },
            {0, 2, -134063015 },
            {1, 2, 504890879 },
        };

        [Theory]
        [MemberData(nameof(QuerySum_Data))]
        public void SumQuery(int from, int toExclusive, long expected)
        {
            segSum.Query(from, toExclusive).Should().Be(expected);
            segSum[from..toExclusive].Should().Be(expected);
        }
        [Fact]
        public void SumSeg()
        {
            var seg = new SegmentTreeSum(10);
            for (int i = 0; i < 10; i++) seg[i].Should().Be(0);
            seg[0..10].Should().Be(0);
            seg.Update(1, 10);
            seg[1].Should().Be(10);
            seg[0..1].Should().Be(0);
            seg[2..10].Should().Be(0);
            seg[0..10].Should().Be(10);
            seg.Update(2, 7);
            seg[2].Should().Be(7);
            seg[0..1].Should().Be(0);
            seg[2..10].Should().Be(7);
            seg[0..10].Should().Be(17);
            seg.Update(1, 1);
            seg[1].Should().Be(1);
            seg[0..1].Should().Be(0);
            seg[2..10].Should().Be(7);
            seg[0..10].Should().Be(8);


            for (int i = 0; i < 10; i++)
                seg.Update(i, i);

            for (int l = 0; l < 10; l++)
                for (int r = l + 1; r <= 10; r++)
                    seg[l..r].Should().Be(((long)r - l) * (r - 1 + l) / 2);


            seg.MaxRight(0, n => n < 20).Should().Be(6);
            seg.MaxRight(2, n => n < 20).Should().Be(6);
            seg.MaxRight(3, n => n < 20).Should().Be(7);
            seg.MaxRight(7, n => n < 20).Should().Be(9);
            seg.MaxRight(8, n => n < 20).Should().Be(10);
            seg.MaxRight(9, n => n < 20).Should().Be(10);

            seg.MinLeft(10, n => n < 20).Should().Be(8);
            seg.MinLeft(9, n => n < 20).Should().Be(7);
            seg.MinLeft(8, n => n < 20).Should().Be(5);
            seg.MinLeft(7, n => n < 20).Should().Be(3);
        }
    }
}
