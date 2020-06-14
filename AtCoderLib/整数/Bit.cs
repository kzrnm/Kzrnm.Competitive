using System;
using System.Collections.Generic;
using static AtCoderProject.Global;

static class Bit
{
    public static string ToBitString(this int num, int padLeft = sizeof(int) * 8) => Convert.ToString(num, 2).PadLeft(padLeft, '0');
    public static string ToBitString(this long num, int padLeft = sizeof(long) * 8) => Convert.ToString(num, 2).PadLeft(padLeft, '0');
    public static string ToBitString(this ulong num, int padLeft = sizeof(ulong) * 8) => Convert.ToString(unchecked((long)num), 2).PadLeft(padLeft, '0');
    public static bool On(this int num, int index) => ((num >> index) & 1) != 0;
    public static bool On(this long num, int index) => ((num >> index) & 1) != 0;
    public static bool On(this ulong num, int index) => ((num >> index) & 1) != 0;
    private static IEnumerable<int> BitsClasic(this int num)
    {
        for (var i = 0; num > 0; i++, num >>= 1)
            if ((num & 1) == 1)
                yield return i;
    }
    public static EnumerableBits32 Bits(this int num) => new EnumerableBits32(num);
    public static EnumerableBits32 Bits(this uint num) => new EnumerableBits32(num);
    public static EnumerableBits64 Bits(this long num) => new EnumerableBits64(num);
    public static EnumerableBits64 Bits(this ulong num) => new EnumerableBits64(num);
    public struct EnumerableBits64
    {
        private ulong num;
        public EnumerableBits64(long num) : this((ulong)num) { }
        public EnumerableBits64(ulong num) { this.num = num; this.Current = 0; }
        public EnumerableBits64 GetEnumerator() => this;
        public int Current { get; private set; }
        public bool MoveNext()
        {
            if (num == 0) return false;
            num &= ~1UL << (Current = LSB(num));
            return true;
        }
    }
    public struct EnumerableBits32
    {
        private uint num;
        public EnumerableBits32(int num) : this((uint)num) { }
        public EnumerableBits32(uint num) { this.num = num; this.Current = 0; }
        public EnumerableBits32 GetEnumerator() => this;
        public int Current { get; private set; }
        public bool MoveNext()
        {
            if (num == 0) return false;
            num &= ~1U << (Current = LSB(num));
            return true;
        }
    }
}