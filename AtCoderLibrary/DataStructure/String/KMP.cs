using System;
using System.Collections.Generic;
#region https://algoful.com/Archive/Algorithm/KMPSearch
#endregion


namespace AtCoder.DataStructure.String
{
    public class KMP
    {
        readonly string pattern;
        readonly int[] table;
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
                { i++; p++; }
                else if (p == 0)
                    i++;
                else
                    p = table[p];

                if (p == pattern.Length)
                {
                    yield return i - p;
                    p = table[p];
                }
            }
        }
    }
}