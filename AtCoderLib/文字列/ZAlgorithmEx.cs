
using System;

/* https://bitbucket.org/camypaper/complib/src/master/lib/Strings/ZAlgorithm.cs */

class ZAlgorithmEx
{
    /** <summary> <paramref name="s"/> と <paramref name="s"/>[i:] の最長共通接頭辞を O(|<paramref name="s"/>|) で求める。</summary> */
    int[] ZAlgorithm(ReadOnlySpan<char> s)
    {
        var a = new int[s.Length + 1];
        a[0] = s.Length;
        int i = 1, j = 0;
        while (i < s.Length)
        {
            while (i + j < s.Length && s[j] == s[i + j]) ++j;
            a[i] = j;
            if (j == 0) { ++i; continue; }
            int k = 1;
            while (i + k < s.Length && k + a[k] < j) { a[i + k] = a[k]; ++k; }
            i += k; j -= k;
        }
        return a;
    }

}
