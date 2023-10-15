using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public sealed class PersistentStack<T> : IImmutableStack<T>, IReadOnlyCollection<T>
    {
        private PersistentStack() { }
        private PersistentStack(T head, PersistentStack<T> prev)
        {
            _head = head;
            _prev = prev;
            Count = prev.Count + 1;
        }
        private readonly T _head;
        private readonly PersistentStack<T> _prev;

        public static PersistentStack<T> Empty { get; } = new PersistentStack<T>();

        public bool IsEmpty => _prev == null;
        public int Count { get; }

        [凾(256)] public T Peek() => _head;

        [凾(256)] public PersistentStack<T> Clear() => Empty;
        [凾(256)] public PersistentStack<T> Pop() => IsEmpty ? Throw<PersistentStack<T>>() : _prev;
        [凾(256)] public PersistentStack<T> Push(T value) => new PersistentStack<T>(value, this);

        IImmutableStack<T> IImmutableStack<T>.Clear() => Clear();
        IImmutableStack<T> IImmutableStack<T>.Pop() => Pop();
        IImmutableStack<T> IImmutableStack<T>.Push(T value) => Push(value);

        public Enumerator GetEnumerator() => new Enumerator(this);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        static T2 Throw<T2>() => throw new InvalidOperationException("This operation does not apply to an empty instance.");
        public struct Enumerator : IEnumerator<T>
        {
            public T Current { get; private set; }
            object IEnumerator.Current => Current;
            private PersistentStack<T> root;
            private PersistentStack<T> currentStack;
            public Enumerator(PersistentStack<T> stack)
            {
                root = currentStack = stack;
                Current = default;
            }

            public bool MoveNext()
            {
                if (currentStack.IsEmpty) return false;
                Current = currentStack._head;
                currentStack = currentStack._prev;
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
