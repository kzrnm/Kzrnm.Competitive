using FluentAssertions;
using System;
using System.Numerics;
using Xunit;

namespace Kzrnm.Competitive.GlobalNS
{
    public class BinarySearchExTests
    {
        private struct DelegateOk<T> : IOk<T>
        {
            private Predicate<T> predicate;
            public bool Ok(T value) => predicate(value);
            public DelegateOk(Predicate<T> predicate)
            {
                this.predicate = predicate;
            }
        }
        private struct IntLower : IOk<int>
        {
            public bool Ok(int value) => value < 0;
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
            __BinarySearchEx.BinarySearch(-1000000000, 10, new DelegateOk<int>(num => num < 9)).Should().Be(8);
            __BinarySearchEx.BinarySearch(0, int.MaxValue, new DelegateOk<int>(num => num < 9)).Should().Be(8);
            __BinarySearchEx.BinarySearch(-1000000000, 10, new DelegateOk<int>(num => num < -19)).Should().Be(-20);
            __BinarySearchEx.BinarySearch(1000, -1000000000, new DelegateOk<int>(num => num > -19)).Should().Be(-18);
        }

        private struct LongLower : IOk<long>
        {
            public bool Ok(long value) => value < 0;
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
            __BinarySearchEx.BinarySearch(-1000000000, 10, new DelegateOk<long>(num => num < 9)).Should().Be(8);
            __BinarySearchEx.BinarySearch(0, long.MaxValue, new DelegateOk<long>(num => num < 9)).Should().Be(8);
            __BinarySearchEx.BinarySearch(-1000000000, 10, new DelegateOk<long>(num => num < -19)).Should().Be(-20);
            __BinarySearchEx.BinarySearch(1000, -1000000000, new DelegateOk<long>(num => num > -19)).Should().Be(-18);
        }

        private struct ULongLower : IOk<ulong>
        {
            public bool Ok(ulong value) => value < 10;
        }
        [Fact]
        public void ULongDefault()
        {
            __BinarySearchEx.BinarySearch<ULongLower>(0, ulong.MaxValue).Should().Be(9);
        }
        [Fact]
        public void ULongArg()
        {
            __BinarySearchEx.BinarySearch(0, 10, new DelegateOk<ulong>(num => num < 9)).Should().Be(8);
            __BinarySearchEx.BinarySearch(0, ulong.MaxValue, new DelegateOk<ulong>(num => num < 9)).Should().Be(8);
            __BinarySearchEx.BinarySearch(0, ulong.MaxValue, new DelegateOk<ulong>(num => num < ulong.MaxValue)).Should().Be(ulong.MaxValue - 1);
            __BinarySearchEx.BinarySearch(ulong.MaxValue, 0, new DelegateOk<ulong>(num => num == ulong.MaxValue)).Should().Be(ulong.MaxValue);
        }

        private struct BigLower : IOk<BigInteger>
        {
            private static readonly BigInteger INF = new BigInteger(1) << 1000;
            public bool Ok(BigInteger value) => value < INF;
        }
        [Fact]
        public void BigDefault()
        {
            __BinarySearchEx.BinarySearch<BigLower>(0, BigInteger.Pow(10, 1000)).Should().Be(BigInteger.Pow(2, 1000) - 1);
        }
        [Fact]
        public void BigArg()
        {
            __BinarySearchEx.BinarySearch(0, 10, new DelegateOk<BigInteger>(num => num < 9)).Should().Be(8);
            __BinarySearchEx.BinarySearch(0, ulong.MaxValue, new DelegateOk<BigInteger>(num => num < 9)).Should().Be(8);
            __BinarySearchEx.BinarySearch(0, ulong.MaxValue, new DelegateOk<BigInteger>(num => num < ulong.MaxValue)).Should().Be(ulong.MaxValue - 1);
            __BinarySearchEx.BinarySearch(ulong.MaxValue, 0, new DelegateOk<BigInteger>(num => num == ulong.MaxValue)).Should().Be(ulong.MaxValue);
        }


        [Fact]
        public void BigUpper()
        {
            __BinarySearchEx.BinarySearchBig(0, new DelegateOk<BigInteger>(num => num < new BigInteger(1) << 1000)).Should().Be(BigInteger.Pow(2, 1000) - 1);
            __BinarySearchEx.BinarySearchBig(new BigInteger(1) << 10, new DelegateOk<BigInteger>(num => num < new BigInteger(1) << 1000)).Should().Be(BigInteger.Pow(2, 1000) - 1);
        }

        private struct DoubleLower : IOk<double>
        {
            public bool Ok(double value) => value < 0;
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
            __BinarySearchEx.BinarySearch(-1000000000, 1, new DelegateOk<double>(num => num < .5)).Should().Be(0.49999995812981446);
            __BinarySearchEx.BinarySearch(-1000000000, 10, new DelegateOk<double>(num => num < .5)).Should().Be(0.49999995163374444);
            __BinarySearchEx.BinarySearch(-1000000000, 1, new DelegateOk<double>(num => num < .5), 1).Should().Be(0.06867742445319891);
            __BinarySearchEx.BinarySearch(-1000000000, 10, new DelegateOk<double>(num => num < .5), 1).Should().Be(-0.24454842321574688);
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
