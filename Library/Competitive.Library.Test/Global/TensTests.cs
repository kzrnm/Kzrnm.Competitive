namespace Kzrnm.Competitive.Testing.GlobalNS
{
    public class TensTests
    {
        [Fact]
        public void Ints()
        {
            Tens.Ints[0].ShouldBe(1);
            for (int i = 1; i < Tens.Ints.Length; i++)
            {
                Tens.Ints[i].ShouldBe(Tens.Ints[i - 1] * 10);
            }
        }
        [Fact]
        public void Longs()
        {
            Tens.Longs[0].ShouldBe(1);
            for (int i = 1; i < Tens.Longs.Length; i++)
            {
                Tens.Longs[i].ShouldBe(Tens.Longs[i - 1] * 10);
            }
        }
    }
}
