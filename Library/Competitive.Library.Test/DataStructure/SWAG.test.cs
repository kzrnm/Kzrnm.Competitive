using AtCoder;
using FluentAssertions;
using System;
using Xunit;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    // verification-helper: SAMEAS Library/run.test.py
    public class SWAGTests
    {
        public struct SlideMinOp : ISegtreeOperator<int>
        {
            public int Identity => int.MaxValue;
            public int Operate(int x, int y) => Math.Min(x, y);
        }
        [Fact]
        public void SlideMin()
        {
            var array = new int[] { 4, 1, 4, 6, 2, 3, 9, 7 };
            var swag = new SWAG<int, SlideMinOp>(array);
            swag.Slide(0, 3).Should().Be(1);
            swag.Slide(1, 4).Should().Be(1);
            swag.Slide(2, 5).Should().Be(2);
            swag.Slide(3, 6).Should().Be(2);
            swag.Slide(4, 7).Should().Be(2);
            swag.Slide(5, 8).Should().Be(3);
            swag.Slide(6, 8).Should().Be(7);
            swag.Slide(7, 8).Should().Be(7);

            swag = new SWAG<int, SlideMinOp>(array);
            swag.Slide(0, 1).Should().Be(4);
            swag.Slide(0, 2).Should().Be(1);
            swag.Slide(0, 3).Should().Be(1);
            swag.Slide(0, 4).Should().Be(1);
            swag.Slide(0, 5).Should().Be(1);
            swag.Slide(0, 6).Should().Be(1);
            swag.Slide(0, 7).Should().Be(1);
            swag.Slide(0, 8).Should().Be(1);
            swag.Slide(1, 8).Should().Be(1);
            swag.Slide(2, 8).Should().Be(2);
            swag.Slide(3, 8).Should().Be(2);
            swag.Slide(4, 8).Should().Be(2);
            swag.Slide(5, 8).Should().Be(3);
            swag.Slide(6, 8).Should().Be(7);
            swag.Slide(7, 8).Should().Be(7);
        }
    }
}
