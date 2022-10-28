using AtCoder;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

// https://nyaannyaan.github.io/library/tree/rerooting.hpp
namespace Kzrnm.Competitive
{
    public static class 全方位木DP
    {
        /// <summary>
        /// 全方位木DPを行う
        /// </summary>
        [凾(256)]
        [DebuggerStepThrough]
        public static Impl<TNode, TEdge> Rerooting<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
              where TNode : ITreeNode<TEdge>
              where TEdge : IGraphEdge, IReversable<TEdge>
            => new Impl<TNode, TEdge>(tree);


        public readonly struct Impl<TNode, TEdge>
              where TNode : ITreeNode<TEdge>
              where TEdge : IGraphEdge, IReversable<TEdge>
        {
            [DebuggerStepThrough]
            public Impl(ITreeGraph<TNode, TEdge> tree)
            {
                Tree = tree;
            }

            private readonly ITreeGraph<TNode, TEdge> Tree;
            public T[] Run<T, TOp>(TOp op = default)
                where TOp : struct, IRerootingOperator<T, TEdge>
            {
                var tree = Tree.AsArray();
                var memo = new T[tree.Length];
                var dp = new T[tree.Length];

                // Dfs1
                {
                    var idx = Tree.DfsDescendant();
                    for (int i = idx.Length - 1; i >= 0; i--)
                    {
                        var ix = idx[i];
                        memo[ix] = op.Identity;
                        foreach (var e in tree[ix].Children)
                            memo[ix] = op.Merge(memo[ix], op.Propagate(memo[e.To], ix, e));
                    }
                }

                // Dfs2
                {
                    var stack = new Stack<(int Current, T Prev)>();
                    stack.Push((Tree.Root, op.Identity));
                    while (stack.TryPop(out var tuple))
                    {
                        var (cur, pval) = tuple;

                        // get cumulative sum
                        var edges = tree[cur].Children;
                        var buf = new T[edges.Length];
                        for (int i = 0; i < edges.Length; i++)
                        {
                            var e = edges[i];
                            buf[i] = op.Propagate(memo[e.To], cur, e);
                        }

                        var head = new T[buf.Length + 1];
                        var tail = new T[buf.Length + 1];
                        head[0] = tail[^1] = op.Identity;

                        for (int i = 0; i < buf.Length; i++) head[i + 1] = op.Merge(head[i], buf[i]);
                        for (int i = buf.Length - 1; i >= 0; i--) tail[i] = op.Merge(tail[i + 1], buf[i]);

                        // update
                        dp[cur] = op.Merge(pval, head[^1]);

                        // propagate
                        for (int i = 0; i < edges.Length; i++)
                        {
                            var e = edges[i];
                            var dst = e.To;
                            stack.Push((dst, op.Propagate(op.Merge(pval, op.Merge(head[i], tail[i + 1])), dst, e.Reversed(cur))));
                        }
                    }
                }

                return dp;
            }
        }
    }

    [IsOperator]
    public interface IRerootingOperator<T, TEdge> where TEdge : IGraphEdge
    {
        /// <summary>
        /// 動的計画法の単位元。
        /// </summary>
        T Identity { get; }

        /// <summary>
        /// 部分木の値 <paramref name="x"/> に <paramref name="y"/> をマージする関数。
        /// </summary>
        /// <param name="x">元になる部分木の値</param>
        /// <param name="y">マージされる部分木の値</param>
        /// <returns>部分木の値をマージした値</returns>
        T Merge(T x, T y);

        /// <summary>
        /// 子要素から親要素へ伝播させる関数。
        /// </summary>
        /// <param name="x">子要素を根とする部分木の値</param>
        /// <param name="parent">親</param>
        /// <param name="edge">子への辺</param>
        /// <returns></returns>
        T Propagate(T x, int parent, TEdge edge);
    }
}
