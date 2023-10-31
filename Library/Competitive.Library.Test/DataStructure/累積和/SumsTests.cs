using AtCoder;
using AtCoder.Operators;
using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class SumsTests
    {
        [Fact]
        public void IntSums()
        {
            var rnd = new Random();
            var arr = rnd.NextIntArray(100, -1000, 1000);
            var sums = new IntSums(arr);
            for (int l = 0; l <= arr.Length; l++)
                for (int r = l; r <= arr.Length; r++)
                    sums[l..r].Should().Be(arr[l..r].Sum());
        }
        [Fact]
        public void LongSums()
        {
            var rnd = new Random();
            var arr = rnd.NextIntArray(100, -1000, 1000).Select(i => (long)1e10 * i).ToArray();
            var sums = new LongSums(arr);
            for (int l = 0; l <= arr.Length; l++)
                for (int r = l; r <= arr.Length; r++)
                    sums[l..r].Should().Be(arr[l..r].Sum());
        }
        [Fact]
        public void ModSums()
        {
            var rnd = new Random();
            var arr = rnd.NextIntArray(100, -1000, 1000).Select(i => (StaticModInt<Mod100>)i).ToArray();
            var sums = new Sums<StaticModInt<Mod100>>(arr);
            for (int l = 0; l <= arr.Length; l++)
                for (int r = l; r <= arr.Length; r++)
                    sums[l..r].Should().Be(arr[l..r].Sum());
        }
        readonly struct Mod100 : IStaticMod
        {
            public uint Mod => 100;
            public bool IsPrime => false;
        }
    }
}
