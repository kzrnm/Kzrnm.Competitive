
namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class FenwickTree2DTests
    {
        [Fact]
        public void AddAndQuery()
        {
            var bit = new IntFenwickTree2D(2, 3);
            bit[0..2][0..3].ShouldBe(0);
            bit.Add(0, 0, 1);
            bit.Add(0, 1, 2);
            bit.Add(0, 2, 3);
            bit[0..1][0..3].ShouldBe(6);
            bit[0..2][0..3].ShouldBe(6);
            bit.Add(1, 0, 4);
            bit.Add(1, 1, 5);
            bit.Add(1, 2, 6);
            bit[0..2][0..1].ShouldBe(5);
            bit[1..2][1..2].ShouldBe(5);
            bit[0..2][0..2].ShouldBe(12);
            bit[0..1][0..3].ShouldBe(6);
            bit[1..2][0..3].ShouldBe(15);
            bit[0..2][0..3].ShouldBe(21);
        }
    }
}
