using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

// https://ei1333.github.io/library/structure/trie/binary-trie.cpp

namespace Kzrnm.Competitive
{
    /// <summary>
    /// BinaryTrie。整数値の集合を扱えるデータ構造
    /// </summary>
    public class BinaryTrie : BinaryTrie<ulong>
    {
        /// <summary>
        /// <see langword="ulong"/> 型の数値を取り扱う BinaryTrie を作成します。
        /// </summary>
        public BinaryTrie() : base() { }
        /// <summary>
        /// <c>(1 &lt;&lt; <paramref name="maxDepth"/>)</c> 未満の数値を取り扱う BinaryTrie を作成します。
        /// </summary>
        public BinaryTrie(int maxDepth = 64) : base(maxDepth) { }
    }
    /// <summary>
    /// BinaryTrie。整数値の集合を扱えるデータ構造
    /// </summary>
    public class BinaryTrie<T> where T : struct, IBitwiseOperators<T, T, T>, IShiftOperators<T, int, T>, IMultiplicativeIdentity<T, T>, IEqualityOperators<T, T, bool>
    {
        /// <summary>
        /// <typeparamref name="T"/> 型の数値を取り扱う BinaryTrie を作成します。
        /// </summary>
        public BinaryTrie() : this(new(), Unsafe.SizeOf<T>() * 8) { }
        /// <summary>
        /// <c>(1 &lt;&lt; <paramref name="maxDepth"/>)</c> 未満の数値を取り扱う BinaryTrie を作成します。
        /// </summary>
        public BinaryTrie(int maxDepth) : this(new(), maxDepth) { }
        BinaryTrie(Node root, int maxDepth)
        {
            _r = root;
            MaxDepth = maxDepth - 1;
        }
        public Node Root => _r;
        Node _r;
        readonly int MaxDepth;

        /// <summary>
        /// <para><paramref name="num"/> を <paramref name="delta"/> 個追加します。</para>
        /// <para><paramref name="idx"/> に対して −1 以外を与えると、マッチしたノードの accept に <paramref name="idx"/> が追加されます。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱います。</para>
        /// <para><paramref name="clear"/> が true で <paramref name="num"/> が 0 個になったときにはノードを削除します。</para>
        /// </summary>
        /// <remarks>
        /// <para><paramref name="num"/> が負とならないようにしなければなりません。</para>
        /// </remarks>
        [凾(256)]
        public void Add(T num, int delta, T xorVal = default, int idx = -1, bool clear = false)
             => _r = _r.Add(num, MaxDepth, delta, xorVal, idx, clear);

        /// <summary>
        /// <para><paramref name="num"/> をデクリメントする。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱います。</para>
        /// </summary>
        [凾(256)]
        public void Increment(T num, T xorVal = default)
             => Add(num, 1, xorVal);

        /// <summary>
        /// <para><paramref name="num"/> をデクリメントする。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱います。</para>
        /// <para><paramref name="clear"/> が true で <paramref name="num"/> が 0 個になったときにはノードを削除します。</para>
        /// </summary>
        [凾(256)]
        public void Decrement(T num, bool clear = false, T xorVal = default)
             => Add(num, -1, xorVal, clear: clear);

        /// <summary>
        /// <para><paramref name="num"/> に対応するノードを返す。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱います。</para>
        /// </summary>
        [凾(256)]
        public Node Find(T num, T xorVal = default)
             => _r.Find(num, MaxDepth, xorVal);

        /// <summary>
        /// 現在の要素数を返す。
        /// </summary>
        public int CountAll => _r.Exist;

        /// <summary>
        /// <para><paramref name="num"/> に対応する個数を返す。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱います。</para>
        /// </summary>
        [凾(256)]
        public int Count(T num, T xorVal = default) => Find(num, xorVal)?.Exist ?? 0;

        /// <summary>
        /// <para>最小値と対応するノードを返す。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱います。</para>
        /// </summary>
        [凾(256)]
        public (T Num, Node Node) MinElement(T xorVal = default)
        {
            Contract.Assert(_r.Exist > 0);
            return KthElement(0, xorVal);
        }

        /// <summary>
        /// <para>最大値と対応するノードを返す。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱います。</para>
        /// </summary>
        [凾(256)]
        public (T Num, Node Node) MaxElement(T xorVal = default)
        {
            Contract.Assert(_r.Exist > 0);
            return KthElement(_r.Exist - 1, xorVal);
        }

        /// <summary>
        /// <para><paramref name="k"/> 番目(0-indexed) に小さい値とそれに対応するノードを返す。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱います。</para>
        /// </summary>
        [凾(256)]
        public (T Num, Node Node) KthElement(int k, T xorVal = default)
        {
            Contract.Assert(0 <= k && k < _r.Exist);
            return _r.KthElement(k, MaxDepth, xorVal);
        }

        /// <summary>
        /// <para><paramref name="num"/> 未満の個数を返す。</para>
        /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱います。</para>
        /// </summary>
        [凾(256)]
        public int CountLess(T num, T xorVal = default)
        {
            return _r.CountLess(num, MaxDepth, xorVal);
        }

        public class Node
        {
            Node _l, _r;
            public Node Left => _l;
            public Node Right => _r;
            public int Exist { get; private set; }
            List<int> ac;
            public ReadOnlySpan<int> Accepts => ac == null ? default : CollectionsMarshal.AsSpan(ac);


            /// <summary>
            /// <para><paramref name="num"/> を <paramref name="x"/> 個追加します。</para>
            /// <para><paramref name="idx"/> に対して −1 以外を与えると、マッチしたノードの accept に <paramref name="idx"/> が追加されます。</para>
            /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱います。</para>
            /// <para><paramref name="clear"/> が true で <paramref name="num"/> が 0 個になったときにはノードを削除します。</para>
            /// </summary>
            /// <remarks>
            /// <para><paramref name="num"/> が負とならないようにしなければなりません。</para>
            /// </remarks>
            [凾(256)]
            public Node Add(T num, int depth, int x, T xorVal = default, int idx = -1, bool clear = false)
            {
                if (depth == -1)
                {
                    Exist += x;
                    if (idx >= 0) (ac ??= new()).Add(idx);
                }
                else
                {
                    Exist += x;
                    ref var to = ref _l;
                    if ((((xorVal >> depth) ^ (num >> depth)) & T.MultiplicativeIdentity) != default)
                        to = ref _r;
                    to = (to ?? new()).Add(num, depth - 1, x, xorVal, idx, clear);
                    if (to.Exist <= 0 && clear)
                        to = null;
                }
                return this;
            }
            [凾(256)]
            public Node Find(T bit, int depth, T xorVal)
            {
                if (depth == -1)
                    return this;
                var to = _l;
                if ((((xorVal >> depth) ^ (bit >> depth)) & T.MultiplicativeIdentity) != default)
                    to = _r;
                return to?.Find(bit, depth - 1, xorVal);
            }


            /// <summary>
            /// <para><paramref name="k"/> 番目(0-indexed) に小さい値とそれに対応するノードを返す。</para>
            /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱います。</para>
            /// </summary>
            [凾(256)]
            public (T Num, Node Node) KthElement(int k, int bitIndex, T xorVal)
            {
                var t = this;
                T num = default;
                var bb = T.MultiplicativeIdentity << bitIndex;
                for (; bb != default; bb >>= 1)
                {
                    bool f = (xorVal & bb) != default;
                    var tn = f ? t._r : t._l;
                    var nb = tn?.Exist ?? 0;
                    if (nb <= k)
                    {
                        t = !f ? t._r : t._l;
                        k -= nb;
                        num |= bb;
                    }
                    else
                    {
                        t = tn;
                    }
                }
                return (num, t);
            }

            /// <summary>
            /// <para><paramref name="num"/> 未満の個数を返す。</para>
            /// <para>すべての値に <paramref name="xorVal"/> と XOR を取った値で扱います。</para>
            /// </summary>
            [凾(256)]
            public int CountLess(T num, int bitIndex, T xorVal)
            {
                int ret = 0;
                var bb = T.MultiplicativeIdentity << bitIndex;
                for (var t = this; bb != default && t != null; bb >>= 1)
                {
                    bool f = (xorVal & bb) != default;
                    var tn = f ? t._r : t._l;
                    if ((num & bb) != default && tn != null)
                        ret += tn.Exist;
                    t = (((num & bb) != default) != f) ? t._r : t._l;
                }
                return ret;
            }
        }
    }
}
