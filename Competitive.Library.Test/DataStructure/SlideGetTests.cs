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
            var slideFront3 = SlideGet.Front(array, 3, new SlideMinOp());
            slideFront3.Should().Equal(4, 1, 1, 1, 2, 2, 2, 3);
            var slideBack3 = SlideGet.Back(array, 3, new SlideMinOp());
            slideBack3.Should().Equal(1, 1, 2, 2, 2, 3, 7, 7);
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
                var slideFront = SlideGet.Front(array, k, new SlideMinOp());
                var slideBack = SlideGet.Back(array, k, new SlideMinOp());
                var expectedFront = new int[30];
                var expectedBack = new int[30];
                for (int i = 0; i < expectedBack.Length; i++)
                {
                    expectedFront[i] = st[Math.Max(0, i - k + 1)..(i + 1)];
                    expectedBack[i] = st[i..Math.Min(30, i + k)];
                }
                slideFront.Should().Equal(expectedFront);
                slideBack.Should().Equal(expectedBack);
            }
        }
    }
}
