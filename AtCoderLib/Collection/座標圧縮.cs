using System;
using System.Collections.Generic;
using System.Linq;

static class 座標圧縮
{
    public static Dictionary<T, int> Compress<T>(IEnumerable<T> orig) where T : IComparable<T>
    {
        var ox = new HashSet<T>(orig).ToArray();
        Array.Sort(ox);
        var zip = new Dictionary<T, int>();
        for (int i = 0; i < ox.Length; i++)
            zip[ox[i]] = i;
        return zip;
    }
    public static int[] Compressed<T>(T[] orig) where T : IComparable<T>
    {
        static int[] Compressed(T[] orig, Dictionary<T, int> zip)
        {
            var res = new int[orig.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = zip[orig[i]];
            return res;
        }
        return Compressed(orig, Compress(orig));
    }
}