// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public readonly partial struct BigIntegerDecimal
    {
        const int CopyToThreshold = 8;
        const int StackAllocThreshold = 64;

        static int Compare(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right)
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

        static int ActualLength(ReadOnlySpan<uint> value)
        {
            // Since we're reusing memory here, the actual length
            // of a given value may be less then the array's length

            int length = value.Length;

            while (length > 0 && value[length - 1] == 0)
                --length;
            return length;
        }
        [凾(256)]
        static ulong DivRemBase(ulong v, out uint remainder)
        {
            var q = v / BASE;
            remainder = (uint)(v - q * BASE);
            return q;
        }
        [凾(256)]
        static uint DivRemBase(uint v, out uint remainder)
        {
            var q = v / BASE;
            remainder = v - q * BASE;
            return q;
        }
        [凾(256)]
        static long DivRemBase(long v, out uint remainder)
        {
            var q = v / BASE;
            var rem = v - q * BASE;
            if (rem < 0)
            {
                rem += BASE;
                --q;
            }
            remainder = (uint)rem;
            return q;
        }
    }
}