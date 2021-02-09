using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Kzrnm.Competitive.DataStructure.String
{
    public class RollingHashTests
    {
        [Fact]
        public void Simple()
        {
            const string a2m = "abcdefghijklm";
            ReadOnlySpan<char> str = a2m + a2m;
            var rh = new RollingHash(str);

            var notMatchCount = 0;
            var notMatchHashNotMatchCount = 0;

            for (int l1 = 0; l1 < str.Length; l1++)
                for (int r1 = l1 + 1; r1 < str.Length; r1++)
                    for (int l2 = 0; l2 < str.Length; l2++)
                        for (int r2 = l2 + 1; r2 < str.Length; r2++)
                        {
                            if (str[l1..r1].Equals(str[l2..r2], StringComparison.Ordinal))
                            {
                                rh[l1..r1].Should().Be(rh[l2..r2]);
                            }
                            else
                            {
                                notMatchCount++;
                                if (!rh[l1..r1].Equals(rh[l2..r2]))
                                    notMatchHashNotMatchCount++;
                            }
                        }
            ((double)notMatchHashNotMatchCount / notMatchCount).Should().BeGreaterThan(0.99);
        }
        [Fact]
        public void Repeat()
        {
            ReadOnlySpan<char> str = string.Join("", Enumerable.Repeat(new Random().NextString(10), 7));
            var rh = new RollingHash(str);

            var notMatchCount = 0;
            var notMatchHashNotMatchCount = 0;

            for (int l1 = 0; l1 < str.Length; l1++)
                for (int r1 = l1 + 1; r1 < str.Length; r1++)
                    for (int l2 = 0; l2 < str.Length; l2++)
                        for (int r2 = l2 + 1; r2 < str.Length; r2++)
                        {
                            if (str[l1..r1].Equals(str[l2..r2], StringComparison.Ordinal))
                            {
                                rh[l1..r1].Should().Be(rh[l2..r2]);
                            }
                            else
                            {
                                notMatchCount++;
                                if (!rh[l1..r1].Equals(rh[l2..r2]))
                                    notMatchHashNotMatchCount++;
                            }
                        }
           ((double)notMatchHashNotMatchCount / notMatchCount).Should().BeGreaterThan(0.99);
        }
        [Fact]
        public void Large()
        {
            ReadOnlySpan<char> str = new Random().NextString(70);
            var rh = new RollingHash(str);

            var notMatchCount = 0;
            var notMatchHashNotMatchCount = 0;

            for (int l1 = 0; l1 < str.Length; l1++)
                for (int r1 = l1 + 1; r1 < str.Length; r1++)
                    for (int l2 = 0; l2 < str.Length; l2++)
                        for (int r2 = l2 + 1; r2 < str.Length; r2++)
                        {
                            if (str[l1..r1].Equals(str[l2..r2], StringComparison.Ordinal))
                            {
                                rh[l1..r1].Should().Be(rh[l2..r2]);
                            }
                            else
                            {
                                notMatchCount++;
                                if (!rh[l1..r1].Equals(rh[l2..r2]))
                                    notMatchHashNotMatchCount++;
                            }
                        }
           ((double)notMatchHashNotMatchCount / notMatchCount).Should().BeGreaterThan(0.99);
        }
    }
}
