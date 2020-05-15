using AtCoderProject;
using System.Collections.Generic;
using System.Linq;

static class 座標圧縮
{
    static Dictionary<int, int> Compress(int[] orig)
    {
        var ox = new HashSet<int>(orig).ToArray().Sort();
        var zip = new Dictionary<int, int>();
        for (int i = 0; i < ox.Length; i++)
            zip[ox[i]] = i;
        return zip;
    }
    static int[] Compressed(int[] orig)
    {
        var res = new int[orig.Length];
        var zip = Compress(orig);
        for (int i = 0; i < res.Length; i++)
            res[i] = zip[orig[i]];
        return res;
    }
}