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
        static IEnumerable<T[]> Enumerate<T>(IReadOnlyCollection<T> items)
        {
            if (items.Count == 1)
            {
                yield return new T[] { items.First() };
                yield break;
            }
            foreach (var item in items)
            {
                var ret = new T[items.Count];
                ret[0] = item;
                var nokori = new HashSet<T>(items);
                nokori.Remove(item);
                foreach (var right in Enumerate(nokori))
                {
                    right.CopyTo(ret, 1);
                    yield return ret;
                }
            }
        }
    }
    // キーの重複がOKな優先度付きキュー
    class PriorityQueue<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
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
                foreach (var queue in pair.Value)
                    yield return new KeyValuePair<TKey, TValue>(pair.Key, queue);
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();

        public PriorityQueue() { dic = new SortedDictionary<TKey, Queue<TValue>>(); }
        public PriorityQueue(IComparer<TKey> comparer) { dic = new SortedDictionary<TKey, Queue<TValue>>(comparer); }
    }
    class SortedCollection<T> : ICollection<T>
    {
        SortedDictionary<T, int> dic;
        public T First => dic.Keys.First();
        public SortedCollection() : this(Comparer<T>.Default) { }
        public SortedCollection(IComparer<T> comparer) { dic = new SortedDictionary<T, int>(comparer); }
        public SortedCollection(IEnumerable<T> original) : this(original, Comparer<T>.Default) { }
        public SortedCollection(IEnumerable<T> original, IComparer<T> comparer) : this(comparer)
        {
            foreach (var item in original)
                this.Add(item);
        }

        public bool IsReadOnly => false;
        public int Count { get; private set; } = 0;
        public void Add(T item)
        {
            if (dic.ContainsKey(item)) ++dic[item];
            else dic[item] = 1;
            Count++;
        }
        public void Clear() => dic.Clear();
        public bool Contains(T item) => dic.ContainsKey(item);
        public void CopyTo(T[] array, int arrayIndex) { throw new NotImplementedException(); }

        public bool Remove(T item)
        {
            int count;
            if (dic.TryGetValue(item, out count))
            {
                Count--;
                if (count > 1) --dic[item];
                else dic.Remove(item);
                return true;
            }
            else
                return false;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var pair in dic)
                foreach (var item in Enumerable.Repeat(pair.Key, pair.Value))
                    yield return item;
        }
    }
}
