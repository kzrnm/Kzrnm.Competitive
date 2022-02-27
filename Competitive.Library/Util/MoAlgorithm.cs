using System;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public interface IMoAlgorithmOperator<T>
    {
        T Indentity { get; }
        void Add(ref T current, int index);
        void Remove(ref T current, int index);
    }
    /// <summary>
    /// Mo's algorithm. 平方分割
    /// </summary>
    public class MoAlgorithm<T, TOp> where TOp : struct, IMoAlgorithmOperator<T>
    {
        private readonly TOp op;
        private readonly int Length;
        private readonly (int From, int ToExclusive, int Index)[] Queries;
        /// <summary>
        /// 平方分割でオフラインクエリを計算する
        /// </summary>
        /// <param name="n">元データの長さ</param>
        /// <param name="queries"></param>
        /// <param name="op"></param>
        public MoAlgorithm(int n, (int From, int ToExclusive)[] queries, TOp op = default)
        {
            this.op = op;
            this.Length = n;
            Queries = queries.Indexed().Select(t => (t.Value.From, t.Value.ToExclusive, t.Index))
                .ToArray();
            int sq = (int)Math.Sqrt(n);
            Array.Sort(Queries.Select(t => (t.From / sq, t.ToExclusive)).ToArray(), Queries);
        }

        /// <summary>
        /// クエリの結果を返す
        /// </summary>
        /// <returns></returns>
        [凾(256)]
        public T[] Solve()
        {
            var result = new T[Queries.Length];
            int left = 0, right = 0;
            foreach (var (f, t, i) in Queries)
            {
                ref var res = ref result[i];
                res = op.Indentity;
                while (f < left)
                    op.Add(ref res, --left);
                while (left < f)
                    op.Remove(ref res, left++);
                while (right < t)
                    op.Add(ref res, right++);
                while (t < right)
                    op.Remove(ref res, --right);
            }
            return result;
        }
    }
}
