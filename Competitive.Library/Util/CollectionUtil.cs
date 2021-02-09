using System;

namespace Kzrnm.Competitive
{
    public static class CollectionUtil
    {
        public static T[] Flatten<T>(T[][] array) => Flatten((ReadOnlySpan<T[]>)array);
        public static T[] Flatten<T>(Span<T[]> span) => Flatten((ReadOnlySpan<T[]>)span);
        public static T[] Flatten<T>(ReadOnlySpan<T[]> span)
        {
            var res = new T[span.Length * span[0].Length];
            for (int i = 0; i < span.Length; i++)
                for (int j = 0; j < span[i].Length; j++)
                    res[i * span[i].Length + j] = span[i][j];
            return res;
        }
        public static char[] Flatten(string[] strs)
        {
            var res = new char[strs.Length * strs[0].Length];
            for (int i = 0; i < strs.Length; i++)
                for (int j = 0; j < strs[i].Length; j++)
                    res[i * strs[i].Length + j] = strs[i][j];
            return res;
        }
    }
}
