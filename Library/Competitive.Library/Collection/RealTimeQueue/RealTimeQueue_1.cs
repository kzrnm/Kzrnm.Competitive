// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// An immutable queue.
    /// </summary>
    /// <typeparam name="T">The type of elements stored in the queue.</typeparam>
    [DebuggerDisplay("IsEmpty = {IsEmpty}")]
    public sealed partial class RealTimeQueue<T> : IImmutableQueue<T>
    {
        /*
         * Original is ImmutableQueue<T>
         *
         * Copyright (c) .NET Foundation and Contributors
         * Released under the MIT license
         * https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
         */
        internal const string LISENCE = @"
Original is ImmutableQueue<T>

Copyright (c) .NET Foundation and Contributors
Released under the MIT license
https://github.com/dotnet/runtime/blob/master/LICENSE.TXT
";
        /// <summary>
        /// The singleton empty queue.
        /// </summary>
        /// <remarks>
        /// Additional instances representing the empty queue may exist on deserialized instances.
        /// Actually since this queue is a struct, instances don't even apply and there are no singletons.
        /// </remarks>
        private static readonly RealTimeQueue<T> s_EmptyField = new RealTimeQueue<T>(LazyStack.Empty, ImmutableStack<T>.Empty, LazyStack.Empty);

        /// <summary>
        /// The end of the queue that enqueued elements are pushed onto.
        /// </summary>
        private readonly ImmutableStack<T> _backwards;

        /// <summary>
        /// The end of the queue from which elements are dequeued.
        /// </summary>
        private readonly LazyStack _forwards;

        /// <summary>
        /// The status for lazy initialization
        /// </summary>
        private readonly LazyStack _lazy;

        /// <summary>
        /// Backing field for the <see cref="BackwardsReversed"/> property.
        /// </summary>
        private ImmutableStack<T> _backwardsReversed;

        private RealTimeQueue(LazyStack forwards, ImmutableStack<T> backwards, LazyStack lazy)
        {
            Debug.Assert(forwards is { });
            Debug.Assert(backwards is { });
            Debug.Assert(lazy is { });

            _forwards = forwards;
            _backwards = backwards;
            _lazy = lazy;
        }
        [凾(256)]
        internal static RealTimeQueue<T> Create(LazyStack forwards, ImmutableStack<T> backwards, LazyStack lazy)
        {
            if (!lazy.IsEmpty)
            {
                return new RealTimeQueue<T>(forwards, backwards, lazy.Pop());
            }
            var stack = new LazyStack(forwards, backwards, LazyStack.Empty);
            return new RealTimeQueue<T>(stack, ImmutableStack<T>.Empty, stack);
        }

        /// <summary>
        /// Gets the empty queue.
        /// </summary>
        public RealTimeQueue<T> Clear()
        {
            Debug.Assert(s_EmptyField.IsEmpty);
            return Empty;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get
            {
                Debug.Assert(!_forwards.IsEmpty || _backwards.IsEmpty);
                return _forwards.IsEmpty;
            }
        }

        /// <summary>
        /// Gets the empty queue.
        /// </summary>
        public static RealTimeQueue<T> Empty
        {
            get
            {
                Debug.Assert(s_EmptyField.IsEmpty);
                return s_EmptyField;
            }
        }

        /// <summary>
        /// Gets an empty queue.
        /// </summary>
        IImmutableQueue<T> IImmutableQueue<T>.Clear()
        {
            Debug.Assert(s_EmptyField.IsEmpty);
            return Clear();
        }

        /// <summary>
        /// Gets the reversed <see cref="_backwards"/> stack.
        /// </summary>
        private ImmutableStack<T> BackwardsReversed
        {
            get
            {
                // Although this is a lazy-init pattern, no lock is required because
                // this instance is immutable otherwise, and a double-assignment from multiple
                // threads is harmless.
                _backwardsReversed ??= Reverse(_backwards);

                Debug.Assert(_backwardsReversed != null);
                return _backwardsReversed;

                static ImmutableStack<T> Reverse(ImmutableStack<T> stack)
                {
                    var r = stack.Clear();
                    for (ImmutableStack<T> f = stack; !f.IsEmpty; f = f.Pop())
                    {
                        r = r.Push(f.Peek());
                    }

                    Debug.Assert(r != null);
                    Debug.Assert(r.IsEmpty == stack.IsEmpty);
                    return r;
                }
            }
        }

        /// <summary>
        /// Gets the element at the front of the queue.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when the queue is empty.</exception>
        [凾(256)]
        public T Peek()
        {
            AtCoder.Internal.Contract.Assert(!IsEmpty, "This operation does not apply to an empty instance.");

            return _forwards.Peek();
        }

        /// <summary>
        /// Gets a read-only reference to the element at the front of the queue.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when the queue is empty.</exception>
        [凾(256)]
        public ref readonly T PeekRef()
        {
            AtCoder.Internal.Contract.Assert(!IsEmpty, "This operation does not apply to an empty instance.");

            return ref _forwards.PeekRef();
        }

        /// <summary>
        /// Adds an element to the back of the queue.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The new queue.
        /// </returns>
        [凾(256)]
        public RealTimeQueue<T> Enqueue(T value)
        {
            return Create(_forwards, _backwards.Push(value), _lazy);
        }

        /// <summary>
        /// Adds an element to the back of the queue.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The new queue.
        /// </returns>
        IImmutableQueue<T> IImmutableQueue<T>.Enqueue(T value)
        {
            return Enqueue(value);
        }

        /// <summary>
        /// Returns a queue that is missing the front element.
        /// </summary>
        /// <returns>A queue; never <c>null</c>.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the queue is empty.</exception>
        [凾(256)]
        public RealTimeQueue<T> Dequeue()
        {
            AtCoder.Internal.Contract.Assert(!IsEmpty, "This operation does not apply to an empty instance.");
            var nextForwards = _forwards.Pop();
            if (nextForwards.IsEmpty && _backwards.IsEmpty)
            {
                return Empty;
            }
            return Create(nextForwards, _backwards, _lazy);
        }

        /// <summary>
        /// Retrieves the item at the head of the queue, and returns a queue with the head element removed.
        /// </summary>
        /// <param name="value">Receives the value from the head of the queue.</param>
        /// <returns>The new queue with the head element removed.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the queue is empty.</exception>
        [凾(256)]
        public RealTimeQueue<T> Dequeue(out T value)
        {
            value = Peek();
            return Dequeue();
        }

        /// <summary>
        /// Returns a queue that is missing the front element.
        /// </summary>
        /// <returns>A queue; never <c>null</c>.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the queue is empty.</exception>
        IImmutableQueue<T> IImmutableQueue<T>.Dequeue()
        {
            return Dequeue();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An <see cref="Enumerator"/> that can be used to iterate through the collection.
        /// </returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return IsEmpty ?
                Enumerable.Empty<T>().GetEnumerator() :
                new EnumeratorObject(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new EnumeratorObject(this);
        }

        static T ThrowInvalidOperationException() => throw new InvalidOperationException();
    }
}
