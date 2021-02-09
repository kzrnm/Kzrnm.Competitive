using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;
    public static class Bit
    {
        public static string ToBitString(this int num, int padLeft = sizeof(int) * 8) => Convert.ToString(num, 2).PadLeft(padLeft, '0');
        public static string ToBitString(this long num, int padLeft = sizeof(long) * 8) => Convert.ToString(num, 2).PadLeft(padLeft, '0');
        public static string ToBitString(this ulong num, int padLeft = sizeof(ulong) * 8) => Convert.ToString(unchecked((long)num), 2).PadLeft(padLeft, '0');
        [MethodImpl(AggressiveInlining)]
        public static bool On(this int num, int index) => ((num >> index) & 1) != 0;
        [MethodImpl(AggressiveInlining)]
        public static bool On(this long num, int index) => ((num >> index) & 1) != 0;
        [MethodImpl(AggressiveInlining)]
        public static bool On(this ulong num, int index) => ((num >> index) & 1) != 0;
        public static Enumerator Bits(this int num) => new Enumerator(num);
        public static Enumerator Bits(this uint num) => new Enumerator(num);
        public static Enumerator Bits(this long num) => new Enumerator(num);
        public static Enumerator Bits(this ulong num) => new Enumerator(num);
        public struct Enumerator : IEnumerable<int>, IEnumerator<int>
        {
            ulong num;
            public Enumerator(int num) : this((ulong)(uint)num) { }
            public Enumerator(long num) : this((ulong)num) { }
            public Enumerator(ulong num) { this.num = num; Current = -1; }
            public Enumerator GetEnumerator() => this;
            public int Current { get; private set; }
            [MethodImpl(AggressiveInlining)]
            public bool MoveNext()
            {
                if (num == 0) return false;
                if (Bmi1.X64.IsSupported)
                {
                    Current = unchecked((int)Bmi1.X64.TrailingZeroCount(num));
                    num = Bmi1.X64.ResetLowestSetBit(num);
                }
                else MoveNextLogical();
                return true;
            }
            void MoveNextLogical()
            {
                Current = BitOperations.TrailingZeroCount(num);
                num &= num - 1;
            }
            object IEnumerator.Current => Current;
            public int[] ToArray()
            {
                var res = new int[BitOperations.PopCount(num)];
                for (int i = 0; i < res.Length; i++)
                {
                    MoveNext();
                    res[i] = Current;
                }
                return res;
            }
            void IEnumerator.Reset() => throw new NotSupportedException();
            void IDisposable.Dispose() { }
            IEnumerator<int> IEnumerable<int>.GetEnumerator() => this;
            IEnumerator IEnumerable.GetEnumerator() => this;
        }
    }
}