using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AtCoder
{
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
            { -1, 2, -1, 2 },
            { 1, -2, -1, 2 },
            { -1, -2, 1, 2 },
        };
        [Theory]
        [MemberData(nameof(Construct_Data))]
        [Trait("Category", "Normal")]
        public void ConstructTest(long 分子in, long 分母in, long 分子out, long 分母out)
        {
            var f = new Fraction(分子in, 分母in);
            f.numerator.Should().Be(分子out);
            f.denominator.Should().Be(分母out);
        }

        public static TheoryData ToString_Data = new TheoryData<long, long, string>
        {
            { 16, 4, "4/1" },
            { 2, 845106, "1/422553" },
            { 230895518700, 230811434700, "9995477/9991837" },
            { 1, 2, "1/2" },
            { -1, 2, "-1/2" },
            { 1, -2, "-1/2" },
            { -1, -2, "1/2" },
        };
        [Theory]
        [MemberData(nameof(ToString_Data))]
        [Trait("Category", "Normal")]
        public void ToStringTest(long 分子in, long 分母in, string expected)
        {
            new Fraction(分子in, 分母in).ToString().Should().Be(expected);
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

        public static TheoryData GreaterThan_Data = new TheoryData<long, long, long, long>
        {
            { 3, 1, 2, 1 },
            { 2, 3, 1, 2 },
            { 5, 6, 4, 9 },
        };
        [Theory]
        [MemberData(nameof(GreaterThan_Data))]
        [Trait("Category", "Normal")]
        public void GreaterThanTest(long 分子left, long 分母left, long 分子right, long 分母right)
        {
            var left = new Fraction(分子left, 分母left);
            var right = new Fraction(分子right, 分母right);

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
                (-f).Should().Be(new Fraction(-f.numerator, f.denominator));
            }
        }

        public static TheoryData Plus_Data = new TheoryData<long, long, long, long, long, long>
        {
            { 3, 1, 2, 1, 5, 1 },
            { 1, 2, 1, 3, 5, 6 },
            { 1, 6, 1, 3, 1, 2 },
            { -1, 6, 1, 3, 1, 6 },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Plus_Data))]
        public void PlusTest(long 分子1, long 分母1, long 分子2, long 分母2, long 分子expected, long 分母expected)
        {
            (new Fraction(分子1, 分母1) + new Fraction(分子2, 分母2)).Should().Be(new Fraction(分子expected, 分母expected));
        }

        public static TheoryData Minus_Data = new TheoryData<long, long, long, long, long, long>
        {
            { 3, 1, 2, 1, 1, 1 },
            { 1, 2, 1, 3, 1, 6 },
            { 1, 6, 1, 3, -1, 6 },
            { -1, 6, 1, 3, -1, 2 },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Minus_Data))]
        public void MinusTest(long 分子1, long 分母1, long 分子2, long 分母2, long 分子expected, long 分母expected)
        {
            (new Fraction(分子1, 分母1) - new Fraction(分子2, 分母2)).Should().Be(new Fraction(分子expected, 分母expected));
        }

        public static TheoryData Multi_Data = new TheoryData<long, long, long, long, long, long>
        {
            { 3, 1, 5, 1, 15, 1 },
            { 1, 2, 1, 7, 1, 14 },
            { -1, 6, 2, 3, -1, 9 },
            { -1, 16, -4, 3, 1, 12 },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multi_Data))]
        public void MultiTest(long 分子1, long 分母1, long 分子2, long 分母2, long 分子expected, long 分母expected)
        {
            (new Fraction(分子1, 分母1) * new Fraction(分子2, 分母2)).Should().Be(new Fraction(分子expected, 分母expected));
        }


        public static TheoryData Div_Data = new TheoryData<long, long, long, long, long, long>
        {
            { 3, 1, 2, 1, 3, 2 },
            { 1, 2, 1, 7, 7, 2 },
            { -1, 6, 2, 3, -1, 4 },
            { -1, 12, -4, 3, 1, 16 },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Div_Data))]
        public void DivTest(long 分子1, long 分母1, long 分子2, long 分母2, long 分子expected, long 分母expected)
        {
            (new Fraction(分子1, 分母1) / new Fraction(分子2, 分母2)).Should().Be(new Fraction(分子expected, 分母expected));
        }

        [Fact]
        public void InverseTest()
        {
            foreach (var f in RandomFractions(new Random(48463)))
            {
                f.Inverse().Should().Be(new Fraction(f.denominator, f.numerator));
            }
        }

    }
}
