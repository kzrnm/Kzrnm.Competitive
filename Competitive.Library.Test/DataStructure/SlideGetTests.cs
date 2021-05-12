using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kzrnm.Competitive.DataStructure
{
    public class SlideGetTests
    {
        public struct SlideMinOp : SlideGetOperator<int>, ISparseTableOperator<int>
        {
            public bool NeedUpdate(int oldValue, int newValue) => newValue <= oldValue;
            public int Operate(int x, int y) => Math.Min(x, y);
        }
        [Fact]
        public void SlideMin()
        {
            var array = new int[] { 4, 1, 4, 6, 2, 3, 9, 7 };
            var slide3 = SlideGet.Get(array, 3, new SlideMinOp());
            slide3.Should().Equal(1, 1, 2, 2, 2, 3, 7, 7);
        }

        [Fact]
        public void SlideMinRandom()
        {
            var rnd = new Random(227);
            var array = new int[30];
            for (int i = 0; i < array.Length; i++)
                array[i] = rnd.Next();
            var st = new SparseTable<int, SlideMinOp>(array);
            for (int k = 1; k <= 30; k++)
            {
                var slide = SlideGet.Get(array, k, new SlideMinOp());
                var expected = new int[30];
                for (int i = 0; i < expected.Length; i++)
                    expected[i] = st[i..Math.Min(30, i + k)];
                slide.Should().Equal(expected);
            }
        }
    }
}
