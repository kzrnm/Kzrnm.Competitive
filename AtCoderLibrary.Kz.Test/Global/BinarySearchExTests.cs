using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AtCoder
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
            BinarySearchEx.BinarySearch<IntLower>(-1000000000, 0).Should().Be(-1);
            BinarySearchEx.BinarySearch<IntLower>(-1000000000, 10).Should().Be(-1);
            BinarySearchEx.BinarySearch<IntLower>(-100, 20005080).Should().Be(-1);
        }
        [Fact]
        public void IntArg()
        {
            BinarySearchEx.BinarySearch(-1000000000, 10, new IntLower { th = 9 }).Should().Be(8);
            BinarySearchEx.BinarySearch(0, int.MaxValue, new IntLower { th = 9 }).Should().Be(8);
            BinarySearchEx.BinarySearch(-1000000000, 10, new IntLower { th = -19 }).Should().Be(-20);
        }

        private struct LongLower : IOk<long>
        {
            public long th;
            public bool Ok(long value) => value < th;
        }
        [Fact]
        public void LongDefault()
        {
            BinarySearchEx.BinarySearch<LongLower>(-1000000000, 0).Should().Be(-1);
            BinarySearchEx.BinarySearch<LongLower>(-1000000000, 10).Should().Be(-1);
            BinarySearchEx.BinarySearch<LongLower>(-100, 20005015601080).Should().Be(-1);
        }
        [Fact]
        public void LongArg()
        {
            BinarySearchEx.BinarySearch(-1000000000, 10, new LongLower { th = 9 }).Should().Be(8);
            BinarySearchEx.BinarySearch(0, long.MaxValue, new LongLower { th = 9 }).Should().Be(8);
            BinarySearchEx.BinarySearch(-1000000000, 10, new LongLower { th = -19 }).Should().Be(-20);
        }
        private struct DoubleLower : IOk<double>
        {
            public double th;
            public bool Ok(double value) => value < th;
        }
        [Fact]
        public void DoubleDefault()
        {
            BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 0).Should().Be(-5.551115123125783E-08);
            BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 10).Should().Be(-3.9225289683031406E-08);
            BinarySearchEx.BinarySearch<DoubleLower>(-150, 2e100).Should().Be(-2.3876500682776684E-08);
            BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 0, 1).Should().Be(-0.9313225746154785);
            BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 10, 1).Should().Be(-0.24454842321574688);
            BinarySearchEx.BinarySearch<DoubleLower>(-150, 2e100, 1).Should().Be(-0.26865174202197495);
        }
        [Fact]
        public void DoubleArg()
        {
            BinarySearchEx.BinarySearch(-1000000000, 1, new DoubleLower { th = 0.5 }).Should().Be(0.49999995812981446);
            BinarySearchEx.BinarySearch(-1000000000, 10, new DoubleLower { th = 0.5 }).Should().Be(0.49999995163374444);
            BinarySearchEx.BinarySearch(-1000000000, 1, new DoubleLower { th = 0.5 }, 1).Should().Be(0.06867742445319891);
            BinarySearchEx.BinarySearch(-1000000000, 10, new DoubleLower { th = 0.5 }, 1).Should().Be(-0.24454842321574688);
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
            BinarySearchEx.BinarySearch<float, FloatFull>(-1000000000F, 0F).Should().Be(-29.802322F);
            BinarySearchEx.BinarySearch<float, FloatFull>(-1000000000F, 10F).Should().Be(-19.802324F);
        }
        [Fact]
        public void BinaryOkArg()
        {
            BinarySearchEx.BinarySearch(-1000000000F, 1F, new FloatFull { th = 0.5F }).Should().Be(-28.802324F);
            BinarySearchEx.BinarySearch(-1000000000F, 10F, new FloatFull { th = 0.5F }).Should().Be(-19.802324F);
        }
    }
}
