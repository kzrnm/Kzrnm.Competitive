using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AtCoderLib.文字列
{
    public class SuffixArrayTests
    {
        readonly SuffixArray sa = new SuffixArray(Util.Str);

        [Theory]
        [InlineData(0, 1, 1998)]
        [InlineData(0, 23000, 999)]
        [InlineData(0, 24000, 999)]
        public void LCP(int i, int j, int expect)
        {
            sa.GetLCP(i, j).Should().Be(expect);
        }
    }
}
