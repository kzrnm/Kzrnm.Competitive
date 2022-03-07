using AtCoder;
using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// Mo's algorithm の 状態を保持する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [IsOperator]
    public interface IMoAlgorithmState<T>
    {
        /// <summary>
        /// [l, r) から [l-1, r) または [l, r+1) を求める。
        /// </summary>
        void Add(int index);
        /// <summary>
        /// [l, r) から [l+1, r) または [l, r-1) を求める。
        /// </summary>
        void Remove(int index);

        /// <summary>
        /// 現在の [l, r) の 状態を返す。
        /// </summary>
        T Current { get; }
    }
    /// <summary>
    /// Mo's algorithm. 平方分割
    /// </summary>
    public class MoAlgorithm<T, TOp> where TOp : struct, IMoAlgorithmState<T>
    {
        private TOp st;
        private int Max;
        private readonly List<(int From, int ToExclusive, int Index)> builder;
        /// <summary>
        /// 平方分割でオフラインクエリを計算する
        /// </summary>
        /// <param name="st">現在の状態を保持する構造体</param>
        public MoAlgorithm(TOp st)
        {
            this.st = st;
            builder = new List<(int From, int ToExclusive, int Index)>();
        }

        /// <summary>
        /// クエリを追加する
        /// </summary>
        [凾(256)]
        public void AddQuery(int from, int toExclusive)
        {
            Max = Math.Max(toExclusive, Max);
            builder.Add((from, toExclusive, builder.Count));
        }

        /// <summary>
        /// クエリの結果を返す
        /// </summary>
        [凾(256)]
        public T[] Solve()
        {
            if (builder.Count == 0) 
                return Array.Empty<T>();

            var queries = builder.ToArray();

            int sq = (int)Math.Sqrt(Max);
            Array.Sort(queries.Select(t =>
            {
                var block = t.From / sq;
                var second = (uint)t.ToExclusive;
                if ((block & 1) != 0)
                    second = ~second;
                return ((ulong)block) << 32 | second;
            }).ToArray(), queries);

            var result = new T[queries.Length];
            int left = 0, right = 0;
            foreach (var (f, t, i) in queries)
            {
                while (f < left)
                    st.Add(--left);
                while (right < t)
                    st.Add(right++);
                while (left < f)
                    st.Remove(left++);
                while (t < right)
                    st.Remove(--right);

                result[i] = st.Current;
            }
            return result;
        }
    }
}
