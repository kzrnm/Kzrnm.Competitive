using System;
using System.Collections.Generic;
#region https://algoful.com/Archive/Algorithm/BMSearch
#endregion


namespace AtCoder.DataStructure.String
{
    public class BoyerMoore
    {
        readonly string pattern;
        readonly Dictionary<char, int> table;
        public BoyerMoore(string pattern) { this.pattern = pattern; table = CreateTable(pattern); }
        static Dictionary<char, int> CreateTable(string pattern)
        {
            var table = new Dictionary<char, int>();
            for (int i = 0; i < pattern.Length; i++)
            {
                table[pattern[i]] = pattern.Length - i - 1;
            }
            return table;
        }

        /**
         * <summary>
         * 一致する箇所を返す。複数返そうとすると遅いのでBM法では1つだけを対象にする
         * </summary>
         * <param name="target"></param>
         */
        public int Match(string target)
        {
            var i = pattern.Length - 1;
            while (i < target.Length)
            {
                var p = pattern.Length - 1;

                while (p >= 0 && i < target.Length)
                {
                    if (target[i] == pattern[p])
                    {
                        i--; p--;
                    }
                    else
                    {
                        break;
                    }
                }
                if (p < 0) return i + 1;

                /* 不一致の場合、ずらし表を参照し i を進める
                 * ただし、今比較した位置より後の位置とする
                 */
                var shift1 = table.ContainsKey(pattern[p]) ? table[pattern[p]] : pattern.Length;
                var shift2 = pattern.Length - p;    /* 比較を開始した地点の1つ後ろの文字 */
                i += Math.Max(shift1, shift2);
            }

            return -1;
        }
    }
}