using System;
using IEnumerable = System.Collections.IEnumerable;
using IEnumerator = System.Collections.IEnumerator;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtCoderProject.Hide
{
    static class 順列を求める
    {
        static T[][] Enumerate<T>(T[] items)
        {
            static T[][] EnumerateImpl(ReadOnlySpan<T> items)
            {
                if (items.Length == 0)
                    throw new IndexOutOfRangeException();
                if (items.Length == 1)
                    return new T[][] { items.ToArray() };

                var arr = items.ToArray();
                var size = 1;
                for (int i = 2; i <= items.Length; i++)
                    size *= i;
                var ret = new T[size][];
                for (int i = 0; i < items.Length; i++)
                {
                    (arr[0], arr[i]) = (arr[i], arr[0]);
                    foreach (var item in EnumerateImpl(new ReadOnlySpan<T>(arr).Slice(1)))
                    {
                        ret[--size] = new T[items.Length];
                        ret[size][0] = arr[0];
                        item.CopyTo(ret[size], 1);
                    };
                }

                return ret;
            }
            return EnumerateImpl(items);
        }
    }

    [System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    class SortedMultiSet<TKey> : IEnumerable<TKey>
    {
        SortedDictionary<TKey, int> dic;

        public int Count { get; private set; } = 0;

        public void Add(TKey key)
        {
            dic.TryGetValue(key, out var val);
            dic[key] = val + 1;
            Count++;
        }
        public bool Remove(TKey key)
        {
            dic.TryGetValue(key, out var val);
            if (val == 0)
                return false;

            if (val == 1)
                dic.Remove(key);
            else
                dic[key] = val - 1;
            Count--;
            return true;
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            foreach (var pair in dic)
                for (int i = 0; i < pair.Value; i++)
                    yield return pair.Key;
        }
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public SortedMultiSet() { dic = new SortedDictionary<TKey, int>(); }
        public SortedMultiSet(IComparer<TKey> comparer) { dic = new SortedDictionary<TKey, int>(comparer); }
    }


    // キーの重複がOKな優先度付きキュー
    [System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    class PriorityQueue<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
    {
        SortedDictionary<TKey, Queue<TValue>> dic;

        public int Count { get; private set; } = 0;

        public void Add(TKey key, TValue value)
        {
            if (!dic.ContainsKey(key)) dic[key] = new Queue<TValue>();

            dic[key].Enqueue(value);
            Count++;
        }

        public KeyValuePair<TKey, TValue> Dequeue()
        {
            var queue = dic.First();
            if (queue.Value.Count <= 1) dic.Remove(queue.Key);
            Count--;
            return new KeyValuePair<TKey, TValue>(queue.Key, queue.Value.Dequeue());
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var pair in dic)
                foreach (var val in pair.Value)
                    yield return new KeyValuePair<TKey, TValue>(pair.Key, val);
        }
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public PriorityQueue() { dic = new SortedDictionary<TKey, Queue<TValue>>(); }
        public PriorityQueue(IComparer<TKey> comparer) { dic = new SortedDictionary<TKey, Queue<TValue>>(comparer); }
    }

    [System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    class SortedCollection<T> : IList<T>
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)] List<T> list;

        public SortedCollection() : this(Comparer<T>.Default) { }
        public SortedCollection(int capacity) : this(capacity, Comparer<T>.Default) { }
        public SortedCollection(IComparer<T> comparer) { this.Comparer = comparer; list = new List<T>(); }
        public SortedCollection(int capacity, IComparer<T> comparer) { this.Comparer = comparer; list = new List<T>(capacity); }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)] public IComparer<T> Comparer { get; }
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)] public bool IsReadOnly => false;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)] public int Count => list.Count;

        public T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }

        /// <summary>
        /// 与えられた比較関数に従って，<paramref name="item"/> であるような最小のインデックスを取得します←ほんとに？．見つからなかった場合は<paramref name="item"/>より大きい最小のインデックスのビット反転を返します.
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="item">対象となる要素</param>
        /// <returns><paramref name="item"/> が見つかった場合は0-indexed でのインデックス．見つからなかった場合は<paramref name="item"/>より大きい最小のインデックスのビット反転.</returns>
        /// <remarks> 比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        protected int BinarySearch(T item) => list.BinarySearch(item, this.Comparer);

        private int BinarySearchImpl(T item, bool isLowerBound)
        {
            var l = 0;
            var r = this.Count - 1;
            while (l <= r)
            {
                var m = (l + r) >> 1;
                var res = this.Comparer.Compare(this[m], item);
                if (res < 0 || (res == 0 && !isLowerBound)) l = m + 1;
                else r = m - 1;
            }
            return l;
        }

        /// <summary>
        ///　デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="item"/> 以上の要素であるような最小のインデックスを取得します．
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="item">対象となる要素</param>
        /// <param name="f"></param>
        /// <returns><paramref name="item"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public int LowerBound(T item) => BinarySearchImpl(item, true);

        /// <summary>
        ///　デフォルトの比較関数に従って，<paramref name="a"/> の要素のうち，<paramref name="v"/> より真に大きい要素が現れる最小のインデックスを取得します．
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="a">対象となるコレクション</param>
        /// <param name="v">対象となる要素</param>
        /// <param name="f"></param>
        /// <returns><paramref name="v"/> 以上の要素であるような最小の o-indexed でのインデックス．</returns>
        /// <remarks> <paramref name="a"/> は比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        public int UpperBound(T item) => BinarySearchImpl(item, false);

        void ICollection<T>.Add(T item) => this.Add(item);
        public int Add(T item)
        {
            var index = BinarySearch(item);
            if (index < 0)
                index = ~index;
            list.Insert(index, item);
            return index;
        }

        public int IndexOf(T item)
        {
            var index = BinarySearch(item);
            if (index >= 0) return index;
            else return -1;
        }
        public bool Contains(T item) => BinarySearch(item) >= 0;
        public void Clear() => list.Clear();
        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index < 0)
                return false;
            list.RemoveAt(index);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
        public void Insert(int index, T item) => throw new NotImplementedException();
        public void RemoveAt(int index) => list.RemoveAt(index);
    }


    [System.Diagnostics.DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    class SortedUniqueCollection<T> : IList<T>
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)] List<T> list;

        public SortedUniqueCollection() : this(Comparer<T>.Default) { }
        public SortedUniqueCollection(int capacity) : this(capacity, Comparer<T>.Default) { }
        public SortedUniqueCollection(IComparer<T> comparer) { this.Comparer = comparer; list = new List<T>(); }
        public SortedUniqueCollection(int capacity, IComparer<T> comparer) { this.Comparer = comparer; list = new List<T>(capacity); }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)] public IComparer<T> Comparer { get; }
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)] public bool IsReadOnly => false;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)] public int Count => list.Count;

        public T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }

        /// <summary>
        /// 与えられた比較関数に従って，<paramref name="item"/> であるような最小のインデックスを取得します←ほんとに？．見つからなかった場合は<paramref name="item"/>より大きい最小のインデックスのビット反転を返します.
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> の要素の型を指定します．</typeparam>
        /// <param name="item">対象となる要素</param>
        /// <returns><paramref name="item"/> が見つかった場合は0-indexed でのインデックス．見つからなかった場合は<paramref name="item"/>より大きい最小のインデックスのビット反転.</returns>
        /// <remarks> 比較関数に対して昇順であることを仮定しています．この関数は O(log N) で実行されます．</remarks>
        protected int BinarySearch(T item) => list.BinarySearch(item, this.Comparer);

        void ICollection<T>.Add(T item) => this.Add(item);
        public int Add(T item)
        {
            var index = BinarySearch(item);
            if (index < 0)
                index = ~index;
            else
                return -1;
            list.Insert(index, item);
            return index;
        }

        public int IndexOf(T item)
        {
            var index = BinarySearch(item);
            if (index >= 0) return index;
            else return -1;
        }
        public bool Contains(T item) => BinarySearch(item) >= 0;
        public void Clear() => list.Clear();
        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index < 0)
                return false;
            list.RemoveAt(index);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
        public void Insert(int index, T item) => throw new NotImplementedException();
        public void RemoveAt(int index) => list.RemoveAt(index);
    }

    // ある範囲の数値を管理するHashSetのようなもの
    class IntSet : ISet<int>
    {
        private bool[] impl;
        public IntSet(int size)
        {
            impl = new bool[size];
        }

        public int Count { private set; get; }

        public bool IsReadOnly => false;

        public bool Add(int item)
        {
            if (impl.Length <= item)
                return false;
            if (impl[item])
                return false;
            impl[item] = true;
            ++Count;
            return true;
        }

        public void Clear()
        {
            impl = new bool[impl.Length];
            Count = 0;
        }
        public bool Remove(int item)
        {
            if (impl.Length <= item)
                return false;
            if (!impl[item])
                return false;
            impl[item] = false;
            return true;
        }


        public bool Contains(int item) => impl[item];

        public void CopyTo(int[] array, int arrayIndex)
        {
            for (int i = 0; i < impl.Length; i++)
                if (impl[i])
                    array[arrayIndex++] = i;
        }

        public void ExceptWith(IEnumerable<int> other)
        {
            foreach (var item in other)
                impl[item] = false;
        }

        public void IntersectWith(IEnumerable<int> other)
        {
            var next = new bool[impl.Length];
            foreach (var item in other)
                if (impl[item])
                    next[item] = true;
            impl = next;
        }
        public void UnionWith(IEnumerable<int> other)
        {
            foreach (var item in other)
                impl[item] = true;
        }


        bool ISet<int>.IsProperSubsetOf(IEnumerable<int> other) { throw new NotSupportedException(); }

        bool ISet<int>.IsProperSupersetOf(IEnumerable<int> other)
        {
            var cnt = 0;
            foreach (var item in other)
            {
                ++cnt;
                if (!impl[item])
                    return false;
            }
            return this.Count != cnt;
        }

        bool ISet<int>.IsSubsetOf(IEnumerable<int> other) { throw new NotSupportedException(); }
        bool ISet<int>.IsSupersetOf(IEnumerable<int> other)
        {
            foreach (var item in other)
                if (!impl[item])
                    return false;
            return true;
        }

        bool ISet<int>.Overlaps(IEnumerable<int> other)
        {
            foreach (var item in other)
                if (impl[item])
                    return true;
            return false;
        }

        bool ISet<int>.SetEquals(IEnumerable<int> other) { throw new NotSupportedException(); }
        void ISet<int>.SymmetricExceptWith(IEnumerable<int> other) { throw new NotSupportedException(); }
        void ICollection<int>.Add(int item) => this.Add(item);

        public IEnumerator<int> GetEnumerator()
        {
            for (var i = 0; i < impl.Length; i++)
                if (impl[i])
                    yield return i;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}