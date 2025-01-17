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
                fw.LowerBound(0).ShouldBe(0);
                for (int v = 1; v < max; v++)
                {
                    var expected = sums.LowerBound(v) + 1;
                    fw.LowerBound(v).ShouldBe(expected, $"v={v}");
                    if (expected - 1 == sums.Length)
                        fw[..].ShouldBeLessThan(v);
                    else
                    {
                        fw[..expected].ShouldBeGreaterThanOrEqualTo(v);
                        fw[..(expected - 1)].ShouldBeLessThan(v);
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
                    fw.UpperBound(v).ShouldBe(expected, $"v={v}");
                    if (expected - 1 == sums.Length)
                        fw[..].ShouldBeLessThanOrEqualTo(v);
                    else
                    {
                        fw[..expected].ShouldBeGreaterThan(v);
                        fw[..(expected - 1)].ShouldBeLessThanOrEqualTo(v);
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
                    fw.Get(i).ShouldBe(array[i]);
            }
        }

        [Fact]
        public void AddSpan()
        {
            var nums = Enumerable.Range(1, 100).ToArray().AsSpan();

            for (int n = 0; n <= 100; n++)
            {
                var fw1 = new IntFenwickTree(n);
                var fw2 = new IntFenwickTree(n);
                fw1.data.ShouldBe(fw2.data);

                fw1.Add(nums[..n]);
                for (int i = 0; i < n; i++)
                    fw2.Add(i, i + 1);
                fw1.data.ShouldBe(fw2.data);

                fw1.Add(nums[..(n / 2)]);
                for (int i = 0; i < (n / 2); i++)
                    fw2.Add(i, i + 1);
                fw1.data.ShouldBe(fw2.data);
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
                fw.ToArray().ShouldBe(expected);
            }
        }
    }
}
