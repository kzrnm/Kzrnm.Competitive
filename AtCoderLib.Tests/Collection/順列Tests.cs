using FluentAssertions;
using Xunit;

namespace AtCoderLib.Collection
{
    public class 順列Tests
    {
        [Fact]
        public void Permutation3Test()
        {
            var permutation = 順列を求める.Permutation(new[] { 0, 1, 2 }).GetEnumerator();
            var expected = new int[][]
            {
                new[]{0,1,2},
                new[]{0,2,1},
                new[]{1,0,2},
                new[]{1,2,0},
                new[]{2,0,1},
                new[]{2,1,0},
            };
            foreach (var ex in expected)
            {
                permutation.MoveNext().Should().BeTrue();
                permutation.Current.Should().Equal(ex);
            }
            permutation.MoveNext().Should().BeFalse();
        }
        [Fact]
        public void Permutation4Test()
        {
            var permutation = 順列を求める.Permutation(new[] { 0, 1, 2, 3 }).GetEnumerator();
            var expected = new int[][]
            {
                new[]{0,1,2,3},
                new[]{0,1,3,2},
                new[]{0,2,1,3},
                new[]{0,2,3,1},
                new[]{0,3,1,2},
                new[]{0,3,2,1},
                new[]{1,0,2,3},
                new[]{1,0,3,2},
                new[]{1,2,0,3},
                new[]{1,2,3,0},
                new[]{1,3,0,2},
                new[]{1,3,2,0},
                new[]{2,0,1,3},
                new[]{2,0,3,1},
                new[]{2,1,0,3},
                new[]{2,1,3,0},
                new[]{2,3,0,1},
                new[]{2,3,1,0},
                new[]{3,0,1,2},
                new[]{3,0,2,1},
                new[]{3,1,0,2},
                new[]{3,1,2,0},
                new[]{3,2,0,1},
                new[]{3,2,1,0},
            };
            foreach (var ex in expected)
            {
                permutation.MoveNext().Should().BeTrue();
                permutation.Current.Should().Equal(ex);
            }
            permutation.MoveNext().Should().BeFalse();
        }
        [Fact]
        public void Permutation4_2Test()
        {
            var permutation = 順列を求める.Permutation(new[] { 3, 1, 0, 2 }).GetEnumerator();
            var expected = new int[][]
            {
                new[]{3,1,0,2},
                new[]{3,1,2,0},
                new[]{3,2,0,1},
                new[]{3,2,1,0},
            };
            foreach (var ex in expected)
            {
                permutation.MoveNext().Should().BeTrue();
                permutation.Current.Should().Equal(ex);
            }
            permutation.MoveNext().Should().BeFalse();
        }
    }
}
