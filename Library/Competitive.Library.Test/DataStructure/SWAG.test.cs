using AtCoder;
using AtCoder.Internal;
using FluentAssertions;
using System;
using Xunit;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    // verification-helper: SAMEAS Library/run.test.py
    public class SwagTests
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
            var swag = new Swag<int, SlideMinOp>();
            swag.Push(4);
            swag.AllProd.Should().Be(4);
            swag.Push(1);
            swag.AllProd.Should().Be(1);
            swag.Push(4);
            swag.AllProd.Should().Be(1);
            swag.Push(6);
            swag.AllProd.Should().Be(1);
            swag.Pop();
            swag.AllProd.Should().Be(1);
            swag.Push(2);
            swag.AllProd.Should().Be(1);
            swag.Pop();
            swag.AllProd.Should().Be(2);
            swag.Push(3);
            swag.AllProd.Should().Be(2);
            swag.Pop();
            swag.AllProd.Should().Be(2);
            swag.Pop();
            swag.AllProd.Should().Be(2);
            swag.Pop();
            swag.AllProd.Should().Be(3);
            swag.Push(9);
            swag.AllProd.Should().Be(3);
            swag.Push(7);
            swag.AllProd.Should().Be(3);
            swag.Pop();
            swag.AllProd.Should().Be(7);
            swag.Pop();
            swag.AllProd.Should().Be(7);
            swag.Pop();
            swag.AllProd.Should().Be(int.MaxValue);
            swag.Invoking(swag => swag.Pop()).Should()
                .ThrowExactly<ContractAssertException>()
                .WithMessage("data is empty.");

            swag = new Swag<int, SlideMinOp>();
            swag.Push(4);
            swag.AllProd.Should().Be(4);
            swag.Push(1);
            swag.AllProd.Should().Be(1);
            swag.Push(4);
            swag.AllProd.Should().Be(1);
            swag.Push(6);
            swag.AllProd.Should().Be(1);
            swag.Push(2);
            swag.AllProd.Should().Be(1);
            swag.Push(3);
            swag.AllProd.Should().Be(1);
            swag.Push(9);
            swag.AllProd.Should().Be(1);
            swag.Push(7);
            swag.AllProd.Should().Be(1);
            swag.Pop();
            swag.AllProd.Should().Be(1);
            swag.Pop();
            swag.AllProd.Should().Be(2);
            swag.Pop();
            swag.AllProd.Should().Be(2);
            swag.Pop();
            swag.AllProd.Should().Be(2);
            swag.Pop();
            swag.AllProd.Should().Be(3);
            swag.Pop();
            swag.AllProd.Should().Be(7);
            swag.Pop();
            swag.AllProd.Should().Be(7);
            swag.Pop();
            swag.AllProd.Should().Be(int.MaxValue);
            swag.Invoking(swag => swag.Pop()).Should()
                .ThrowExactly<ContractAssertException>()
                .WithMessage("data is empty.");
        }
    }
}
