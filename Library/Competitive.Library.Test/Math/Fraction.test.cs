using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kzrnm.Competitive.Testing.MathNS
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
    public class FractionTests
    {
        static IEnumerable<Fraction> RandomFractions(Random rnd)
            => Enumerable.Repeat(rnd, 1000).Select(rnd => new Fraction(rnd.Next(), rnd.Next()));
        public static TheoryData Construct_Data = new TheoryData<long, long, long, long>
        {
            { 16, 4, 4, 1 },
            { 2, 845106, 1, 422553 },
            { 230895518700, 230811434700, 9995477, 9991837 },
            { 1, 2, 1, 2 },
            { -1,  2, -1, 2 },
            {  1, -2, -1, 2 },
            { -1, -2,  1, 2 },
            {  2,  2,  1, 1 },
            { -2,  2, -1, 1 },
            {  2, -2, -1, 1 },
            { -2, -2,  1, 1 },
        };
        [Theory]
        [MemberData(nameof(Construct_Data))]
        [Trait("Category", "Normal")]
        public void ConstructTest(long 分子in, long 分母in, long 分子out, long 分母out)
        {
            var f = new Fraction(分子in, 分母in);
            f.Numerator.Should().Be(分子out);
            f.Denominator.Should().Be(分母out);
        }

        public static TheoryData ToString_Data = new TheoryData<Fraction, string>
        {
            { new Fraction(16, 4), "4/1" },
            { new Fraction(2, 845106), "1/422553" },
            { new Fraction(230895518700, 230811434700), "9995477/9991837" },
            { new Fraction(1, 2), "1/2" },
            { new Fraction(-1, 2), "-1/2" },
            { new Fraction(1, -2), "-1/2" },
            { new Fraction(-1, -2), "1/2" },
        };
        [Theory]
        [MemberData(nameof(ToString_Data))]
        [Trait("Category", "Normal")]
        public void ToStringTest(Fraction num, string expected)
        {
            num.ToString().Should().Be(expected);
        }
        [Fact]
        [Trait("Category", "Normal")]
        public void LongImplicitTest()
        {
            foreach (var num in Enumerable.Repeat(new Random(150), 1000).Select(rnd => rnd.Next()))
            {
                Fraction f = num;
                f.Should().Be(new Fraction(num, 1));
            }
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void EqualsTest()
        {
            foreach (var f in RandomFractions(new Random(3153)))
            {
                var f2 = f;
                (f == f2).Should().BeTrue();
                (f != f2).Should().BeFalse();
                (f >= f2).Should().BeTrue();
                (f <= f2).Should().BeTrue();
                (f > f2).Should().BeFalse();
                (f < f2).Should().BeFalse();
                f.Equals(f2).Should().BeTrue();
                f.Equals((object)f2).Should().BeTrue();
                f.CompareTo(f2).Should().Be(0);
            }
        }

        public static TheoryData GreaterThan_Data = new TheoryData<Fraction, Fraction>
        {
            { new Fraction(3, 1), new Fraction(2, 1) },
            { new Fraction(2, 3), new Fraction(1, 2) },
            { new Fraction(5, 6), new Fraction(4, 9) },
        };
        [Theory]
        [MemberData(nameof(GreaterThan_Data))]
        [Trait("Category", "Normal")]
        public void GreaterThanTest(Fraction left, Fraction right)
        {
            (left == right).Should().BeFalse();
            (left != right).Should().BeTrue();
            (left >= right).Should().BeTrue();
            (left <= right).Should().BeFalse();
            (left > right).Should().BeTrue();
            (left < right).Should().BeFalse();
            left.Equals(right).Should().BeFalse();
            left.Equals((object)right).Should().BeFalse();
            left.CompareTo(right).Should().BeGreaterThan(0);
            right.CompareTo(left).Should().BeLessThan(0);
        }

        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinusTest()
        {
            foreach (var f in RandomFractions(new Random(13)))
            {
                (-f).Should().Be(new Fraction(-f.Numerator, f.Denominator));
                default(FractionOperator).Minus(f).Should().Be(new Fraction(-f.Numerator, f.Denominator));
            }
        }

        public static TheoryData Add_Data = new TheoryData<Fraction, Fraction, Fraction>
        {
            { new Fraction(3, 1), new Fraction(2, 1), new Fraction(5, 1) },
            { new Fraction(1, 2), new Fraction(1, 3), new Fraction(5, 6) },
            { new Fraction(1, 6), new Fraction(1, 3), new Fraction(1, 2) },
            { new Fraction(-1,6), new Fraction(1, 3), new Fraction(1, 6) },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Add_Data))]
        public void AddTest(Fraction num1, Fraction num2, Fraction expected)
        {
            (num1 + num2).Should().Be(expected);
            default(FractionOperator).Add(num1, num2).Should().Be(expected);
        }

        public static TheoryData Subtract_Data = new TheoryData<Fraction, Fraction, Fraction>
        {
            { new Fraction(3, 1), new Fraction(2, 1), new Fraction(1, 1) },
            { new Fraction(1, 2), new Fraction(1, 3), new Fraction(1, 6) },
            { new Fraction(1, 6), new Fraction(1, 3), new Fraction(-1, 6) },
            { new Fraction(-1, 6), new Fraction(1, 3), new Fraction(-1, 2) },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Subtract_Data))]
        public void SubtractTest(Fraction num1, Fraction num2, Fraction expected)
        {
            (num1 - num2).Should().Be(expected);
            default(FractionOperator).Subtract(num1, num2).Should().Be(expected);
        }

        public static TheoryData Multiply_Data = new TheoryData<Fraction, Fraction, Fraction>
        {
            { new Fraction(3, 1), new Fraction(5, 1), new Fraction(15, 1) },
            { new Fraction(1, 2), new Fraction(1, 7), new Fraction(1, 14) },
            { new Fraction(-1, 6), new Fraction(2, 3), new Fraction(-1, 9) },
            { new Fraction(-1, 16), new Fraction(-4, 3), new Fraction(1, 12) },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void MultiplyTest(Fraction num1, Fraction num2, Fraction expected)
        {
            (num1 * num2).Should().Be(expected);
            default(FractionOperator).Multiply(num1, num2).Should().Be(expected);
        }

        public static TheoryData Divide_Data = new TheoryData<Fraction, Fraction, Fraction>
        {
            { new Fraction(3, 1), new Fraction(2, 1), new Fraction(3, 2) },
            { new Fraction(1, 2), new Fraction(1, 7), new Fraction(7, 2) },
            { new Fraction(-1, 6), new Fraction(2, 3), new Fraction(-1, 4) },
            { new Fraction(-1, 12), new Fraction(-4, 3), new Fraction(1, 16) },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Divide_Data))]
        public void DivideTest(Fraction num1, Fraction num2, Fraction expected)
        {
            (num1 / num2).Should().Be(expected);
            default(FractionOperator).Divide(num1, num2).Should().Be(expected);
        }

        [Fact]
        public void InverseTest()
        {
            foreach (var f in RandomFractions(new Random(48463)))
            {
                f.Inverse().Should().Be(new Fraction(f.Denominator, f.Numerator));
            }
        }

    }
}
