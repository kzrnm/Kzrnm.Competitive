using System.Linq;

namespace Kzrnm.Competitive.Testing.GlobalNS
{
    public class GlobalNewArrayTests
    {
        [Fact]
        public void NewArray1()
        {
            var arr = Global.NewArray(2, 1);
            arr.Length.ShouldBe(2);
            arr.ShouldBe(Enumerable.Repeat(1, 2));
        }
        [Fact]
        public void NewArrayFunc1()
        {
            var arr = Global.NewArray(2, () => new object());
            arr.Length.ShouldBe(2);
            arr.Distinct().Count().ShouldBe(2);
        }
        [Fact]
        public void NewArray2()
        {
            var arr = Global.NewArray(2, 3, 1);
            arr.SelectMany(a => a).Count().ShouldBe(6);
            arr.SelectMany(a => a).ShouldBe(Enumerable.Repeat(1, 6));
        }
        [Fact]
        public void NewArrayFunc2()
        {
            var arr = Global.NewArray(2, 3, () => new object());
            arr.SelectMany(a => a).Count().ShouldBe(6);
            arr.SelectMany(a => a).Distinct().Count().ShouldBe(6);
        }
        [Fact]
        public void NewArray3()
        {
            var arr = Global.NewArray(2, 3, 5, 1);
            arr.SelectMany(a => a).SelectMany(a => a).Count().ShouldBe(30);
            arr.SelectMany(a => a).SelectMany(a => a).ShouldBe(Enumerable.Repeat(1, 30));
        }
        [Fact]
        public void NewArrayFunc3()
        {
            var arr = Global.NewArray(2, 3, 5, () => new object());
            arr.SelectMany(a => a).SelectMany(a => a).Count().ShouldBe(30);
            arr.SelectMany(a => a).SelectMany(a => a).Distinct().Count().ShouldBe(30);
        }
        [Fact]
        public void NewArray4()
        {
            var arr = Global.NewArray(2, 3, 5, 7, 1);
            arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Count().ShouldBe(210);
            arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).ShouldBe(Enumerable.Repeat(1, 210));
        }
        [Fact]
        public void NewArrayFunc4()
        {
            var arr = Global.NewArray(2, 3, 5, 7, () => new object());
            arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Count().ShouldBe(210);
            arr.SelectMany(a => a).SelectMany(a => a).SelectMany(a => a).Distinct().Count().ShouldBe(210);
        }
    }
}
