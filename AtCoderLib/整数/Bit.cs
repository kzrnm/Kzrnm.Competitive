using System;
using System.Collections.Generic;


static class Bit
{
    public static string ToBitString(this int num, int padLeft = sizeof(int) * 8) => Convert.ToString(num, 2).PadLeft(padLeft, '0');
    public static string ToBitString(this long num, int padLeft = sizeof(long) * 8) => Convert.ToString(num, 2).PadLeft(padLeft, '0');
    public static string ToBitString(this ulong num, int padLeft = sizeof(ulong) * 8) => Convert.ToString(unchecked((long)num), 2).PadLeft(padLeft, '0');
    public static bool On(this int num, int index) => ((num >> index) & 1) != 0;
    public static bool On(this long num, int index) => ((num >> index) & 1) != 0;
    public static bool On(this ulong num, int index) => ((num >> index) & 1) != 0;
    public static IEnumerable<int> Bits(this int num)
    {
        for (var i = 0; num > 0; i++, num >>= 1)
            if ((num & 1) == 1)
                yield return i;
    }
    public static IEnumerable<int> Bits(this long num)
    {
        for (var i = 0; num > 0; i++, num >>= 1)
            if ((num & 1) == 1)
                yield return i;
    }
    public static IEnumerable<int> Bits(this ulong num)
    {
        for (var i = 0; num > 0; i++, num >>= 1)
            if ((num & 1) == 1)
                yield return i;
    }
}