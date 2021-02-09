using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Kzrnm.Competitive.MathNS
{
    public class PrimeNumberTests
    {
        [Fact]
        public void PrimeTest()
        {
            new PrimeNumber(5).Should().Equal(new[] { 2, 3, 5 });
            new PrimeNumber(6).Should().Equal(new[] { 2, 3, 5 });
            new PrimeNumber(7).Should().Equal(new[] { 2, 3, 5, 7 });
            new PrimeNumber(9).Should().Equal(new[] { 2, 3, 5, 7 });
            new PrimeNumber(10).Should().Equal(new[] { 2, 3, 5, 7 });
            new PrimeNumber(11).Should().Equal(new[] { 2, 3, 5, 7, 11 });
            new PrimeNumber(20).Should().Equal(new[] { 2, 3, 5, 7, 11, 13, 17, 19 });
        }

        [Fact]
        public void PrimeFactoringTest()
        {
            var pr = new PrimeNumber((int)1e7);
            pr.PrimeFactoring(1 << 16).Should().Equal(new Dictionary<int, int>
            {
                { 2, 16 },
            });
            pr.PrimeFactoring(2 * 3 * 5).Should().Equal(new Dictionary<int, int>
            {
                { 2, 1 },
                { 3, 1 },
                { 5, 1 },
            });
            pr.PrimeFactoring(99991).Should().Equal(new Dictionary<int, int>
            {
                { 99991, 1 },
            });
            pr.PrimeFactoring(2147483647).Should().Equal(new Dictionary<int, int>
            {
                { 2147483647, 1 },
            });
            pr.PrimeFactoring(132147483703).Should().Equal(new Dictionary<long, int>
            {
                { 132147483703, 1 },
            });
            pr.PrimeFactoring(903906555552).Should().Equal(new Dictionary<long, int>
            {
                { 2, 5 },
                { 3, 8 },
                { 7, 1 },
                { 11, 2 },
                { 13, 1 },
                { 17, 1 },
                { 23, 1 },
            });
        }
    }
}
