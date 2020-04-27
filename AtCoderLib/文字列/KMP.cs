using System;
using System.Collections.Generic;


class KMP // https://algoful.com/Archive/Algorithm/KMPSearch
{
    string pattern;
    int[] table;
    public KMP(string pattern) { this.pattern = pattern; table = CreateTable(pattern); }
    static int[] CreateTable(string pattern)
    {
        var table = new int[pattern.Length + 1];
        table[0] = -1;
        int j = -1;
        for (int i = 0; i < pattern.Length; i++)
        {
            while (j >= 0 && pattern[i] != pattern[j]) j = table[j];
            table[i + 1] = ++j;
        }
        return table;
    }

    public IEnumerable<int> Matches(string target)
    {
        for (int i = 0, p = 0; i < target.Length;)
        {
            if (target[i] == pattern[p])
            {
                // 文字が一致していれば次の文字に進む
                i++; p++;
            }
            else if (p == 0)
            {
                // パターン先頭文字が不一致の場合、次の文字
                i++;
            }
            else
            {
                // 不一致の場合、パターンのどの位置から再開するか設定
                p = table[p];
            }

            if (p == pattern.Length)
            {
                yield return i - p;
                p = table[p];
            }
        }
    }
}
