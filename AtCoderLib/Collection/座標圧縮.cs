using AtCoderProject;
using System;
using System.Collections.Generic;
using System.Linq;

static class 座標圧縮
{
    static Dictionary<T, int> Compress<T>(IEnumerable<T> orig)
    {
        var ox = new HashSet<T>(orig).ToArray();
        if (typeof(IComparable<T>).IsAssignableFrom(typeof(T)))
            ox.Sort();
        var zip = new Dictionary<T, int>();
        for (int i = 0; i < ox.Length; i++)
            zip[ox[i]] = i;
        return zip;
    }
    static int[] Compressed<T>(T[] orig) => Compressed(orig, Compress(orig));
    static int[] Compressed<T>(T[] orig, Dictionary<T, int> zip)
    {
        var res = new int[orig.Length];
        for (int i = 0; i < res.Length; i++)
            res[i] = zip[orig[i]];
        return res;
    }
}