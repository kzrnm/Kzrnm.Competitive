using AtCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// HL分解の実装。合わせてオイラーツアーの内容も保持する。
    /// </summary>
    public class HeavyLightDecomposition<TNode, TEdge>
        where TNode : ITreeNode<TEdge>
        where TEdge : IGraphEdge
    {
        /// <summary>
        /// 対象の木
        /// </summary>
        public TNode[] tree;
        /// <summary>
        /// オイラーツアーで入ってくるインデックス。
        /// </summary>
        public int[] down;
        /// <summary>
        /// オイラーツアーで出ていくインデックス(=最後に入った子孫要素のインデックスdown+1)。
        /// </summary>
        public int[] up;
        /// <summary>
        /// HL分解 して次に見る要素
        /// </summary>
        public int[] nxt;
        public HeavyLightDecomposition(TNode[] tree, int[] nxt, int[] down, int[] up)
        {
            Debug.Assert(tree.Length == nxt.Length);
            Debug.Assert(tree.Length == down.Length);
            Debug.Assert(tree.Length == up.Length);
            this.tree = tree;
            this.nxt = nxt;
            this.down = down;
            this.up = up;
        }

        /// <summary>
        /// 頂点 <paramref name="u"/> のオイラーツアーのインデックスを返します。
        /// </summary>
        [凾(256)] public (int From, int To) Index(int u) => (down[u], up[u]);

        /// <summary>
        /// <paramref name="u"/>と<paramref name="v"/>の最小共通祖先を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public int LowestCommonAncestor(int u, int v)
        {
            if (u == v) return u;
            // v が u　の子孫ならば、u に入る辺はカウントせず、v に入る辺までカウントする。(u が v の子孫でも同様)
            var f = down[u] + 1;
            var t = down[v] + 1;
            if (t < f) (f, t) = (t, f);
            return lcaTable[f..t].Node;
        }

        /// <summary>
        /// <paramref name="u"/> から <paramref name="v"/> に <paramref name="f"/> を適用する。
        /// </summary>
        /// <param name="u">パスの始点</param>
        /// <param name="v">パスの終点</param>
        /// <param name="vertex">trueなら頂点クエリ、falseなら辺クエリ</param>
        /// <param name="f">クエリ関数。<see cref="down"/> の順に格納した Segtree などの範囲演算をさせると良い。</param>
        /// <remarks>
        /// <para>計算量(<paramref name="f"/> を O(1), 木のノード数を n とするとき): O(n)</para>
        /// </remarks>
        [凾(256)]
        public void PathQuery(int u, int v, bool vertex, Action<int, int> f)
            => PathQuery(u, v, vertex, new FWrapper(f));

        /// <summary>
        /// <paramref name="u"/> から <paramref name="v"/> に <paramref name="f"/> を適用する。
        /// </summary>
        /// <param name="u">パスの始点</param>
        /// <param name="v">パスの終点</param>
        /// <param name="vertex">trueなら頂点クエリ、falseなら辺クエリ</param>
        /// <param name="f">クエリ関数オペレータ。<see cref="down"/> の順に格納した Segtree などの範囲演算をさせると良い。</param>
        /// <remarks>
        /// <para>計算量(<paramref name="f"/> を O(1), 木のノード数を n とするとき): O(n)</para>
        /// </remarks>
        [凾(256)]
        public void PathQuery<TOp>(int u, int v, bool vertex, TOp f)
            where TOp : IHlDecompositionOperator
        {
            foreach (var (from, to) in new PathEnumerator(this, u, v, vertex))
                f.Operate(from, to);
        }

        /// <summary>
        /// <paramref name="u"/> から <paramref name="v"/> に <paramref name="f"/> を適用する。
        /// </summary>
        /// <param name="u">パスの始点</param>
        /// <param name="v">パスの終点</param>
        /// <param name="vertex">trueなら頂点クエリ、falseなら辺クエリ</param>
        /// <param name="f">クエリ関数オペレータ。<see cref="down"/> の順に格納した Segtree などの範囲演算をさせると良い。</param>
        /// <remarks>
        /// <para>計算量(<paramref name="f"/> を O(1), 木のノード数を n とするとき): O(n)</para>
        /// </remarks>
        [凾(256)]
        public void PathQuery<TOp>(int u, int v, bool vertex, ref TOp f)
            where TOp : IHlDecompositionOperator
        {
            foreach (var (from, to) in new PathEnumerator(this, u, v, vertex))
                f.Operate(from, to);
        }


        /// <summary>
        /// 部分木に <paramref name="f"/> を適用する。
        /// </summary>
        /// <param name="u">部分木の根</param>
        /// <param name="vertex">trueなら頂点クエリ、falseなら辺クエリ</param>
        /// <param name="f">クエリ関数。<see cref="down"/> の順に格納した Segtree などの範囲演算をさせると良い。</param>
        /// <remarks>
        /// <para>計算量(<paramref name="f"/> を O(1) とするとき): O(1)</para>
        /// </remarks>
        [凾(256)]
        public void SubtreeQuery(int u, bool vertex, Action<int, int> f)
            => SubtreeQuery(u, vertex, new FWrapper(f));

        /// <summary>
        /// 部分木に <paramref name="f"/> を適用する。
        /// </summary>
        /// <param name="u">部分木の根</param>
        /// <param name="vertex">trueなら頂点クエリ、falseなら辺クエリ</param>
        /// <param name="f">クエリ関数オペレータ。<see cref="down"/> の順に格納した Segtree などの範囲演算をさせると良い。</param>
        /// <remarks>
        /// <para>計算量(<paramref name="f"/> を O(1) とするとき): O(1)</para>
        /// </remarks>
        [凾(256)]
        public void SubtreeQuery<TOp>(int u, bool vertex, TOp f)
            where TOp : IHlDecompositionOperator
            => f.Operate(down[u] + (vertex ? 0 : 1), up[u]);

        /// <summary>
        /// 部分木に <paramref name="f"/> を適用する。
        /// </summary>
        /// <param name="u">部分木の根</param>
        /// <param name="vertex">trueなら頂点クエリ、falseなら辺クエリ</param>
        /// <param name="f">クエリ関数オペレータ。<see cref="down"/> の順に格納した Segtree などの範囲演算をさせると良い。</param>
        /// <remarks>
        /// <para>計算量(<paramref name="f"/> を O(1) とするとき): O(1)</para>
        /// </remarks>
        [凾(256)]
        public void SubtreeQuery<TOp>(int u, bool vertex, ref TOp f)
            where TOp : IHlDecompositionOperator
            => f.Operate(down[u] + (vertex ? 0 : 1), up[u]);


        private SparseTable<(int Depth, int Node), NodeMinOp> _lcaTable;
        private SparseTable<(int Depth, int Node), NodeMinOp> lcaTable
            => _lcaTable ??= BuildLcaTable();
        private SparseTable<(int Depth, int Node), NodeMinOp> BuildLcaTable()
        {
            var arr = new (int Depth, int Node)[down.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                var n = tree[i];
                arr[down[i]] = (n.Depth, n.Parent.To);
            }
            return new SparseTable<(int Depth, int Node), NodeMinOp>(arr);
        }
        private struct NodeMinOp : ISparseTableOperator<(int Depth, int Node)>
        {
            [凾(256)]
            public (int Depth, int Node) Operate((int Depth, int Node) x, (int Depth, int Node) y) => x.Depth <= y.Depth ? x : y;
        }
        private readonly struct FWrapper : IHlDecompositionOperator
        {
            private readonly Action<int, int> f;
            public FWrapper(Action<int, int> func)
            {
                f = func;
            }
            [凾(256)]
            public void Operate(int u, int v) => f(u, v);
        }
        private struct PathEnumerator
        {
            HeavyLightDecomposition<TNode, TEdge> hl;
            (int f, int t) _c;
            int u, v, lca;
            Stack<(int f, int t)> desc;
            St st;
            public PathEnumerator(HeavyLightDecomposition<TNode, TEdge> hl, int u, int v, bool vertex)
            {
                lca = hl.LowestCommonAncestor(u, v);
                desc = BuildDesc(hl, lca, v);
                _c = default;
                this.hl = hl;
                this.u = u;
                this.v = lca;
                st = (St)(vertex ? 0b11 : 0b01);
            }
            public PathEnumerator GetEnumerator() => this;
            static Stack<(int f, int t)> BuildDesc(HeavyLightDecomposition<TNode, TEdge> hl, int u, int v)
            {
                var s = new Stack<(int f, int t)>();
                var tree = hl.tree;
                var nxt = hl.nxt;
                var down = hl.down;
                while (u != v)
                {
                    if (nxt[u] == nxt[v])
                    {
                        s.Push((down[u] + 1, down[v] + 1));
                        break;
                    }
                    else
                    {
                        s.Push((down[nxt[v]], down[v] + 1));
                        v = tree[nxt[v]].Parent.To;
                    }
                }
                return s;
            }
            public (int f, int t) Current => _c;
            public bool MoveNext()
            {
                var down = hl.down;
                var nxt = hl.nxt;

                if (u == v)
                    goto Desc;

                if (st.HasFlag(St.Start))
                    st &= ~St.Start;
                else
                    u = hl.tree[nxt[u]].Parent.To;

                // Asc
                if (nxt[u] != nxt[v])
                {
                    _c = (down[u] + 1, down[nxt[u]]);
                    return true;
                }
                if (u != v)
                {
                    _c = (down[u] + 1, down[v] + 1);
                    u = v;
                    return true;
                }
            Desc:
                if (st.HasFlag(St.Vertex))
                {
                    st &= ~St.Vertex;
                    var r = hl.down[lca];
                    _c = (r, r + 1);
                    return true;
                }
                return desc.TryPop(out _c);
            }
            [Flags]
            enum St : byte
            {
                Start = 1,
                Vertex = 0b10,
            }
        }
    }
    [IsOperator]
    public interface IHlDecompositionOperator
    {
        /// <summary>
        /// <paramref name="f"/> から <paramref name="t"/> までのパスに適用する関数。Segtree などの範囲演算をさせると良い。<paramref name="t"/> はパスに含まないのに注意。
        /// </summary>
        /// <param name="f">オイラーツアーの開始のインデックス</param>
        /// <param name="t">オイラーツアーの終了のインデックス</param>
        void Operate(int f, int t);
    }
}
