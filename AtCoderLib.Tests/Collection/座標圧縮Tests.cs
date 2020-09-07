using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace AtCoderLib.Collection
{
    public class 座標圧縮Tests
    {
        [Fact]
        public void CompressTest()
        {
            座標圧縮.Compress(new long[] { long.MinValue, long.MaxValue, -5, 4, 6, 1, 4, -5 })
                .Should().Equal(new Dictionary<long, int>
                {
                    { long.MinValue, 0 },
                    { -5, 1 },
                    { 1, 2 },
                    { 4, 3 },
                    { 6, 4 },
                    { long.MaxValue, 5 },
                });
            座標圧縮.Compress(new uint[] { 0, uint.MaxValue, 5, 4, 6, 1, 2, 2, 2, 2, 4, 4 })
                .Should().Equal(new Dictionary<uint, int>
                {
                    { 0, 0 },
                    { 1, 1 },
                    { 2, 2 },
                    { 4, 3 },
                    { 5, 4 },
                    { 6, 5 },
                    { uint.MaxValue, 6 },
                });
        }
        [Fact]
        public void CompressedTest()
        {
            座標圧縮.Compressed(new long[] { long.MinValue, long.MaxValue, -5, 4, 6, 1, 4, -5 })
                .Should().Equal(new int[] { 0, 5, 1, 3, 4, 2, 3, 1 });
            座標圧縮.Compressed(new uint[] { 0, uint.MaxValue, 5, 4, 6, 1, 2, 2, 2, 2, 4, 4 })
                .Should().Equal(new int[] { 0, 6, 4, 3, 5, 1, 2, 2, 2, 2, 3, 3 });
        }
    }
}
