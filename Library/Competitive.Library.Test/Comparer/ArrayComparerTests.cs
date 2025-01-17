using System;

namespace Kzrnm.Competitive.Testing.Comparer
{
    public class ArrayComparerTests
    {
        [Fact]
        public void Compare()
        {
            var arr = new[]
            {
                [1,2,3,],
                [1,2,0,],
                [2,2,0,],
                [1,2,0,],
                [2,2,],
                [3,],
                [1,2,],
                [0,],
                Array.Empty<int>(),
            };
            Array.Sort(arr, ArrayComparer<int>.Default);

            var expected = new[]
            {
                Array.Empty<int>(),
                [0,],
                [1,2,],
                [1,2,0,],
                [1,2,0,],
                [1,2,3,],
                [2,2,],
                [2,2,0,],
                [3,],
            };
            arr.Length.ShouldBe(expected.Length);
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].ShouldBe(expected[i], $"Index: {i}");
            }
        }

        [Fact]
        public void Reverse()
        {
            var arr = new[]
            {
                [1,2,3,],
                [1,2,0,],
                [2,2,0,],
                [1,2,0,],
                [2,2,],
                [3,],
                [1,2,],
                [0,],
                Array.Empty<int>(),
            };
            Array.Sort(arr, ArrayComparer<int>.Reverse);

            var expected = new[]
            {
                [3,],
                [2,2,0,],
                [2,2,],
                [1,2,3,],
                [1,2,0,],
                [1,2,0,],
                [1,2,],
                [0,],
                Array.Empty<int>(),
            };
            arr.Length.ShouldBe(expected.Length);
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].ShouldBe(expected[i], $"Index: {i}");
            }
        }
    }
}
