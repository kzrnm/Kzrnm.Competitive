using FluentAssertions;
using System.Linq;

namespace Kzrnm.Competitive.Testing.GlobalNS
{
    public class GlobalNewArrayTests
    {
        [Fact]
        public void NewArray1()
        {
            var arr = Global.NewArray(2, 1);
            arr.Should().HaveCount(2);
            arr.Should().Equal(Enumerable.Repeat(1, 2));
        }
        [Fact]
        public void NewArrayFunc1()
        {
            var arr = Global.NewArray(2, () => new object());
            arr.Should().HaveCount(2);
            arr.Distinct().Should().HaveCount(2);
        }
        [Fact]
        public void NewArray2()
        {
            var arr = Global.NewArray(2, 3, 1);
            arr.SelectMany(a => a).Should().HaveCount(6);
            arr.SelectMany(a => a).Should().Equal(Enumerable.Repeat(1, 6));
        }
        [Fact]
        public void NewArrayFunc2()
        {
            var arr = Global.NewArray(2, 3, () => new object());
            arr.SelectMany(a => a).Should().HaveCount(6);
            arr.SelectMany(a => a).Distinct().Should().HaveCount(6);
        }
        [Fact]
        public void NewArray3()
        {
            var arr = Global.NewArray(2, 3, 5, 1);
            arr.SelectMany(a => a).SelectMany(a => a).Should().HaveCount(30);
            arr.SelectMany(a => a).SelectMany(a => a).Should().Equal(Enumerable.Repeat(1, 30));
        }
        [Fact]
        public void NewArrayFunc3()
        {
            var arr = Global.NewArray(2, 3, 5, () => new object());
            arr.SelectMany(a => a).SelectMany(a => a).Should().HaveCount(30);
            arr.SelectMany(a => a).SelectMany(a => a).Distinct().Should().HaveCount(30);
        }
        [Fact]
        public void NewArray4()
        {
            var arr = Global.NewArray(2, 3, 5, 7, 1);
            arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Should().HaveCount(210);
            arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Should().Equal(Enumerable.Repeat(1, 210));
        }
        [Fact]
        public void NewArrayFunc4()
        {
            var arr = Global.NewArray(2, 3, 5, 7, () => new object());
            arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Should().HaveCount(210);
            arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Distinct().Should().HaveCount(210);
        }
    }
}
