#if false

        private static BigIntegerDecimal Add(ReadOnlySpan<uint> leftBits, int leftSign, ReadOnlySpan<uint> rightBits, int rightSign)
        {
            bool trivialLeft = leftBits.IsEmpty;
            bool trivialRight = rightBits.IsEmpty;

            if (trivialLeft && trivialRight)
            {
                return (long)leftSign + rightSign;
            }

            BigIntegerDecimal result;
            uint[] bitsFromPool = null;

            if (trivialLeft)
            {
                Debug.Assert(!rightBits.IsEmpty);

                int size = rightBits.Length + 1;
                Span<uint> bits = ((uint)size <= BigIntegerDecimalCalculator.StackAllocThreshold
                                ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                : bitsFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);

                BigIntegerDecimalCalculator.Add(rightBits, (uint)Math.Abs(leftSign), bits);
                result = new BigIntegerDecimal(bits, leftSign < 0);
            }
            else if (trivialRight)
            {
                Debug.Assert(!leftBits.IsEmpty);

                int size = leftBits.Length + 1;
                Span<uint> bits = ((uint)size <= BigIntegerDecimalCalculator.StackAllocThreshold
                                ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                : bitsFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);

                BigIntegerDecimalCalculator.Add(leftBits, (uint)Math.Abs(rightSign), bits);
                result = new BigIntegerDecimal(bits, leftSign < 0);
            }
            else if (leftBits.Length < rightBits.Length)
            {
                Debug.Assert(!leftBits.IsEmpty && !rightBits.IsEmpty);

                int size = rightBits.Length + 1;
                Span<uint> bits = ((uint)size <= BigIntegerDecimalCalculator.StackAllocThreshold
                                ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                : bitsFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);

                BigIntegerDecimalCalculator.Add(rightBits, leftBits, bits);
                result = new BigIntegerDecimal(bits, leftSign < 0);
            }
            else
            {
                Debug.Assert(!leftBits.IsEmpty && !rightBits.IsEmpty);

                int size = leftBits.Length + 1;
                Span<uint> bits = ((uint)size <= BigIntegerDecimalCalculator.StackAllocThreshold
                                ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                : bitsFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);

                BigIntegerDecimalCalculator.Add(leftBits, rightBits, bits);
                result = new BigIntegerDecimal(bits, leftSign < 0);
            }

            if (bitsFromPool != null)
                ArrayPool<uint>.Shared.Return(bitsFromPool);

            return result;
        }

        public static BigIntegerDecimal operator -(BigIntegerDecimal left, BigIntegerDecimal right)
        {
            left.AssertValid();
            right.AssertValid();

            if (left._sign < 0 != right._sign < 0)
                return Add(left._bits, left._sign, right._bits, -1 * right._sign);
            return Subtract(left._bits, left._sign, right._bits, right._sign);
        }

        private static BigIntegerDecimal Subtract(ReadOnlySpan<uint> leftBits, int leftSign, ReadOnlySpan<uint> rightBits, int rightSign)
        {
            bool trivialLeft = leftBits.IsEmpty;
            bool trivialRight = rightBits.IsEmpty;

            if (trivialLeft && trivialRight)
            {
                return (long)leftSign - rightSign;
            }

            BigIntegerDecimal result;
            uint[] bitsFromPool = null;

            if (trivialLeft)
            {
                Debug.Assert(!rightBits.IsEmpty);

                int size = rightBits.Length;
                Span<uint> bits = (size <= BigIntegerDecimalCalculator.StackAllocThreshold
                                ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                : bitsFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);

                BigIntegerDecimalCalculator.Subtract(rightBits, (uint)Math.Abs(leftSign), bits);
                result = new BigIntegerDecimal(bits, leftSign >= 0);
            }
            else if (trivialRight)
            {
                Debug.Assert(!leftBits.IsEmpty);

                int size = leftBits.Length;
                Span<uint> bits = (size <= BigIntegerDecimalCalculator.StackAllocThreshold
                                ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                : bitsFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);

                BigIntegerDecimalCalculator.Subtract(leftBits, (uint)Math.Abs(rightSign), bits);
                result = new BigIntegerDecimal(bits, leftSign < 0);
            }
            else if (BigIntegerDecimalCalculator.Compare(leftBits, rightBits) < 0)
            {
                int size = rightBits.Length;
                Span<uint> bits = (size <= BigIntegerDecimalCalculator.StackAllocThreshold
                                ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                : bitsFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);

                BigIntegerDecimalCalculator.Subtract(rightBits, leftBits, bits);
                result = new BigIntegerDecimal(bits, leftSign >= 0);
            }
            else
            {
                Debug.Assert(!leftBits.IsEmpty && !rightBits.IsEmpty);

                int size = leftBits.Length;
                Span<uint> bits = (size <= BigIntegerDecimalCalculator.StackAllocThreshold
                                ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                : bitsFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);

                BigIntegerDecimalCalculator.Subtract(leftBits, rightBits, bits);
                result = new BigIntegerDecimal(bits, leftSign < 0);
            }

            if (bitsFromPool != null)
                ArrayPool<uint>.Shared.Return(bitsFromPool);

            return result;
        }







        
        public static BigIntegerDecimal operator ++(BigIntegerDecimal value)
        {
            return value + One;
        }

        public static BigIntegerDecimal operator --(BigIntegerDecimal value)
        {
            return value - One;
        }




        public static BigIntegerDecimal operator *(BigIntegerDecimal left, BigIntegerDecimal right)
        {
            left.AssertValid();
            right.AssertValid();

            return Multiply(left._bits, left._sign, right._bits, right._sign);
        }

        private static BigIntegerDecimal Multiply(ReadOnlySpan<uint> left, int leftSign, ReadOnlySpan<uint> right, int rightSign)
        {
            bool trivialLeft = left.IsEmpty;
            bool trivialRight = right.IsEmpty;

            if (trivialLeft && trivialRight)
            {
                return (long)leftSign * rightSign;
            }

            BigIntegerDecimal result;
            uint[] bitsFromPool = null;

            if (trivialLeft)
            {
                Debug.Assert(!right.IsEmpty);

                int size = right.Length + 1;
                Span<uint> bits = ((uint)size <= BigIntegerDecimalCalculator.StackAllocThreshold
                                ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                : bitsFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);

                BigIntegerDecimalCalculator.Multiply(right, (uint)Math.Abs(leftSign), bits);
                result = new BigIntegerDecimal(bits, (leftSign < 0) ^ (rightSign < 0));
            }
            else if (trivialRight)
            {
                Debug.Assert(!left.IsEmpty);

                int size = left.Length + 1;
                Span<uint> bits = ((uint)size <= BigIntegerDecimalCalculator.StackAllocThreshold
                                ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                : bitsFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);

                BigIntegerDecimalCalculator.Multiply(left, (uint)Math.Abs(rightSign), bits);
                result = new BigIntegerDecimal(bits, (leftSign < 0) ^ (rightSign < 0));
            }
            else if (left == right)
            {
                int size = left.Length + right.Length;
                Span<uint> bits = ((uint)size <= BigIntegerDecimalCalculator.StackAllocThreshold
                                ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                : bitsFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);

                BigIntegerDecimalCalculator.Square(left, bits);
                result = new BigIntegerDecimal(bits, (leftSign < 0) ^ (rightSign < 0));
            }
            else if (left.Length < right.Length)
            {
                Debug.Assert(!left.IsEmpty && !right.IsEmpty);

                int size = left.Length + right.Length;
                Span<uint> bits = ((uint)size <= BigIntegerDecimalCalculator.StackAllocThreshold
                                ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                : bitsFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);
                bits.Clear();

                BigIntegerDecimalCalculator.Multiply(right, left, bits);
                result = new BigIntegerDecimal(bits, (leftSign < 0) ^ (rightSign < 0));
            }
            else
            {
                Debug.Assert(!left.IsEmpty && !right.IsEmpty);

                int size = left.Length + right.Length;
                Span<uint> bits = ((uint)size <= BigIntegerDecimalCalculator.StackAllocThreshold
                                ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                : bitsFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);
                bits.Clear();

                BigIntegerDecimalCalculator.Multiply(left, right, bits);
                result = new BigIntegerDecimal(bits, (leftSign < 0) ^ (rightSign < 0));
            }

            if (bitsFromPool != null)
                ArrayPool<uint>.Shared.Return(bitsFromPool);

            return result;
        }

        public static BigIntegerDecimal operator /(BigIntegerDecimal dividend, BigIntegerDecimal divisor)
        {
            dividend.AssertValid();
            divisor.AssertValid();

            bool trivialDividend = dividend._bits == null;
            bool trivialDivisor = divisor._bits == null;

            if (trivialDividend && trivialDivisor)
            {
                return dividend._sign / divisor._sign;
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
                Debug.Assert(dividend._bits != null);

                int size = dividend._bits.Length;
                Span<uint> quotient = ((uint)size <= BigIntegerDecimalCalculator.StackAllocThreshold
                                    ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                    : quotientFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);

                try
                {
                    //may throw DivideByZeroException
                    BigIntegerDecimalCalculator.Divide(dividend._bits, (uint)Math.Abs(divisor._sign), quotient);
                    return new BigIntegerDecimal(quotient, (dividend._sign < 0) ^ (divisor._sign < 0));
                }
                finally
                {
                    if (quotientFromPool != null)
                        ArrayPool<uint>.Shared.Return(quotientFromPool);
                }
            }

            Debug.Assert(dividend._bits != null && divisor._bits != null);

            if (dividend._bits.Length < divisor._bits.Length)
            {
                return s_bnZeroInt;
            }
            else
            {
                int size = dividend._bits.Length - divisor._bits.Length + 1;
                Span<uint> quotient = ((uint)size < BigIntegerDecimalCalculator.StackAllocThreshold
                                    ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                                    : quotientFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);

                BigIntegerDecimalCalculator.Divide(dividend._bits, divisor._bits, quotient);
                var result = new BigIntegerDecimal(quotient, (dividend._sign < 0) ^ (divisor._sign < 0));

                if (quotientFromPool != null)
                    ArrayPool<uint>.Shared.Return(quotientFromPool);

                return result;
            }
        }

        public static BigIntegerDecimal operator %(BigIntegerDecimal dividend, BigIntegerDecimal divisor)
        {
            dividend.AssertValid();
            divisor.AssertValid();

            bool trivialDividend = dividend._bits == null;
            bool trivialDivisor = divisor._bits == null;

            if (trivialDividend && trivialDivisor)
            {
                return dividend._sign % divisor._sign;
            }

            if (trivialDividend)
            {
                // The divisor is non-trivial
                // and therefore the bigger one
                return dividend;
            }

            if (trivialDivisor)
            {
                Debug.Assert(dividend._bits != null);
                uint remainder = BigIntegerDecimalCalculator.Remainder(dividend._bits, (uint)Math.Abs(divisor._sign));
                return dividend._sign < 0 ? -1 * remainder : remainder;
            }

            Debug.Assert(dividend._bits != null && divisor._bits != null);

            if (dividend._bits.Length < divisor._bits.Length)
            {
                return dividend;
            }

            uint[] bitsFromPool = null;
            int size = dividend._bits.Length;
            Span<uint> bits = (size <= BigIntegerDecimalCalculator.StackAllocThreshold
                            ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                            : bitsFromPool = ArrayPool<uint>.Shared.Rent(size)).Slice(0, size);

            BigIntegerDecimalCalculator.Remainder(dividend._bits, divisor._bits, bits);
            var result = new BigIntegerDecimal(bits, dividend._sign < 0);

            if (bitsFromPool != null)
                ArrayPool<uint>.Shared.Return(bitsFromPool);

            return result;
        }
        




        //
        // IAdditiveIdentity
        //


        public static (BigIntegerDecimal Quotient, BigIntegerDecimal Remainder) DivRem(BigIntegerDecimal left, BigIntegerDecimal right)
        {
            BigIntegerDecimal quotient = DivRem(left, right, out BigIntegerDecimal remainder);
            return (quotient, remainder);
        }


        //
        // IMultiplicativeIdentity
        //













        //
        // IShiftOperators
        //

        public static BigIntegerDecimal operator >>>(BigIntegerDecimal value, int shiftAmount)
        {
            value.AssertValid();

            if (shiftAmount == 0)
                return value;

            if (shiftAmount == int.MinValue)
                return ((value << int.MaxValue) << 1);

            if (shiftAmount < 0)
                return value << -shiftAmount;

            (int digitShift, int smallShift) = Math.DivRem(shiftAmount, kcbitUint);

            BigIntegerDecimal result;

            uint[] xdFromPool = null;
            int xl = value._bits?.Length ?? 1;
            Span<uint> xd = (xl <= BigIntegerDecimalCalculator.StackAllocThreshold
                          ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                          : xdFromPool = ArrayPool<uint>.Shared.Rent(xl)).Slice(0, xl);

            bool negx = value.GetPartsForBitManipulation(xd);

            if (negx)
            {
                if (shiftAmount >= ((long)kcbitUint * xd.Length))
                {
                    result = MinusOne;
                    goto exit;
                }

                NumericsHelpers.DangerousMakeTwosComplement(xd); // Mutates xd
            }

            uint[] zdFromPool = null;
            int zl = Math.Max(xl - digitShift, 0);
            Span<uint> zd = ((uint)zl <= BigIntegerDecimalCalculator.StackAllocThreshold
                          ? stackalloc uint[BigIntegerDecimalCalculator.StackAllocThreshold]
                          : zdFromPool = ArrayPool<uint>.Shared.Rent(zl)).Slice(0, zl);
            zd.Clear();

            if (smallShift == 0)
            {
                for (int i = xd.Length - 1; i >= digitShift; i--)
                {
                    zd[i - digitShift] = xd[i];
                }
            }
            else
            {
                int carryShift = kcbitUint - smallShift;
                uint carry = 0;
                for (int i = xd.Length - 1; i >= digitShift; i--)
                {
                    uint rot = xd[i];
                    zd[i - digitShift] = (rot >>> smallShift) | carry;
                    carry = rot << carryShift;
                }
            }

            if (negx && (int)zd[^1] < 0)
            {
                NumericsHelpers.DangerousMakeTwosComplement(zd);
            }
            else
            {
                negx = false;
            }

            result = new BigIntegerDecimal(zd, negx);

            if (zdFromPool != null)
                ArrayPool<uint>.Shared.Return(zdFromPool);
            exit:
            if (xdFromPool != null)
                ArrayPool<uint>.Shared.Return(xdFromPool);

            return result;
        }

    }
#endif

#region Array
#if false



















    internal static partial class BigIntegerDecimalCalculator
    {
        public const int StackAllocThreshold = 64;
        const int BASE = BigIntegerDecimal.BASE;
        const int LOG_B = BigIntegerDecimal.LOG_B;

        public static int Compare(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right)
        {
            if (left.Length < right.Length)
                return -1;
            if (left.Length > right.Length)
                return 1;

            for (int i = left.Length - 1; i >= 0; i--)
            {
                uint leftElement = left[i];
                uint rightElement = right[i];
                if (leftElement < rightElement)
                    return -1;
                if (leftElement > rightElement)
                    return 1;
            }

            return 0;
        }

    }
    internal static partial class BigIntegerDecimalCalculator
    {
        public static void Divide(ReadOnlySpan<uint> left, uint right, Span<uint> quotient, out uint remainder)
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
                quotient[i] = (uint)digit;
                carry = value - digit * right;
            }
            remainder = (uint)carry;
        }

        public static void Divide(ReadOnlySpan<uint> left, uint right, Span<uint> quotient)
            => Divide(left, right, quotient, out _);

        public static uint Remainder(ReadOnlySpan<uint> left, uint right)
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

        public static void Divide(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> quotient, Span<uint> remainder)
        {
            Debug.Assert(left.Length >= 1);
            Debug.Assert(right.Length >= 1);
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(quotient.Length == left.Length - right.Length + 1);
            Debug.Assert(remainder.Length == left.Length);

            left.CopyTo(remainder);
            Divide(remainder, right, quotient);
        }

        public static void Divide(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> quotient)
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
                                  : leftCopyFromPool = ArrayPool<uint>.Shared.Rent(left.Length))[..left.Length];
            left.CopyTo(leftCopy);

            Divide(leftCopy, right, quotient);

            if (leftCopyFromPool != null)
                ArrayPool<uint>.Shared.Return(leftCopyFromPool);
        }

        public static void Remainder(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> remainder)
        {
            Debug.Assert(left.Length >= 1);
            Debug.Assert(right.Length >= 1);
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(remainder.Length >= left.Length);

            // Same as above, but only returning the remainder.

            left.CopyTo(remainder);
            Divide(remainder, right, default);
        }

        private static void Divide(Span<uint> left, ReadOnlySpan<uint> right, Span<uint> bits)
        {
            Debug.Assert(left.Length >= 1);
            Debug.Assert(right.Length >= 1);
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(bits.Length == left.Length - right.Length + 1
                || bits.Length == 0);

            // Executes the "grammar-school" algorithm for computing q = a / b.
            // Before calculating q_i, we get more bits into the highest bit
            // block of the divisor. Thus, guessing digits of the quotient
            // will be more precise. Additionally we'll get r = a % b.

            uint divHi = right[^1];
            uint divLo = right.Length > 1 ? right[^2] : 0;


            // Then, we divide all of the bits as we would do it using
            // pen and paper: guessing the next digit, subtracting, ...
            for (int i = left.Length; i >= right.Length; i--)
            {
                int n = i - right.Length;
                uint t = (uint)i < (uint)left.Length ? left[i] : 0;

                ulong valHi = ((ulong)t * BASE) + left[i - 1];
                uint valLo = i > 1 ? left[i - 2] : 0;

                // First guess for the current digit of the quotient,
                // which naturally must have only 32 bits...
                ulong digit = valHi / divHi;
                if (digit >= BASE)
                    digit = BASE - 1;

                // Our first guess may be a little bit to big
                while (DivideGuessTooBig(digit, valHi, valLo, divHi, divLo))
                    --digit;

                if (digit > 0)
                {
                    // Now it's time to subtract our current quotient
                    uint carry = SubtractDivisor(left.Slice(n), right, digit);
                    if (carry != t)
                    {
                        Debug.Assert(carry == t + 1);

                        // Our guess was still exactly one too high
                        carry = AddDivisor(left.Slice(n), right);
                        --digit;

                        Debug.Assert(carry == 1);
                    }
                }

                // We have the digit!
                if ((uint)n < (uint)bits.Length)
                    bits[n] = (uint)digit;

                if ((uint)i < (uint)left.Length)
                    left[i] = 0;
            }
        }

        private static uint AddDivisor(Span<uint> left, ReadOnlySpan<uint> right)
        {
            Debug.Assert(left.Length >= right.Length);

            // Repairs the dividend, if the last subtract was too much

            ulong carry = 0UL;

            for (int i = 0; i < right.Length; i++)
            {
                ref uint leftElement = ref left[i];
                ulong digit = (leftElement + carry) + right[i];
                leftElement = (uint)(digit % BASE);
                carry = digit / BASE;
            }

            return (uint)carry;
        }

        private static uint SubtractDivisor(Span<uint> left, ReadOnlySpan<uint> right, ulong q)
        {
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(q < BASE);

            // Combines a subtract and a multiply operation, which is naturally
            // more efficient than multiplying and then subtracting...

            ulong carry = 0UL;

            for (int i = 0; i < right.Length; i++)
            {
                carry += right[i] * q;
                uint digit = (uint)(carry % BASE);
                carry /= BASE;
                ref uint leftElement = ref left[i];
                if (leftElement < digit)
                {
                    ++carry;
                    leftElement = (digit - leftElement) % BASE;
                    if (leftElement != 0) leftElement = BASE - leftElement;
                }
                else
                    leftElement = (leftElement - digit) % BASE;
            }

            return (uint)carry;
        }

        private static bool DivideGuessTooBig(ulong q, ulong valHi, uint valLo,
                                              uint divHi, uint divLo)
        {
            Debug.Assert(q < BASE);

            // We multiply the two most significant limbs of the divisor
            // with the current guess for the quotient. If those are bigger
            // than the three most significant limbs of the current dividend
            // we return true, which means the current guess is still too big.

            ulong chkHi = divHi * q;
            ulong chkLo = divLo * q;

            chkHi += (chkLo / BASE);
            chkLo %= BASE;

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
    internal static partial class BigIntegerDecimalCalculator
    {
        private const int CopyToThreshold = 8;

        private static void CopyTail(ReadOnlySpan<uint> source, Span<uint> dest, int start)
        {
            source.Slice(start).CopyTo(dest.Slice(start));
        }

        public static void Add(ReadOnlySpan<uint> left, uint right, Span<uint> bits)
        {
            Debug.Assert(left.Length >= 1);
            Debug.Assert(bits.Length == left.Length + 1);

            Add(left, bits, ref MemoryMarshal.GetReference(bits), startIndex: 0, initialCarry: right);
        }

        public static void Add(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> bits)
        {
            Debug.Assert(right.Length >= 1);
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(bits.Length == left.Length + 1);

            // Switching to managed references helps eliminating
            // index bounds check for all buffers.
            ref uint resultPtr = ref MemoryMarshal.GetReference(bits);
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
                carry += Unsafe.Add(ref rightPtr, i);
                Unsafe.Add(ref resultPtr, i) = (uint)(carry % BASE);
                carry /= BASE;
                i++;
            } while (i < right.Length);

            Add(left, bits, ref resultPtr, startIndex: i, initialCarry: carry);
        }

        private static void AddSelf(Span<uint> left, ReadOnlySpan<uint> right)
        {
            Debug.Assert(left.Length >= right.Length);

            int i = 0;
            long carry = 0L;

            // Switching to managed references helps eliminating
            // index bounds check...
            ref uint leftPtr = ref MemoryMarshal.GetReference(left);

            // Executes the "grammar-school" algorithm for computing z = a + b.
            // Same as above, but we're writing the result directly to a and
            // stop execution, if we're out of b and c is already 0.

            for (; i < right.Length; i++)
            {
                long digit = (Unsafe.Add(ref leftPtr, i) + carry) + right[i];
                Unsafe.Add(ref leftPtr, i) = (uint)(digit % BASE);
                carry = digit / BASE;
            }
            for (; carry != 0 && i < left.Length; i++)
            {
                long digit = left[i] + carry;
                left[i] = (uint)(digit % BASE);
                carry = digit / BASE;
            }

            Debug.Assert(carry == 0);
        }

        public static void Subtract(ReadOnlySpan<uint> left, uint right, Span<uint> bits)
        {
            Debug.Assert(left.Length >= 1);
            Debug.Assert(left[0] >= right || left.Length >= 2);
            Debug.Assert(bits.Length == left.Length);

            Subtract(left, bits, ref MemoryMarshal.GetReference(bits), startIndex: 0, initialCarry: -right);
        }

        public static void Subtract(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> bits)
        {
            Debug.Assert(right.Length >= 1);
            Debug.Assert(left.Length >= right.Length);
            Debug.Assert(Compare(left, right) >= 0);
            Debug.Assert(bits.Length == left.Length);

            // Switching to managed references helps eliminating
            // index bounds check for all buffers.
            ref uint resultPtr = ref MemoryMarshal.GetReference(bits);
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
                Unsafe.Add(ref resultPtr, i) = (uint)(carry % BASE);
                carry /= BASE;
                i++;
            } while (i < right.Length);

            Subtract(left, bits, ref resultPtr, startIndex: i, initialCarry: carry);
        }

        private static void SubtractSelf(Span<uint> left, ReadOnlySpan<uint> right)
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
                Unsafe.Add(ref leftPtr, i) = (uint)(digit % BASE);
                carry = digit / BASE;
            }
            for (; carry != 0 && i < left.Length; i++)
            {
                long digit = left[i] + carry;
                left[i] = (uint)(digit % BASE);
                carry = digit / BASE;
            }

            Debug.Assert(carry == 0);
        }

        [凾(256)]
        private static void Add(ReadOnlySpan<uint> left, Span<uint> bits, ref uint resultPtr, int startIndex, long initialCarry)
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
                    Unsafe.Add(ref resultPtr, i) = (uint)(carry % BASE);
                    carry /= BASE;
                }

                Unsafe.Add(ref resultPtr, left.Length) = (uint)(carry % BASE);
            }
            else
            {
                for (; i < left.Length;)
                {
                    carry += left[i];
                    Unsafe.Add(ref resultPtr, i) = (uint)(carry % BASE);
                    i++;
                    carry /= BASE;

                    // Once carry is set to 0 it can not be 1 anymore.
                    // So the tail of the loop is just the movement of argument values to result span.
                    if (carry == 0)
                    {
                        break;
                    }
                }

                Unsafe.Add(ref resultPtr, left.Length) = (uint)(carry % BASE);

                if (i < left.Length)
                {
                    CopyTail(left, bits, i);
                }
            }
        }

        [凾(256)]
        private static void Subtract(ReadOnlySpan<uint> left, Span<uint> bits, ref uint resultPtr, int startIndex, long initialCarry)
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
                    Unsafe.Add(ref resultPtr, i) = (uint)(carry % BASE);
                    carry /= BASE;
                }
            }
            else
            {
                for (; i < left.Length;)
                {
                    carry += left[i];
                    Unsafe.Add(ref resultPtr, i) = (uint)(carry % BASE);
                    i++;
                    carry /= BASE;

                    // Once carry is set to 0 it can not be 1 anymore.
                    // So the tail of the loop is just the movement of argument values to result span.
                    if (carry == 0)
                    {
                        break;
                    }
                }

                if (i < left.Length)
                {
                    CopyTail(left, bits, i);
                }
            }
        }
    }

    internal static partial class BigIntegerDecimalCalculator
    {
        const int SquareThreshold = 32;

        public static void Square(ReadOnlySpan<uint> value, Span<uint> bits)
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
                        ulong digit2 = (ulong)value[j] * v;
                        Unsafe.Add(ref resultPtr, i + j) = (uint)((digit1 + (digit2 << 1)) % BASE);
                        carry = (digit2 + (digit1 >> 1)) >> 31;
                    }
                    ulong digits = (ulong)v * v + carry;
                    Unsafe.Add(ref resultPtr, i + i) = (uint)(digits % BASE);
                    Unsafe.Add(ref resultPtr, i + i + 1) = (uint)(digits / BASE);
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
                ReadOnlySpan<uint> valueLow = value.Slice(0, n);
                ReadOnlySpan<uint> valueHigh = value.Slice(n);

                // ... prepare our result array (to reuse its memory)
                Span<uint> bitsLow = bits.Slice(0, n2);
                Span<uint> bitsHigh = bits.Slice(n2);

                // ... compute z_0 = a_0 * a_0 (squaring again!)
                Square(valueLow, bitsLow);

                // ... compute z_2 = a_1 * a_1 (squaring again!)
                Square(valueHigh, bitsHigh);

                int foldLength = valueHigh.Length + 1;
                uint[] foldFromPool = null;
                Span<uint> fold = ((uint)foldLength <= StackAllocThreshold ?
                                  stackalloc uint[StackAllocThreshold]
                                  : foldFromPool = ArrayPool<uint>.Shared.Rent(foldLength)).Slice(0, foldLength);
                fold.Clear();

                int coreLength = foldLength + foldLength;
                uint[] coreFromPool = null;
                Span<uint> core = ((uint)coreLength <= StackAllocThreshold ?
                                  stackalloc uint[StackAllocThreshold]
                                  : coreFromPool = ArrayPool<uint>.Shared.Rent(coreLength)).Slice(0, coreLength);
                core.Clear();

                // ... compute z_a = a_1 + a_0 (call it fold...)
                Add(valueHigh, valueLow, fold);

                // ... compute z_1 = z_a * z_a - z_0 - z_2
                Square(fold, core);

                if (foldFromPool != null)
                    ArrayPool<uint>.Shared.Return(foldFromPool);

                SubtractCore(bitsHigh, bitsLow, core);

                // ... and finally merge the result! :-)
                AddSelf(bits.Slice(n), core);

                if (coreFromPool != null)
                    ArrayPool<uint>.Shared.Return(coreFromPool);
            }
        }

        public static void Multiply(ReadOnlySpan<uint> left, uint right, Span<uint> bits)
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
                bits[i] = (uint)(digits % BASE);
                carry = digits / BASE;
            }
            bits[i] = (uint)(carry % BASE);
        }

        const int MultiplyThreshold = 32;

        public static void Multiply(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right, Span<uint> bits)
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
                        elementPtr = (uint)(digits % BASE);
                        carry = digits / BASE;
                    }
                    Unsafe.Add(ref resultPtr, i + left.Length) = (uint)(carry % BASE);
                }
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
                Multiply(leftLow, rightLow, bitsLow);

                // ... compute z_2 = a_1 * b_1 (multiply again)
                Multiply(leftHigh, rightHigh, bitsHigh);

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
                Multiply(leftFold, rightFold, core);

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
                Unsafe.Add(ref corePtr, i) = (uint)(digit % BASE);
                carry = digit / BASE;
            }

            for (; i < left.Length; i++)
            {
                long digit = (Unsafe.Add(ref corePtr, i) + carry) - left[i];
                Unsafe.Add(ref corePtr, i) = (uint)(digit % BASE);
                carry = digit / BASE;
            }

            for (; carry != 0 && i < core.Length; i++)
            {
                long digit = core[i] + carry;
                core[i] = (uint)(digit % BASE);
                carry = digit / BASE;
            }
        }
    }

}
#endif
#endregion