using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtCoderProject.Hide
{
    // キーの重複がOKな優先度付きキュー
    class PriorityQueue<TKey, TValue>
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
        public PriorityQueue() { dic = new SortedDictionary<TKey, Queue<TValue>>(); }
        public PriorityQueue(IComparer<TKey> comparer) { dic = new SortedDictionary<TKey, Queue<TValue>>(comparer); }
    }
    class SortedCollection<T> : List<T>
    {
        private IComparer<T> Comparer { get; }
        public SortedCollection() : this(Comparer<T>.Default) { }
        public SortedCollection(IComparer<T> comparer) { this.Comparer = comparer; }
        public new int Add(T item)
        {
            if (this.Count == 0)
            {
                base.Add(item);
                return 0;
            }

            int start = 0;
            int end = this.Count;
            while (start != end)
            {
                var half = (start + end) / 2;
                if (Comparer.Compare(item, this[half]) < 0)
                {
                    end = half;
                }
                else
                {
                    start = half + 1;
                }
            }

            Insert(start, item);
            return start;
        }
    }
}
