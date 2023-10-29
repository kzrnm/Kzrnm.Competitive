using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class 最小有向全域木
    {
        /// <summary>
        /// <para>有向グラフの最小全域木を求める。</para>
        /// <para><paramref name="root"/>を根とする木を構築する。</para>
        /// <para>計算量は O(E V log(V))</para>
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="T"/> は符号付き</para>
        /// <para>制約: <paramref name="graph"/> の <paramref name="root"/> からすべての頂点に到達できる</para>
        /// </remarks>
        public static MstResult<T, TEdge> DirectedMinimumSpanningTree<T, TNode, TEdge>(this IWGraph<T, TNode, TEdge> graph, int root)
            where T : IComparable<T>, IAdditionOperators<T, T, T>, IUnaryNegationOperators<T, T>
            where TNode : IGraphNode<TEdge>
            where TEdge : struct, IWGraphEdge<T>, IReversable<TEdge>
        {
            (int from, TEdge edge)[] edges = graph.AsArray()
                .SelectMany((n, i) => n.Children.Select(e => (i, e)))
                .Concat(Enumerable.Range(0, graph.Length).Where(i => i != root).Select(i => (i, new TEdge().Reversed(root))))
                .ToArray();

            var heap = new SkewHeap<T, T, HOp<T>>();
            var parents = new int[2 * graph.Length];
            Array.Fill(parents, -1);
            var visited = new bool?[2 * graph.Length];
            var link = (int[])parents.Clone();
            var ins = new SkewHeap<T, T, HOp<T>>.Node[2 * graph.Length];

            for (int i = 0; i < edges.Length; i++)
            {
                var ee = edges[i].edge;
                var t = ee.To;
                var c = ee.Value;
                ins[t] = heap.Push(ins[t], c, i);
            }
            var st = new List<int>();
            int Go(int x)
            {
                x = edges[ins[x].index].from;
                while (link[x] != -1)
                {
                    st.Add(x);
                    x = link[x];
                }
                foreach (var p in st.AsSpan())
                    link[p] = x;
                st.Clear();
                return x;
            }
            int x = 0;
            for (int i = graph.Length; ins[x] != null; i++)
            {
                for (; visited[x] == null; x = Go(x))
                    visited[x] = false;
                for (; x != i; x = Go(x))
                {
                    var w = ins[x].key;
                    var v = heap.Pop(ins[x]);
                    v = heap.Add(v, -w);
                    ins[i] = heap.Merge(ins[i], v);
                    parents[x] = i;
                    link[x] = i;
                }
                while (ins[x] != null && Go(x) == x)
                    ins[x] = heap.Pop(ins[x]);
            }
            T cost = default;
            var result = new List<(int from, TEdge edge)>(graph.Length - 1);
            for (int i = root; i != -1; i = parents[i])
            {
                visited[i] = true;
            }
            for (int i = x; i >= 0; i--)
            {
                if (visited[i] == true) continue;
                var p = ins[i].index;
                result.Add(edges[p]);
                cost += edges[p].edge.Value;
                for (int j = edges[p].edge.To; j >= 0 && visited[j] == false; j = parents[j])
                {
                    visited[j] = true;
                }
            }
            return new MstResult<T, TEdge>
            {
                Cost = cost,
                Edges = result.ToArray()
            };
        }
        readonly struct HOp<T> : ISkewHeapOperator<T, T>
            where T : IComparable<T>, IAdditionOperators<T, T, T>
        {
            public T FIdentity => default;
            [凾(256)]
            public int Compare(T x, T y) => x.CompareTo(y);
            [凾(256)] public T Mapping(T f, T x) => x + f;
            [凾(256)] public T Composition(T nf, T cf) => nf + cf;
        }
    }
}
