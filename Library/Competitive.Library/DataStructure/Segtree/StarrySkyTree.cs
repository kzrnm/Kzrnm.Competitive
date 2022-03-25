using AtCoder;
using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.DataStructure
{
    [System.Diagnostics.DebuggerTypeProxy(typeof(StarrySkyTree<,>.DebugView))]
    public class StarrySkyTree<T, TOp>
        where T : struct
        where TOp : struct, ILazySegtreeOperator<T, T>
    {
        protected static readonly TOp op = default;

        readonly T[] lazy;
        readonly T[] data;
        public readonly int rootLength;
        public int Length { get; }
        public StarrySkyTree(int size)
        {
            Length = size;
            rootLength = 1 << (BitOperationsEx.MSB(size - 1) + 1);
            lazy = new T[(rootLength << 1) - 1];
            data = new T[(rootLength << 1) - 1];
            Array.Fill(lazy, op.Identity);
            Array.Fill(data, op.Identity);
        }
        public void Apply(int from, int toExclusive, T value)
        {
            void Add(int from, int toExclusive, T val, int k, int l, int r)
            {
                if (r <= from || toExclusive <= l) return;
                if (from <= l && r <= toExclusive)
                {
                    data[k] = op.Mapping(val, data[k]);
                    return;
                }
                int left = k * 2 + 1, right = k * 2 + 2;
                Add(from, toExclusive, val, left, l, (l + r) / 2);
                Add(from, toExclusive, val, right, (l + r) / 2, r);
                lazy[k] = op.Operate(op.Mapping(lazy[left], data[left]), op.Mapping(lazy[right], data[right]));
            }
            Add(from, toExclusive, value, 0, 0, rootLength);
        }
        public T Slice(int from, int length) => Prod(from, from + length);
        public T Prod(int from, int toExclusive)
        {
            T Prod(int from, int toExclusive, int k, int l, int r)
            {
                if (r <= from || toExclusive <= l) return op.Identity;
                if (from <= l && r <= toExclusive) return op.Mapping(lazy[k], data[k]);
                return op.Mapping(op.Operate(Prod(from, toExclusive, k * 2 + 1, l, (l + r) / 2), Prod(from, toExclusive, k * 2 + 2, (l + r) / 2, r)), data[k]);
            }
            return Prod(from, toExclusive, 0, 0, rootLength);
        }


        [System.Diagnostics.DebuggerDisplay("{" + nameof(value) + "}", Name = "{" + nameof(key) + ",nq}")]
        struct KeyValuePairs
        {
            readonly string key;
            (T data, T lazy) value;

            public KeyValuePairs(string key, (T data, T lazy) value)
            {
                this.key = key;
                this.value = value;
            }
        }
        class DebugView
        {
            readonly StarrySkyTree<T, TOp> tree;
            public DebugView(StarrySkyTree<T, TOp> tree)
            {
                this.tree = tree;
            }

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public KeyValuePairs[] Tree
            {
                get
                {
                    var keys = new List<KeyValuePairs>(tree.lazy.Length);
                    for (var len = tree.rootLength; len > 0; len >>= 1)
                    {
                        var unit = tree.rootLength / len;
                        for (var i = 0; i < len; i++)
                        {
                            var index = i + len - 1;
                            if (unit == 1)
                                keys.Add(new KeyValuePairs($"[{i}]", (tree.data[index], tree.lazy[index])));
                            else
                            {
                                var from = i * unit;
                                keys.Add(new KeyValuePairs($"[{from}-{from + unit})", (tree.data[index], tree.lazy[index])));
                            }
                        }
                    }
                    return keys.ToArray();
                }
            }
        }
    }
    public struct StarrySkyTreeOperator : ILazySegtreeOperator<long, long>
    {
        public long Identity => 0;

        public long FIdentity => 0;

        [凾(256)]
        public long Composition(long f, long g) => f + g;
        [凾(256)]
        public long Mapping(long f, long x) => x + f;

        [凾(256)]
        public long Operate(long x, long y) => Math.Max(x, y);
    }
}