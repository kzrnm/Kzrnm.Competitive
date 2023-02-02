using AtCoder;
using AtCoder.Extension;
using System;
using System.Linq;
#if NET7_0_OR_GREATER
using IntFenwickTree = AtCoder.FenwickTree<int>;
#endif

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

                var max = fw[..] + 2;
                for (int v = 0; v < max; v++)
                {
                    fw.LowerBound(v).Should().Be(sums.LowerBound(v), "v={0}", v);
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

                var max = fw[..] + 2;
                for (int v = 0; v < max; v++)
                {
                    fw.UpperBound(v).Should().Be(sums.UpperBound(v), "v={0}", v);
                }
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
