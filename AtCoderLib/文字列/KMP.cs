using System;
using System.Collections.Generic;


ref struct KMP // https://algoful.com/Archive/Algorithm/KMPSearch
{
    ReadOnlySpan<char> pattern;
    int[] table;
    public KMP(ReadOnlySpan<char> pattern) { this.pattern = pattern; table = CreateTable(pattern); }
    static int[] CreateTable(ReadOnlySpan<char> pattern)
    {
        var table = new int[pattern.Length + 1];
        int j = 0;
        for (int i = 1; i < table.Length; i++)
        {
            if (i < pattern.Length && pattern[i] == pattern[j])
                table[i] = j++;
            else
            {
                table[i] = j;
                j = 0;
            }
        }
        return table;
    }

    public IEnumerable<int> Matches(ReadOnlySpan<char> target)
    {
        int i = 0, p = 0;
        while (i < target.Length && p < pattern.Length)
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
