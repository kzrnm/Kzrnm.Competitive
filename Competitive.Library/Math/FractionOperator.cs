using AtCoder.Operators;
using System;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    public struct FractionOperator : INumOperator<Fraction>, ICompareOperator<Fraction>
    {
        public Fraction MinValue => new Fraction(long.MinValue, 1);
        public Fraction MaxValue => new Fraction(long.MaxValue, 1);
        public Fraction MultiplyIdentity => new Fraction(1, 1);

        [MethodImpl(AggressiveInlining)]
        public Fraction Add(Fraction x, Fraction y) => x + y;
        [MethodImpl(AggressiveInlining)]
        public Fraction Subtract(Fraction x, Fraction y) => x - y;
        [MethodImpl(AggressiveInlining)]
        public Fraction Multiply(Fraction x, Fraction y) => x * y;
        [MethodImpl(AggressiveInlining)]
        public Fraction Divide(Fraction x, Fraction y) => x / y;
        [MethodImpl(AggressiveInlining)]
        public Fraction Modulo(Fraction x, Fraction y) => throw new NotSupportedException();

        [MethodImpl(AggressiveInlining)]
        public int Compare(Fraction x, Fraction y) => x.CompareTo(y);
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThan(Fraction x, Fraction y) => x > y;
        [MethodImpl(AggressiveInlining)]
        public bool GreaterThanOrEqual(Fraction x, Fraction y) => x >= y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThan(Fraction x, Fraction y) => x < y;
        [MethodImpl(AggressiveInlining)]
        public bool LessThanOrEqual(Fraction x, Fraction y) => x <= y;

        [MethodImpl(AggressiveInlining)]
        public Fraction Minus(Fraction x) => -x;
        [MethodImpl(AggressiveInlining)]
        public Fraction Increment(Fraction x) => throw new NotSupportedException();
        [MethodImpl(AggressiveInlining)]
        public Fraction Decrement(Fraction x) => throw new NotSupportedException();
    }
}
