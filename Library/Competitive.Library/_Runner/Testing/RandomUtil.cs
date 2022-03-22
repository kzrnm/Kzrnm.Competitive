using System;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing
{
    public static class RandomUtil
    {
        public static string NextString(this Random rnd, int length)
        {
            var arr = new char[length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = (char)(rnd.Next(26) + 'a');
            return new string(arr);
        }
        public static int[] NextIntArray(this Random rnd, int length)
        {
            var arr = new int[length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = rnd.Next();
            return arr;
        }
        public static int[] NextIntArray(this Random rnd, int length, int minValue, int maxValue)
        {
            var arr = new int[length];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = rnd.Next(minValue, maxValue);
            return arr;
        }
        public static uint NextUInt(this Random rnd)
        {
            Span<byte> bytes = stackalloc byte[4];
            rnd.NextBytes(bytes);
            return MemoryMarshal.AsRef<uint>(bytes);
        }
    }
}
