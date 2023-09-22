// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#pragma warning disable IDE0057 // 範囲演算子を使用
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
        public static BigIntegerDecimal operator *(BigIntegerDecimal left, BigIntegerDecimal right)
        {
            left.AssertValid();
            right.AssertValid();

            if (left.digits == null && right.digits == null)
                return (long)left.sign * right.sign;

            return Multiply(left.digits, left.sign, right.digits, right.sign);
        }
        static BigIntegerDecimal Multiply(ReadOnlySpan<uint> left, int leftSign, ReadOnlySpan<uint> right, int rightSign)
        {
            bool trivialLeft = left.IsEmpty;
            bool trivialRight = right.IsEmpty;

            Debug.Assert(!(trivialLeft && trivialRight), "Trivial cases should be handled on the caller operator");

            BigIntegerDecimal result;
            uint[] digitsFromPool = null;

            if (trivialLeft)
            {
                Debug.Assert(!right.IsEmpty);

                int size = right.Length + 1;
                Span<uint> digits = ((uint)size <= StackAllocThreshold
                                ? stackalloc uint[StackAllocThreshold]
                                : digitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                Multiply(right, (uint)Math.Abs(leftSign), digits);
                result = new(digits, (leftSign < 0) ^ (rightSign < 0));
            }
            else if (trivialRight)
            {
                Debug.Assert(!left.IsEmpty);

                int size = left.Length + 1;
                Span<uint> digits = ((uint)size <= StackAllocThreshold
                                ? stackalloc uint[StackAllocThreshold]
                                : digitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                Multiply(left, (uint)Math.Abs(rightSign), digits);
                result = new(digits, (leftSign < 0) ^ (rightSign < 0));
            }
            else if (left == right)
            {
                int size = left.Length + right.Length;
                Span<uint> digits = ((uint)size <= StackAllocThreshold
                                ? stackalloc uint[StackAllocThreshold]
                                : digitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                Square(left, digits);
                result = new(digits, (leftSign < 0) ^ (rightSign < 0));
            }
            else if (left.Length < right.Length)
            {
                Debug.Assert(!left.IsEmpty && !right.IsEmpty);

                int size = left.Length + right.Length;
                Span<uint> digits = ((uint)size <= StackAllocThreshold
                                ? stackalloc uint[StackAllocThreshold]
                                : digitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];
                digits.Clear();

                Multiply(right, left, digits);
                result = new(digits, (leftSign < 0) ^ (rightSign < 0));
            }
            else
            {
                Debug.Assert(!left.IsEmpty && !right.IsEmpty);

                int size = left.Length + right.Length;
                Span<uint> digits = ((uint)size <= StackAllocThreshold
                                ? stackalloc uint[StackAllocThreshold]
                                : digitsFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];
                digits.Clear();

                Multiply(left, right, digits);
                result = new(digits, (leftSign < 0) ^ (rightSign < 0));
            }

            if (digitsFromPool != null)
                ArrayPool<uint>.Shared.Return(digitsFromPool);

            return result;
        }


        const int SquareThreshold = 32;
        static void Square(ReadOnlySpan<uint> value, Span<uint> bits)
        {
            Debug.Assert(bits.Length == value.Length + value.Length);

            // Executes different algorithms for computing z = a * a
            // based on the actual length of a. If a is "small" enough
            // we stick to the classic "grammar-school" method; for the
            // rest we switch to implementations with less complexity
            // albeit more overhead (which needs to pay off!).

            // NOTE: useful thresholds needs some "empirical" testing,
            // which are smaller in DEBUG mode for testing purpose.

            if (value.Length < SquareThreshold)
            {
                // Switching to managed references helps eliminating
                // index bounds check...
                ref uint resultPtr = ref MemoryMarshal.GetReference(bits);

                // Squares the bits using the "grammar-school" method.
                // Envisioning the "rhombus" of a pen-and-paper calculation
                // we see that computing z_i+j += a_j * a_i can be optimized
                // since a_j * a_i = a_i * a_j (we're squaring after all!).
                // Thus, we directly get z_i+j += 2 * a_j * a_i + c.

                // ATTENTION: an ordinary multiplication is safe, because
                // z_i+j + a_j * a_i + c <= 2(2^32 - 1) + (2^32 - 1)^2 =
                // = 2^64 - 1 (which perfectly matches with ulong!). But
                // here we would need an UInt65... Hence, we split these
                // operation and do some extra shifts.
                for (int i = 0; i < value.Length; i++)
                {
                    ulong carry = 0UL;
                    uint v = value[i];
                    for (int j = 0; j < i; j++)
                    {
                        ulong digit1 = Unsafe.Add(ref resultPtr, i + j) + carry;
                        ulong digit2 = 2ul * value[j] * v;
                        carry = DivRemBase(digit1 + digit2, out var rem);
                        Unsafe.Add(ref resultPtr, i + j) = rem;
                    }
                    {
                        ulong digits = (ulong)v * v + carry;
                        Unsafe.Add(ref resultPtr, i + i + 1) = (uint)DivRemBase(digits, out var rem);
                        Unsafe.Add(ref resultPtr, i + i) = rem;
                    }
                }
            }
            else
            {
                // Based on the Toom-Cook multiplication we split value
                // into two smaller values, doing recursive squaring.
                // The special form of this multiplication, where we
                // split both operands into two operands, is also known
                // as the Karatsuba algorithm...

                // https://en.wikipedia.org/wiki/Toom-Cook_multiplication
                // https://en.wikipedia.org/wiki/Karatsuba_algorithm

                // Say we want to compute z = a * a ...

                // ... we need to determine our new length (just the half)
                int n = value.Length >> 1;
                int n2 = n << 1;

                // ... split value like a = (a_1 << n) + a_0
                ReadOnlySpan<uint> valueLow = value[..n];
                ReadOnlySpan<uint> valueHigh = value[n..];

                // ... prepare our result array (to reuse its memory)
                Span<uint> bitsLow = bits[..n2];
                Span<uint> bitsHigh = bits[n2..];

                // ... compute z_0 = a_0 * a_0 (squaring again!)
                Square(valueLow, bitsLow);

                // ... compute z_2 = a_1 * a_1 (squaring again!)
                Square(valueHigh, bitsHigh);

                int foldLength = valueHigh.Length + 1;
                uint[] foldFromPool = null;
                Span<uint> fold = ((uint)foldLength <= StackAllocThreshold ?
                                  stackalloc uint[StackAllocThreshold]
                                  : foldFromPool = ArrayPool<uint>.Shared.Rent(foldLength))[..foldLength];
                fold.Clear();

                int coreLength = foldLength + foldLength;
                uint[] coreFromPool = null;
                Span<uint> core = ((uint)coreLength <= StackAllocThreshold ?
                                  stackalloc uint[StackAllocThreshold]
                                  : coreFromPool = ArrayPool<uint>.Shared.Rent(coreLength))[..coreLength];
                core.Clear();

                // ... compute z_a = a_1 + a_0 (call it fold...)
                Add(valueHigh, valueLow, fold);

                // ... compute z_1 = z_a * z_a - z_0 - z_2
                Square(fold, core);

                if (foldFromPool != null)
                    ArrayPool<uint>.Shared.Return(foldFromPool);

                SubtractCore(bitsHigh, bitsLow, core);

                // ... and finally merge the result! :-)
                AddSelf(bits[n..], core);

                if (coreFromPool != null)
                    ArrayPool<uint>.Shared.Return(coreFromPool);
            }
        }

        static void Multiply(ReadOnlySpan<uint> left, uint right, Span<uint> bits)
        {
            Debug.Assert(bits.Length == left.Length + 1);

            // Executes the multiplication for one big and one 32-bit integer.
            // Since every step holds the already slightly familiar equation
            // a_i * b + c <= 2^32 - 1 + (2^32 - 1)^2 < 2^64 - 1,
            // we are safe regarding to overflows.

            int i = 0;
            ulong carry = 0UL;

            for (; i < left.Length; i++)
            {
                ulong digits = (ulong)left[i] * right + carry;
                carry = DivRemBase(digits, out var rem);
                bits[i] = rem;
            }
            bits[i] = (uint)carry;
        }
        const int MultiplyThreshold = 32;

        static void Multiply(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> bits)
        {
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(bits.Length == left.Length + right.Length);

            // Executes different algorithms for computing z = a * b
            // based on the actual length of b. If b is "small" enough
            // we stick to the classic "grammar-school" method; for the
            // rest we switch to implementations with less complexity
            // albeit more overhead (which needs to pay off!).

            // NOTE: useful thresholds needs some "empirical" testing,
            // which are smaller in DEBUG mode for testing purpose.

            if (right.Length < MultiplyThreshold)
            {
                Naive(left, right, bits);
            }
            else if (left.Length - right.Length < 2)
            {
                MultiplyNearLength(left, right, bits);
            }
            else
            {
                // Based on the Toom-Cook multiplication we split left/right
                // into two smaller values, doing recursive multiplication.
                // The special form of this multiplication, where we
                // split both operands into two operands, is also known
                // as the Karatsuba algorithm...

                // https://en.wikipedia.org/wiki/Toom-Cook_multiplication
                // https://en.wikipedia.org/wiki/Karatsuba_algorithm

                // Say we want to compute z = a * b ...

                // ... we need to determine our new length (just the half)
                int n = left.Length >> 1;
                if (right.Length <= n + 1)
                {
                    // ... split left like a = (a_1 << n) + a_0
                    ReadOnlySpan<uint> leftLow = left.Slice(0, n);
                    ReadOnlySpan<uint> leftHigh = left.Slice(n);

                    // ... split right like b = (b_1 << n) + b_0
                    ReadOnlySpan<uint> rightLow;
                    uint rightHigh;
                    if (n < right.Length)
                    {
                        Debug.Assert(right.Length == n + 1);
                        rightLow = right.Slice(0, n);
                        rightHigh = right[n];
                    }
                    else
                    {
                        rightLow = right;
                        rightHigh = 0;
                    }

                    // ... prepare our result array (to reuse its memory)
                    Span<uint> bitsLow = bits.Slice(0, n + rightLow.Length);
                    Span<uint> bitsHigh = bits.Slice(n);

                    int carryLength = rightLow.Length;
                    uint[] carryFromPool = null;
                    Span<uint> carry = ((uint)carryLength <= StackAllocThreshold ?
                                      stackalloc uint[StackAllocThreshold]
                                      : carryFromPool = ArrayPool<uint>.Shared.Rent(carryLength)).Slice(0, carryLength);

                    // ... compute low
                    Multiply(leftLow, rightLow, bitsLow);
                    Span<uint> carryOrig = bits.Slice(n, rightLow.Length);
                    carryOrig.CopyTo(carry);
                    carryOrig.Clear();

                    if (rightHigh != 0)
                    {
                        // ... compute high
                        Multiply(leftHigh, rightLow, bitsHigh.Slice(0, leftHigh.Length + n));

                        int upperRightLength = left.Length + 1;
                        uint[] upperRightFromPool = null;
                        Span<uint> upperRight = ((uint)upperRightLength <= StackAllocThreshold ?
                                          stackalloc uint[StackAllocThreshold]
                                          : upperRightFromPool = ArrayPool<uint>.Shared.Rent(upperRightLength)).Slice(0, upperRightLength);
                        upperRight.Clear();

                        Multiply(left, rightHigh, upperRight);

                        AddSelf(bitsHigh, upperRight);

                        if (upperRightFromPool != null)
                            ArrayPool<uint>.Shared.Return(upperRightFromPool);
                    }
                    else
                    {
                        // ... compute high
                        Multiply(leftHigh, rightLow, bitsHigh);
                    }

                    AddSelf(bitsHigh, carry);

                    if (carryFromPool != null)
                        ArrayPool<uint>.Shared.Return(carryFromPool);
                }
                else
                {
                    int n2 = n << 1;

                    Debug.Assert(left.Length > right.Length);

                    // ... split left like a = (a_1 << n) + a_0
                    ReadOnlySpan<uint> leftLow = left.Slice(0, n);
                    ReadOnlySpan<uint> leftHigh = left.Slice(n);

                    // ... split right like b = (b_1 << n) + b_0
                    ReadOnlySpan<uint> rightLow = right.Slice(0, n);
                    ReadOnlySpan<uint> rightHigh = right.Slice(n);

                    // ... prepare our result array (to reuse its memory)
                    Span<uint> bitsLow = bits.Slice(0, n2);
                    Span<uint> bitsHigh = bits.Slice(n2);

                    // ... compute z_0 = a_0 * b_0 (multiply again)
                    Multiply(rightLow, leftLow, bitsLow);

                    // ... compute z_2 = a_1 * b_1 (multiply again)
                    Multiply(leftHigh, rightHigh, bitsHigh);

                    int leftFoldLength = leftHigh.Length + 1;
                    uint[] leftFoldFromPool = null;
                    Span<uint> leftFold = ((uint)leftFoldLength <= StackAllocThreshold ?
                                          stackalloc uint[StackAllocThreshold]
                                          : leftFoldFromPool = ArrayPool<uint>.Shared.Rent(leftFoldLength)).Slice(0, leftFoldLength);
                    leftFold.Clear();

                    int rightFoldLength = n + 1;
                    uint[] rightFoldFromPool = null;
                    Span<uint> rightFold = ((uint)rightFoldLength <= StackAllocThreshold ?
                                           stackalloc uint[StackAllocThreshold]
                                           : rightFoldFromPool = ArrayPool<uint>.Shared.Rent(rightFoldLength)).Slice(0, rightFoldLength);
                    rightFold.Clear();

                    int coreLength = leftFoldLength + rightFoldLength;
                    uint[] coreFromPool = null;
                    Span<uint> core = ((uint)coreLength <= StackAllocThreshold ?
                                      stackalloc uint[StackAllocThreshold]
                                      : coreFromPool = ArrayPool<uint>.Shared.Rent(coreLength)).Slice(0, coreLength);
                    core.Clear();

                    Debug.Assert(bits.Length - n >= core.Length);
                    Debug.Assert(rightLow.Length >= rightHigh.Length);

                    // ... compute z_a = a_1 + a_0 (call it fold...)
                    Add(leftHigh, leftLow, leftFold);

                    // ... compute z_b = b_1 + b_0 (call it fold...)
                    Add(rightLow, rightHigh, rightFold);

                    // ... compute z_1 = z_a * z_b - z_0 - z_2
                    Multiply(leftFold, rightFold, core);

                    if (leftFoldFromPool != null)
                        ArrayPool<uint>.Shared.Return(leftFoldFromPool);

                    if (rightFoldFromPool != null)
                        ArrayPool<uint>.Shared.Return(rightFoldFromPool);

                    if (bitsHigh.Length < bitsLow.Length)
                        SubtractCore(bitsLow, bitsHigh, core);
                    else
                        SubtractCore(bitsHigh, bitsLow, core);

                    // ... and finally merge the result! :-)
                    AddSelf(bits.Slice(n), core);

                    if (coreFromPool != null)
                        ArrayPool<uint>.Shared.Return(coreFromPool);
                }
            }

            static void MultiplyNearLength(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> bits)
            {
                Debug.Assert(left.Length - right.Length < 2);
                Debug.Assert(bits.Length == left.Length + right.Length);

                // Executes different algorithms for computing z = a * b
                // based on the actual length of b. If b is "small" enough
                // we stick to the classic "grammar-school" method; for the
                // rest we switch to implementations with less complexity
                // albeit more overhead (which needs to pay off!).

                // NOTE: useful thresholds needs some "empirical" testing,
                // which are smaller in DEBUG mode for testing purpose.

                if (right.Length < MultiplyThreshold)
                {
                    Naive(left, right, bits);
                }
                else
                {
                    // Based on the Toom-Cook multiplication we split left/right
                    // into two smaller values, doing recursive multiplication.
                    // The special form of this multiplication, where we
                    // split both operands into two operands, is also known
                    // as the Karatsuba algorithm...

                    // https://en.wikipedia.org/wiki/Toom-Cook_multiplication
                    // https://en.wikipedia.org/wiki/Karatsuba_algorithm

                    // Say we want to compute z = a * b ...

                    // ... we need to determine our new length (just the half)
                    int n = right.Length >> 1;
                    int n2 = n << 1;

                    // ... split left like a = (a_1 << n) + a_0
                    ReadOnlySpan<uint> leftLow = left.Slice(0, n);
                    ReadOnlySpan<uint> leftHigh = left.Slice(n);

                    // ... split right like b = (b_1 << n) + b_0
                    ReadOnlySpan<uint> rightLow = right.Slice(0, n);
                    ReadOnlySpan<uint> rightHigh = right.Slice(n);

                    // ... prepare our result array (to reuse its memory)
                    Span<uint> bitsLow = bits.Slice(0, n2);
                    Span<uint> bitsHigh = bits.Slice(n2);

                    // ... compute z_0 = a_0 * b_0 (multiply again)
                    MultiplyNearLength(leftLow, rightLow, bitsLow);

                    // ... compute z_2 = a_1 * b_1 (multiply again)
                    MultiplyNearLength(leftHigh, rightHigh, bitsHigh);

                    int leftFoldLength = leftHigh.Length + 1;
                    uint[] leftFoldFromPool = null;
                    Span<uint> leftFold = ((uint)leftFoldLength <= StackAllocThreshold ?
                                          stackalloc uint[StackAllocThreshold]
                                          : leftFoldFromPool = ArrayPool<uint>.Shared.Rent(leftFoldLength)).Slice(0, leftFoldLength);
                    leftFold.Clear();

                    int rightFoldLength = rightHigh.Length + 1;
                    uint[] rightFoldFromPool = null;
                    Span<uint> rightFold = ((uint)rightFoldLength <= StackAllocThreshold ?
                                           stackalloc uint[StackAllocThreshold]
                                           : rightFoldFromPool = ArrayPool<uint>.Shared.Rent(rightFoldLength)).Slice(0, rightFoldLength);
                    rightFold.Clear();

                    int coreLength = leftFoldLength + rightFoldLength;
                    uint[] coreFromPool = null;
                    Span<uint> core = ((uint)coreLength <= StackAllocThreshold ?
                                      stackalloc uint[StackAllocThreshold]
                                      : coreFromPool = ArrayPool<uint>.Shared.Rent(coreLength)).Slice(0, coreLength);
                    core.Clear();

                    // ... compute z_a = a_1 + a_0 (call it fold...)
                    Add(leftHigh, leftLow, leftFold);

                    // ... compute z_b = b_1 + b_0 (call it fold...)
                    Add(rightHigh, rightLow, rightFold);

                    // ... compute z_1 = z_a * z_b - z_0 - z_2
                    MultiplyNearLength(leftFold, rightFold, core);

                    if (leftFoldFromPool != null)
                        ArrayPool<uint>.Shared.Return(leftFoldFromPool);

                    if (rightFoldFromPool != null)
                        ArrayPool<uint>.Shared.Return(rightFoldFromPool);

                    SubtractCore(bitsHigh, bitsLow, core);

                    // ... and finally merge the result! :-)
                    AddSelf(bits.Slice(n), core);

                    if (coreFromPool != null)
                        ArrayPool<uint>.Shared.Return(coreFromPool);
                }
            }
            static void Naive(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> bits)
            {
                Debug.Assert(left.Length >= right.Length);
                Debug.Assert(bits.Length == left.Length + right.Length);

                // Switching to managed references helps eliminating
                // index bounds check...
                ref uint resultPtr = ref MemoryMarshal.GetReference(bits);

                // Multiplies the bits using the "grammar-school" method.
                // Envisioning the "rhombus" of a pen-and-paper calculation
                // should help getting the idea of these two loops...
                // The inner multiplication operations are safe, because
                // z_i+j + a_j * b_i + c <= 2(2^32 - 1) + (2^32 - 1)^2 =
                // = 2^64 - 1 (which perfectly matches with ulong!).

                for (int i = 0; i < right.Length; i++)
                {
                    ulong carry = 0UL;
                    for (int j = 0; j < left.Length; j++)
                    {
                        ref uint elementPtr = ref Unsafe.Add(ref resultPtr, i + j);
                        ulong digits = elementPtr + carry + (ulong)left[j] * right[i];
                        carry = DivRemBase(digits, out var rem);
                        elementPtr = rem;
                    }
                    {
                        _ = DivRemBase(carry, out var rem);
                        Unsafe.Add(ref resultPtr, i + left.Length) = rem;
                    }
                }
            }
        }

        private static void SubtractCore(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> core)
        {
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(core.Length >= left.Length);

            // Executes a special subtraction algorithm for the multiplication,
            // which needs to subtract two different values from a core value,
            // while core is always bigger than the sum of these values.

            // NOTE: we could do an ordinary subtraction of course, but we spare
            // one "run", if we do this computation within a single one...

            int i = 0;
            long carry = 0L;

            // Switching to managed references helps eliminating
            // index bounds check...
            ref uint leftPtr = ref MemoryMarshal.GetReference(left);
            ref uint corePtr = ref MemoryMarshal.GetReference(core);

            for (; i < right.Length; i++)
            {
                long digit = (Unsafe.Add(ref corePtr, i) + carry) - Unsafe.Add(ref leftPtr, i) - right[i];
                carry = DivRemBase(digit, out var rem);
                Unsafe.Add(ref corePtr, i) = rem;
            }

            for (; i < left.Length; i++)
            {
                long digit = (Unsafe.Add(ref corePtr, i) + carry) - left[i];
                carry = DivRemBase(digit, out var rem);
                Unsafe.Add(ref corePtr, i) = rem;
            }

            for (; carry != 0 && i < core.Length; i++)
            {
                long digit = core[i] + carry;
                carry = DivRemBase(digit, out var rem);
                core[i] = rem;
            }
        }
    }
}