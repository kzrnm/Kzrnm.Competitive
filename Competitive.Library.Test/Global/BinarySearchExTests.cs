using FluentAssertions;
using System;
using Xunit;

namespace Kzrnm.Competitive.GlobalNS
{
    public class BinarySearchExTests
    {
        private struct IntLower : IOk<int>
        {
            public int th;
            public bool Ok(int value) => value < th;
        }
        [Fact]
        public void IntDefault()
        {
            __BinarySearchEx.BinarySearch<IntLower>(-1000000000, 0).Should().Be(-1);
            __BinarySearchEx.BinarySearch<IntLower>(-1000000000, 10).Should().Be(-1);
            __BinarySearchEx.BinarySearch<IntLower>(-100, 20005080).Should().Be(-1);
        }
        [Fact]
        public void IntArg()
        {
            __BinarySearchEx.BinarySearch(-1000000000, 10, new IntLower { th = 9 }).Should().Be(8);
            __BinarySearchEx.BinarySearch(0, int.MaxValue, new IntLower { th = 9 }).Should().Be(8);
            __BinarySearchEx.BinarySearch(-1000000000, 10, new IntLower { th = -19 }).Should().Be(-20);
        }

        private struct LongLower : IOk<long>
        {
            public long th;
            public bool Ok(long value) => value < th;
        }
        [Fact]
        public void LongDefault()
        {
            __BinarySearchEx.BinarySearch<LongLower>(-1000000000, 0).Should().Be(-1);
            __BinarySearchEx.BinarySearch<LongLower>(-1000000000, 10).Should().Be(-1);
            __BinarySearchEx.BinarySearch<LongLower>(-100, 20005015601080).Should().Be(-1);
        }
        [Fact]
        public void LongArg()
        {
            __BinarySearchEx.BinarySearch(-1000000000, 10, new LongLower { th = 9 }).Should().Be(8);
            __BinarySearchEx.BinarySearch(0, long.MaxValue, new LongLower { th = 9 }).Should().Be(8);
            __BinarySearchEx.BinarySearch(-1000000000, 10, new LongLower { th = -19 }).Should().Be(-20);
        }
        private struct DoubleLower : IOk<double>
        {
            public double th;
            public bool Ok(double value) => value < th;
        }
        [Fact]
        public void DoubleDefault()
        {
            __BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 0).Should().Be(-5.551115123125783E-08);
            __BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 10).Should().Be(-3.9225289683031406E-08);
            __BinarySearchEx.BinarySearch<DoubleLower>(-150, 2e100).Should().Be(-2.3876500682776684E-08);
            __BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 0, 1).Should().Be(-0.9313225746154785);
            __BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 10, 1).Should().Be(-0.24454842321574688);
            __BinarySearchEx.BinarySearch<DoubleLower>(-150, 2e100, 1).Should().Be(-0.26865174202197495);
        }
        [Fact]
        public void DoubleArg()
        {
            __BinarySearchEx.BinarySearch(-1000000000, 1, new DoubleLower { th = 0.5 }).Should().Be(0.49999995812981446);
            __BinarySearchEx.BinarySearch(-1000000000, 10, new DoubleLower { th = 0.5 }).Should().Be(0.49999995163374444);
            __BinarySearchEx.BinarySearch(-1000000000, 1, new DoubleLower { th = 0.5 }, 1).Should().Be(0.06867742445319891);
            __BinarySearchEx.BinarySearch(-1000000000, 10, new DoubleLower { th = 0.5 }, 1).Should().Be(-0.24454842321574688);
        }

        private struct FloatFull : IBinaryOk<float>
        {
            public float th;
            public bool Continue(float ok, float ng) => Math.Abs(ok - ng) > 50;
            public float Mid(float ok, float ng) => (ok + ng) / 2;

            public bool Ok(float value) => value < th;
        }
        [Fact]
        public void BinaryOkDefault()
        {
            __BinarySearchEx.BinarySearch<float, FloatFull>(-1000000000F, 0F).Should().Be(-29.802322F);
            __BinarySearchEx.BinarySearch<float, FloatFull>(-1000000000F, 10F).Should().Be(-19.802324F);
        }
        [Fact]
        public void BinaryOkArg()
        {
            __BinarySearchEx.BinarySearch(-1000000000F, 1F, new FloatFull { th = 0.5F }).Should().Be(-28.802324F);
            __BinarySearchEx.BinarySearch(-1000000000F, 10F, new FloatFull { th = 0.5F }).Should().Be(-19.802324F);
        }
    }
}
