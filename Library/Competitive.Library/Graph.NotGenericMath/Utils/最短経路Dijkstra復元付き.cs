using AtCoder.Operators;
using System;
using System.Collections.Immutable;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 経路復元付き最短経路(Dijkstra)
    public static class 最短経路Dijkstra復元付き
    {
        /// <summary>
        /// <para><paramref name="from"/> からの最短経路長をダイクストラ法で求める。</para>
        /// <para>また、最短経路となるルートをスタックに積んで返す</para>
        /// <para>計算量: O( (|E| + |V|) log |V| )</para>
        /// </summary>
        public static (T Distance, ImmutableStack<TEdge> Route)[] DijkstraWithRoute<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph, int from)
            where T : struct, IComparable<T>
            where TOp : struct, IAdditionOperator<T>, IMinMaxValueOperator<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            TOp op = default;
            var graphArr = graph.AsArray();
            var res = new (T Distance, ImmutableStack<TEdge> Route)[graphArr.Length];
            Array.Fill(res, (op.MaxValue, null));
            res[from] = (default, ImmutableStack<TEdge>.Empty);

            var used = new bool[graphArr.Length];
            int count = 0;
            var remains = new PriorityQueueDijkstra<T>(graphArr.Length);
            remains.Enqueue(default, from);

            while (remains.TryDequeue(out var len, out var ix))
            {
                if (used[ix]) continue;
                used[ix] = true;
                if (++count >= graphArr.Length) break;
                foreach (var e in graphArr[ix].Children)
                {
                    var to = e.To;
                    var nextLength = op.Add(len, e.Value);
                    if (res[to].Distance.CompareTo(nextLength) > 0)
                    {
                        res[to] = (nextLength, res[ix].Route.Push(e));
                        remains.Enqueue(nextLength, to);
                    }
                }
            }
            return res;
        }
    }
}
