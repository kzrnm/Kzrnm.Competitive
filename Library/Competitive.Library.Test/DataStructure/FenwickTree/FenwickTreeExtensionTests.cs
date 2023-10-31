using AtCoder.Extension;
using System;
using System.Linq;
using IntFenwickTree = AtCoder.FenwickTree<int>;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class FenwickTreeExtensionTests
    {
        [Fact]
        public void LowerBound()
        {
            var rnd = new Random(227);
            var rndArray = new int[128];
            for (int i = 0; i < rndArray.Length; i++)
            {
                rndArray[i] = rnd.Next(314);
            }
            foreach (var array in new[]
            {
                Enumerable.Range(1, 31).ToArray(),
                Enumerable.Range(1, 32).ToArray(),
                Enumerable.Range(1, 33).ToArray(),
                Enumerable.Range(1, 100).ToArray(),
                rndArray,
            })
            {
                var fw = new IntFenwickTree(array.Length);
                for (int i = 0; i < array.Length; i++)
                    fw.Add(i, array[i]);
                var sums = array.ToArray();
                for (int i = 1; i < sums.Length; i++)
                    sums[i] += sums[i - 1];

                var max = sums[^1] + 2;
                fw.LowerBound(0).Should().Be(0);
                for (int v = 1; v < max; v++)
                {
                    var expected = sums.LowerBound(v) + 1;
                    fw.LowerBound(v).Should().Be(expected, "v={0}", v);
                    if (expected - 1 == sums.Length)
                        fw[..].Should().BeLessThan(v);
                    else
                    {
                        fw[..expected].Should().BeGreaterThanOrEqualTo(v);
                        fw[..(expected - 1)].Should().BeLessThan(v);
                    }
                }
            }
        }
        [Fact]
        public void UpperBound()
        {
            var rnd = new Random(227);
            var rndArray = new int[128];
            for (int i = 0; i < rndArray.Length; i++)
            {
                rndArray[i] = rnd.Next(314);
            }
            foreach (var array in new[]
            {
                Enumerable.Range(1, 31).ToArray(),
                Enumerable.Range(1, 32).ToArray(),
                Enumerable.Range(1, 33).ToArray(),
                Enumerable.Range(1, 100).ToArray(),
                rndArray,
            })
            {
                var fw = new IntFenwickTree(array.Length);
                for (int i = 0; i < array.Length; i++)
                    fw.Add(i, array[i]);
                var sums = array.ToArray();
                for (int i = 1; i < sums.Length; i++)
                    sums[i] += sums[i - 1];

                var max = sums[^1] + 2;
                for (int v = 0; v < max; v++)
                {
                    var expected = sums.UpperBound(v) + 1;
                    fw.UpperBound(v).Should().Be(expected, "v={0}", v);
                    if (expected - 1 == sums.Length)
                        fw[..].Should().BeLessThanOrEqualTo(v);
                    else
                    {
                        fw[..expected].Should().BeGreaterThan(v);
                        fw[..(expected - 1)].Should().BeLessThanOrEqualTo(v);
                    }
                }
            }
        }

        [Fact]
        public void Get()
        {
            var rnd = new Random(227);
            var rndArray = new int[128];
            for (int i = 0; i < rndArray.Length; i++)
            {
                rndArray[i] = rnd.Next(314);
            }
            foreach (var array in new[]
            {
                Enumerable.Range(1, 31).ToArray(),
                Enumerable.Range(1, 32).ToArray(),
                Enumerable.Range(1, 33).ToArray(),
                Enumerable.Range(1, 100).ToArray(),
                rndArray,
            })
            {
                var fw = new IntFenwickTree(array.Length);
                for (int i = 0; i < array.Length; i++)
                    fw.Add(i, array[i]);
                for (int i = 0; i < array.Length; i++)
                    fw.Get(i).Should().Be(array[i]);
            }
        }

        [Fact]
        public void ToArray()
        {
            var rnd = new Random(227);
            var rndArray = new int[128];
            for (int i = 0; i < rndArray.Length; i++)
            {
                rndArray[i] = rnd.Next(314);
            }
            foreach (var array in new[]
            {
                Enumerable.Range(1, 31).ToArray(),
                Enumerable.Range(1, 32).ToArray(),
                Enumerable.Range(1, 33).ToArray(),
                Enumerable.Range(1, 100).ToArray(),
                rndArray,
            })
            {
                var fw = new IntFenwickTree(array.Length);
                for (int i = 0; i < array.Length; i++)
                    fw.Add(i, array[i]);
                var expected = array.Select<int, (int Item, int Sum)>(i => (i, i)).ToArray();
                for (int i = 1; i < expected.Length; i++)
                {
                    expected[i].Sum += expected[i - 1].Sum;
                }
                fw.ToArray().Should().Equal(expected);
            }
        }
    }
}
