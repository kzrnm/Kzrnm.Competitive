using System;
using System.Numerics;


static class Bit
{
    public static string ToBitString(this int num, int padLeft = sizeof(int) * 8) => Convert.ToString(num, 2).PadLeft(padLeft, '0');
    public static string ToBitString(this long num, int padLeft = sizeof(long) * 8) => Convert.ToString(num, 2).PadLeft(padLeft, '0');
    public static string ToBitString(this ulong num, int padLeft = sizeof(ulong) * 8) => Convert.ToString(unchecked((long)num), 2).PadLeft(padLeft, '0');
    public static bool On(this int num, int index) => ((num >> index) & 1) != 0;
    public static bool On(this long num, int index) => ((num >> index) & 1) != 0;
    public static bool On(this ulong num, int index) => ((num >> index) & 1) != 0;
    public static Enumerator Bits(this int num) => new Enumerator(num);
    public static Enumerator Bits(this uint num) => new Enumerator(num);
    public static Enumerator Bits(this long num) => new Enumerator(num);
    public static Enumerator Bits(this ulong num) => new Enumerator(num);
    public struct Enumerator
    {
        private ulong num;
        public Enumerator(int num) : this((ulong)(uint)num) { }
        public Enumerator(long num) : this((ulong)num) { }
        public Enumerator(ulong num) { this.num = num; Current = -1; }
        public Enumerator GetEnumerator() => this;
        public int Current { get; private set; }
        public bool MoveNext()
        {
            if (num == 0) return false;
            var lsb1 = BitOperations.TrailingZeroCount(num) + 1;
            Current += lsb1;
            num >>= lsb1;
            return true;
        }
    }
}
