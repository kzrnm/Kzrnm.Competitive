using AtCoderProject;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Xunit;

namespace AtCoderLib.Global
{
    public class ComparerTests
    {
        [Fact]
        public void DefaultReverse()
        {
            ExComparer<int>.DefaultReverse.Compare(2, 1).Should().BeLessThan(0);
            ExComparer<int>.DefaultReverse.Compare(1, 2).Should().BeGreaterThan(0);
            ExComparer<int>.DefaultReverse.Compare(2, 2).Should().Be(0);
        }
        [Fact]
        public void ExpressionComparer()
        {
            var cmp = ExComparer<int>.CreateExp(i => -i);
            cmp.Compare(2, 1).Should().BeLessThan(0);
            cmp.Compare(1, 2).Should().BeGreaterThan(0);
            cmp.Compare(2, 2).Should().Be(0);
        }
    }
}