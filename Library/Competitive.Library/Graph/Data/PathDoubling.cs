using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// ダブリング: mod Nで循環しているなどのときに 前計算 O(logK) で K 個先の遷移を O(logK) で求められるデータ構造
    /// </summary>
    public class PathDoubling
    {
        /// <summary>
        /// <para><paramref name="to"/>: 頂点 i からの遷移先。負数は遷移先なしとする。</para>
        /// <para><paramref name="max"/>: 最大何番目までの遷移を見たいか</para>
        /// <para>制約: <paramref name="to"/>[i] &lt; |<paramref name="to"/>|, 0 ≦ <paramref name="max"/></para>
        /// </summary>
        /// <param name="to">数値 i からの遷移先</param>
        /// <param name="max">最大何番目までの遷移を見たいか</param>
        public PathDoubling(int[] to, long max) : this(to, (ulong)max) { }
        /// <summary>
        /// <para><paramref name="to"/>: 頂点 i からの遷移先。負数は遷移先なしとする。</para>
        /// <para><paramref name="max"/>: 最大何番目までの遷移を見たいか</para>
        /// <para>制約: <paramref name="to"/>[i] &lt; |<paramref name="to"/>|</para>
        /// </summary>
        /// <param name="to">数値 i からの遷移先</param>
        /// <param name="max">最大何番目までの遷移を見たいか</param>
        public PathDoubling(int[] to, ulong max = ulong.MaxValue)
        {
            paths = new int[BitOperations.Log2(max) + 1][];
            paths[0] = to;
            for (int k = 0; k + 1 < paths.Length; k++)
            {
                var s = paths[k];
                var t = paths[k + 1] = new int[s.Length];
                for (int i = 0; i < s.Length; i++)
                    if ((uint)s[i] < (uint)s.Length)
                    {
                        t[i] = s[s[i]];
                    }
                    else
                    {
                        t[i] = -1;
                    }
            }
        }
        internal readonly int[][] paths;

        /// <summary>
        /// log(最大何番目まで見れるか) を返します。
        /// </summary>
        public int PathSize => paths.Length;

        /// <summary>
        /// <para><paramref name="start"/>: スタートするインデックス</para>
        /// <para><paramref name="moveNum"/>: 移動回数</para>
        /// </summary>
        /// <param name="start">: スタートするインデックス</param>
        /// <param name="moveNum">: 移動回数</param>
        /// <returns></returns>
        [凾(256)]
        public int Move(int start, long moveNum) => Move(start, (ulong)moveNum);
        /// <summary>
        /// <para><paramref name="start"/>: スタートするインデックス</para>
        /// <para><paramref name="moveNum"/>: 移動回数</para>
        /// </summary>
        /// <param name="start">: スタートするインデックス</param>
        /// <param name="moveNum">: 移動回数</param>
        /// <returns></returns>
        [凾(256)]
        public int Move(int start, ulong moveNum)
        {
            int current = start;
            foreach (var b in moveNum.Bits())
            {
                if ((uint)current >= (uint)paths[b].Length)
                    return current;
                current = paths[b][current];
            }
            return current;
        }
    }
}
