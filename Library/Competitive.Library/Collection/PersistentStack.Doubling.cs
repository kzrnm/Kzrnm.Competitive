using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public sealed class PersistentStackDoubling<T> : IImmutableStack<T>, IReadOnlyCollection<T>
    {
        private PersistentStackDoubling() { _prevs = Array.Empty<PersistentStackDoubling<T>>(); }
        private PersistentStackDoubling(T head, PersistentStackDoubling<T> prev)
        {
            _head = head;
            Count = prev.Count + 1;
            _prevs = new PersistentStackDoubling<T>[BitOperations.Log2((uint)Count) + 1];
            _prevs[0] = prev;
            for (int i = 0; i + 1 < _prevs.Length; i++)
                _prevs[i + 1] = _prevs[i]._prevs[i];
        }
        private readonly T _head;
        /// <summary>
        /// 2^n個前の要素への参照を保持する配列
        /// </summary>
        private readonly PersistentStackDoubling<T>[] _prevs;

        public static PersistentStackDoubling<T> Empty { get; } = new PersistentStackDoubling<T>();

        public bool IsEmpty => _prevs.Length == 0;
        public int Count { get; }

        /// <summary>
        /// O(log N) でインデクサから取得する。
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public T this[int index]
        {
            [凾(256)]
            get
            {
                if ((uint)index < (uint)Count)
                {
                    if (index == 0) return _head;

                    var log2 = BitOperations.Log2((uint)index);
                    return _prevs[log2][index - (1 << log2)];
                }
                return Throw();
                static T Throw() => throw new IndexOutOfRangeException();
            }
        }

        [凾(256)] public T Peek() => _head;

        [凾(256)] public PersistentStackDoubling<T> Clear() => Empty;
        [凾(256)] public PersistentStackDoubling<T> Pop() => IsEmpty ? ThrowInvalidOperation<PersistentStackDoubling<T>>() : _prevs[0];
        [凾(256)] public PersistentStackDoubling<T> Push(T value) => new PersistentStackDoubling<T>(value, this);

        IImmutableStack<T> IImmutableStack<T>.Clear() => Clear();
        IImmutableStack<T> IImmutableStack<T>.Pop() => Pop();
        IImmutableStack<T> IImmutableStack<T>.Push(T value) => Push(value);

        public Enumerator GetEnumerator() => new Enumerator(this);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        static T2 ThrowInvalidOperation<T2>() => throw new InvalidOperationException("This operation does not apply to an empty instance.");
        public struct Enumerator : IEnumerator<T>
        {
            public T Current { get; private set; }
            object IEnumerator.Current => Current;
            private PersistentStackDoubling<T> root;
            private PersistentStackDoubling<T> currentStack;
            public Enumerator(PersistentStackDoubling<T> stack)
            {
                root = currentStack = stack;
                Current = default;
            }

            public bool MoveNext()
            {
                if (currentStack.IsEmpty) return false;
                Current = currentStack._head;
                currentStack = currentStack._prevs[0];
                return true;
            }

            public void Reset()
            {
                currentStack = root;
            }
            public void Dispose() { }
        }
    }
}
