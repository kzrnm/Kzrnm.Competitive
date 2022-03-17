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
    public sealed partial class RealTimeQueue<T>
    {

        /// <summary>
        /// An immutable stack with lazy initialization.
        /// </summary>
        [DebuggerDisplay("IsEmpty = {IsEmpty}; Top = {_head}")]
        internal class LazyStack : IImmutableStack<T>
        {
            /// <summary>
            /// The singleton empty stack.
            /// </summary>
            /// <remarks>
            /// Additional instances representing the empty stack may exist on deserialized stacks.
            /// </remarks>
            private static readonly LazyStack s_EmptyField = new LazyStack();

            /// <summary>
            /// The element on the top of the stack.
            /// </summary>
            private T _head;

            /// <summary>
            /// A stack that contains the rest of the elements (under the top element).
            /// </summary>
            private LazyStack _tail;


            /// <summary>
            /// A stack that contains the heads of the elements.
            /// </summary>
            private readonly LazyStack _lazyHeads;

            /// <summary>
            /// A stack that contains the ends of the elements.
            /// </summary>
            private readonly ImmutableStack<T> _lazyTails;

            /// <summary>
            /// A stack that contains the middle of the elements.
            /// </summary>
            private readonly LazyStack _lazySchedule;

            /// <summary>
            /// Initializes a new instance of the <see cref="LazyStack"/> class
            /// that acts as the empty stack.
            /// </summary>
            private LazyStack()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="LazyStack"/> class.
            /// </summary>
            /// <param name="head">The head element on the stack.</param>
            /// <param name="tail">The rest of the elements on the stack.</param>
            internal LazyStack(T head, LazyStack tail)
            {
                Debug.Assert(tail is { });

                _head = head;
                _tail = tail;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="LazyStack"/> class with lazy initialization.
            /// </summary>
            internal LazyStack(LazyStack heads, ImmutableStack<T> tails, LazyStack schedule)
            {
                _lazyHeads = heads;
                _lazyTails = tails;
                _lazySchedule = schedule;
            }

            /// <summary>
            /// Gets the empty stack, upon which all stacks are built.
            /// </summary>
            public static LazyStack Empty
            {
                [凾(256)]
                get
                {
                    Debug.Assert(s_EmptyField.IsEmpty);
                    return s_EmptyField;
                }
            }

            /// <summary>
            /// Gets the empty stack, upon which all stacks are built.
            /// </summary>
            [凾(256)]
            public LazyStack Clear()
            {
                Debug.Assert(s_EmptyField.IsEmpty);
                return Empty;
            }

            /// <summary>
            /// Gets an empty stack.
            /// </summary>
            IImmutableStack<T> IImmutableStack<T>.Clear()
            {
                return this.Clear();
            }

            /// <summary>
            /// Gets a value indicating whether this instance is empty.
            /// </summary>
            /// <value>
            ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
            /// </value>
            public bool IsEmpty
            {
                [凾(256)]
                get
                {
                    return _tail is null && _lazySchedule is null;
                }
            }

            /// <summary>
            /// Gets the element on the top of the stack.
            /// </summary>
            /// <returns>
            /// The element on the top of the stack.
            /// </returns>
            /// <exception cref="InvalidOperationException">Thrown when the stack is empty.</exception>
            [凾(256)]
            public T Peek()
            {
                AtCoder.Internal.Contract.Assert(!IsEmpty, "This operation does not apply to an empty instance.");

                if (_tail is null && _lazySchedule is { })
                {
                    Rotate();
                }
                return _head!;
            }

            /// <summary>
            /// Gets a read-only reference to the element on the top of the stack.
            /// </summary>
            /// <returns>
            /// A read-only reference to the element on the top of the stack.
            /// </returns>
            /// <exception cref="InvalidOperationException">Thrown when the stack is empty.</exception>
            [凾(256)]
            public ref readonly T PeekRef()
            {
                AtCoder.Internal.Contract.Assert(!IsEmpty, "This operation does not apply to an empty instance.");

                if (_tail is null && _lazySchedule is { })
                {
                    Rotate();
                }
                return ref _head!;
            }


            /// <summary>
            /// Pushes an element onto a stack and returns the new stack.
            /// </summary>
            /// <param name="value">The element to push onto the stack.</param>
            /// <returns>The new stack.</returns>
            [凾(256)]
            public LazyStack Push(T value)
            {
                return new LazyStack(value, this);
            }

            /// <summary>
            /// Pushes an element onto a stack and returns the new stack.
            /// </summary>
            /// <param name="value">The element to push onto the stack.</param>
            /// <returns>The new stack.</returns>
            IImmutableStack<T> IImmutableStack<T>.Push(T value)
            {
                return this.Push(value);
            }

            /// <summary>
            /// Returns a stack that lacks the top element on this stack.
            /// </summary>
            /// <returns>A stack; never <c>null</c></returns>
            /// <exception cref="InvalidOperationException">Thrown when the stack is empty.</exception>
            [凾(256)]
            public LazyStack Pop()
            {
                AtCoder.Internal.Contract.Assert(!IsEmpty, "This operation does not apply to an empty instance.");

                if (_tail is null && _lazySchedule is { })
                {
                    Rotate();
                }
                Debug.Assert(_tail is { });
                return _tail;
            }

            /// <summary>
            /// Returns a stack that lacks the top element on this stack.
            /// </summary>
            /// <returns>A stack; never <c>null</c></returns>
            /// <exception cref="InvalidOperationException">Thrown when the stack is empty.</exception>
            IImmutableStack<T> IImmutableStack<T>.Pop()
            {
                return this.Pop();
            }

            /// <summary>
            /// Pops the top element off the stack.
            /// </summary>
            /// <param name="value">The value that was removed from the stack.</param>
            /// <returns>
            /// A stack; never <c>null</c>
            /// </returns>
            [凾(256)]
            public LazyStack Pop(out T value)
            {
                if (_tail is null && _lazySchedule is { })
                {
                    Rotate();
                }
                value = this.Peek();
                return this.Pop();
            }

            [凾(256)]
            internal void Rotate()
            {
                Debug.Assert(_lazyHeads is { });
                Debug.Assert(_lazyTails is { });
                Debug.Assert(_lazySchedule is { });

                LazyStack heads = _lazyHeads;
                ImmutableStack<T> tails = _lazyTails;
                LazyStack schedule = _lazySchedule;

                if (heads.IsEmpty)
                {
                    _head = tails.Peek();
                    _tail = schedule;
                }
                else
                {
                    heads = heads.Pop(out _head);
                    tails = tails.Pop(out T tailValue);
                    _tail = new LazyStack(heads, tails, schedule.Push(tailValue));
                }
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// An <see cref="LazyStackEnumerator"/> that can be used to iterate through the collection.
            /// </returns>
            public LazyStackEnumerator GetEnumerator()
            {
                return new LazyStackEnumerator(this);
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
                    new LazyStackEnumeratorObject(this);
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return new LazyStackEnumeratorObject(this);
            }
        }
    }
}
