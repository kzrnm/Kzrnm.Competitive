// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Buffers;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly partial struct BigIntegerDecimal
    {
        [凾(256)]
        public static BigIntegerDecimal operator /(BigIntegerDecimal dividend, BigIntegerDecimal divisor)
        {
            dividend.AssertValid();
            divisor.AssertValid();

            bool trivialDividend = dividend.digits == null;
            bool trivialDivisor = divisor.digits == null;

            if (trivialDividend && trivialDivisor)
            {
                return dividend.sign / divisor.sign;
            }

            if (trivialDividend)
            {
                // The divisor is non-trivial
                // and therefore the bigger one
                return s_bnZeroInt;
            }

            uint[] quotientFromPool = null;

            if (trivialDivisor)
            {
                Debug.Assert(dividend.digits != null);

                int size = dividend.digits.Length;
                Span<uint> quotient = ((uint)size <= StackAllocThreshold
                                    ? stackalloc uint[StackAllocThreshold]
                                    : quotientFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                try
                {
                    //may throw DivideByZeroException
                    Divide(dividend.digits, (uint)Math.Abs(divisor.sign), quotient);
                    return new(quotient, (dividend.sign < 0) ^ (divisor.sign < 0));
                }
                finally
                {
                    if (quotientFromPool != null)
                        ArrayPool<uint>.Shared.Return(quotientFromPool);
                }
            }

            Debug.Assert(dividend.digits != null && divisor.digits != null);

            if (dividend.digits.Length < divisor.digits.Length)
            {
                return s_bnZeroInt;
            }
            else
            {
                int size = dividend.digits.Length - divisor.digits.Length + 1;
                Span<uint> quotient = ((uint)size < StackAllocThreshold
                                    ? stackalloc uint[StackAllocThreshold]
                                    : quotientFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                Divide(dividend.digits, divisor.digits, quotient);
                var result = new BigIntegerDecimal(quotient, (dividend.sign < 0) ^ (divisor.sign < 0));

                if (quotientFromPool != null)
                    ArrayPool<uint>.Shared.Return(quotientFromPool);

                return result;
            }
        }

        [凾(256)]
        public static BigIntegerDecimal operator %(BigIntegerDecimal dividend, BigIntegerDecimal divisor)
        {
            dividend.AssertValid();
            divisor.AssertValid();

            bool trivialDividend = dividend.digits == null;
            bool trivialDivisor = divisor.digits == null;

            if (trivialDividend && trivialDivisor)
            {
                return dividend.sign % divisor.sign;
            }

            if (trivialDividend)
            {
                // The divisor is non-trivial
                // and therefore the bigger one
                return dividend;
            }

            if (trivialDivisor)
            {
                Debug.Assert(dividend.digits != null);
                uint remainder = Remainder(dividend.digits, (uint)Math.Abs(divisor.sign));
                return dividend.sign < 0 ? -1 * remainder : remainder;
            }

            Debug.Assert(dividend.digits != null && divisor.digits != null);

            if (dividend.digits.Length < divisor.digits.Length)
            {
                return dividend;
            }

            uint[] digitsFromPool = null;
            int size = dividend.digits.Length;
            Span<uint> digits = (size <= StackAllocThreshold
                            ? stackalloc uint[StackAllocThreshold]
                            : digitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

            Remainder(dividend.digits, divisor.digits, digits);
            var result = new BigIntegerDecimal(digits, dividend.sign < 0);

            if (digitsFromPool != null)
                ArrayPool<uint>.Shared.Return(digitsFromPool);

            return result;
        }
        [凾(256)]
        public static (BigIntegerDecimal Quotient, BigIntegerDecimal Remainder) DivRem(BigIntegerDecimal dividend, BigIntegerDecimal divisor)
        {
            return (DivRem(dividend, divisor, out var r), r);
        }
        [凾(256)]
        public static BigIntegerDecimal DivRem(BigIntegerDecimal dividend, BigIntegerDecimal divisor, out BigIntegerDecimal remainder)
        {
            dividend.AssertValid();
            divisor.AssertValid();

            bool trivialDividend = dividend.digits == null;
            bool trivialDivisor = divisor.digits == null;

            if (trivialDividend && trivialDivisor)
            {
                BigIntegerDecimal quotient;
                (quotient, remainder) = Math.DivRem(dividend.sign, divisor.sign);
                return quotient;
            }

            if (trivialDividend)
            {
                // The divisor is non-trivial
                // and therefore the bigger one
                remainder = dividend;
                return s_bnZeroInt;
            }

            Debug.Assert(dividend.digits != null);

            if (trivialDivisor)
            {
                uint rest;

                uint[] bitsFromPool = null;
                int size = dividend.digits.Length;
                Span<uint> quotient = ((uint)size <= StackAllocThreshold
                                    ? stackalloc uint[StackAllocThreshold]
                                    : bitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                try
                {
                    // may throw DivideByZeroException
                    Divide(dividend.digits, (uint)Math.Abs(divisor.sign), quotient, out rest);

                    remainder = dividend.sign < 0 ? -1 * rest : rest;
                    return new BigIntegerDecimal(quotient, (dividend.sign < 0) ^ (divisor.sign < 0));
                }
                finally
                {
                    if (bitsFromPool != null)
                        ArrayPool<uint>.Shared.Return(bitsFromPool);
                }
            }

            Debug.Assert(divisor.digits != null);

            if (dividend.digits.Length < divisor.digits.Length)
            {
                remainder = dividend;
                return s_bnZeroInt;
            }
            else
            {
                uint[] remainderFromPool = null;
                int size = dividend.digits.Length;
                Span<uint> rest = ((uint)size <= StackAllocThreshold
                                ? stackalloc uint[StackAllocThreshold]
                                : remainderFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                uint[] quotientFromPool = null;
                size = dividend.digits.Length - divisor.digits.Length + 1;
                Span<uint> quotient = ((uint)size <= StackAllocThreshold
                                    ? stackalloc uint[StackAllocThreshold]
                                    : quotientFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                Divide(dividend.digits, divisor.digits, quotient, rest);

                remainder = new BigIntegerDecimal(rest, dividend.sign < 0);
                var result = new BigIntegerDecimal(quotient, (dividend.sign < 0) ^ (divisor.sign < 0));

                if (remainderFromPool != null)
                    ArrayPool<uint>.Shared.Return(remainderFromPool);

                if (quotientFromPool != null)
                    ArrayPool<uint>.Shared.Return(quotientFromPool);

                return result;
            }
        }



        //private static int Reduce(Span<uint> digits, ReadOnlySpan<uint> modulus)
        //{
        //    // Executes a modulo operation using the divide operation.

        //    if (digits.Length >= modulus.Length)
        //    {
        //        Divide(digits, modulus, default);

        //        return ActualLength(digits.Slice(0, modulus.Length));
        //    }
        //    return digits.Length;
        //}

        static void Divide(ReadOnlySpan<uint> left, uint right, Span<uint> quotient, out uint remainder)
        {
            Debug.Assert(left.Length >= 1);
            Debug.Assert(quotient.Length == left.Length);

            // Executes the division for one big and one 32-bit integer.
            // Thus, we've similar code than below, but there is no loop for
            // processing the 32-bit integer, since it's a single element.

            ulong carry = 0UL;

            for (int i = left.Length - 1; i >= 0; i--)
            {
                ulong value = (carry * BASE) + left[i];
                ulong digit = value / right;
                Debug.Assert(digit < BASE);
                quotient[i] = (uint)digit;
                carry = value - digit * right;
                Debug.Assert(carry < BASE);
            }
            remainder = (uint)carry;
        }

        static void Divide(ReadOnlySpan<uint> left, uint right, Span<uint> quotient)
        {
            Debug.Assert(left.Length >= 1);
            Debug.Assert(quotient.Length == left.Length);

            // Same as above, but only computing the quotient.

            ulong carry = 0UL;
            for (int i = left.Length - 1; i >= 0; i--)
            {
                ulong value = (carry * BASE) + left[i];
                ulong digit = value / right;
                Debug.Assert(digit < BASE);
                quotient[i] = (uint)digit;
                carry = value - digit * right;
                Debug.Assert(carry < BASE);
            }
        }

        static uint Remainder(ReadOnlySpan<uint> left, uint right)
        {
            Debug.Assert(left.Length >= 1);

            // Same as above, but only computing the remainder.
            ulong carry = 0UL;
            for (int i = left.Length - 1; i >= 0; i--)
            {
                ulong value = (carry * BASE) + left[i];
                carry = value % right;
            }

            return (uint)carry;
        }

        static void Divide(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> quotient, Span<uint> remainder)
        {
            Debug.Assert(left.Length >= 1);
            Debug.Assert(right.Length >= 1);
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(quotient.Length == left.Length - right.Length + 1);
            Debug.Assert(remainder.Length == left.Length);

            left.CopyTo(remainder);
            Divide2(remainder, right, quotient);
        }

        static void Divide(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> quotient)
        {
            Debug.Assert(left.Length >= 1);
            Debug.Assert(right.Length >= 1);
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(quotient.Length == left.Length - right.Length + 1);

            // Same as above, but only returning the quotient.

            uint[] leftCopyFromPool = null;

            // NOTE: left will get overwritten, we need a local copy
            // However, mutated left is not used afterwards, so use array pooling or stack alloc
            Span<uint> leftCopy = (left.Length <= StackAllocThreshold ?
                                  stackalloc uint[StackAllocThreshold]
                                  : leftCopyFromPool = ArrayPool<uint>.Shared.Rent(left.Length)).Slice(0, left.Length);
            left.CopyTo(leftCopy);

            Divide2(leftCopy, right, quotient);

            if (leftCopyFromPool != null)
                ArrayPool<uint>.Shared.Return(leftCopyFromPool);
        }

        static void Remainder(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> remainder)
        {
            Debug.Assert(left.Length >= 1);
            Debug.Assert(right.Length >= 1);
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(remainder.Length >= left.Length);

            // Same as above, but only returning the remainder.

            left.CopyTo(remainder);
            Divide2(remainder, right, default);
        }

        static void Divide2(Span<uint> left, ReadOnlySpan<uint> right, Span<uint> digits)
        {
            Debug.Assert(left.Length >= 1);
            Debug.Assert(right.Length >= 1);
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(digits.Length == left.Length - right.Length + 1
                || digits.Length == 0);

            // Executes the "grammar-school" algorithm for computing q = a / b.
            // Before calculating q_i, we get more digits into the highest bit
            // block of the divisor. Thus, guessing digits of the quotient
            // will be more precise. Additionally we'll get r = a % b.

            uint divHi = right[^1];
            uint divLo = right.Length > 1 ? right[^2] : 0;

            // Then, we divide all of the digits as we would do it using
            // pen and paper: guessing the next digit, subtracting, ...
            for (int i = left.Length; i >= right.Length; i--)
            {
                int n = i - right.Length;
                uint t = (uint)i < (uint)left.Length ? left[i] : 0;

                ulong valHi = ((ulong)t * BASE) + left[i - 1];
                uint valLo = i > 1 ? left[i - 2] : 0;

                // First guess for the current digit of the quotient,
                // which naturally must have only 32 digits...
                //ulong digit = valHi / divHi;
                ulong digit = (ulong)(((UInt128)valHi * BASE + valLo) / ((UInt128)divHi * BASE + divLo));
                // Our first guess may be a little bit to big
                Debug.Assert(!DivideGuessTooBig(digit, valHi, valLo, divHi, divLo));

                if (digit > 0)
                {
                    // Now it's time to subtract our current quotient
                    uint carry = SubtractDivisor(left[n..], right, digit);
                    if (carry != t)
                    {
                        Debug.Assert(carry == t + 1);

                        // Our guess was still exactly one too high
                        carry = AddDivisor(left[n..], right);
                        --digit;

                        Debug.Assert(carry == 1);
                    }
                }

                // We have the digit!
                if ((uint)n < (uint)digits.Length)
                    digits[n] = (uint)digit;

                if ((uint)i < (uint)left.Length)
                    left[i] = 0;
            }
        }

        static uint AddDivisor(Span<uint> left, ReadOnlySpan<uint> right)
        {
            Debug.Assert(left.Length >= right.Length);

            // Repairs the dividend, if the last subtract was too much

            ulong carry = 0UL;

            for (int i = 0; i < right.Length; i++)
            {
                ref uint leftElement = ref left[i];
                ulong digit = (leftElement + carry) + right[i];
                carry = DivRemBase(digit, out var rem);
                leftElement = rem;
            }

            return (uint)carry;
        }

        static uint SubtractDivisor(Span<uint> left, ReadOnlySpan<uint> right, ulong q)
        {
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(q <= 0xFFFFFFFF);

            // Combines a subtract and a multiply operation, which is naturally
            // more efficient than multiplying and then subtracting...

            ulong carry = 0UL;

            for (int i = 0; i < right.Length; i++)
            {
                carry += right[i] * q;
                carry = DivRemBase(carry, out var rem);
                uint digit = rem;

                ref uint leftElement = ref left[i];
                if (leftElement < digit)
                {
                    ++carry;
                    leftElement = BASE + leftElement - digit;
                }
                else
                    leftElement -= digit;
            }

            return (uint)carry;
        }

        static bool DivideGuessTooBig(ulong q, ulong valHi, uint valLo,
                                              uint divHi, uint divLo)
        {
            Debug.Assert(q < BASE);

            // We multiply the two most significant limbs of the divisor
            // with the current guess for the quotient. If those are bigger
            // than the three most significant limbs of the current dividend
            // we return true, which means the current guess is still too big.

            ulong chkHi = divHi * q;
            ulong chkLo = divLo * q;

            chkHi += DivRemBase(chkLo, out var rem);
            chkLo = rem;

            if (chkHi < valHi)
                return false;
            if (chkHi > valHi)
                return true;

            if (chkLo < valLo)
                return false;
            if (chkLo > valLo)
                return true;

            return false;
        }
    }
}