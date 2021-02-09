using FluentAssertions;
using System.Linq;
using Xunit;

namespace AtCoder.MathNS
{
    public class StaticModIntExtensionTests
    {
        [Fact]
        public void Sum()
        {
            var arrInt = Enumerable.Range(1, 10).ToArray();
            var arrMod = arrInt.Select(i => (StaticModInt<Mod1000000007>)i).ToArray();
            for (int i = 0; i < 10; i++)
                for (int j = i; j < 10; j++)
                    arrMod[i..j].Sum().Value.Should().Be(arrInt[i..j].Sum());
        }
    }
}
