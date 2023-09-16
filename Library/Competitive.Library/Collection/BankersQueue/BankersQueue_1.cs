// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// An immutable queue.
    /// </summary>
    /// <typeparam name="T">The type of elements stored in the queue.</typeparam>
    [DebuggerDisplay("IsEmpty = {IsEmpty}")]
    public sealed partial class BankersQueue<T> : IImmutableQueue<T>
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
        private static readonly BankersQueue<T> s_EmptyField = new BankersQueue<T>(ImmutableStack<T>.Empty, 0, ImmutableStack<T>.Empty, 0);

        /// <summary>
        /// The end of the queue that enqueued elements are pushed onto.
        /// </summary>
        private ImmutableStack<T> _backwards;

        /// <summary>
        /// The end of the queue from which elements are dequeued.
        /// </summary>
        private IImmutableStack<T> _forwards;

        /// <summary>
        /// The size of <see cref="_backwards"/>.
        /// </summary>
        private int _backwardsSize;

        /// <summary>
        /// The size of <see cref="_forwards"/>.
        /// </summary>
        private int _forwardsSize;


        /// <summary>
        /// Initializes a new instance of the <see cref="BankersQueue{T}"/> class.
        /// </summary>
        /// <param name="forwards">The forwards stack.</param>
        /// <param name="forwardsSize">The size of forwards stack.</param>
        /// <param name="backwards">The backwards stack.</param>
        /// <param name="backwardsSize">The size of backwards stack.</param>
        internal BankersQueue(IImmutableStack<T> forwards, int forwardsSize, ImmutableStack<T> backwards, int backwardsSize)
        {
            Debug.Assert(forwards != null);
            Debug.Assert(backwards != null);

            _forwards = forwards;
            _backwards = backwards;

            _forwardsSize = forwardsSize;
            _backwardsSize = backwardsSize;
        }

        /// <summary>
        /// Gets the empty queue.
        /// </summary>
        public BankersQueue<T> Clear()
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
                return _forwardsSize == 0 && _backwardsSize == 0;
            }
        }

        /// <summary>
        /// Gets the empty queue.
        /// </summary>
        public static BankersQueue<T> Empty
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
            return this.Clear();
        }

        /// <summary>
        /// Gets the element at the front of the queue.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
        public T Peek()
        {
            if (this.IsEmpty)
            {
                ThrowInvalidOperationException("This operation does not apply to an empty instance.");
            }

            return _forwards.Peek();
        }

        /// <summary>
        /// Gets a read-only reference to the element at the front of the queue.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
        public ref readonly T PeekRef()
        {
            if (this.IsEmpty)
            {
                ThrowInvalidOperationException("This operation does not apply to an empty instance.");
            }

            if (_forwards is ImmutableStack<T> im)
            {
                return ref im.PeekRef();
            }
            if (_forwards is LazyReverseStack lz)
            {
                return ref lz.PeekRef();
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Adds an element to the back of the queue.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The new queue.
        /// </returns>
        public BankersQueue<T> Enqueue(T value)
        {
            if (this.IsEmpty)
            {
                return new BankersQueue<T>(ImmutableStack.Create(value), 1, ImmutableStack<T>.Empty, 0);
            }
            else
            {
                return new BankersQueue<T>(_forwards, _forwardsSize, _backwards.Push(value), _backwardsSize + 1).Normalize();
            }
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
            return this.Enqueue(value);
        }

        /// <summary>
        /// Returns a queue that is missing the front element.
        /// </summary>
        /// <returns>A queue; never <c>null</c>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
        public BankersQueue<T> Dequeue()
        {
            if (this.IsEmpty)
            {
                ThrowInvalidOperationException("This operation does not apply to an empty instance.");
            }

            IImmutableStack<T> f = _forwards.Pop();
            return new BankersQueue<T>(f, _forwardsSize - 1, _backwards, _backwardsSize).Normalize();
        }

        /// <summary>
        /// Retrieves the item at the head of the queue, and returns a queue with the head element removed.
        /// </summary>
        /// <param name="value">Receives the value from the head of the queue.</param>
        /// <returns>The new queue with the head element removed.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
        public BankersQueue<T> Dequeue(out T value)
        {
            value = this.Peek();
            return this.Dequeue();
        }

        /// <summary>
        /// Returns a queue that is missing the front element.
        /// </summary>
        /// <returns>A queue; never <c>null</c>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
        IImmutableQueue<T> IImmutableQueue<T>.Dequeue()
        {
            return this.Dequeue();
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
            return this.IsEmpty ?
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

        BankersQueue<T> Normalize(bool force = false)
        {
            if (_forwardsSize == 0 && _backwardsSize == 0)
            {
                return Empty;
            }
            if (force || _forwardsSize < _backwardsSize)
            {
                _forwards = new LazyReverseStack(_forwards, _backwards);
                _backwards = ImmutableStack<T>.Empty;
                _forwardsSize += _backwardsSize;
                _backwardsSize = 0;
            }
            return this;
        }
        static T ThrowInvalidOperationException(string msg) => ThrowInvalidOperationException(msg);
    }
}