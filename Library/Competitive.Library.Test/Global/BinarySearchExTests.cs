using System;

namespace Kzrnm.Competitive.Testing.GlobalNS
{
    public class BinarySearchExTests
    {
        private readonly struct DelegateOk<T>(Predicate<T> predicate) : IOk<T>
        {
            public bool Ok(T value) => predicate(value);
        }
        private readonly struct IntLower : IOk<int>
        {
            public bool Ok(int value) => value < 0;
        }
        [Fact]
        public void IntDefault()
        {
            __BinarySearchEx.BinarySearch<IntLower>(-1000000000, 0).ShouldBe(-1);
            __BinarySearchEx.BinarySearch<IntLower>(-1000000000, 10).ShouldBe(-1);
            __BinarySearchEx.BinarySearch<IntLower>(-100, 20005080).ShouldBe(-1);
        }
        [Fact]
        public void IntArg()
        {
            new DelegateOk<int>(num => num < 9).BinarySearch(-1000000000, 10).ShouldBe(8);
            new DelegateOk<int>(num => num < 9).BinarySearch(0, int.MaxValue).ShouldBe(8);
            new DelegateOk<int>(num => num < -19).BinarySearch(-1000000000, 10).ShouldBe(-20);
            new DelegateOk<int>(num => num > -19).BinarySearch(1000, -1000000000).ShouldBe(-18);
        }

        private readonly struct LongLower : IOk<long>
        {
            public bool Ok(long value) => value < 0;
        }
        [Fact]
        public void LongDefault()
        {
            __BinarySearchEx.BinarySearch<LongLower>(-1000000000, 0).ShouldBe(-1);
            __BinarySearchEx.BinarySearch<LongLower>(-1000000000, 10).ShouldBe(-1);
            __BinarySearchEx.BinarySearch<LongLower>(-100, 20005015601080).ShouldBe(-1);
        }
        [Fact]
        public void LongArg()
        {
            new DelegateOk<long>(num => num < 9).BinarySearch(-1000000000, 10).ShouldBe(8);
            new DelegateOk<long>(num => num < 9).BinarySearch(0, long.MaxValue).ShouldBe(8);
            new DelegateOk<long>(num => num < -19).BinarySearch(-1000000000, 10).ShouldBe(-20);
            new DelegateOk<long>(num => num > -19).BinarySearch(1000, -1000000000).ShouldBe(-18);
        }

        private readonly struct ULongLower : IOk<ulong>
        {
            public bool Ok(ulong value) => value < 10;
        }
        [Fact]
        public void ULongDefault()
        {
            __BinarySearchEx.BinarySearch<ULongLower>(0, ulong.MaxValue).ShouldBe(9u);
        }
        [Fact]
        public void ULongArg()
        {
            new DelegateOk<ulong>(num => num < 9).BinarySearch(0, 10).ShouldBe(8u);
            new DelegateOk<ulong>(num => num < 9).BinarySearch(0, ulong.MaxValue).ShouldBe(8u);
            new DelegateOk<ulong>(num => num < ulong.MaxValue).BinarySearch(0, ulong.MaxValue).ShouldBe(ulong.MaxValue - 1);
            new DelegateOk<ulong>(num => num == ulong.MaxValue).BinarySearch(ulong.MaxValue, 0).ShouldBe(ulong.MaxValue);
        }

        private readonly struct DoubleLower : IOk<double>
        {
            public bool Ok(double value) => value < 0;
        }
        [Fact]
        public void DoubleDefault()
        {
            __BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 0).ShouldBe(-5.551115123125783E-08);
            __BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 10).ShouldBe(-3.9225289683031406E-08);
            __BinarySearchEx.BinarySearch<DoubleLower>(-150, 2e100).ShouldBe(-2.3876500682776684E-08);
            __BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 0, 1).ShouldBe(-0.9313225746154785);
            __BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 10, 1).ShouldBe(-0.24454842321574688);
            __BinarySearchEx.BinarySearch<DoubleLower>(-150, 2e100, 1).ShouldBe(-0.26865174202197495);
        }
        [Fact]
        public void DoubleArg()
        {
            new DelegateOk<double>(num => num < .5).BinarySearch(-1000000000, 1).ShouldBe(0.49999995812981446);
            new DelegateOk<double>(num => num < .5).BinarySearch(-1000000000, 10).ShouldBe(0.49999995163374444);
            new DelegateOk<double>(num => num < .5).BinarySearch(-1000000000, 1, 1).ShouldBe(0.06867742445319891);
            new DelegateOk<double>(num => num < .5).BinarySearch(-1000000000, 10, 1).ShouldBe(-0.24454842321574688);
        }


#pragma warning disable CS0649
        private readonly struct FloatFull(float th) : IBinaryOk<float>
        {
            public bool Continue(float ok, float ng) => Math.Abs(ok - ng) > 50;
            public float Mid(float ok, float ng) => (ok + ng) / 2;

            public bool Ok(float value) => value < th;
        }
        [Fact]
        public void BinaryOkDefault()
        {
            __BinarySearchEx.BinarySearch<float, FloatFull>(-1000000000F, 0F).ShouldBe(-29.802322F);
            __BinarySearchEx.BinarySearch<float, FloatFull>(-1000000000F, 10F).ShouldBe(-19.802324F);
        }
        // [Fact]
        // public void BinaryOkArg()
        // {
        //     new FloatFull { th = 0.5F }.BinarySearch(-1000000000F, 1F).ShouldBe(-28.802324F);
        //     new FloatFull { th = 0.5F }.BinarySearch(-1000000000F, 10F).ShouldBe(-19.802324F);
        // }
    }
}
