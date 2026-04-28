namespace Kzrnm.Competitive.Testing.GlobalNS;

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
    [Test, MultipleAssertions]
    public async Task IntDefault()
    {
        await __BinarySearchEx.BinarySearch<IntLower>(-1000000000, 0).Should().BeEqualTo(-1);
        await __BinarySearchEx.BinarySearch<IntLower>(-1000000000, 10).Should().BeEqualTo(-1);
        await __BinarySearchEx.BinarySearch<IntLower>(-100, 20005080).Should().BeEqualTo(-1);
    }
    [Test, MultipleAssertions]
    public async Task IntArg()
    {
        await new DelegateOk<int>(num => num < 9).BinarySearch(-1000000000, 10).Should().BeEqualTo(8);
        await new DelegateOk<int>(num => num < 9).BinarySearch(0, int.MaxValue).Should().BeEqualTo(8);
        await new DelegateOk<int>(num => num < -19).BinarySearch(-1000000000, 10).Should().BeEqualTo(-20);
        await new DelegateOk<int>(num => num > -19).BinarySearch(1000, -1000000000).Should().BeEqualTo(-18);
    }

    private readonly struct LongLower : IOk<long>
    {
        public bool Ok(long value) => value < 0;
    }
    [Test, MultipleAssertions]
    public async Task LongDefault()
    {
        await __BinarySearchEx.BinarySearch<LongLower>(-1000000000, 0).Should().BeEqualTo(-1);
        await __BinarySearchEx.BinarySearch<LongLower>(-1000000000, 10).Should().BeEqualTo(-1);
        await __BinarySearchEx.BinarySearch<LongLower>(-100, 20005015601080).Should().BeEqualTo(-1);
    }
    [Test, MultipleAssertions]
    public async Task LongArg()
    {
        await new DelegateOk<long>(num => num < 9).BinarySearch(-1000000000, 10).Should().BeEqualTo(8);
        await new DelegateOk<long>(num => num < 9).BinarySearch(0, long.MaxValue).Should().BeEqualTo(8);
        await new DelegateOk<long>(num => num < -19).BinarySearch(-1000000000, 10).Should().BeEqualTo(-20);
        await new DelegateOk<long>(num => num > -19).BinarySearch(1000, -1000000000).Should().BeEqualTo(-18);
    }

    private readonly struct ULongLower : IOk<ulong>
    {
        public bool Ok(ulong value) => value < 10;
    }
    [Test, MultipleAssertions]
    public async Task ULongDefault()
    {
        await __BinarySearchEx.BinarySearch<ULongLower>(0, ulong.MaxValue).Should().BeEqualTo(9u);
    }
    [Test, MultipleAssertions]
    public async Task ULongArg()
    {
        await new DelegateOk<ulong>(num => num < 9).BinarySearch(0, 10).Should().BeEqualTo(8u);
        await new DelegateOk<ulong>(num => num < 9).BinarySearch(0, ulong.MaxValue).Should().BeEqualTo(8u);
        await new DelegateOk<ulong>(num => num < ulong.MaxValue).BinarySearch(0, ulong.MaxValue).Should().BeEqualTo(ulong.MaxValue - 1);
        await new DelegateOk<ulong>(num => num == ulong.MaxValue).BinarySearch(ulong.MaxValue, 0).Should().BeEqualTo(ulong.MaxValue);
    }

    private readonly struct DoubleLower : IOk<double>
    {
        public bool Ok(double value) => value < 0;
    }
    [Test, MultipleAssertions]
    public async Task DoubleDefault()
    {
        await __BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 0).Should().BeEqualTo(-5.551115123125783E-08);
        await __BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 10).Should().BeEqualTo(-3.9225289683031406E-08);
        await __BinarySearchEx.BinarySearch<DoubleLower>(-150, 2e100).Should().BeEqualTo(-2.3876500682776684E-08);
        await __BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 0, 1).Should().BeEqualTo(-0.9313225746154785);
        await __BinarySearchEx.BinarySearch<DoubleLower>(-1000000000, 10, 1).Should().BeEqualTo(-0.24454842321574688);
        await __BinarySearchEx.BinarySearch<DoubleLower>(-150, 2e100, 1).Should().BeEqualTo(-0.26865174202197495);
    }
    [Test, MultipleAssertions]
    public async Task DoubleArg()
    {
        await new DelegateOk<double>(num => num < .5).BinarySearch(-1000000000, 1).Should().BeEqualTo(0.49999995812981446);
        await new DelegateOk<double>(num => num < .5).BinarySearch(-1000000000, 10).Should().BeEqualTo(0.49999995163374444);
        await new DelegateOk<double>(num => num < .5).BinarySearch(-1000000000, 1, 1).Should().BeEqualTo(0.06867742445319891);
        await new DelegateOk<double>(num => num < .5).BinarySearch(-1000000000, 10, 1).Should().BeEqualTo(-0.24454842321574688);
    }


#pragma warning disable CS0649
    private readonly struct FloatFull(float th) : IBinaryOk<float>
    {
        public bool Continue(float ok, float ng) => Math.Abs(ok - ng) > 50;
        public float Mid(float ok, float ng) => (ok + ng) / 2;

        public bool Ok(float value) => value < th;
    }
    [Test, MultipleAssertions]
    public async Task BinaryOkDefault()
    {
        await __BinarySearchEx.BinarySearch<float, FloatFull>(-1000000000F, 0F).Should().BeEqualTo(-29.802322F);
        await __BinarySearchEx.BinarySearch<float, FloatFull>(-1000000000F, 10F).Should().BeEqualTo(-19.802324F);
    }
    //[Fact]
    //public async Task BinaryOkArg()
    //{
    //    await new FloatFull { th = 0.5F }.BinarySearch(-1000000000F, 1F).Should().BeEqualTo(-28.802324F);
    //    await new FloatFull { th = 0.5F }.BinarySearch(-1000000000F, 10F).Should().BeEqualTo(-19.802324F);
    //}
}