using AtCoder;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// ダブリング: mod Nで循環しているなどのときに 前計算 O(logK) で K 個先の遷移を O(logK) で求められるデータ構造
    /// </summary>
    public class PathDoubling<T, TOp> where TOp : ISegtreeOperator<T>
    {
        /// <summary>
        /// <para><paramref name="to"/>: 頂点 i からの遷移先。負数は遷移先なしとする。</para>
        /// <para><paramref name="max"/>: 最大何番目までの遷移を見たいか</para>
        /// <para>制約: <paramref name="to"/>[i] &lt; |<paramref name="to"/>|, 0 ≦ <paramref name="max"/></para>
        /// </summary>
        /// <param name="to">数値 i からの遷移先</param>
        /// <param name="data">数値 i でのデータ</param>
        /// <param name="max">最大何番目までの遷移を見たいか</param>
        /// <param name="op">現在地 x と 遷移先の y の値を <c>Operate(x, y)</c> でマージする関数定義</param>
        public PathDoubling(int[] to, T[] data, long max, TOp op = default) : this(to, data, (ulong)max, op) { }
        /// <summary>
        /// <para><paramref name="to"/>: 頂点 i からの遷移先。負数は遷移先なしとする。</para>
        /// <para><paramref name="max"/>: 最大何番目までの遷移を見たいか</para>
        /// <para>制約: <paramref name="to"/>[i] &lt; |<paramref name="to"/>|</para>
        /// </summary>
        /// <param name="to">数値 i からの遷移先</param>
        /// <param name="data">数値 i でのデータ</param>
        /// <param name="max">最大何番目までの遷移を見たいか</param>
        /// <param name="op">現在地 x と 遷移先の y の値を <c>Operate(x, y)</c> でマージする関数定義</param>
        public PathDoubling(int[] to, T[] data, ulong max = ulong.MaxValue, TOp op = default)
        {
            this.op = op;
            paths = new int[BitOperations.Log2(max) + 1][];
            paths[0] = to;

            this.data = new T[paths.Length][];
            this.data[0] = data;

            for (int k = 0; k + 1 < paths.Length; k++)
            {
                var s = paths[k];
                var t = paths[k + 1] = new int[s.Length];
                var ds = this.data[k];
                var dt = this.data[k + 1] = new T[ds.Length];
                for (int i = 0; i < s.Length; i++)
                    if ((uint)s[i] < (uint)s.Length)
                    {
                        t[i] = s[s[i]];
                        dt[i] = op.Operate(ds[i], ds[s[i]]);
                    }
                    else
                    {
                        t[i] = -1;
                        dt[i] = op.Identity;
                    }
            }
        }
        internal readonly TOp op;
        public readonly int[][] paths;
        public readonly T[][] data;

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
        [凾(256)]
        public (int To, T Data) Move(int start, long moveNum) => Move(start, (ulong)moveNum);
        /// <summary>
        /// <para><paramref name="start"/>: スタートするインデックス</para>
        /// <para><paramref name="moveNum"/>: 移動回数</para>
        /// </summary>
        /// <param name="start">: スタートするインデックス</param>
        /// <param name="moveNum">: 移動回数</param>
        [凾(256)]
        public (int To, T Data) Move(int start, ulong moveNum)
        {
            T d = op.Identity;
            int current = start;
            foreach (var b in moveNum.Bits())
            {
                if ((uint)current >= (uint)paths[b].Length)
                    return (current, d);
                d = op.Operate(d, data[b][current]);
                current = paths[b][current];
            }
            return (current, d);
        }
    }
}
