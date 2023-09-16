// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly partial struct BigIntegerDecimal
    {
        [凾(256)]
        public static BigIntegerDecimal operator ++(BigIntegerDecimal value) => value + One;
        [凾(256)]
        public static BigIntegerDecimal operator +(BigIntegerDecimal left, BigIntegerDecimal right)
        {
            left.AssertValid();
            right.AssertValid();

            if (left.digits == null && right.digits == null)
                return (long)left.sign + right.sign;

            if (left.sign < 0 != right.sign < 0)
                return Subtract(left.digits, left.sign, right.digits, -1 * right.sign);
            return Add(left.digits, left.sign, right.digits, right.sign);
        }
        [凾(256)]
        public static BigIntegerDecimal operator --(BigIntegerDecimal value) => value - One;

        [凾(256)]
        public static BigIntegerDecimal operator -(BigIntegerDecimal left, BigIntegerDecimal right)
        {
            left.AssertValid();
            right.AssertValid();

            if (left.digits == null && right.digits == null)
                return (long)left.sign - right.sign;

            if (left.sign < 0 != right.sign < 0)
                return Add(left.digits, left.sign, right.digits, -1 * right.sign);
            return Subtract(left.digits, left.sign, right.digits, right.sign);
        }

        static BigIntegerDecimal Add(ReadOnlySpan<uint> leftBits, int leftSign, ReadOnlySpan<uint> rightBits, int rightSign)
        {
            bool trivialLeft = leftBits.IsEmpty;
            bool trivialRight = rightBits.IsEmpty;

            Debug.Assert(!(trivialLeft && trivialRight), "Trivial cases should be handled on the caller operator");

            BigIntegerDecimal result;
            uint[] digitsFromPool = null;

            if (trivialLeft)
            {
                Debug.Assert(!rightBits.IsEmpty);

                int size = rightBits.Length + 1;
                Span<uint> digits = ((uint)size <= StackAllocThreshold
                                ? stackalloc uint[StackAllocThreshold]
                                : digitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                Add(rightBits, (uint)Math.Abs(leftSign), digits);
                result = new(digits, leftSign < 0);
            }
            else if (trivialRight)
            {
                Debug.Assert(!leftBits.IsEmpty);

                int size = leftBits.Length + 1;
                Span<uint> digits = ((uint)size <= StackAllocThreshold
                                ? stackalloc uint[StackAllocThreshold]
                                : digitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                Add(leftBits, (uint)Math.Abs(rightSign), digits);
                result = new(digits, leftSign < 0);
            }
            else if (leftBits.Length < rightBits.Length)
            {
                Debug.Assert(!leftBits.IsEmpty && !rightBits.IsEmpty);

                int size = rightBits.Length + 1;
                Span<uint> digits = ((uint)size <= StackAllocThreshold
                                ? stackalloc uint[StackAllocThreshold]
                                : digitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                Add(rightBits, leftBits, digits);
                result = new(digits, leftSign < 0);
            }
            else
            {
                Debug.Assert(!leftBits.IsEmpty && !rightBits.IsEmpty);

                int size = leftBits.Length + 1;
                Span<uint> digits = ((uint)size <= StackAllocThreshold
                                ? stackalloc uint[StackAllocThreshold]
                                : digitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                Add(leftBits, rightBits, digits);
                result = new(digits, leftSign < 0);
            }

            if (digitsFromPool != null)
                ArrayPool<uint>.Shared.Return(digitsFromPool);

            return result;
        }
        static BigIntegerDecimal Subtract(ReadOnlySpan<uint> leftBits, int leftSign, ReadOnlySpan<uint> rightBits, int rightSign)
        {
            bool trivialLeft = leftBits.IsEmpty;
            bool trivialRight = rightBits.IsEmpty;

            Debug.Assert(!(trivialLeft && trivialRight), "Trivial cases should be handled on the caller operator");

            BigIntegerDecimal result;
            uint[] digitsFromPool = null;

            if (trivialLeft)
            {
                Debug.Assert(!rightBits.IsEmpty);

                int size = rightBits.Length;
                Span<uint> digits = (size <= StackAllocThreshold
                                ? stackalloc uint[StackAllocThreshold]
                                : digitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                Subtract(rightBits, (uint)Math.Abs(leftSign), digits);
                result = new(digits, leftSign >= 0);
            }
            else if (trivialRight)
            {
                Debug.Assert(!leftBits.IsEmpty);

                int size = leftBits.Length;
                Span<uint> digits = (size <= StackAllocThreshold
                                ? stackalloc uint[StackAllocThreshold]
                                : digitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                Subtract(leftBits, (uint)Math.Abs(rightSign), digits);
                result = new(digits, leftSign < 0);
            }
            else if (Compare(leftBits, rightBits) < 0)
            {
                int size = rightBits.Length;
                Span<uint> digits = (size <= StackAllocThreshold
                                ? stackalloc uint[StackAllocThreshold]
                                : digitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                Subtract(rightBits, leftBits, digits);
                result = new(digits, leftSign >= 0);
            }
            else
            {
                Debug.Assert(!leftBits.IsEmpty && !rightBits.IsEmpty);

                int size = leftBits.Length;
                Span<uint> digits = (size <= StackAllocThreshold
                                ? stackalloc uint[StackAllocThreshold]
                                : digitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                Subtract(leftBits, rightBits, digits);
                result = new(digits, leftSign < 0);
            }

            if (digitsFromPool != null)
                ArrayPool<uint>.Shared.Return(digitsFromPool);

            return result;
        }

        static void Add(ReadOnlySpan<uint> left, uint right, Span<uint> digits)
        {
            Debug.Assert(left.Length >= 1);
            Debug.Assert(digits.Length == left.Length + 1);

            Add(left, digits, ref MemoryMarshal.GetReference(digits), startIndex: 0, initialCarry: right);
        }

        static void Add(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> digits)
        {
            Debug.Assert(right.Length >= 1);
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(digits.Length == left.Length + 1);

            // Switching to managed references helps eliminating
            // index bounds check for all buffers.
            ref uint resultPtr = ref MemoryMarshal.GetReference(digits);
            ref uint rightPtr = ref MemoryMarshal.GetReference(right);
            ref uint leftPtr = ref MemoryMarshal.GetReference(left);

            int i = 0;
            uint carry = 0;

            // 10^9 + 10^9 < uint.MaxValue = 4294967295 なので uint で足りる

            do
            {
                carry += Unsafe.Add(ref leftPtr, i);
                carry += Unsafe.Add(ref rightPtr, i);
                carry = DivRemBase(carry, out var rem);
                Unsafe.Add(ref resultPtr, i) = rem;
                i++;
            } while (i < right.Length);

            Add(left, digits, ref resultPtr, startIndex: i, initialCarry: carry);
        }

        static void AddSelf(Span<uint> left, ReadOnlySpan<uint> right)
        {
            Debug.Assert(left.Length >= right.Length);

            int i = 0;
            uint carry = 0;

            // Switching to managed references helps eliminating
            // index bounds check...
            ref uint leftPtr = ref MemoryMarshal.GetReference(left);

            // Executes the "grammar-school" algorithm for computing z = a + b.
            // Same as above, but we're writing the result directly to a and
            // stop execution, if we're out of b and c is already 0.

            for (; i < right.Length; i++)
            {
                uint digit = (Unsafe.Add(ref leftPtr, i) + carry) + right[i];
                carry = DivRemBase(digit, out var rem);
                Unsafe.Add(ref leftPtr, i) = rem;
            }
            for (; carry != 0 && i < left.Length; i++)
            {
                uint digit = left[i] + carry;
                carry = DivRemBase(digit, out var rem);
                left[i] = rem;
            }

            Debug.Assert(carry == 0);
        }

        static void Subtract(ReadOnlySpan<uint> left, uint right, Span<uint> digits)
        {
            Debug.Assert(left.Length >= 1);
            Debug.Assert(left[0] >= right || left.Length >= 2);
            Debug.Assert(digits.Length == left.Length);

            Subtract(left, digits, ref MemoryMarshal.GetReference(digits), startIndex: 0, initialCarry: -right);
        }

        static void Subtract(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> digits)
        {
            Debug.Assert(right.Length >= 1);
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(Compare(left, right) >= 0);
            Debug.Assert(digits.Length == left.Length);

            // Switching to managed references helps eliminating
            // index bounds check for all buffers.
            ref uint resultPtr = ref MemoryMarshal.GetReference(digits);
            ref uint rightPtr = ref MemoryMarshal.GetReference(right);
            ref uint leftPtr = ref MemoryMarshal.GetReference(left);

            int i = 0;
            long carry = 0;

            // Executes the "grammar-school" algorithm for computing z = a + b.
            // While calculating z_i = a_i + b_i we take care of overflow:
            // Since a_i + b_i + c <= 2(2^32 - 1) + 1 = 2^33 - 1, our carry c
            // has always the value 1 or 0; hence, we're safe here.

            do
            {
                carry += Unsafe.Add(ref leftPtr, i);
                carry -= Unsafe.Add(ref rightPtr, i);
                carry = DivRemBase(carry, out var rem);
                Unsafe.Add(ref resultPtr, i) = rem;
                i++;
            } while (i < right.Length);

            Subtract(left, digits, ref resultPtr, startIndex: i, initialCarry: carry);
        }

        static void SubtractSelf(Span<uint> left, ReadOnlySpan<uint> right)
        {
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(Compare(left, right) >= 0);

            int i = 0;
            long carry = 0L;

            // Switching to managed references helps eliminating
            // index bounds check...
            ref uint leftPtr = ref MemoryMarshal.GetReference(left);

            // Executes the "grammar-school" algorithm for computing z = a - b.
            // Same as above, but we're writing the result directly to a and
            // stop execution, if we're out of b and c is already 0.

            for (; i < right.Length; i++)
            {
                long digit = (Unsafe.Add(ref leftPtr, i) + carry) - right[i];
                carry = DivRemBase(digit, out var rem);
                Unsafe.Add(ref leftPtr, i) = rem;
            }
            for (; carry != 0 && i < left.Length; i++)
            {
                long digit = left[i] + carry;
                carry = DivRemBase(digit, out var rem);
                left[i] = rem;
            }

            Debug.Assert(carry == 0);
        }

        [凾(256)]
        static void Add(ReadOnlySpan<uint> left, Span<uint> digits, ref uint resultPtr, int startIndex, long initialCarry)
        {
            // Executes the addition for one big and one 32-bit integer.
            // Thus, we've similar code than below, but there is no loop for
            // processing the 32-bit integer, since it's a single element.

            int i = startIndex;
            long carry = initialCarry;

            if (left.Length <= CopyToThreshold)
            {
                for (; i < left.Length; i++)
                {
                    carry += left[i];
                    carry = DivRemBase(carry, out var rem);
                    Unsafe.Add(ref resultPtr, i) = rem;
                }
                {
                    _ = DivRemBase(carry, out var rem);
                    Unsafe.Add(ref resultPtr, left.Length) = rem;
                }
            }
            else
            {
                for (; i < left.Length;)
                {
                    carry += left[i];
                    carry = DivRemBase(carry, out var rem);
                    Unsafe.Add(ref resultPtr, i) = rem;
                    i++;

                    // Once carry is set to 0 it can not be 1 anymore.
                    // So the tail of the loop is just the movement of argument values to result span.
                    if (carry == 0)
                    {
                        break;
                    }
                }
                {
                    _ = DivRemBase(carry, out var rem);
                    Unsafe.Add(ref resultPtr, left.Length) = rem;
                }

                if (i < left.Length)
                {
                    CopyTail(left, digits, i);
                }
            }
        }

        [凾(256)]
        static void Subtract(ReadOnlySpan<uint> left, Span<uint> digits, ref uint resultPtr, int startIndex, long initialCarry)
        {
            // Executes the addition for one big and one 32-bit integer.
            // Thus, we've similar code than below, but there is no loop for
            // processing the 32-bit integer, since it's a single element.

            int i = startIndex;
            long carry = initialCarry;

            if (left.Length <= CopyToThreshold)
            {
                for (; i < left.Length; i++)
                {
                    carry += left[i];
                    carry = DivRemBase(carry, out var rem);
                    Unsafe.Add(ref resultPtr, i) = rem;
                }
            }
            else
            {
                for (; i < left.Length;)
                {
                    carry += left[i];
                    carry = DivRemBase(carry, out var rem);
                    Unsafe.Add(ref resultPtr, i) = rem;
                    i++;

                    // Once carry is set to 0 it can not be 1 anymore.
                    // So the tail of the loop is just the movement of argument values to result span.
                    if (carry == 0)
                    {
                        break;
                    }
                }

                if (i < left.Length)
                {
                    CopyTail(left, digits, i);
                }
            }
        }

        static void CopyTail(ReadOnlySpan<uint> source, Span<uint> dest, int start)
        {
            source[start..].CopyTo(dest[start..]);
        }
    }
}