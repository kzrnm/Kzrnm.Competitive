using System;
using System.Collections.Generic;
using static AtCoder.Global;


namespace AtCoder.DataStructure
{
    [System.Diagnostics.DebuggerTypeProxy(typeof(DebugView))]
    public class StarrySkyTree
    {
        protected virtual long DefaultValue => default;
        protected virtual long OpDefault => default;
        protected virtual long Merge(long v1, long v2) => Math.Max(v1, v2);

        readonly long[] lazy;
        readonly long[] data;
        public readonly int rootLength;
        public int Length { get; }
        public StarrySkyTree(int size)
        {
            this.Length = size;
            rootLength = 1 << (MSB(size - 1) + 1);
            lazy = NewArray((rootLength << 1) - 1, DefaultValue);
            data = NewArray((rootLength << 1) - 1, DefaultValue);
        }
        public void Add(int from, int toExclusive, long value)
        {
            void Add(int from, int toExclusive, long val, int k, int l, int r)
            {
                if (r <= from || toExclusive <= l) return;
                if (from <= l && r <= toExclusive)
                {
                    data[k] += val;
                    return;
                }
                int left = k * 2 + 1, right = k * 2 + 2;
                Add(from, toExclusive, val, left, l, (l + r) / 2);
                Add(from, toExclusive, val, right, (l + r) / 2, r);
                lazy[k] = Merge(lazy[left] + data[left], lazy[right] + data[right]);
            }
            Add(from, toExclusive, value, 0, 0, rootLength);
        }
        public long Slice(int from, int length) => Query(from, from + length);
        public long Query(int from, int toExclusive)
        {
            long Query(int from, int toExclusive, int k, int l, int r)
            {
                if (r <= from || toExclusive <= l) return DefaultValue;
                if (from <= l && r <= toExclusive) return lazy[k] + data[k];
                return Merge(Query(from, toExclusive, k * 2 + 1, l, (l + r) / 2), Query(from, toExclusive, k * 2 + 2, (l + r) / 2, r)) + data[k];
            }
            return Query(from, toExclusive, 0, 0, rootLength);
        }


        [System.Diagnostics.DebuggerDisplay("{" + nameof(value) + "}", Name = "{" + nameof(key) + ",nq}")]
        struct KeyValuePairs
        {
            readonly string key;
            (long data, long lazy) value;

            public KeyValuePairs(string key, (long data, long lazy) value)
            {
                this.key = key;
                this.value = value;
            }
        }
        class DebugView
        {
            readonly StarrySkyTree tree;
            public DebugView(StarrySkyTree tree)
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
}