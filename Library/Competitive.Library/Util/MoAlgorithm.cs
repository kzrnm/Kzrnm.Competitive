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
    [IsOperator]
    public interface IMoAlgorithmState<T>
    {
        /// <summary>
        /// [l, r) から [l-1, r) または [l, r+1) を求める。
        /// </summary>
        void Add(int idx);
        /// <summary>
        /// [l, r) から [l+1, r) または [l, r-1) を求める。
        /// </summary>
        void Remove(int idx);

        /// <summary>
        /// 現在の [l, r) の 状態を返す。
        /// </summary>
        T Current { get; }
    }
    /// <summary>
    /// Mo's algorithm の 状態を保持する
    /// </summary>
    [IsOperator]
    public interface IMoAlgorithmStateStrict<T>
    {
        /// <summary>
        /// [l, r) から [l-1, r) を求める。
        /// </summary>
        void AddLeft(int idx);
        /// <summary>
        /// [l, r) から [l, r+1) を求める。
        /// </summary>
        void AddRight(int idx);
        /// <summary>
        /// [l, r) から [l+1, r) を求める。
        /// </summary>
        void RemoveLeft(int idx);
        /// <summary>
        /// [l, r) から [l, r-1) を求める。
        /// </summary>
        void RemoveRight(int idx);

        /// <summary>
        /// 現在の [l, r) の 状態を返す。
        /// </summary>
        T Current { get; }
    }
    /// <summary>
    /// Mo's algorithm. 平方分割
    /// </summary>
    public class MoAlgorithm
    {
        private int Max;
        private readonly List<(int From, int ToExclusive, int Index)> builder;
        /// <summary>
        /// 平方分割でオフラインクエリを計算する
        /// </summary>
        public MoAlgorithm()
        {
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

        private struct StrictWrapper<T, TSt> : IMoAlgorithmStateStrict<T>
            where TSt : struct, IMoAlgorithmState<T>
        {
            private TSt st;
            public StrictWrapper(TSt status) { st = status; }
            public T Current => st.Current;
            [凾(256)] public void AddLeft(int idx) => st.Add(idx);
            [凾(256)] public void AddRight(int idx) => st.Add(idx);
            [凾(256)] public void RemoveLeft(int idx) => st.Remove(idx);
            [凾(256)] public void RemoveRight(int idx) => st.Remove(idx);
        }

        /// <summary>
        /// クエリの結果を返す
        /// </summary>
        /// <param name="st">現在の状態を保持する構造体</param>
        /// <param name="blockSize">分割するサイズ(デフォルト: √max(toExclusive))</param>
        [凾(256)]
        public T[] Solve<T, TSt>(TSt st, int blockSize = 0) where TSt : struct, IMoAlgorithmState<T>
            => SolveStrict<T, StrictWrapper<T, TSt>>(new StrictWrapper<T, TSt>(st), blockSize);

        /// <summary>
        /// クエリの結果を返す
        /// </summary>
        /// <param name="st">現在の状態を保持する構造体</param>
        /// <param name="blockSize">分割するサイズ(デフォルト: √max(toExclusive))</param>
        [凾(256 | 512)]
        public T[] SolveStrict<T, TSt>(TSt st, int blockSize = 0) where TSt : struct, IMoAlgorithmStateStrict<T>
        {
            if (builder.Count == 0)
                return Array.Empty<T>();
            if (blockSize == 0)
                blockSize = (int)Math.Sqrt(Max);
            var queries = builder.ToArray();
            Array.Sort(queries.Select(t =>
            {
                var block = t.From / blockSize;
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
                    st.AddLeft(--left);
                while (right < t)
                    st.AddRight(right++);
                while (left < f)
                    st.RemoveLeft(left++);
                while (t < right)
                    st.RemoveRight(--right);

                result[i] = st.Current;
            }
            return result;
        }
    }
}
