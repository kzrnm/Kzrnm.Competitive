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
                new[]{ 1,2,3, },
                new[]{ 1,2,0, },
                new[]{ 2,2,0, },
                new[]{ 1,2,0, },
                new[]{ 2,2, },
                new[]{ 3, },
                new[]{ 1,2, },
                new[]{ 0, },
                Array.Empty<int>(),
            };
            Array.Sort(arr, ArrayComparer<int>.Default);

            var expected = new[]
            {
                Array.Empty<int>(),
                new[]{ 0, },
                new[]{ 1,2, },
                new[]{ 1,2,0, },
                new[]{ 1,2,0, },
                new[]{ 1,2,3, },
                new[]{ 2,2, },
                new[]{ 2,2,0, },
                new[]{ 3, },
            };
            arr.Should().HaveSameCount(expected);
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].Should().BeEquivalentTo(expected[i], "Index: {0}", i);
            }
        }

        [Fact]
        public void Reverse()
        {
            var arr = new[]
            {
                new[]{ 1,2,3, },
                new[]{ 1,2,0, },
                new[]{ 2,2,0, },
                new[]{ 1,2,0, },
                new[]{ 2,2, },
                new[]{ 3, },
                new[]{ 1,2, },
                new[]{ 0, },
                Array.Empty<int>(),
            };
            Array.Sort(arr, ArrayComparer<int>.Reverse);

            var expected = new[]
            {
                new[]{ 3, },
                new[]{ 2,2,0, },
                new[]{ 2,2, },
                new[]{ 1,2,3, },
                new[]{ 1,2,0, },
                new[]{ 1,2,0, },
                new[]{ 1,2, },
                new[]{ 0, },
                Array.Empty<int>(),
            };
            arr.Should().HaveSameCount(expected);
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].Should().BeEquivalentTo(expected[i], "Index: {0}", i);
            }
        }
    }
}
