using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Number
{
    public class FractionTests
    {
        static IEnumerable<Fraction> RandomFractions(Random rnd)
            => Enumerable.Repeat(rnd, 1000).Select(rnd => new Fraction(rnd.Next(), rnd.Next()));
        public static TheoryData<long, long, long, long> Construct_Data => new()
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
            f.Numerator.ShouldBe(分子out);
            f.Denominator.ShouldBe(分母out);
        }

        public static TheoryData<long, long, string> ToString_Data => new()
        {
            { 16, 4, "4/1" },
            { 2, 845106, "1/422553" },
            { 230895518700, 230811434700, "9995477/9991837" },
            { 1, 2, "1/2" },
            { -1, 2, "-1/2" },
            { 1, -2, "-1/2" },
        };
        [Theory]
        [MemberData(nameof(ToString_Data))]
        [Trait("Category", "Normal")]
        public void ParseAndToStringTest(long numerator, long denominator, string text)
        {
            var num = new Fraction(numerator, denominator);
            num.ToString().ShouldBe(text);
            Fraction.Parse(text).ShouldBe(num);
            Fraction.Parse($"{numerator}/{denominator}").ShouldBe(num);
        }
        [Fact]
        [Trait("Category", "Normal")]
        public void LongImplicitTest()
        {
            foreach (var num in Enumerable.Repeat(new Random(150), 1000).Select(rnd => rnd.Next()))
            {
                Fraction f = num;
                f.ShouldBe(new Fraction(num, 1));
            }
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void EqualsTest()
        {
            foreach (var f in RandomFractions(new Random(3153)))
            {
                var f2 = f;
                (f == f2).ShouldBeTrue();
                (f != f2).ShouldBeFalse();
                (f >= f2).ShouldBeTrue();
                (f <= f2).ShouldBeTrue();
                (f > f2).ShouldBeFalse();
                (f < f2).ShouldBeFalse();
                f.Equals(f2).ShouldBeTrue();
                f.Equals((object)f2).ShouldBeTrue();
                f.CompareTo(f2).ShouldBe(0);
            }
        }

        public static TheoryData<Fraction, Fraction> GreaterThan_Data => new()
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
            (left == right).ShouldBeFalse();
            (left != right).ShouldBeTrue();
            (left >= right).ShouldBeTrue();
            (left <= right).ShouldBeFalse();
            (left > right).ShouldBeTrue();
            (left < right).ShouldBeFalse();
            left.Equals(right).ShouldBeFalse();
            left.Equals((object)right).ShouldBeFalse();
            left.CompareTo(right).ShouldBeGreaterThan(0);
            right.CompareTo(left).ShouldBeLessThan(0);
        }

        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinusTest()
        {
            foreach (var f in RandomFractions(new Random(13)))
            {
                (-f).ShouldBe(new Fraction(-f.Numerator, f.Denominator));
            }
        }

        public static TheoryData<Fraction, Fraction, Fraction> Add_Data => new()
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
            (num1 + num2).ShouldBe(expected);
        }

        public static TheoryData<Fraction, Fraction, Fraction> Subtract_Data => new()
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
            (num1 - num2).ShouldBe(expected);
        }

        public static TheoryData<Fraction, Fraction, Fraction> Multiply_Data => new()
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
            (num1 * num2).ShouldBe(expected);
        }

        public static TheoryData<Fraction, Fraction, Fraction> Divide_Data => new()
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
            (num1 / num2).ShouldBe(expected);
        }

        [Fact]
        public void InverseTest()
        {
            foreach (var f in RandomFractions(new Random(48463)))
            {
                f.Inverse().ShouldBe(new Fraction(f.Denominator, f.Numerator));
            }
        }

    }
}
