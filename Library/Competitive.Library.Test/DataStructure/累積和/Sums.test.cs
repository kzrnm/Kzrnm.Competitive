using AtCoder;
using AtCoder.Operators;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
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
            var sums = new StaticModIntSums<Mod100>(arr);
            for (int l = 0; l <= arr.Length; l++)
                for (int r = l; r <= arr.Length; r++)
                    sums[l..r].Should().Be(arr[l..r].Sum());
        }
        [Fact]
        public void Operator()
        {
            var rnd = new Random();
            var arr = new (int x, int y)[4] {
                (1,3),
                (2,5),
                (4,9),
                (8,17),
            };

            var sums = new Sums<(int x, int y), Op>(arr);
            sums[0..0].Should().Be((0, 0));
            sums[0..1].Should().Be((1, 3));
            sums[0..2].Should().Be((3, 8));
            sums[0..3].Should().Be((7, 17));
            sums[0..4].Should().Be((15, 34));
            sums[1..1].Should().Be((0, 0));
            sums[1..2].Should().Be((2, 5));
            sums[1..3].Should().Be((6, 14));
            sums[1..4].Should().Be((14, 31));
            sums[2..2].Should().Be((0, 0));
            sums[2..3].Should().Be((4, 9));
            sums[2..4].Should().Be((12, 26));
            sums[3..3].Should().Be((0, 0));
            sums[3..4].Should().Be((8, 17));
            sums[4..4].Should().Be((0, 0));
        }
        struct Mod100 : IStaticMod
        {
            public uint Mod => 100;
            public bool IsPrime => false;
        }

        struct Op : IAdditionOperator<(int x, int y)>, ISubtractOperator<(int x, int y)>
        {
            public (int x, int y) Add((int x, int y) x, (int x, int y) y) => (x.x + y.x, x.y + y.y);
            public (int x, int y) Subtract((int x, int y) x, (int x, int y) y) => (x.x - y.x, x.y - y.y);
        }
    }
}
