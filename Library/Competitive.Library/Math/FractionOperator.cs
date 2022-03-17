using AtCoder.Operators;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public struct FractionOperator : INumOperator<Fraction>, ICompareOperator<Fraction>
    {
        public Fraction MinValue => new Fraction(long.MinValue, 1);
        public Fraction MaxValue => new Fraction(long.MaxValue, 1);
        public Fraction MultiplyIdentity => new Fraction(1, 1);

        [凾(256)]
        public Fraction Add(Fraction x, Fraction y) => x + y;
        [凾(256)]
        public Fraction Subtract(Fraction x, Fraction y) => x - y;
        [凾(256)]
        public Fraction Multiply(Fraction x, Fraction y) => x * y;
        [凾(256)]
        public Fraction Divide(Fraction x, Fraction y) => x / y;
        [凾(256)]
        public Fraction Modulo(Fraction x, Fraction y) => throw new NotSupportedException();

        [凾(256)]
        public int Compare(Fraction x, Fraction y) => x.CompareTo(y);
        [凾(256)]
        public bool GreaterThan(Fraction x, Fraction y) => x > y;
        [凾(256)]
        public bool GreaterThanOrEqual(Fraction x, Fraction y) => x >= y;
        [凾(256)]
        public bool LessThan(Fraction x, Fraction y) => x < y;
        [凾(256)]
        public bool LessThanOrEqual(Fraction x, Fraction y) => x <= y;

        [凾(256)]
        public Fraction Minus(Fraction x) => -x;
        [凾(256)]
        public Fraction Increment(Fraction x) => throw new NotSupportedException();
        [凾(256)]
        public Fraction Decrement(Fraction x) => throw new NotSupportedException();
    }
}
