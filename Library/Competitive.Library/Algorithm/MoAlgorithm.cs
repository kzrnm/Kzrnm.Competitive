using AtCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
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
        /// 平方分割でオフラインクエリを計算する
        /// </summary>
        public MoAlgorithm(IEnumerable<(int From, int ToExclusive)> queries) : this()
        {
            foreach (var (f, t) in queries)
                AddQuery(f, t);
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
        /// <param name="st">現在の状態</param>
        [凾(256)]
        public T[] Solve<T, TSt>(TSt st = default) where TSt : IMoAlgorithmState<T>
        {
            var wrapper = new StateStrictWrapper<T, TSt>(st, builder.Count);
            SolveStrict(wrapper);
            return wrapper.result;
        }

        /// <summary>
        /// クエリの結果を返す
        /// </summary>
        /// <param name="st">現在の状態</param>
        [凾(256)]
        public T[] SolveStrict<T, TSt>(TSt st = default) where TSt : IMoAlgorithmStateStrict<T>
        {
            var wrapper = new StrictStateStrictWrapper<T, TSt>(st, builder.Count);
            SolveStrict(wrapper);
            return wrapper.result;
        }

        /// <summary>
        /// クエリを適用します。
        /// </summary>
        /// <param name="add">[l, r) から [l-1, r) または [l, r+1) を求める。i = l-1 または r</param>
        /// <param name="remove">[l, r) から [l+1, r) または [l, r-1) を求める。i = l または r-1</param>
        /// <param name="update">/// i のクエリを更新します。</param>
        [凾(256)]
        public void Solve(Action<int> add, Action<int> remove, Action<int> update)
            => SolveStrict(new ActionStrictWrapper(add, add, remove, remove, update));


        /// <summary>
        /// クエリを適用します。
        /// </summary>
        /// <param name="addLeft">[l, r) から [l-1, r) を求める。i = l-1</param>
        /// <param name="addRight">[l, r) から [l, r+1) を求める。i = r</param>
        /// <param name="removeLeft">[l, r) から [l+1, r) を求める。i = l</param>
        /// <param name="removeRight">[l, r) から [l, r-1) を求める。i = r-1</param>
        /// <param name="update">/// i のクエリを更新します。</param>
        [凾(256)]
        public void SolveStrict(
                Action<int> addLeft,
                Action<int> addRight,
                Action<int> removeLeft,
                Action<int> removeRight,
                Action<int> update)
            => SolveStrict(new ActionStrictWrapper(addLeft, addRight, removeLeft, removeRight, update));

        /// <summary>
        /// クエリを適用します。
        /// </summary>
        /// <param name="op">現在の状態を更新する</param>
        [凾(256)]
        public void Solve<TOp>(TOp op = default) where TOp : IMoAlgorithmOperator => SolveStrict(new OpStrictWrapper<TOp>(op), 0);

        /// <summary>
        /// クエリを適用します。
        /// </summary>
        /// <param name="op">現在の状態を更新する</param>
        [凾(256)]
        public void SolveStrict<TOp>(TOp op = default) where TOp : IMoAlgorithmOperatorStrict => SolveStrict(op, 0);

        /// <summary>
        /// クエリを適用します。
        /// </summary>
        /// <param name="op">現在の状態を更新する</param>
        /// <param name="blockSize">分割するサイズ(デフォルト: √3 max(toExclusive) / √(2Q))</param>
        [凾(512)]
        internal void SolveStrict<TOp>(TOp op, int blockSize) where TOp : IMoAlgorithmOperatorStrict
        {
            if (builder.Count == 0) return;
            if (blockSize == 0)
                blockSize = 1 + (int)(1.732 * Max / Math.Sqrt(2 * builder.Count));
            var queries = builder.ToArray();
            Array.Sort(queries.Select(t =>
            {
                var block = t.From / blockSize;
                var second = (uint)t.ToExclusive;
                if ((block & 1) != 0)
                    second = ~second;
                return ((ulong)block) << 32 | second;
            }).ToArray(), queries);

            int left = 0, right = 0;
            foreach (var (f, t, i) in queries)
            {
                while (f < left)
                    op.AddLeft(--left);
                while (right < t)
                    op.AddRight(right++);
                while (left < f)
                    op.RemoveLeft(left++);
                while (t < right)
                    op.RemoveRight(--right);
                op.Update(i);
            }
        }

        private struct StateStrictWrapper<T, TSt> : IMoAlgorithmOperatorStrict
            where TSt : IMoAlgorithmState<T>
        {
            private TSt st;
            public readonly T[] result;
            public StateStrictWrapper(TSt status, int length)
            {
                st = status;
                result = new T[length];
            }
            [凾(256)] public void AddLeft(int idx) => st.Add(idx);
            [凾(256)] public void AddRight(int idx) => st.Add(idx);
            [凾(256)] public void RemoveLeft(int idx) => st.Remove(idx);
            [凾(256)] public void RemoveRight(int idx) => st.Remove(idx);
            [凾(256)] public void Update(int idx) => result[idx] = st.Current;
        }
        private struct StrictStateStrictWrapper<T, TSt> : IMoAlgorithmOperatorStrict
            where TSt : IMoAlgorithmStateStrict<T>
        {
            private TSt st;
            public readonly T[] result;
            public StrictStateStrictWrapper(TSt status, int length)
            {
                st = status;
                result = new T[length];
            }
            [凾(256)] public void AddLeft(int idx) => st.AddLeft(idx);
            [凾(256)] public void AddRight(int idx) => st.AddRight(idx);
            [凾(256)] public void RemoveLeft(int idx) => st.RemoveLeft(idx);
            [凾(256)] public void RemoveRight(int idx) => st.RemoveRight(idx);
            [凾(256)] public void Update(int idx) => result[idx] = st.Current;
        }
        private struct OpStrictWrapper<TOp> : IMoAlgorithmOperatorStrict
            where TOp : IMoAlgorithmOperator
        {
            private TOp st;
            public OpStrictWrapper(TOp status) { st = status; }
            [凾(256)] public void AddLeft(int idx) => st.Add(idx);
            [凾(256)] public void AddRight(int idx) => st.Add(idx);
            [凾(256)] public void RemoveLeft(int idx) => st.Remove(idx);
            [凾(256)] public void RemoveRight(int idx) => st.Remove(idx);
            [凾(256)] public void Update(int idx) => st.Update(idx);
        }
        private readonly struct ActionStrictWrapper : IMoAlgorithmOperatorStrict
        {
            private readonly Action<int> addLeft;
            private readonly Action<int> addRight;
            private readonly Action<int> removeLeft;
            private readonly Action<int> removeRight;
            private readonly Action<int> update;
            public ActionStrictWrapper(
                Action<int> addLeft,
                Action<int> addRight,
                Action<int> removeLeft,
                Action<int> removeRight,
                Action<int> update)
            {
                this.addLeft = addLeft;
                this.addRight = addRight;
                this.removeLeft = removeLeft;
                this.removeRight = removeRight;
                this.update = update;
            }
            [凾(256)] public void AddLeft(int idx) => addLeft(idx);
            [凾(256)] public void AddRight(int idx) => addRight(idx);
            [凾(256)] public void RemoveLeft(int idx) => removeLeft(idx);
            [凾(256)] public void RemoveRight(int idx) => removeRight(idx);
            [凾(256)] public void Update(int idx) => update(idx);
        }
    }
    /// <summary>
    /// Mo's algorithm の 状態を保持する
    /// </summary>
    [IsOperator]
    public interface IMoAlgorithmState<T>
    {
        /// <summary>
        /// [l, r) から [l-1, r) または [l, r+1) を求める。<paramref name="idx"/> = l-1 または r
        /// </summary>
        void Add(int idx);
        /// <summary>
        /// [l, r) から [l+1, r) または [l, r-1) を求める。<paramref name="idx"/> = l または r-1
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
        /// [l, r) から [l-1, r) を求める。<paramref name="idx"/> = l-1
        /// </summary>
        void AddLeft(int idx);
        /// <summary>
        /// [l, r) から [l, r+1) を求める。<paramref name="idx"/> = r
        /// </summary>
        void AddRight(int idx);
        /// <summary>
        /// [l, r) から [l+1, r) を求める。<paramref name="idx"/> = l
        /// </summary>
        void RemoveLeft(int idx);
        /// <summary>
        /// [l, r) から [l, r-1) を求める。<paramref name="idx"/> = r-1
        /// </summary>
        void RemoveRight(int idx);

        /// <summary>
        /// 現在の [l, r) の 状態を返す。
        /// </summary>
        T Current { get; }
    }

    /// <summary>
    /// Mo's algorithm を操作を定義します。
    /// </summary>
    [IsOperator]
    public interface IMoAlgorithmOperator
    {
        /// <summary>
        /// [l, r) から [l-1, r) または [l, r+1) を求める。<paramref name="idx"/> = l-1 または r
        /// </summary>
        void Add(int idx);
        /// <summary>
        /// [l, r) から [l+1, r) または [l, r-1) を求める。<paramref name="idx"/> = l または r-1
        /// </summary>
        void Remove(int idx);
        /// <summary>
        /// <paramref name="idx"/> のクエリを更新します。
        /// </summary>
        void Update(int idx);
    }
    /// <summary>
    /// Mo's algorithm を操作を定義します。
    /// </summary>
    [IsOperator]
    public interface IMoAlgorithmOperatorStrict
    {
        /// <summary>
        /// [l, r) から [l-1, r) を求める。<paramref name="idx"/> = l-1
        /// </summary>
        void AddLeft(int idx);
        /// <summary>
        /// [l, r) から [l, r+1) を求める。<paramref name="idx"/> = r
        /// </summary>
        void AddRight(int idx);
        /// <summary>
        /// [l, r) から [l+1, r) を求める。<paramref name="idx"/> = l
        /// </summary>
        void RemoveLeft(int idx);
        /// <summary>
        /// [l, r) から [l, r-1) を求める。<paramref name="idx"/> = r-1
        /// </summary>
        void RemoveRight(int idx);
        /// <summary>
        /// <paramref name="idx"/> のクエリを更新します。
        /// </summary>
        void Update(int idx);
    }
}
